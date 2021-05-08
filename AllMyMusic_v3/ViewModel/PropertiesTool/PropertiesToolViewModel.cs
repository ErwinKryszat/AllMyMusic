using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;


using AllMyMusic.DataService;
using AllMyMusic.Settings;
using Metadata.Mp3;
using Metadata.ID3;

namespace AllMyMusic.ViewModel
{
    public class PropertiesToolViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private IDataServiceListItems _dataServiceListItems;
        private IDataServiceCountries _dataServiceCountries;
        

        private ObservableCollection<SongItem> _songs;
        private ObservableCollection<String> _countries;
        private ObservableCollection<String> _languages;
        private ObservableCollection<String> _genres;
        
        private SongItem _song;

        private Boolean _singleSong;
        private Boolean _multipleSongs;
        private Boolean _tooltipsEnabled;
       
        private CountryItem _country;
        private ChangedPropertiesList _changedProperties;
        
        private RelayCommand<object> _bandSortNameClickedCommand;
        #endregion // Fields

        #region Commands
        public ICommand BandSortNameClickedCommand
        {
            get
            {
                if (null == _bandSortNameClickedCommand)
                    _bandSortNameClickedCommand = new RelayCommand<object>(ExecuteBandSortNameClickedCommand, CanBandSortNameClickedCommand);

                return _bandSortNameClickedCommand;
            }
        }
        private void ExecuteBandSortNameClickedCommand(object notUsed)
        {
             if (String.IsNullOrEmpty(Song.BandSortName) == true)
             {
                 Song.BandSortName = SortOrderGenerator(Song.BandName);
             }
        }
        private bool CanBandSortNameClickedCommand(object notUsed)
        {
            return true;
        }
        #endregion


        #region Presentation Properties
        public SongItem Song
        {
            get { return _song; }
            set
            {
                if ((value == _song) || (value == null))
                    return;

                _song = value;
                RaisePropertyChanged("Song");
            }
        }

        public CountryItem Country
        {
            get { return _country; }
            set
            {
                if ((value == _country) || (value == null))
                    return;

                _country = value;
                RaisePropertyChanged("Country");
            }
        }

        public Boolean SingleSong
        {
            get { return _singleSong; }
            set
            {
                if (value == _singleSong)
                    return;

                _singleSong = value;

                RaisePropertyChanged("SingleSong");
            }
        }
        public Boolean MultipleSongs
        {
            get { return _multipleSongs; }
            set
            {
                if (value == _multipleSongs)
                    return;

                _multipleSongs = value;

                RaisePropertyChanged("MultipleSongs");
            }
        }
        public Boolean TooltipsEnabled
        {
            get { return _tooltipsEnabled; }
            set
            {
                if (value == _tooltipsEnabled)
                    return;

                _tooltipsEnabled = value;

                RaisePropertyChanged("TooltipsEnabled");
            }
        }
        public ChangedPropertiesList ChangedProperties
        {
            get { return _changedProperties; }
            set
            {
                if (value == _changedProperties)
                    return;

                _changedProperties = value;

                RaisePropertyChanged("ChangedProperties");
            }
        }
        public ObservableCollection<String> Countries
        {
            get { return _countries; }
            set
            {
                if (value == _countries)
                    return;

                _countries = value;

                RaisePropertyChanged("Countries");
            }
        }
        public ObservableCollection<String> Languages
        {
            get { return _languages; }
            set
            {
                if (value == _languages)
                    return;

                _languages = value;

                RaisePropertyChanged("Languages");
            }
        }
        public ObservableCollection<String> Genres
        {
            get { return _genres; }
            set
            {
                if (value == _genres)
                    return;

                _genres = value;

                RaisePropertyChanged("Genres");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public PropertiesToolViewModel()
        {          
       
        }

        public PropertiesToolViewModel(ConnectionInfo conInfo, ObservableCollection<SongItem> songs)
        {
            _songs = songs;


            if (_songs.Count > 1)
            {
                GetCommonProperties();
                SingleSong = false;
                MultipleSongs = true;
            }
            if (_songs.Count == 1)
            {
                _song = songs[0];
                SingleSong = true;

                Mp3Metaedit mp3Metaedit = new Mp3Metaedit(_song.SongFullPath);
                _song = mp3Metaedit.ReadMetadata();
            }

            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.SqlServer)
            {
                _dataServiceListItems = new DataServiceListItems_SQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_SQL(conInfo);
            }
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.MySql)
            {
                _dataServiceListItems = new DataServiceListItems_MYSQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_MYSQL(conInfo);
            }


            _changedProperties = new ChangedPropertiesList();
            _song.PropertyChanged += new PropertyChangedEventHandler(Song_PropertyChanged);
            GetCountry();

            Task.Run(() => InitializeListItems());

            
            Localize();
        }

        public PropertiesToolViewModel(ConnectionInfo conInfo, SongItem song)
        {
            _song = song;
            SingleSong = true;
            MultipleSongs = false;

            _songs = new ObservableCollection<SongItem>();
            _songs.Add(_song);
            

            Mp3Metaedit mp3Metaedit = new Mp3Metaedit(_song.SongFullPath);
            _song = mp3Metaedit.ReadMetadata();

            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceListItems = new DataServiceListItems_SQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceListItems = new DataServiceListItems_MYSQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_MYSQL(conInfo);
            }
            
            _changedProperties = new ChangedPropertiesList();
            _song.PropertyChanged += new PropertyChangedEventHandler(Song_PropertyChanged);
            GetCountry();

            Task.Run(() => InitializeListItems());
            
            Localize();
        }

        public void Close()
        {
            if (_dataServiceListItems != null)
            {
                _dataServiceListItems.Close();
            }

             if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Close();
            }
        }
        #endregion  // Constructor


        #region private
        private async Task InitializeListItems()
        {
            _countries = await _dataServiceListItems.GetCountries();
            _languages = await _dataServiceListItems.GetLanguages();
            _genres = await _dataServiceListItems.GetGenres();
        }
        private void GetCommonProperties()
        {
            _song = new SongItem();
            _song.AlbumGenre = GetCommonProperty("AlbumGenre");
            _song.AlbumName = GetCommonProperty("AlbumName");
            _song.AlbumSortName = GetCommonProperty("AlbumSortName");
            _song.BandName = GetCommonProperty("BandName");
            _song.BandSortName = GetCommonProperty("BandSortName");
            _song.Comment = GetCommonProperty("Comment");
            _song.ComposerName = GetCommonProperty("ComposerName");
            _song.ConductorName = GetCommonProperty("ConductorName");
            _song.Country = GetCommonProperty("Country");
            _song.Genre = GetCommonProperty("Genre");
            _song.Language = GetCommonProperty("Language");
            _song.LeadPerformer = GetCommonProperty("LeadPerformer");
            _song.SongFilename = GetCommonProperty("SongFilename");
            _song.SongPath = GetCommonProperty("SongPath");
            _song.SongTitle = GetCommonProperty("SongTitle");
            _song.Track = GetCommonProperty("Track");
            _song.Year = GetCommonProperty("Year");

            _song.Rating = GetCommonPropertyInt("Rating");

            _song.Seconds = GetAverage("Seconds");
            _song.Bitrate = GetAverage("Bitrate");
            _song.SampleRate = GetAverage("Samplerate");

            _song.ArtistType = _songs[0].ArtistType;
        }
        private String GetCommonProperty(String fieldName)
        {
            String compareValue = _songs[0].GetValueByFieldName(fieldName).ToString();
            if (String.IsNullOrEmpty(compareValue) == false)
            {
                foreach (SongItem song in _songs)
                {
                    String itemValue = song.GetValueByFieldName(fieldName).ToString();
                    if (itemValue != compareValue)
                    {
                        return String.Empty;
                    }
                }
                return compareValue.Trim();
            }
            return String.Empty;
        }
        private Int32 GetCommonPropertyInt(String fieldName)
        {
            Int32 compareValue = Convert.ToInt32(_songs[0].GetValueByFieldName(fieldName));
            foreach (SongItem song in _songs)
            {
                Int32 itemValue = Convert.ToInt32(song.GetValueByFieldName(fieldName));
                if (itemValue != compareValue)
                {
                    return 0;
                }
            }
            return compareValue;
        }
        private Int32 GetAverage(String fieldName)
        {
            Int32 averageValue = 0;
            foreach (SongItem song in _songs)
            {
                averageValue += Convert.ToInt32(song.GetValueByFieldName(fieldName));
            }
            if (_songs.Count > 0)
            {
                averageValue = averageValue / _songs.Count;
            }
            return averageValue;
        }
        private void Song_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SongItem song = (SongItem)sender;

            object propertyValue = song.GetValueByFieldName(e.PropertyName);
            _changedProperties.Add(e.PropertyName, propertyValue);

            UpdateSongTable(e.PropertyName, propertyValue);

            if (e.PropertyName == "Country")
            {
                GetCountry();
            }
        }
        private void UpdateSongTable(String propertyName, object value)
        {
            for (int i = 0; i < _songs.Count; i++)
            {
                SongItem song = _songs[i];
                song.SetValueByFieldName(propertyName, value);
            }
        }
        private async void GetCountry()
        {
            if (String.IsNullOrEmpty(_song.Country) == false)
            {
                _country = await _dataServiceCountries.GetCountry(_song.Country);
            }
        }
        private String SortOrderGenerator(String itemName)
        {
            String sortOrderName = String.Empty;
            String[] tmpNames = itemName.Split(' ');

            if (tmpNames.Length == 2)
            {
                sortOrderName = tmpNames[1] + ", " + tmpNames[0];
            }

            if (tmpNames.Length > 2)
            {
                switch (tmpNames[0].ToLower())
                {
                    case "the":
                    case "der":
                    case "die":
                    case "das":
                    case "le":
                    case "la":
                        Int32 posSpace = itemName.IndexOf(' ');
                        if (posSpace > 0)
                        {
                            sortOrderName = itemName.Substring(posSpace + 1, itemName.Length - posSpace - 1) + ", " + tmpNames[0];
                        }
                        break;
                    default:
                        sortOrderName = itemName;
                        break;
                }
            }

            return sortOrderName;
        }  
        #endregion

        #region Localization
        private String _contributorsLabel = "Contributer Information";
        private String _album_InfoLabel = "Album Information";
        private String _song_InfoLabel = "Song Information";
        private String _technical_InfoLabel = "Technical Information";
        private String _website_InfoLabel = "Website Information";

        private String _albumLabel = "Album";
        private String _albumGenreLabel = "Album Genre";
        private String _artistTypeLabel = "Artist Type";
        private String _bandLabel = "Band / Artist";
        private String _bandSortLabel = "Band Sortname";
        private String _bitrateLabel = "Bitrate";
        private String _bitrateTypeLabel = "Bitrate Type";
        private String _channelsLabel = "Channels";
        private String _commentLabel = "Comment";
        private String _composerLabel = "Composer";
        private String _conductorLabel = "Conductor";
        private String _countryLabel = "Country";
        private String _filenameLabel = "Filename";
        private String _filesizeLabel = "Filesize";
        private String _genreLabel = "Genre";
        private String _languageLabel = "Language";
        private String _leadPerformerLabel = "Lead Performer";
        private String _lengthLabel = "Length";
        private String _pathLabel = "Path";
        private String _ratingLabel = "Rating";
        private String _sampleRateLabel = "Samplerate";
        private String _songTitleLabel = "Title";
        private String _trackLabel = "Track";
        private String _yearLabel = "Year";

        private String _website_UserLabel = "Website User";
        private String _website_ArtistLabel = "Website Artist";

        private String _title = "Poperties";
        private String _titleTip = "This dialog helps you changing the properties for all selected songs";
        private String _propertiesToolTip = "Edit the fields that you want to change. The table will preview the changes. Press OK to apply changes to all files listed in the table";

        public String ContributorsLabel
        {
            get { return _contributorsLabel; }
            set
            {
                if (value == _contributorsLabel)
                    return;

                _contributorsLabel = value;

                RaisePropertyChanged("ContributorsLabel");
            }
        }
        public String Album_InfoLabel
        {
            get { return _album_InfoLabel; }
            set
            {
                if (value == _album_InfoLabel)
                    return;

                _album_InfoLabel = value;

                RaisePropertyChanged("Album_InfoLabel");
            }
        }
        public String Song_InfoLabel
        {
            get { return _song_InfoLabel; }
            set
            {
                if (value == _song_InfoLabel)
                    return;

                _song_InfoLabel = value;

                RaisePropertyChanged("Song_InfoLabel");
            }
        }
        public String Technical_InfoLabel
        {
            get { return _technical_InfoLabel; }
            set
            {
                if (value == _technical_InfoLabel)
                    return;

                _technical_InfoLabel = value;

                RaisePropertyChanged("Technical_InfoLabel");
            }
        }
        public String Website_InfoLabel
        {
            get { return _website_InfoLabel; }
            set
            {
                if (value == _website_InfoLabel)
                    return;

                _website_InfoLabel = value;

                RaisePropertyChanged("Website_InfoLabel");
            }
        }

        public String AlbumLabel
        {
            get { return _albumLabel; }
            set
            {
                if (value == _albumLabel)
                    return;

                _albumLabel = value;

                RaisePropertyChanged("AlbumLabel");
            }
        }
        public String AlbumGenreLabel
        {
            get { return _albumGenreLabel; }
            set
            {
                if (value == _albumGenreLabel)
                    return;

                _albumGenreLabel = value;

                RaisePropertyChanged("AlbumGenre");
            }
        }
        public String ArtistTypeLabel
        {
            get { return _artistTypeLabel; }
            set
            {
                if (value == _artistTypeLabel)
                    return;

                _artistTypeLabel = value;

                RaisePropertyChanged("ArtistTypeLabel");
            }
        }
        public String BandLabel
        {
            get { return _bandLabel; }
            set
            {
                if (value == _bandLabel)
                    return;

                _bandLabel = value;

                RaisePropertyChanged("Band");
            }
        }
        public String BandSortLabel
        {
            get { return _bandSortLabel; }
            set
            {
                if (value == _bandSortLabel)
                    return;

                _bandSortLabel = value;

                RaisePropertyChanged("BandSort");
            }
        }
        public String BitrateLabel
        {
            get { return _bitrateLabel; }
            set
            {
                if (value == _bitrateLabel)
                    return;

                _bitrateLabel = value;

                RaisePropertyChanged("Bitrate");
            }
        }
        public String BitrateTypeLabel
        {
            get { return _bitrateTypeLabel; }
            set
            {
                if (value == _bitrateTypeLabel)
                    return;

                _bitrateTypeLabel = value;

                RaisePropertyChanged("BitrateTypeLabel");
            }
        }
        public String ChannelsLabel
        {
            get { return _channelsLabel; }
            set
            {
                if (value == _channelsLabel)
                    return;

                _channelsLabel = value;

                RaisePropertyChanged("ChannelsLabel");
            }
        }
        public String ComposerLabel
        {
            get { return _composerLabel; }
            set
            {
                if (value == _composerLabel)
                    return;

                _composerLabel = value;

                RaisePropertyChanged("Composer");
            }
        }
        public String CommentLabel
        {
            get { return _commentLabel; }
            set
            {
                if (value == _commentLabel)
                    return;

                _commentLabel = value;

                RaisePropertyChanged("CommentLabel");
            }
        }
        public String ConductorLabel
        {
            get { return _conductorLabel; }
            set
            {
                if (value == _conductorLabel)
                    return;

                _conductorLabel = value;

                RaisePropertyChanged("Conductor");
            }
        }
        public String CountryLabel
        {
            get { return _countryLabel; }
            set
            {
                if (value == _countryLabel)
                    return;

                _countryLabel = value;

                RaisePropertyChanged("CountryName");
            }
        }
        public String FilenameLabel
        {
            get { return _filenameLabel; }
            set
            {
                if (value == _filenameLabel)
                    return;

                _filenameLabel = value;

                RaisePropertyChanged("Filename");
            }
        }
        public String FileSizeLabel
        {
            get { return _filesizeLabel; }
            set
            {
                if (value == _filesizeLabel)
                    return;

                _filesizeLabel = value;

                RaisePropertyChanged("FileSizeLabel");
            }
        }
        public String GenreLabel
        {
            get { return _genreLabel; }
            set
            {
                if (value == _genreLabel)
                    return;

                _genreLabel = value;

                RaisePropertyChanged("Genre");
            }
        }
        public String LanguageLabel
        {
            get { return _languageLabel; }
            set
            {
                if (value == _languageLabel)
                    return;

                _languageLabel = value;

                RaisePropertyChanged("Language");
            }
        }
        public String LeadPerformerLabel
        {
            get { return _leadPerformerLabel; }
            set
            {
                if (value == _leadPerformerLabel)
                    return;

                _leadPerformerLabel = value;

                RaisePropertyChanged("LeadPerformer");
            }
        }
        public String LengthLabel
        {
            get { return _lengthLabel; }
            set
            {
                if (value == _lengthLabel)
                    return;

                _lengthLabel = value;

                RaisePropertyChanged("Length");
            }
        }
        public String PathLabel
        {
            get { return _pathLabel; }
            set
            {
                if (value == _pathLabel)
                    return;

                _pathLabel = value;

                RaisePropertyChanged("Path");
            }
        }
        public String RatingLabel
        {
            get { return _ratingLabel; }
            set
            {
                if (value == _ratingLabel)
                    return;

                _ratingLabel = value;

                RaisePropertyChanged("RatingValue");
            }
        }
        public String SampleRateLabel
        {
            get { return _sampleRateLabel; }
            set
            {
                if (value == _sampleRateLabel)
                    return;

                _sampleRateLabel = value;

                RaisePropertyChanged("SampleRate");
            }
        }
        public String SongTitleLabel
        {
            get { return _songTitleLabel; }
            set
            {
                if (value == _songTitleLabel)
                    return;

                _songTitleLabel = value;

                RaisePropertyChanged("SongTitle");
            }
        }
        public String TrackLabel
        {
            get { return _trackLabel; }
            set
            {
                if (value == _trackLabel)
                    return;

                _trackLabel = value;

                RaisePropertyChanged("Track");
            }
        }
        public String Website_UserLabel
        {
            get { return _website_UserLabel; }
            set
            {
                if (value == _website_UserLabel)
                    return;

                _website_UserLabel = value;

                RaisePropertyChanged("Website_UserLabel");
            }
        }
        public String Website_ArtistLabel
        {
            get { return _website_ArtistLabel; }
            set
            {
                if (value == _website_ArtistLabel)
                    return;

                _website_ArtistLabel = value;

                RaisePropertyChanged("Website_ArtistLabel");
            }
        }
        public String YearLabel
        {
            get { return _yearLabel; }
            set
            {
                if (value == _yearLabel)
                    return;

                _yearLabel = value;

                RaisePropertyChanged("Year");
            }
        }

        public String Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                    return;

                _title = value;

                RaisePropertyChanged("Title");
            }
        }
        public String TitleTip
        {
            get { return _titleTip; }
            set
            {
                if (value == _titleTip)
                    return;

                _titleTip = value;

                RaisePropertyChanged("TitleTip");
            }
        }
        public String PropertiesToolTip
        {
            get { return _propertiesToolTip; }
            set
            {
                if (value == _propertiesToolTip)
                    return;

                _propertiesToolTip = value;

                RaisePropertyChanged("PropertiesToolTip");
            }
        }

        public void Localize()
        {
            Title = AmmLocalization.GetLocalizedString("frmProperties_Title");
            TitleTip = AmmLocalization.GetLocalizedString("frmProperties_TitleTip");
            PropertiesToolTip = AmmLocalization.GetLocalizedString("frmTools_PropertiesToolTip");
            ContributorsLabel = AmmLocalization.GetLocalizedString("frmProperties_Contributors");
            Album_InfoLabel = AmmLocalization.GetLocalizedString("frmProperties_Album_Info");
            Song_InfoLabel = AmmLocalization.GetLocalizedString("frmProperties_Song_Info");
            Technical_InfoLabel = AmmLocalization.GetLocalizedString("frmProperties_Technical_Info");
            Website_InfoLabel = AmmLocalization.GetLocalizedString("frmProperties_Websites");

            AlbumLabel = AmmLocalization.GetLocalizedString("Col_Album");
            AlbumGenreLabel = AmmLocalization.GetLocalizedString("Col_AlbumGenre");
            ArtistTypeLabel = AmmLocalization.GetLocalizedString("Col_ArtistType");
            BandLabel = AmmLocalization.GetLocalizedString("Col_Band");
            BandSortLabel = AmmLocalization.GetLocalizedString("Col_BandSort");
            BitrateLabel = AmmLocalization.GetLocalizedString("Col_Bitrate");
            BitrateTypeLabel = AmmLocalization.GetLocalizedString("Col_BitrateType");
            ChannelsLabel = AmmLocalization.GetLocalizedString("Col_Channels");
            CommentLabel = AmmLocalization.GetLocalizedString("Col_Comment");
            ComposerLabel = AmmLocalization.GetLocalizedString("Col_Composer");
            ConductorLabel = AmmLocalization.GetLocalizedString("Col_Conductor");
            CountryLabel = AmmLocalization.GetLocalizedString("Col_Country");
            FilenameLabel = AmmLocalization.GetLocalizedString("Col_Filename");
            FileSizeLabel = AmmLocalization.GetLocalizedString("Col_Filesize");
            GenreLabel = AmmLocalization.GetLocalizedString("Col_Genre");
            LanguageLabel = AmmLocalization.GetLocalizedString("Col_Language");
            LeadPerformerLabel = AmmLocalization.GetLocalizedString("Col_LeadPerformer");
            LengthLabel = AmmLocalization.GetLocalizedString("Col_Length");
            PathLabel = AmmLocalization.GetLocalizedString("Col_Path");
            RatingLabel = AmmLocalization.GetLocalizedString("Col_RatingValue");
            SampleRateLabel = AmmLocalization.GetLocalizedString("Col_SampleRate");
            SongTitleLabel = AmmLocalization.GetLocalizedString("Col_Title");
            TrackLabel = AmmLocalization.GetLocalizedString("Col_Track");
            Website_UserLabel = AmmLocalization.GetLocalizedString("Col_Website_User");
            Website_ArtistLabel = AmmLocalization.GetLocalizedString("Col_Website_Artist");
            YearLabel = AmmLocalization.GetLocalizedString("Col_Year");
        }
        #endregion // Localization

        #region IDisposable Members
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();
        }

        ~PropertiesToolViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_dataServiceListItems != null)
            {
                _dataServiceListItems.Dispose();
            }

            if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
