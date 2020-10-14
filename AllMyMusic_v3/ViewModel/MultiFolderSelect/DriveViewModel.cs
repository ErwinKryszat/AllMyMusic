using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.IO;

namespace AllMyMusic_v3.ViewModel
{
    public class DriveViewModel : TreeViewItemViewModel
    {
        public String LabelName { get; set; }
        public String DriveName { get; set; }
        public String DriveDescription { get; set; }
        public DriveType DriveType { get; set; }
        public Boolean DriveIsReady { get; set; }

        public DriveViewModel(DriveInfo drive, ComputerViewModel computerViewModel)
            : base(computerViewModel, true)
        {
            DriveName = drive.Name;
            DriveType = drive.DriveType;
            DriveIsReady = drive.IsReady;

            if (drive.IsReady == true)
            {
                DriveDescription = drive.VolumeLabel;
                LabelName = DriveDescription + " (" + DriveName + ")";
            }
            else
            {
                LabelName = " (" + DriveName + ")";
            }
        }

        protected override void LoadChildren()
        {
            if (DriveIsReady == true)
            {
                DirectoryInfo[] directoryInfos = new DirectoryInfo(DriveName).GetDirectories();

                foreach (DirectoryInfo directoryInfo in directoryInfos)
                {
                    FolderViewModel folderVM = new FolderViewModel(directoryInfo.Name, directoryInfo.FullName, this);

                    base.Children.Add(folderVM);
                }
            }
        }
    }

}
