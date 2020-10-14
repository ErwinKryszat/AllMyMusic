using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AllMyMusic_v3.ViewModel
{
    public class FolderViewModel : TreeViewItemViewModel
    {
        public String Name { get; set; }
        public String FullPath { get; set; }

        private bool? isChecked;
        public bool? IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    this.OnPropertyChanged("IsChecked");
                    ChildrenCkeckbox();

                    if (isChecked == false)
                    {
                        if (this.Parent.GetType() == typeof(FolderViewModel))
                        {
                            ((FolderViewModel)this.Parent).IsChecked = null;
                        }
                    }
                }
            }
        }

        public FolderViewModel(String name, String fullPath, DriveViewModel driveViewModel)
            : base(driveViewModel, true)
        {
            Name = name;
            FullPath = fullPath;
            isChecked = false;
        }

        public FolderViewModel(String name, String fullPath, FolderViewModel folderViewModel)
            : base(folderViewModel, true)
        {
            Name = name;
            FullPath = fullPath;
            isChecked = false;
        }



        protected override void LoadChildren()
        {
            DirectoryInfo[] directoryInfos = new DirectoryInfo(FullPath).GetDirectories();

            Array.Sort<DirectoryInfo>(directoryInfos, delegate(DirectoryInfo a, DirectoryInfo b)
            {
                return a.Name.CompareTo(b.Name);
            });


            if (directoryInfos != null)
            {
                foreach (DirectoryInfo directoryInfo in directoryInfos)
                {
                    FolderViewModel folderVM = new FolderViewModel(directoryInfo.Name, directoryInfo.FullName, this);
                    if (this.isChecked == true)
                    {
                        folderVM.IsChecked = true;
                    }
                    base.Children.Add(folderVM);
                }
            }
        }

        private void ChildrenCkeckbox()
        {
            if (isChecked == true)
            {
                // Set IsChecked for all children Checkboxes
                if (this.HasDummyChild == false)
                {
                    foreach (FolderViewModel item in this.Children)
                    {
                        item.IsChecked = true;
                    }
                }
            }
            else
            {
                // Clear IsChecked for all children Checkboxes
                if (this.HasDummyChild == false)
                {
                    // Do not uncheck children if this.Ischecke == null
                    if (((FolderViewModel)this).IsChecked == false )
                    {
                        foreach (FolderViewModel item in this.Children)
                        {
                            item.IsChecked = false;
                        }
                    }
                   
                }
            }
        }
    }

}
