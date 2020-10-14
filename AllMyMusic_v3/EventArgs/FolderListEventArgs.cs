using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AllMyMusic_v3
{
    public class FolderListEventArgs : EventArgs
    {
        private List<String> _folderList;
        public List<String> FolderList
        {
            get { return _folderList; }
        }


        public FolderListEventArgs(List<String> folderList)
        {
            this._folderList = folderList;
        }
    }
}
