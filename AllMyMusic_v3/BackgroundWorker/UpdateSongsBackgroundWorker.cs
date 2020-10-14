using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.IO;
using System.Security.AccessControl;

using System.Threading;
using System.Threading.Tasks;

using System.Diagnostics;

using AllMyMusic_v3.ViewModel;

using Metadata.Mp3;
using Metadata.ID3;



namespace AllMyMusic_v3
{
    public class UpdateSongsBackgroundWorker : IBackgroundQueueJob, IDisposable
    {
        #region Fields
        private ChangedPropertiesList _changedProperties;
        private ObservableCollection<SongItem> _songs;
        private CancellationToken _ct;
        private ConnectionInfo _conInfo;
        #endregion

        #region Constructor
        public UpdateSongsBackgroundWorker(ConnectionInfo conInfo, CancellationToken ct) 
        {
            _conInfo = conInfo;
            _ct = ct;
        }
        #endregion

        public async Task DoWork(object taskQueueItem)
        {
            TaskQueueItem taskParams = (TaskQueueItem)taskQueueItem;
            ChangedPropertiesListEventArgs args = (ChangedPropertiesListEventArgs)taskParams.UserData;
            ReportProgress_Callback progress = taskParams.ProgressCallback;
            WorkDone_Callback workDoneCallback = taskParams.WorkDoneCallback;

            _changedProperties = args.ChangedProperties;
            _songs = args.Songs;

            BackgroundJobHelper jobHelper = new BackgroundJobHelper(_conInfo);

            SongItem lastSong = null;
            double secondsPerFile;
            Int32 secondsTotal;
            Int32 filesRemaining;
            Int32 secondsRemaining;

            Int32 songCount = 0;
            ProgressDataViewModel progressData = new ProgressDataViewModel();
            progressData.ActionName = "Update Songs";
            progressData.ProgressMaximum = _songs.Count;


            Stopwatch ElapseTimer = new Stopwatch();
            ElapseTimer.Start();

            for (int i = 0; i < _songs.Count; i++)
            {
                FileInfo fi = new FileInfo(_songs[i].SongFullPath);
                Boolean writePermissions = (fi.IsReadOnly == false);


                if (writePermissions)
                {
                    if (_changedProperties.Count > 0)
                    {
                        // _changedProperties.UpdateSong(_songs[i]);
                        Mp3Metaedit metaData = new Mp3Metaedit(_songs[i].SongFullPath);
                        metaData.UpdateMetadata(_changedProperties);
                        _songs[i].Update(_changedProperties);
                    }
                    else
                    {
                        // we come from the AutotagTool
                        Mp3Metaedit metaData = new Mp3Metaedit(_songs[i].SongFullPath);
                        metaData.UpdateMetadata(_songs[i]);
                    }

                    if (_songs[i].ArtistType == ArtistType.VariousArtist)
                    {
                        Int32 k;
                        k = 0;
                        k++;
                    }

                    await jobHelper.AddOneBandAlbumSong(_songs[i], lastSong);
                }

               
                lastSong = _songs[i];

                progressData.ProgressValue = i + 1;
                progressData.FileCount = String.Format("{0} / {1}", i+1, songCount);
                progressData.FolderCount = String.Empty;
                progressData.CurrentFolder = _songs[i].SongPath;
                progressData.TimeElapsed = ElapseTimer.Elapsed.ToString().Substring(0, 8);
                progressData.FileWriteAccessDenied = !writePermissions;

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

                if (writePermissions == false)
                {
                    break;
                }
            }

            await jobHelper.UpdateCountryTable();
   

            ElapseTimer.Stop();

            jobHelper.Close();

            if (workDoneCallback != null)
            {
                workDoneCallback();
            }
        }

        public static bool HasWritePermissionOnDir(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true,
                                        typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    writeAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    writeDeny = true;
            }

            return writeAllow && !writeDeny;
        }

        public static bool HasWritePermissionOnFile(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            var accessControlList = File.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true,
                                        typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    writeAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    writeDeny = true;
            }

            return writeAllow && !writeDeny;
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

        ~UpdateSongsBackgroundWorker()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            //if (_connection != null)
            //{
            //    _connection.Dispose();
            //}          
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
