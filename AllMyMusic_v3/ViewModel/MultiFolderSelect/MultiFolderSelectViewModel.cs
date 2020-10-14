using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Management;



namespace AllMyMusic_v3.ViewModel
{
    /// <summary>
    /// Interaction logic for MultiFolderSelectControl.xaml
    /// </summary>
    /// 
    public partial class MultiFolderSelectViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ManagementEventWatcher _watcher;
        private ComputerViewModel _computerViewModel;
        private ComputerTopViewModel _computerTopViewModel;
        private List<String> _folderList;
        private List<String> _expandedDrives;
        private List<String> _expandedFolders;
        #endregion


        #region Presentation Properties
        public ComputerViewModel ComputerViewModel
        {
            get { return _computerViewModel; }
            set
            {
                if (value == _computerViewModel)
                    return;

                _computerViewModel = value;

                RaisePropertyChanged("ComputerViewModel");
            }
        }
        public ComputerTopViewModel ComputerTopViewModel
        {
            get { return _computerTopViewModel; }
            set
            {
                if (value == _computerTopViewModel)
                    return;

                _computerTopViewModel = value;

                RaisePropertyChanged("ComputerTopViewModel");
            }
        }   
        public List<String> FolderList
        {
            get { return _folderList; }
        }  
        public List<String> ExpandedDrives
        {
            get { return _expandedDrives; }
            set 
            {
                if (value == _expandedDrives)
                    return;

                _expandedDrives = value;

                RaisePropertyChanged("ExpandedDrives");

                OnExpandedDrivesChanged();
            }
        }
        public List<String> ExpandedFolders
        {
            get { return _expandedFolders; }
            set 
            {
                if (value == _expandedFolders)
                    return;

                _expandedFolders = value;

                RaisePropertyChanged("ExpandedFolders");
                OnExpandedFoldersChanged();
            }
        }
        #endregion

        #region Constructor
        public MultiFolderSelectViewModel()
        {
            _computerTopViewModel = new ComputerTopViewModel();
            _computerViewModel = _computerTopViewModel.ComputerNames[0];

            //Monitor_LogicalDisks();
        }
        public void Unload()
        {
            if (_watcher != null)
            {
                _watcher.Stop();
                _watcher.Dispose();
            }
        }
        #endregion
        
        public List<String> GetAllCheckedFolders()
        {
            _folderList = new List<String>();
            _expandedDrives = new List<String>();
            _expandedFolders = new List<String>();

            foreach (DriveViewModel driveNode in _computerViewModel.Children)
            {
                if ((driveNode.DriveIsReady) && (driveNode.IsExpanded))
                {
                    GetAllCheckedFolders(driveNode);
                    _expandedDrives.Add(driveNode.DriveName);
                }
                else if ((driveNode.Children.Count > 0) && (driveNode.HasDummyChild == false))
                {
                    GetAllCheckedFolders(driveNode);
                }
            }
            return _folderList;
        }
        private void GetAllCheckedFolders(DriveViewModel currentNode)
        {
            foreach (FolderViewModel folderNode in currentNode.Children)
            {
                if (folderNode.IsChecked == true)
                {
                    _folderList.Add(folderNode.FullPath);
                    if ((folderNode.Children.Count > 0) && (folderNode.HasDummyChild == false))
                    {
                        GetAllCheckedFolders(folderNode);
                    }
                }
                else if ((folderNode.Children.Count > 0) && (folderNode.HasDummyChild == false))
                {
                    // Call this method rekursiv to get all subfolder
                    GetAllCheckedFolders(folderNode);
                }

                if (folderNode.IsExpanded == true)
                {
                    AddExpandedFolder(folderNode.FullPath);
                }
            }
        }
        private void GetAllCheckedFolders(FolderViewModel currentNode)
        {
            foreach (FolderViewModel folderNode in currentNode.Children)
            {
                if (folderNode.IsChecked == true)
                {
                    _folderList.Add(folderNode.FullPath);

                    if ((folderNode.Children.Count > 0) && (folderNode.HasDummyChild == false))
                    {
                        GetAllCheckedFolders(folderNode);
                    }
                }
                else if ((folderNode.Children.Count > 0) && (folderNode.HasDummyChild == false))
                {
                    // Call this method rekursiv to get all subfolder
                    GetAllCheckedFolders(folderNode);
                }

                if (folderNode.IsExpanded == true)
                {
                    AddExpandedFolder(folderNode.FullPath);
                }
            }
        }
        private void AddExpandedFolder(String folderPath)
        {
            Boolean foundSubstring = false;
            foreach (String item in _expandedFolders)
            {
                Int32 index = item.IndexOf(folderPath);
                if (item.IndexOf(folderPath) >= 0)
                {
                    foundSubstring = true;
                }
            }

            if (foundSubstring == false)
            {
                _expandedFolders.Add(folderPath);
            }
        }
        private void OnExpandedDrivesChanged()
        {
            foreach (String driveName in _expandedDrives)
            {
                ExpandDriveNode(driveName);
            }
        }
        private void OnExpandedFoldersChanged()
        {
            foreach (String folderName in _expandedFolders)
            {
                ExpandFolderNode(folderName);
            }
        }
        private void ExpandDriveNode(String driveName)
        {
            DriveViewModel driveNode = _computerViewModel.GetDrive(driveName);
            if (driveNode != null)
            {
                driveNode.IsExpanded = true;
            }
        }           
        private void ExpandFolderNode(String folderName)
        {
            String driveName = System.IO.Path.GetPathRoot(folderName);
            DriveViewModel driveNode = _computerViewModel.GetDrive(driveName);

            String[] splitFolder = folderName.Split('\\');


            if (driveNode != null)
            {
                FolderViewModel folderNodeTop = null;
                FolderViewModel folderNode = null;

                if (splitFolder.Length >= 1)
                {
                    folderNodeTop = ExpandFolderNode(driveNode, splitFolder[1]);
                }

                if ((splitFolder.Length >= 2) && (folderNodeTop != null))
                {
                    folderNode = folderNodeTop;
                    for (int i = 2; i < splitFolder.Length; i++)
                    {
                        folderNode = ExpandFolderNode(folderNode, splitFolder[i]);
                    }
                }
            }
        }
        private FolderViewModel ExpandFolderNode(DriveViewModel driveNode, String folderName)
        {
            foreach (FolderViewModel folderNode in driveNode.Children)
            {
                if (folderNode.Name == folderName)
                {
                    folderNode.IsExpanded = true;
                    return folderNode;
                }
            }
            return null;
        }
        private FolderViewModel ExpandFolderNode(FolderViewModel folderNodeParent, String folderName)
        {
            foreach (FolderViewModel folderNode in folderNodeParent.Children)
            {
                if (folderNode.Name == folderName)
                {
                    folderNode.IsExpanded = true;
                    return folderNode;
                }
            }
            return null;
        }


        #region Disk_Watcher
        private void Monitor_LogicalDisks()
        {
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;

            // Create event query to be notified within 1 second of  
            // a change in a service
            WqlEventQuery query = new WqlEventQuery();
            query.EventClassName = "__InstanceOperationEvent";
            query.WithinInterval = new TimeSpan(0, 0, 3);
            query.Condition = @"TargetInstance ISA 'Win32_LogicalDisk' ";


            _watcher = new ManagementEventWatcher(scope, query);
            _watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            _watcher.Start();

            
        }
        void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject baseObject = (ManagementBaseObject)e.NewEvent;
            ManagementBaseObject logicalDisk = (ManagementBaseObject)e.NewEvent["TargetInstance"];

            String driveName = String.Empty;
            foreach (var item in logicalDisk.Properties)
            {
                if ((item.Name == "Name") && (item.Value != null))
                {
                    driveName = item.Value.ToString();
                }
                
                // *** Property Names ****
                //Caption  F:
                //Description (CD Rom Disc)
                //DeviceID  F:
                //DriveType  5
                //Name F:
                //Size != null dann IsReady=true
                //VolumeName   Name der CD  (Papa 75)
            }

            if (String.IsNullOrEmpty(driveName) == false) 
            {
                DriveInfo drive = new DriveInfo(driveName);

                if (drive.DriveType != DriveType.Network)
                {
                    String className = baseObject.ClassPath.ClassName;
                    switch (className)
                    {
                        case "__InstanceCreationEvent":
                            // new drive was added
                            AddDriveNode(drive);
                            break;
                        case "__InstanceDeletionEvent":
                            // drive was removed
                            RemoveDriveNode(drive);
                            break;
                        case "__InstanceModificationEvent":
                            // drive has changed (i.e. added or removed a CD from optical drive
                            UpdateDriveNode(drive);
                            break;
                    }
                }
            }
        }
        private void AddDriveNode(DriveInfo drive)
        { 
            DriveViewModel driveVM = new DriveViewModel(drive, _computerViewModel);

            System.Windows.Application.Current.Dispatcher.Invoke(
                (Action)delegate()
                {
                    _computerViewModel.Children.Add(driveVM);
                });
        }
        private void RemoveDriveNode(DriveInfo drive)
        {
            Int32 removeIndex = -1;
            for (int i = 0; i < _computerViewModel.Children.Count; i++)
			{
                DriveViewModel driveVM = (DriveViewModel)_computerViewModel.Children[i];
                if (driveVM.DriveName == drive.Name)
                {
                    removeIndex = i;
                }
			}

            if (removeIndex >= 0)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(
                    (Action)delegate()
                    {
                        _computerViewModel.Children.RemoveAt(removeIndex);
                    });
            }
        }
        private void UpdateDriveNode(DriveInfo drive)
        {
            Int32 updateIndex = -1;
            for (int i = 0; i < _computerViewModel.Children.Count; i++)
            {
                DriveViewModel driveVM = (DriveViewModel)_computerViewModel.Children[i];
                if (driveVM.DriveName == drive.Name)
                {
                    updateIndex = i;
                }
            }

            if (updateIndex >= 0)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(
                    (Action)delegate()
                    {
                        _computerViewModel.Children[updateIndex] = new DriveViewModel(drive, _computerViewModel);
                    });
            }
        }
        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            _watcher.Stop();
            _watcher.Dispose();
        }
        #endregion

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

        ~MultiFolderSelectViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion

    }

}
