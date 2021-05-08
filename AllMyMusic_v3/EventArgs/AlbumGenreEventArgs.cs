using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AllMyMusic
{
    public class AlbumGenreEventArgs : EventArgs
    {
        private AlbumGenreItem _albumGenre;
        public AlbumGenreItem AlbumGenre
        {
            get { return _albumGenre; }
        }


        public AlbumGenreEventArgs(AlbumGenreItem albumGenre)
        {
            this._albumGenre = albumGenre;
        }
    }
}
