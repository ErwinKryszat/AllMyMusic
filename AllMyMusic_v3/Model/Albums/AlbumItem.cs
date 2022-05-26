using System;
using System.Windows.Media.Imaging;
using System.ComponentModel;

using System.IO;

namespace AllMyMusic
{
    public class AlbumItem : INotifyPropertyChanged 
    {
        private Boolean _isSelected = false;

        private String _albumName = String.Empty;
        private String _albumSortName = String.Empty;
        private ArtistType _artistType;
        private String _bandName = String.Empty;
        private String _bandSortName = String.Empty;
        private String _albumGenre = String.Empty;
        private String _albumPath = String.Empty;
        private String _frontImageFileName = String.Empty;
        private String _backImageFileName = String.Empty;
        private String _stampImageFileName = String.Empty;
        private String _genre = String.Empty;
        private String _year = String.Empty;
        private BitmapImage _stampImage;
        private Guid _albumGuid;

        private Int32 _songCount;
        private Int32 _totalLength;
        private Int32 _bandId;
        private Int32 _albumId;
        private Int32 _albumGenreId;
        private Int32 _bookmarkedAlbum;
        private Int32 _bookmarkedBand;
        private Int32 _previousSongId;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged(string propertyName, object propertyValue)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool SetProperty<T>(ref T storage, T value, String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName, value);
            return true;
        }

        
        public Boolean IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value, "IsSelected");
                BooleanEventArgs args = new BooleanEventArgs(_isSelected);
                OnSelectionChanged(this, args);
            }
        }

        public Guid AlbumGuid
        {
            get { return _albumGuid; }
        }

        
        public BitmapImage StampImage
        {
            get { return _stampImage; }
            set { _stampImage =  value; }
        }

        /// <summary>
        /// Headline
        /// </summary>
        public String Headline
        {
            get { return this._albumName + " (" + this._bandName + ")"; ; }
        }


        /// <summary>
        /// The name of the album
        /// </summary>
        public String AlbumName
        {
            get { return _albumName; }
            set { _albumName = value; }
        }

        /// <summary>
        /// The sort name of the album
        /// </summary>
        public String AlbumSortName
        {
            get { return _albumSortName; }
            set { _albumSortName = value; }
        }

        /// <summary>
        /// The Database ID of the album
        /// </summary>
        public Int32 AlbumId
        {
            get { return _albumId; }
            set { _albumId = value; }
        }

        /// <summary>
        /// The parent folder of the \Band\Album folder
        /// </summary>
        public String AlbumGenre
        {
            get { return _albumGenre; }
            set { _albumGenre = value; }
        }

        /// <summary>
        /// The Database ID of the AlbumGenre
        /// </summary>
        public Int32 AlbumGenreId
        {
            get { return _albumGenreId; }
            set { _albumGenreId = value; }
        }

        /// <summary>
        /// The Path of the album
        /// </summary>
        public String AlbumPath
        {
            get { return _albumPath; }
            set { _albumPath = value; }
        }

        /// <summary>
        /// SingleArtist or VariousArtist
        /// </summary>
        public ArtistType ArtistType
        {
            get { return _artistType; }
            set { _artistType = value; }

        }

        /// <summary>
        /// The name of the Artist/Band that created the album
        /// </summary>
        public String BandName
        {
            get { return _bandName; }
            set { _bandName = value; }
        }

        /// <summary>
        /// The sort name of the Artist/Band that created the album
        /// </summary>
        public String BandSortName
        {
            get { return _bandSortName; }
            set { _bandSortName = value; }
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
        /// Wether the album is bookmarked
        /// </summary>
        public Int32 BookmarkedAlbum
        {
            get { return _bookmarkedAlbum; }
            set { _bookmarkedAlbum = value; }
        }

        /// <summary>
        /// Wether the band/artist is bookmarked
        /// </summary>
        public Int32 BookmarkedBand
        {
            get { return _bookmarkedBand; }
            set { _bookmarkedBand = value; }
        }



        /// <summary>
        /// Path of the fontal cover sleeve
        /// </summary>
        public String FrontImageFileName
        {
            get { return _frontImageFileName; }
            set { _frontImageFileName = value; }
        }


        /// <summary>
        /// Path of the back cover sleeve
        /// </summary>
        public String BackImageFileName
        {
            get { return _backImageFileName; }
            set { _backImageFileName = value; }
        }


        /// <summary>
        /// Path of the thumbnail cover sleeve
        /// </summary>
        public String StampImageFileName
        {
            get { return _stampImageFileName; }
            set { _stampImageFileName = value; }
        }

        /// <summary>
        /// Path of the fontal cover sleeve
        /// </summary>
        public String FrontImageFullpath
        {
            get 
            {
                if ((String.IsNullOrEmpty(_albumPath) == false) && (String.IsNullOrEmpty(_frontImageFileName) == false))
                {
                    return _albumPath + _frontImageFileName; 
                }
                return String.Empty;
            }
            set {  }
        }


        /// <summary>
        /// Path of the back cover sleeve
        /// </summary>
        public String BackImageFullpath
        {
            get 
            {
                if ((String.IsNullOrEmpty(_albumPath) == false) && (String.IsNullOrEmpty(_backImageFileName) == false))
                {
                    return _albumPath + _backImageFileName; 
                }
                return String.Empty;
            }
            set { }
        }


        /// <summary>
        /// Path of the thumbnail cover sleeve
        /// </summary>
        public String StampImageFullpath
        {
            get 
            {
                if ((String.IsNullOrEmpty(_albumPath) == false) && (String.IsNullOrEmpty(_stampImageFileName) == false))
                {
                    return _albumPath + _stampImageFileName; 
                }
                return String.Empty;
            }
            set { }
        }

        /// <summary>
        /// Genre is the style of music Rock, Pop, Jazz, Reggea, ...
        /// </summary>
        public String Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        /// <summary>
        /// The Database ID of the previous processed song
        /// </summary>
        public Int32 PreviousSongId
        {
            get { return _previousSongId; }
            set { _previousSongId = value; }
        }


        /// <summary>
        /// The Database ID of the album
        /// </summary>
        public Int32 SongCount
        {
            get { return _songCount; }
            set { _songCount = value; }
        }

        /// <summary>
        /// The total duration of the album in seconds
        /// </summary>
        public Int32 TotalLength
        {
            get { return _totalLength; }
            set { _totalLength = value; }
        }


        /// <summary>
        /// The year when the album was first published
        /// </summary>
        public String Year
        {
            get { return _year; }
            set { _year = value; }
        }

        /// <summary>
        /// The year when the album was first published
        /// </summary>
        public String YearAndTitle
        {
            get 
            {
                if (String.IsNullOrEmpty(_year)== false)
                {
                    return _year + " " + _albumName; 
                }
                else
                {
                    return _albumName; 
                }
                
            }
        }

        public AlbumItem()
        {
            _albumGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Create AlbumItem based on information from a SongItem
        /// </summary>
        /// <param name="song"></param>
        public AlbumItem(SongItem song)
        {
            Update(song);
        }

        /// <summary>
        /// Update AlbumItem based on information from a SongItem
        /// </summary>
        /// <param name="song"></param>
        public void Update(SongItem song)
        {
            this.AlbumId = song.AlbumId;
            this._albumGenre = song.AlbumGenre;
            this._albumName = song.AlbumName;
            this._albumSortName = song.AlbumSortName;
            this._albumPath = song.SongPath;
            this._artistType = song.ArtistType;
            this._bandName = song.BandName;
            this.BandId = song.BandId;
            this._genre = song.Genre;
            this.Year = song.Year;
            this._frontImageFileName = song.FrontImageFileName;
            this._backImageFileName = song.BackImageFileName;
            this._stampImageFileName = song.StampImageFileName;
        }

        #region Events
        public delegate void SelectionChangedEventHandler(object sender, BooleanEventArgs e);
        public event SelectionChangedEventHandler SelectionChanged;
        protected virtual void OnSelectionChanged(object sender, BooleanEventArgs e)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, e);
            }
        }
        #endregion
    }
}
