using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using System.ComponentModel;
using System.Windows.Media.Imaging;



namespace AllMyMusic_v3
{
     /// <summary>
    /// This class is used to:
    /// Hold all data assiciated to a song
    /// </summary>
    public class SongItem : INotifyPropertyChanged 
    {
        // Date: 25 October 2011
        // Name: Erwin Kryszat


        const String variousArtist = "VariousArtists";



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

        private Boolean _isSelected = false;
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


        #region Contributors

        /// <summary>
        /// The name of the Artist/Band that created the album
        /// </summary>
        private String bandName = String.Empty;
        public String BandName
        {
            get { return bandName; }
            set { SetProperty(ref bandName, value, "BandName"); }   
        }

        /// <summary>
        /// The sort name of the Artist/Band that created the album
        /// </summary>
        private String bandSortName = String.Empty;
        public String BandSortName
        {
            get { return bandSortName; }
            set { SetProperty(ref bandSortName, value, "BandSortName"); }   
        }

        /// <summary>
        /// The Database ID of the Artist/Band
        /// </summary>
        private Int32 bandId = 0;
        public Int32 BandId
        {
            get { return bandId; }
            set
            {
                bandId = value;
            }
        }

        /// <summary>
        /// The name of the composer
        /// </summary>
        private String composerName = String.Empty;
        public String ComposerName
        {
            get { return composerName; }
            set { SetProperty(ref composerName, value, "ComposerName"); }  
        }

        /// <summary>
        /// The Database ID of the composer
        /// </summary>
        private Int32 composerId = 0;
        public Int32 ComposerId
        {
            get { return composerId; }
            set { composerId = value; }
        }

        /// <summary>
        /// The name of the conductor
        /// </summary>
        private String conductorName = String.Empty;
        public String ConductorName
        {
            get { return conductorName; }
            set { SetProperty(ref conductorName, value, "ConductorName"); }  
        }

        /// <summary>
        /// The Database ID of the conductor
        /// </summary>
        private Int32 conductorId = 0;
        public Int32 ConductorId
        {
            get { return conductorId; }
            set { conductorId = value; }
        }

        /// <summary>
        /// Name of the Lead Performer
        /// </summary>
        private String leadPerformer = String.Empty;
        public String LeadPerformer
        {
            get { return leadPerformer; }
            set { SetProperty(ref leadPerformer, value, "LeadPerformer"); }  
        }

        /// <summary>
        /// The Database ID of the Lead Performer
        /// </summary>
        private Int32 leadPerformerId = 0;
        public Int32 LeadPerformerId
        {
            get { return leadPerformerId; }
            set { leadPerformerId = value; }
        }


        #endregion

        #region Album Information
        /// <summary>
        /// The name of the album
        /// </summary>
        private String albumName = String.Empty;
        public String AlbumName
        {
            get { return albumName; }
            set { SetProperty(ref albumName, value, "AlbumName"); }  
        }

        /// <summary>
        /// The sort name of the album
        /// </summary>
        private String albumSortName = String.Empty;
        public String AlbumSortName
        {
            get { return albumSortName; }
            set { SetProperty(ref albumSortName, value, "AlbumSortName"); } 
        }

        /// <summary>
        /// The Database ID of the album
        /// </summary>
        private Int32 albumId = 0;
        public Int32 AlbumId
        {
            get { return albumId; }
            set
            {
                if (albumId != value)
                {
                    albumId = value;
                }
            }
        }

        /// <summary>
        /// The Database ID of the AlbumGenre
        /// </summary>
        private Int32 albumGenreId = 0;
        public Int32 AlbumGenreId
        {
            get { return albumGenreId; }
            set { albumGenreId = value; }
        }

        /// <summary>
        /// The parent folder of the \Band\Album folder
        /// </summary>
        private String albumGenre = String.Empty;
        public String AlbumGenre
        {
            get { return albumGenre; }
            set { SetProperty(ref albumGenre, value, "AlbumGenre"); } 
        }


        /// <summary>
        /// SingleArtist or VariousArtist
        /// </summary>
        private ArtistType artistType;
        public ArtistType ArtistType
        {
            get { return artistType; }
            set { artistType = value; }
        }


        /// <summary>
        /// The Database ID of the country
        /// </summary>
        private Int32 countryId = 0;
        public Int32 CountryId
        {
            get { return countryId; }
            set { countryId = value; }
        }

        /// <summary>
        /// Country where the Artist/Band belongs to
        /// </summary>
        private String country = String.Empty;
        public String Country
        {
            get { return country; }
            set { SetProperty(ref country, value, "Country"); }  
        }

        /// <summary>
        /// The Database ID of the genre
        /// </summary>
        private Int32 genreId = 0;
        public Int32 GenreId
        {
            get { return genreId; }
            set { genreId = value; }
        }

        /// <summary>
        /// Genre is the style of music Rock, Pop, Jazz, Reggea, ...
        /// </summary>
        private String genre = String.Empty;
        public String Genre
        {
            get { return genre; }
            set { SetProperty(ref genre, value, "Genre"); } 
        }

        /// <summary>
        /// The Database ID of the language
        /// </summary>
        private Int32 languageId = 0;
        public Int32 LanguageId
        {
            get { return languageId; }
            set { languageId = value; }
        }

        /// <summary>
        /// Langauge the song is sung
        /// </summary>
        private String language = String.Empty;
        public String Language
        {
            get { return language; }
            set { SetProperty(ref language, value, "Language"); } 
        }

        /// <summary>
        /// The year when the album was first published
        /// </summary>
        private String year = String.Empty;
        public String Year
        {
            get { return year; }
            set { SetProperty(ref year, value, "Year"); } 
        }

        #endregion

        #region Song Information

        /// <summary>
        /// Comment of the songs
        /// </summary>
        private String comment = String.Empty;
        public String Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value, "Comment"); } 
        }

        /// <summary>
        /// Timestamp when the song was last added or updated in the database
        /// </summary>
        private DateTime dateAdded;
        public DateTime DateAdded
        {
            get { return dateAdded; }
            set { dateAdded = value; }
        }

        /// <summary>
        /// Timestamp when the song was last played
        /// </summary>
        private DateTime datePlayed;
        public DateTime DatePlayed
        {
            get { return datePlayed; }
            set { datePlayed = value; }
        }

        /// <summary>
        /// Formated String showing the duration in minutes and seconds mm:ss
        /// </summary>
        private String duration = String.Empty;
        public String Duration
        {
            get { return duration; }
            set { }
        }

        /// <summary>
        /// The duration in milli seconds
        /// </summary>
        public String MilliSeconds
        {
            get { return Convert.ToString(seconds * 1000); }
        }

        /// <summary>
        /// The duration in seconds
        /// </summary>
        private float seconds;
        public float Seconds
        {
            get { return seconds; }
            set
            {
                seconds = value;
                TimeSpan ts = TimeSpan.FromSeconds(seconds);
                if (ts.Hours > 0)
                {
                    duration = String.Format("{0:00}:{1:00}:{2:00}", (int)ts.Hours, ts.Minutes, ts.Seconds);
                }
                else
                {
                    duration = String.Format("{0:00}:{1:00}", (int)ts.Minutes, ts.Seconds);
                }

            }
        }

        /// <summary>
        /// Rating of the song in the range of 0-255
        /// </summary>
        private Int32 rating;
        public Int32 Rating
        {
            get { return rating; }
            set 
            { 
                SetProperty(ref rating, value, "Rating");
                IntegerEventArgs args = new IntegerEventArgs(value);
                OnratingChanged(this, args);
            } 
        }

        private Guid songGuid;
        public Guid SongGuid
        {
            get { return songGuid; }
            set { SetProperty(ref songGuid, value, "SongGuid"); }  
        }

        /// <summary>
        /// The title of he song
        /// </summary>
        private String songTitle = String.Empty;
        public String SongTitle
        {
            get { return songTitle; }
            set { SetProperty(ref songTitle, value, "SongTitle"); } 
        }

        /// <summary>
        /// The Database ID of the song
        /// </summary>
        private Int32 songId = 0;
        public Int32 SongId
        {
            get { return songId; }
            set { songId = value; }
        }

        /// <summary>
        /// The tracknumber of the song
        /// </summary>
        private String track = String.Empty;
        public String Track
        {
            get { return track; }
            set { SetProperty(ref track, value, "Track"); } 
        }

        #endregion

        #region File Information

        private long fileSize = 0;
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        /// <summary>
        /// The Path of the songs, without filename
        /// </summary>
        private String songPath = String.Empty;
        public String SongPath
        {
            get { return songPath; }
            set
            {
                songPath = value;
            }
        }

        /// <summary>
        /// The Filename, without Path
        /// </summary>
        private String songFilename = String.Empty;
        public String SongFilename
        {
            get { return songFilename; }
            set { songFilename = value; }
        }

        /// <summary>
        /// The Full Path of the songs
        /// </summary>
        public String SongFullPath
        {
            get { return songPath + songFilename; }
        }

        /// <summary>
        /// The Filename, without Path
        /// </summary>
        private String newFullPath = String.Empty;
        public String NewFullPath
        {
            get { return newFullPath; }
            set { SetProperty(ref newFullPath, value, "NewFullPath"); }  
        }
        #endregion

        #region Technical Information

        /// <summary>
        /// Bitrate is the bits per second used in the compressed MP3 audio file
        /// </summary>
        private Int32 bitrate;
        public Int32 Bitrate
        {
            get { return bitrate; }
            set { bitrate = value; }
        }

        /// <summary>
        /// CBR = Constant Bitrate, VBR = Variable Bitrate
        /// </summary>
        private BitrateType bitrateType = BitrateType.CBR;
        public BitrateType BitrateType
        {
            get { return bitrateType; }
            set { bitrateType = value; }
        }


        private String channelMode = String.Empty;
        public String ChannelMode
        {
            get { return channelMode; }
            set { channelMode = value; }
        }

        private String encoder;
        public String Encoder
        {
            get { return encoder; }
            set { encoder = value; }
        }

        /// <summary>
        /// The number of samples per second of decompressed audio data (typically 44100 Samples per second
        /// </summary>
        private Int32 sampleRate;
        public Int32 SampleRate
        {
            get { return sampleRate; }
            set { sampleRate = value; }
        }

        #endregion

        #region Websites
        /// <summary>
        /// Primary website of the artist or of a fan group
        /// </summary>
        private String websiteArtist = String.Empty;
        public String WebsiteArtist
        {
            get { return websiteArtist; }
            set { SetProperty(ref websiteArtist, value, "WebsiteArtist"); } 
        }

        /// <summary>
        /// Website that manages the audio file
        /// </summary>
        private String websiteAudioFile = String.Empty;
        public String WebsiteAudioFile
        {
            get { return websiteAudioFile; }
            set { SetProperty(ref websiteAudioFile, value, "WebsiteAudioFile"); } 
        }

        /// <summary>
        /// Website that manages the audio source
        /// </summary>
        private String websiteAudioSource = String.Empty;
        public String WebsiteAudioSource
        {
            get { return websiteAudioSource; }
            set { SetProperty(ref websiteAudioSource, value, "WebsiteAudioSource"); } 
        }

        /// <summary>
        /// Commercial Information Website 
        /// </summary>
        private String websiteCommercial = String.Empty;
        public String WebsiteCommercial
        {
            get { return websiteCommercial; }
            set { SetProperty(ref websiteCommercial, value, "WebsiteCommercial"); } 
        }

        /// <summary>
        /// Copyright/Legal Information Website 
        /// </summary>
        private String websiteCopyright = String.Empty;
        public String WebsiteCopyright
        {
            get { return websiteCopyright; }
            set { SetProperty(ref websiteCopyright, value, "WebsiteCopyright"); } 
        }

        /// <summary>
        /// Official Internet Radio Website 
        /// </summary>
        private String websiteInternetRadio = String.Empty;
        public String WebsiteInternetRadio
        {
            get { return websiteInternetRadio; }
            set { SetProperty(ref websiteInternetRadio, value, "WebsiteInternetRadio"); } 
        }

        /// <summary>
        /// Website that handles payment for this file
        /// </summary>
        private String websitePayment = String.Empty;
        public String WebsitePayment
        {
            get { return websitePayment; }
            set { SetProperty(ref websitePayment, value, "WebsitePayment"); } 
        }

        /// <summary>
        /// Publisher Official Website 
        /// </summary>
        private String websitePublisher = String.Empty;
        public String WebsitePublisher
        {
            get { return websitePublisher; }
            set { SetProperty(ref websitePublisher, value, "WebsitePublisher"); } 
        }

        /// <summary>
        /// User defined Website 
        /// </summary>
        private String websiteUser = String.Empty;
        public String WebsiteUser
        {
            get { return websiteUser; }
            set { SetProperty(ref websiteUser, value, "WebsiteUser"); } 
        }
        #endregion

        #region Coverimage Information
        private BitmapImage _stampImage;
        public BitmapImage StampImage
        {
            get { return _stampImage; }
            set { _stampImage = value; }
        }

        /// <summary>
        /// Path of the fontal cover sleeve
        /// </summary>
        private String frontImageFileName = String.Empty;
        public String FrontImageFileName
        {
            get { return frontImageFileName; }
            set { frontImageFileName = value; }
        }


        /// <summary>
        /// Path of the back cover sleeve
        /// </summary>
        private String backImageFileName = String.Empty;
        public String BackImageFileName
        {
            get { return backImageFileName; }
            set { backImageFileName = value; }
        }


        /// <summary>
        /// Path of the thumbnail cover sleeve
        /// </summary>
        private String stampImageFileName = String.Empty;
        public String StampImageFileName
        {
            get { return stampImageFileName; }
            set { stampImageFileName = value; }
        }

        /// <summary>
        /// Path of the fontal cover sleeve
        /// </summary>
        public String FrontImageFullpath
        {
            get
            {
                if ((String.IsNullOrEmpty(songPath) == false) && (String.IsNullOrEmpty(frontImageFileName) == false))
                {
                    return songPath + frontImageFileName;
                }
                return String.Empty;
            }
            set { }
        }


        /// <summary>
        /// Path of the back cover sleeve
        /// </summary>
        public String BackImageFullpath
        {
            get
            {
                if ((String.IsNullOrEmpty(songPath) == false) && (String.IsNullOrEmpty(backImageFileName) == false))
                {
                    return songPath + backImageFileName;
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
                if ((String.IsNullOrEmpty(songPath) == false) && (String.IsNullOrEmpty(stampImageFileName) == false))
                {
                    return songPath + stampImageFileName;
                }
                return String.Empty;
            }
            set { }
        }

        #endregion


        /// <summary>
        /// Database, DiskFile, AudioCD, WebStream
        /// </summary>
        private SourceMediaType mediaType = SourceMediaType.DiskFile;
        public SourceMediaType MediaType
        {
            get { return mediaType; }
            set { mediaType = value; }
        }


        /// <summary>
        /// Wether or not the album table needs to be updated
        /// </summary>
        private Boolean mustUpdateAlbum = false;
        public Boolean MustUpdateAlbum
        {
            get { return mustUpdateAlbum; }
            set { mustUpdateAlbum = value; }
        }

        /// <summary>
        /// Wether or not the band table needs to be updated
        /// </summary>
        private Boolean mustUpdateBand = false;
        public Boolean MustUpdateBand
        {
            get { return mustUpdateBand; }
            set { mustUpdateBand = value; }
        }


        /// <summary>
        /// User defined information
        /// </summary>
        private String userDefinedInfo = String.Empty;
        public String UserDefinedInfo
        {
            get { return userDefinedInfo; }
            set { SetProperty(ref userDefinedInfo, value, "UserDefinedInfo"); } 
        }

        private Boolean isNowPlaying;
        public Boolean IsNowPlaying
        {
            get { return isNowPlaying; }
            set { SetProperty(ref isNowPlaying, value, "IsNowPlaying"); }  
        }


        public SongItem()
        {
            songGuid =  Guid.NewGuid();
        }

        public BandItem GetBand()
        {
            BandItem band = new BandItem();
            band.BandId = this.bandId;
            band.BandName = this.bandName;
            band.SortName = this.bandSortName;
            return band;
        }

 
        /// <summary>
        /// Get the value of a property by name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>Value for the field specified by the fieldName</returns>
        public object GetValueByFieldName(String fieldName)
        {
            switch (fieldName)
            {
                case "AlbumGenreId":
                    return albumGenreId;
                case "AlbumGenre":
                    return albumGenre;
                case "AlbumId":
                    return albumId;
                case "AlbumName":
                    return albumName;
                case "AlbumSortName":
                    return albumSortName;
                case "BandId":
                    return bandId;
                case "BandName":
                    return bandName;
                case "BandSortName":
                    return bandSortName;
                case "Bitrate":
                    return bitrate;
                case "Comment":
                    return comment;
                case "ComposerId":
                    return composerId;
                case "ComposerName":
                    return composerName;
                case "ConductorId":
                    return conductorId;
                case "ConductorName":
                    return conductorName;
                case "CountryId":
                    return countryId;
                case "Country":
                    return country;
                case "Duration":
                    return duration;
                case "SongFilename":
                    return songFilename;
                case "GenreId":
                    return genreId;
                case "Genre":
                    return genre;
                case "LanguageId":
                    return languageId;
                case "Language":
                    return language;
                case "LeadPerformerId":
                    return leadPerformerId;
                case "LeadPerformer":
                    return leadPerformer;
                case "SongPath":
                    return songPath;
                case "Rating":
                    return rating;
                case "Samplerate":
                    return sampleRate;
                case "Seconds":
                    return seconds;
                case "SongId":
                    return songId;
                case "SongTitle":
                    return songTitle;
                case "Track":
                    return track;
                case "WebsiteArtist":
                    return websiteArtist;
                case "WebsiteAudioFile":
                    return websiteAudioFile;
                case "WebsiteAudioSource":
                    return websiteAudioSource;
                case "WebsiteCommercial":
                    return websiteCommercial;
                case "WebsiteCopyright":
                    return websiteCopyright;
                case "WebsiteInternetRadio":
                    return websiteInternetRadio;
                case "WebsitePayment":
                    return websitePayment;
                case "WebsitePublisher":
                    return websitePublisher;
                case "WebsiteUser":
                    return websiteUser;
                case "VA":
                    return artistType;
                case "VBR":
                    return bitrateType;
                case "Year":
                    return year;
                default:
                    return "Unsupported Fieldname";
            }
        }
        public void SetValueByFieldName(String fieldName, object newValue)
        {
            switch (fieldName)
            {
                case "AlbumGenreId":
                    AlbumGenreId = Convert.ToInt32(newValue);
                    break;
                case "AlbumGenre":
                    AlbumGenre = (String)(String)newValue;
                    albumGenreId = 0;
                    break;
                case "AlbumId":
                    AlbumId = Convert.ToInt32(newValue);
                    break;
                case "AlbumName":
                    AlbumName = (String)newValue;
                    break;
                case "AlbumSortName":
                    AlbumSortName = (String)newValue;
                    break;
                case "BandId":
                    bandId = Convert.ToInt32(newValue);
                    break;
                case "BandName":
                    BandName = (String)newValue;
                    bandId = 0;
                    break;
                case "BandSortName":
                    BandSortName = (String)newValue;
                    bandId = 0;
                    break;
                case "Bitrate":
                    Bitrate = Convert.ToInt32(newValue);
                    break;
                case "Comment":
                    Comment = (String)newValue;
                    break;
                case "ComposerId":
                    ComposerId = Convert.ToInt32(newValue);
                    break;
                case "ComposerName":
                    ComposerName = (String)newValue;
                    composerId = 0;
                    break;
                case "ConductorId":
                    ConductorId = Convert.ToInt32(newValue);
                    break;
                case "ConductorName":
                    ConductorName = (String)newValue;
                    conductorId = 0;
                    break;
                case "CountryId":
                    CountryId = Convert.ToInt32(newValue);
                    break;
                case "Country":
                    Country = (String)newValue;
                    countryId = 0;
                    break;
                case "Filename":
                    songFilename = (String)newValue;
                    break;
                case "GenreId":
                    GenreId = Convert.ToInt32(newValue);
                    break;
                case "Genre":
                    Genre = (String)newValue;
                    genreId = 0;
                    break;
                case "LanguageId":
                    LanguageId = Convert.ToInt32(newValue);
                    break;
                case "Language":
                    Language = (String)newValue;
                    languageId = 0;
                    break;
                case "LeadPerformerId":
                    LeadPerformerId = Convert.ToInt32(newValue);
                    break;
                case "LeadPerformer":
                    LeadPerformer = (String)newValue;
                    leadPerformerId = 0;
                    break;
                case "Path":
                    SongPath = (String)newValue;
                    break;
                case "Rating":
                    Rating = Convert.ToInt32(newValue);
                    break;
                case "Samplerate":
                    SampleRate = Convert.ToInt32(newValue);
                    break;
                case "Seconds":
                    Seconds = (float)Convert.ToDouble(newValue);
                    break;
                case "SongId":
                    SongId = Convert.ToInt32(newValue);
                    break;
                case "SongTitle":
                    SongTitle = (String)newValue;
                    break;
                case "Track":
                    Track = (String)newValue;
                    break;
                case "VA":
                    //artistType.ToString();
                    break;
                case "VBR":
                    //bitrateType.ToString();
                    break;
                case "WebsiteArtist":
                    WebsiteArtist = (String)newValue;
                    break;
                case "WebsiteAudioFile":
                    WebsiteAudioFile = (String)newValue;
                    break;
                case "WebsiteAudioSource":
                    WebsiteAudioSource = (String)newValue;
                    break;
                case "WebsiteCommercial":
                    WebsiteCommercial = (String)newValue;
                    break;
                case "WebsiteCopyright":
                    WebsiteCopyright = (String)newValue;
                    break;
                case "WebsiteInternetRadio":
                    WebsiteInternetRadio = (String)newValue;
                    break;
                case "WebsitePayment":
                    WebsitePayment = (String)newValue;
                    break;
                case "WebsitePublisher":
                    WebsitePublisher = (String)newValue;
                    break;
                case "WebsiteUser":
                    WebsiteUser = (String)newValue;
                    break;
                case "Year":
                    Year = (String)newValue;
                    break;

                //"Unsupported Fieldname";
            }
        }
        /// <summary>
        /// Change, add or delete an multiple ID3Tags according to information provided by the song object
        /// </summary>
        /// <param name="song"></param>
        public void Update(ChangedPropertiesList changedProperties)
        {
            for (int i = 0; i < changedProperties.Count; i++)
            {
                KeyValuePair<string, object> item = changedProperties[i];
                switch (item.Key)
                {
                    case "AlbumName":
                        albumName = (String)item.Value;
                        break;
                    case "AlbumSortName":
                        albumSortName = (String)item.Value;
                        break;
                    case "BandName":
                        bandName = (String)item.Value;
                        break;
                    case "BandSortName":
                        bandSortName = (String)item.Value;
                        break;
                    case "Comment":
                        comment = (String)item.Value;
                        break;
                    case "ComposerName":
                        albumName = (String)item.Value;
                        break;
                    case "ConductorName":
                        conductorName = (String)item.Value;
                        break;
                    case "Country":
                        country = (String)item.Value;
                        break;
                    case "Encoder":
                        encoder = (String)item.Value;
                        break;
                    case "Genre":
                        genre = (String)item.Value;
                        break;
                    case "Language":
                        language = (String)item.Value;
                        break;
                    case "LeadPerformer":
                        leadPerformer = (String)item.Value;
                        break;
                    case "Rating":
                        rating = (Int32)item.Value;
                        break;
                    case "SongTitle":
                        songTitle = (String)item.Value;
                        break;
                    case "Track":
                        track = (String)item.Value;
                        break;
                    case "Year":
                        year = (String)item.Value;
                        break;
                    case "WebsiteCommercial":
                        websiteCommercial = (String)item.Value;
                        break;
                    case "WebsiteCopyright":
                        websiteCopyright = (String)item.Value;
                        break;
                    case "WebsiteAudioFile":
                        websiteAudioFile = (String)item.Value;
                        break;
                    case "WebsiteArtist":
                        websiteArtist = (String)item.Value;
                        break;
                    case "WebsiteAudioSource":
                        websiteAudioSource = (String)item.Value;
                        break;
                    case "WebsiteInternetRadio":
                        websiteInternetRadio = (String)item.Value;
                        break;
                    case "WebsitePayment":
                        websitePayment = (String)item.Value;
                        break;
                    case "WebsitePublisher":
                        websitePublisher = (String)item.Value;
                        break;
                    case "WebsiteURL":
                        websiteUser = (String)item.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Update the database id's of a song that is just created form an ID3 V2 tag
        /// </summary>
        /// <param name="song"></param>
        public void UpdateItemIDs(SongItem song)
        {

            if ((String.IsNullOrEmpty(this.albumName) == false) && (this.albumName == song.albumName))
            {
                this.albumId = song.albumId;
            }

            if ((String.IsNullOrEmpty(this.albumGenre) == false) && (this.albumGenre == song.albumGenre))
            {
                this.albumGenreId = song.albumGenreId;
            }

            if ((String.IsNullOrEmpty(this.bandName) == false) && (this.bandName == song.bandName))
            {
                this.bandId = song.bandId;
            }

            if ((String.IsNullOrEmpty(this.composerName) == false) && (this.composerName == song.composerName))
            {
                this.composerId = song.composerId;
            }

            if ((String.IsNullOrEmpty(this.conductorName) == false) && (this.conductorName == song.conductorName))
            {
                this.conductorId = song.conductorId;
            }

            if ((String.IsNullOrEmpty(this.country) == false) && (this.country == song.country))
            {
                this.countryId = song.countryId;
            }

            if ((String.IsNullOrEmpty(this.genre) == false) && (this.genre == song.genre))
            {
                this.genreId = song.genreId;
            }

            if ((String.IsNullOrEmpty(this.language) == false) && (this.language == song.language))
            {
                this.languageId = song.languageId;
            }

            if ((String.IsNullOrEmpty(this.leadPerformer) == false) && (this.leadPerformer == song.leadPerformer))
            {
                this.leadPerformerId = song.leadPerformerId;
            }

            if ((String.IsNullOrEmpty(this.frontImageFileName) == true) && (String.IsNullOrEmpty(song.frontImageFileName) == false))
            {
                this.frontImageFileName = song.frontImageFileName;
            }

            if ((String.IsNullOrEmpty(this.backImageFileName) == true) && (String.IsNullOrEmpty(song.backImageFileName) == false))
            {
                this.backImageFileName = song.backImageFileName;
            }

            if ((String.IsNullOrEmpty(this.stampImageFileName) == true) && (String.IsNullOrEmpty(song.stampImageFileName) == false))
            {
                this.stampImageFileName = song.stampImageFileName;
            }
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


        public delegate void RatingChangedEventHandler(object sender, IntegerEventArgs e);
        public event RatingChangedEventHandler RatingChanged;
        protected virtual void OnratingChanged(object sender, IntegerEventArgs e)
        {
            if (this.RatingChanged != null)
            {
                this.RatingChanged(this, e);
            }
        }
        #endregion
    }
}
