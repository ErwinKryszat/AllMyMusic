using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic
{
    public class AlbumGenreItem 
    {
        private String _name = String.Empty;
        private Int32 _albumGenreId;
        private Int32 _albumCount;

        /// <summary>
        /// The name of the Artist/albumGenre that created the album
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The Database ID of the Artist/albumGenre
        /// </summary>
        public Int32 AlbumGenreId
        {
            get { return _albumGenreId; }
            set { _albumGenreId = value; }
        }

        public Int32 AlbumCount
        {
            get { return _albumCount; }
            set { _albumCount = value; }
        }

        public Boolean HasMultipleAlbums
        {
            get { return _albumCount > 1; }
        }

        public AlbumGenreItem()
        {
            _name = String.Empty;
            _albumGenreId = 0;
        }

        public AlbumGenreItem(String albumGenre, Int32 albumGenreId)
        {
            _name = albumGenre;
            _albumGenreId = albumGenreId;
        }
    }
}
