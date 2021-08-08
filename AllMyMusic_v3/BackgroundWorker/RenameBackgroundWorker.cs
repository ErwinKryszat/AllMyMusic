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
    public class RenameBackgroundWorker : IBackgroundQueueJob, IDisposable
    {
        #region Fields
        private CancellationToken _ct;
        #endregion

        #region Constructor
        public RenameBackgroundWorker(CancellationToken ct) 
        {
            _ct = ct;
        }
        #endregion

        public async Task DoWork(object taskQueueItem)
        {
            TaskQueueItem taskParams = (TaskQueueItem)taskQueueItem;
            ChangedPropertiesListEventArgs args = (ChangedPropertiesListEventArgs)taskParams.UserData;
            ReportProgress_Callback progress = taskParams.ProgressCallback;
            WorkDone_Callback workDoneCallback = taskParams.WorkDoneCallback;

            ObservableCollection<SongItem>  _songs = args.Songs;
            SongItem lastSong = null;
            BackgroundJobHelper _jobHelper = new BackgroundJobHelper(AppSettings.DatabaseSettings.DefaultDatabase);


            double secondsPerFile;
            Int32 secondsTotal;
            Int32 filesRemaining;
            Int32 secondsRemaining;

            Int32 songCount = 0;
            ProgressDataViewModel progressData = new ProgressDataViewModel();
            progressData.ActionName = "Rename Files";
            progressData.ProgressMaximum = _songs.Count;


            Stopwatch ElapseTimer = new Stopwatch();
            ElapseTimer.Start();

            for (int i = 0; i < _songs.Count; i++)
            {
                if (String.IsNullOrEmpty(_songs[i].NewFullPath) == true)
                {
                    continue;
                }

                String sourcePath = _songs[i].SongPath;
                String sourceFilename = _songs[i].SongFilename;
                String newFilename = Path.GetFileName(_songs[i].NewFullPath);
                String newFoldername = Path.GetDirectoryName(_songs[i].NewFullPath);


                // Append CD1, CD01, or CD2 or CD02 to the newFoldername
                // This avoid that after rename a multi CD album is all copied to the same folder

                Int32 pos0 = sourcePath.IndexOf("CDS", sourcePath.Length - 6);
                if (pos0 < 0 )
                {
                    Int32 pos1 = sourcePath.IndexOf("CD", sourcePath.Length - 6);
                    String _cdNumber = String.Empty;
                    if (pos1 > 0)
                    {
                        _cdNumber = sourcePath.Substring(pos1, sourcePath.Length - pos1);
                        newFoldername = newFoldername + " " + _cdNumber;
                    }
                }
               

                // If the target directory exists we don't want to mix files, therefor create a new folder
                for (int k = 0; k < 10; k++)
                {
                    if (Directory.Exists(newFoldername) == true)
                    {
                        newFoldername = newFoldername + "_" + k.ToString();
                    }
                    else
                    {
                        break;
                    }
                }
                

                String fullFilenameDestination = String.Empty;
                try
                {
                    String destinationPath = BuildDirectoryStructure(newFoldername, sourcePath);
                    String fullFilenameSource = sourcePath + sourceFilename;
                    fullFilenameDestination = destinationPath + newFilename;

                    if (fullFilenameSource != fullFilenameDestination)
                    {
                        File.Move(fullFilenameSource, fullFilenameDestination);
                        _songs[i].SongFilename = Path.GetFileName(fullFilenameDestination);
                        _songs[i].SongPath = Path.GetDirectoryName(fullFilenameDestination) + "\\";
                        await _jobHelper.AddOneBandAlbumSong(_songs[i], lastSong);
                        lastSong = _songs[i];
                    }
                }
                catch (Exception Err)
                {
                    String errorMessage = "Error moving file: " + fullFilenameDestination;
                    ShowError.ShowAndLog(Err, errorMessage, 2001);
                }

                FileSystemTool.DeleteEmptyDirectory(sourcePath);

                progressData.ProgressValue = i + 1;
                progressData.FileCount = String.Format("{0} / {1}", i + 1, songCount);
                progressData.FolderCount = String.Empty;
                progressData.CurrentFolder = _songs[i].SongPath;
                progressData.TimeElapsed = ElapseTimer.Elapsed.ToString().Substring(0, 8);

                // update TimeRemaining display only after we have done first 5%
                if ((progressData.ProgressValue > (0.05 * progressData.ProgressMaximum)) || (ElapseTimer.Elapsed.Seconds > 5))
                {
                    secondsPerFile = (double)(ElapseTimer.Elapsed.TotalSeconds / progressData.ProgressValue);

                    secondsTotal = (Int32)(secondsPerFile * songCount);
                    filesRemaining = songCount - progressData.ProgressValue;
                    secondsRemaining = (Int32)(filesRemaining * secondsPerFile);

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
       

        private String BuildDirectoryStructure(String targetSubfolderName, String sourcePath)
        {
            StringBuilder DestinationPathSB = null;
            StringBuilder SourcePathSB = null;

            if (targetSubfolderName != "")
            {
                if (targetSubfolderName.Substring(0, 1) == "\\")
                {
                    // Cut off "\" if this is the first char
                    targetSubfolderName = targetSubfolderName.Substring(1, targetSubfolderName.Length - 1);
                }

                if (targetSubfolderName.Substring(targetSubfolderName.Length-1, 1) == "\\")
                {
                    // Cut off "\" if this is the last char
                    targetSubfolderName = targetSubfolderName.Substring(0, targetSubfolderName.Length - 1);
                }

                String[] sourcePathArray = sourcePath.Substring(0, sourcePath.Length - 1).Split('\\');
                String[] destinationPathArray = targetSubfolderName.Split('\\');

                SourcePathSB = new StringBuilder();
                DestinationPathSB = new StringBuilder();

                for (int i = 0; i < sourcePathArray.Length; i++)
                {
                    SourcePathSB.Append(sourcePathArray[i]);
                    SourcePathSB.Append("\\");
                }

                for (int i = 0; i < sourcePathArray.Length - 1; i++)
                {
                    DestinationPathSB.Append(sourcePathArray[i]);
                    DestinationPathSB.Append("\\");
                }

                for (int i = 0; i < destinationPathArray.Length; i++)
                {
                    String dp = DestinationPathSB.ToString();
                    if (dp.IndexOf(destinationPathArray[i]) == -1)
                    {
                        DestinationPathSB.Append(destinationPathArray[i]);
                        DestinationPathSB.Append("\\");
                    }
                    
                    if (Directory.Exists(DestinationPathSB.ToString()) == false)
                    {
                        FileSystemTool.CreateDirectory(DestinationPathSB.ToString());
                    }
                }
                MoveImages(SourcePathSB.ToString(), DestinationPathSB.ToString());
                MoveOtherFiles(SourcePathSB.ToString(), DestinationPathSB.ToString());
            }
            else
            {
                DestinationPathSB = new StringBuilder(sourcePath);
            }

            
            return DestinationPathSB.ToString();
        }

        private void MoveImages(String sourcePath, String destinationPath)
        {
            List<FileInfo> ImageFiles = GetImageFileList(sourcePath);
            foreach (FileInfo file in ImageFiles)
            {
                String SourceFileFullName = file.FullName;
                String SourcefileName = file.Name;
                String DestFileFullName = destinationPath + SourcefileName;
                File.Move(SourceFileFullName, DestFileFullName);
            }
        }
        private void MoveOtherFiles(String sourcePath, String destinationPath)
        {
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] otherFiles = di.GetFiles("*.m3u;*.txt;.ini", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in otherFiles)
            {
                String SourceFileFullName = file.FullName;
                String SourcefileName = file.Name;
                String DestFileFullName = destinationPath + SourcefileName;
                File.Move(SourceFileFullName, DestFileFullName);
            }
        }

        private List<FileInfo> GetImageFileList(String folderPath)
        {
            List<FileInfo> imageFiles = new List<System.IO.FileInfo>();
            DirectoryInfo di = new DirectoryInfo(folderPath);
            FileInfo[]  allFiles = di.GetFiles("*.*", SearchOption.TopDirectoryOnly);

            foreach (FileInfo coverFile in allFiles)
            {
                String extension = Path.GetExtension(coverFile.Name).ToLower();
                if ((extension == ".jpg") || (extension == ".png"))
                {
                    imageFiles.Add(coverFile);
                }
            }
            return imageFiles;
        }

        private List<FileInfo> GetOtherFileList(String folderPath)
        {
            List<FileInfo> otherFiles = new List<System.IO.FileInfo>();
            DirectoryInfo di = new DirectoryInfo(folderPath);
            FileInfo[] allFiles = di.GetFiles("*.*", SearchOption.TopDirectoryOnly);

            foreach (FileInfo coverFile in allFiles)
            {
                String extension = Path.GetExtension(coverFile.Name).ToLower();
                if ((extension != ".jpg") && (extension != ".png"))
                {
                    otherFiles.Add(coverFile);
                }
            }
            return otherFiles;
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

        ~RenameBackgroundWorker()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {

        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
