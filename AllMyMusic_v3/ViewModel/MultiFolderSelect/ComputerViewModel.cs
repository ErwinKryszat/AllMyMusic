using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AllMyMusic_v3.ViewModel
{
    public class ComputerViewModel : TreeViewItemViewModel
    {
        public String Name { get; set; }

        public ComputerViewModel(String name)
            : base(null, true)
        {
            Name = name;
            this.IsExpanded = true;
        }

        protected override void LoadChildren()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                DriveViewModel driveVM = new DriveViewModel(drive, this);
                base.Children.Add(driveVM);
            }
        }

        public DriveViewModel GetDrive(String driveName)
        {
            foreach (DriveViewModel driveNode in base.Children)
            {
                if (driveNode.DriveName == driveName)
                {
                    return driveNode;
                }
            }
            return null;
        }
    }
}
