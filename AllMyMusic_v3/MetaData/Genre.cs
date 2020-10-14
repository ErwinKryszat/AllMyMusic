using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Define a genre and the assicated ID according to ID3V1 standard
    /// </summary>
    public class Genre
    {
        private String name;
        private Byte id;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        
        public Byte Id
        {
            get { return id; }
            set { id = value; }
        }

        public Genre(String genreName, Byte genreId)
        {
            this.name = genreName;
            this.id = genreId;
        }
    }
}
