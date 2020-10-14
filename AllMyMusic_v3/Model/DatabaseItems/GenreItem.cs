using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic_v3
{
    public class GenreItem 
    {
        private String _genre = String.Empty;
        private Int32 _genreId;

        /// <summary>
        /// The name of the Artist/genre that created the album
        /// </summary>
        public String Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        /// <summary>
        /// The Database ID of the Artist/genre
        /// </summary>
        public Int32 GenreId
        {
            get { return _genreId; }
            set { _genreId = value; }
        }
       

        public GenreItem()
        {
            _genre = String.Empty;
            _genreId = 0;
        }

        public GenreItem(String genre, Int32 genreId)
        {
            _genre = genre;
            _genreId = genreId;
        }
    }
}
