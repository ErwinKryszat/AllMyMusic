using System;
using System.Collections.ObjectModel;
using System.Text;



namespace AllMyMusic_v3
{
    public class AlbumListEventArgs : EventArgs
    {
        private ObservableCollection<AlbumItem> _albumList;
        public ObservableCollection<AlbumItem> AlbumList
        {
            get { return _albumList; }
        }


        public AlbumListEventArgs(ObservableCollection<AlbumItem> albumList)
        {
            this._albumList = albumList;
        }
    }
}
