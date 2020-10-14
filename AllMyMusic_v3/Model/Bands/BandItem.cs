using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic_v3
{
    public class BandItem 
    {
        private String _bandName = String.Empty;
        private String _sortName = String.Empty;
        private Int32 _bandId;
        private Int32 _albumCount;
        private Int32 _bookmarkedBand;
        private ArtistType _artistType;




        /// <summary>
        /// The name of the Artist/Band that created the album
        /// </summary>
        public String BandName
        {
            get { return _bandName; }
            set { _bandName = value; }
        }

        /// <summary>
        /// The name of the Artist/Band that created the album
        /// </summary>
        public String SortName 
        {
            get { return _sortName; }
            set { _sortName = value; }
        }

        /// <summary>
        /// The Database ID of the Artist/Band
        /// </summary>
        public Int32 BandId
        {
            get { return _bandId; }
            set { _bandId = value; }
        }
       
        /// <summary>
        /// Wether the band/artist is bookmarked
        /// </summary>
        public Int32 BookmarkedBand
        {
            get { return _bookmarkedBand; }
            set { _bookmarkedBand = value; }
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

        /// <summary>
        /// SingleArtist or VariousArtist
        /// </summary>
        public ArtistType ArtistType
        {
            get { return _artistType; }
            set { _artistType = value; }
        }

        public BandItem()
        {
            _bandName = String.Empty;
            _sortName = String.Empty;
            _bandId = 0;
            _albumCount = 0;
            _bookmarkedBand = 0;
            _artistType = ArtistType.SingleArtist;
        }

        public BandItem(String bandName)
        {
            _bandName = bandName;
            _sortName = bandName;
            _bandId = 0;
            _albumCount = 0;
            _bookmarkedBand = 0;
            _artistType = ArtistType.SingleArtist;
        }

        public BandItem(String bandName, String sortName)
        {
            _bandName = bandName;
            _sortName = sortName;
            _bandId = 0;
            _albumCount = 0;
            _bookmarkedBand = 0;
            _artistType = ArtistType.SingleArtist;
        }
    }
}
