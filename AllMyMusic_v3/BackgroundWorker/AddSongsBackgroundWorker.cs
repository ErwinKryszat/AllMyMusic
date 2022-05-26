using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows;

using AllMyMusic.ViewModel;
using AllMyMusic.Settings;
using Metadata.Mp3;
using Metadata.ID3;



namespace AllMyMusic
{
    public class AddSongsBackgroundWorker : IBackgroundQueueJob, IDisposable
    {
        #region Fields
        private FolderListCreator folderCreator = null;
        private BackgroundJobHelper _jobHelper;
        private CancellationToken _ct;
        #endregion

        #region Constructor
        public AddSongsBackgroundWorker(CancellationToken ct) 
        {
            _ct = ct;
        }
        #endregion

        public async Task DoWork(object taskQueueItem)
        {
            TaskQueueItem taskParams = (TaskQueueItem)taskQueueItem;
            List<String> folderList = (List<String>)taskParams.UserData;
            ReportProgress_Callback progress = taskParams.ProgressCallback;
            WorkDone_Callback workDoneCallback = taskParams.WorkDoneCallback;

            _jobHelper = new BackgroundJobHelper(AppSettings.DatabaseSettings.DefaultDatabase);


            folderCreator = new FolderListCreator(folderList, progress, null);

            Int32 songCount = 0;
            Int32 folderCount = folderCreator.Folders.Count;
            ProgressDataViewModel progressData = new ProgressDataViewModel();
            progressData.ActionName = "Add Songs";
            progressData.ProgressMaximum = folderCount;

            double secondsPerFolder = 0;
            double secondsTotal = 0;
            Int32 foldersRemaining = 0;
            double secondsRemaining = 0;

            Stopwatch ElapseTimer = new Stopwatch();
            ElapseTimer.Start();


            for (int i = 0; i < folderCreator.Folders.Count; i++)
            {
                songCount += await AddSongsForFolder(folderCreator.Folders[i]);

                if (songCount == -1)
                {
                    break;
                }

                progressData.ProgressValue = i + 1;
                progressData.FileCount = String.Format("{0}", songCount);
                progressData.FolderCount = progressData.ProgressValue.ToString() + " / " + folderCount.ToString();
                progressData.CurrentFolder = folderCreator.Folders[i];
                //progressData.TimeElapsed = String.Format("{0:0000}", ElapseTimer.Elapsed.TotalSeconds);
                progressData.TimeElapsed = ElapseTimer.Elapsed.ToString().Substring(0, 8);

                // update TimeRemaining display only after we have done first 5%
                if ((progressData.ProgressValue > (0.05 * progressData.ProgressMaximum)) || (ElapseTimer.Elapsed.Seconds > 5))
                {
                    secondsPerFolder = (double)(ElapseTimer.Elapsed.TotalSeconds / progressData.ProgressValue);

                    secondsTotal = secondsPerFolder * folderCreator.Folders.Count;
                    foldersRemaining = folderCreator.Folders.Count - progressData.ProgressValue;
                    secondsRemaining = foldersRemaining * secondsPerFolder;

                    TimeSpan remainingTime = TimeSpan.FromSeconds(secondsRemaining);
                    progressData.TimeRemaining = String.Format("{0:00}:{1:00}:{2:00}", (int)remainingTime.Hours, (int)remainingTime.Minutes, (int)remainingTime.Seconds);
                }


                if (progress != null)
                {
                    progress(progressData);
                }

                if ((_ct != null) && (_ct.IsCancellationRequested == true))
                {
                    break;
                }
            }

            try
            {
                await _jobHelper.UpdateCountryTable();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error in BackgroundJobHelper.UpdateCountryTable ";
                await Application.Current.Dispatcher.BeginInvoke(new Action(() => ShowError.ShowAndLog(Err, errorMessage, 1007)));
            }
            finally
            {
                ElapseTimer.Stop();

                _jobHelper.Close();

                if (workDoneCallback != null)
                {
                    workDoneCallback();
                }
            }
        }
        private async Task<Int32> AddSongsForFolder(String folderPath)
        {
            //Playlist playlist = new Playlist();

            int fileCount = 0;  // fileCounter for number of songs
            SongItem song = null;
            SongItem previousSong = null;
            String fileName = String.Empty;

            try
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                FileInfo[] MultimediaFiles = di.GetFiles("*.*", SearchOption.TopDirectoryOnly);
                if (MultimediaFiles.Length == 0) { return 0; }

                foreach (FileInfo MultimediaFile in MultimediaFiles)
                {
                    fileName = MultimediaFile.FullName;
                    switch (MultimediaFile.Extension.ToLower())
                    {
                        case ".mp3":
                            fileCount++;
                            Mp3Metaedit mp3Metaedit = new Mp3Metaedit(MultimediaFile.FullName);
                            song = mp3Metaedit.ReadMetadata();
                            if (song != null)
                            {
                                if (String.IsNullOrEmpty(song.AlbumName) == true)
                                {
                                    String[] directoryNames = song.SongPath.Split('\\');
                                    song.AlbumName = directoryNames[directoryNames.Length - 2];

                                    ChangedPropertiesList changedProperties = new ChangedPropertiesList();
                                    changedProperties.Add("AlbumName", song.AlbumName);
                                    mp3Metaedit.UpdateMetadata(changedProperties);
                                }

                                if ((mp3Metaedit.BitrateType == BitrateType.VBR) && (mp3Metaedit.TLEN_Exists() == false))
                                {
                                    ChangedPropertiesList changedProperties = new ChangedPropertiesList();
                                    changedProperties.Add("MilliSeconds", song.MilliSeconds);
                                    mp3Metaedit.UpdateMetadata(changedProperties);
                                }
                            }
                            break;

                        default:
                            song = null;
                            break;
                    }

                    if ((previousSong == null) && (song != null))
                    {
                        CoverImages images = new CoverImages(MultimediaFiles);
                        song.BackImageFileName = images.BackImage;
                        song.FrontImageFileName = images.FrontImage;
                        song.StampImageFileName = images.StampImage;
                    }

                    if (song != null)
                    {
                        previousSong = await _jobHelper.AddOneBandAlbumSong(song, previousSong);
                    }
                }
            }
            catch (Exception)
            {
                fileCount = -1;
            }
            return fileCount;
        }
       

        #region IDisposable Members
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();
        }

        ~AddSongsBackgroundWorker()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_jobHelper != null)
            {
                _jobHelper.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
