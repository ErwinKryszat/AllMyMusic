using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AllMyMusic
{
    public class AlbumItemEventArgs : EventArgs
    {
        private AlbumItem _album;
        public AlbumItem Album
        {
            get { return _album; }
        }


        public AlbumItemEventArgs(AlbumItem album)
        {
            this._album = album;
        }
    }
}
