using System;
using System.Text;

namespace AllMyMusic_v3
{
    public class AlbumDetails
    {
        private String _albumGenre = String.Empty;
        private Int32 _bitrate;
        private String _duration = String.Empty;
        private double _samplingRate;
        private Int32 _songCount;
        private Int32 _totalLength;
        private String _year = String.Empty;
        private String _websiteArtist = String.Empty;
        private String _websiteUser = String.Empty;

        public AlbumDetails()
        {

        }

        public AlbumDetails(AlbumItem album)
        {
            _albumGenre = album.AlbumGenre;
            _year = album.Year;
            _totalLength = album.TotalLength;
        }

        public String AlbumGenre
        {
            get { return _albumGenre; }
            set { _albumGenre = value; }
        }


        public Int32 Bitrate
        {
            get { return _bitrate; }
            set { _bitrate = value; }
        }


        public Int32 TotalLength
        {
            get { return _totalLength; }
            set { _totalLength = value; }
        }


        public double SamplingRate
        {
            get { return _samplingRate; }
            set { _samplingRate = value; }
        }


        public Int32 SongCount
        {
            get { return _songCount; }
            set { _songCount = value; }
        }


        public String Year
        {
            get { return _year; }
            set { _year = value; }
        }


        public String WebsiteArtist
        {
            get { return _websiteArtist; }
            set { _websiteArtist = value; }
        }


        public String WebsiteUser
        {
            get { return _websiteUser; }
            set { _websiteUser = value; }
        }
    }
}
