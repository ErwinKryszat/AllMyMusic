
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing;

using AllMyMusic_v3.DataService;
using Metadata.Mp3;

namespace AllMyMusic_v3.ViewModel
{
    public class AlbumViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private static readonly object _lockThis = new object();

        private ConnectionInfo _conInfo;
        private IDataServiceAlbums _dataServiceAlbums;
        private IDataServiceCountries _dataServiceCountries;
        private IDataServiceSongs _dataServiceSongs;

        private SongsViewModel _songsViewModel;
        private MessageBoxViewModel _messageBoxViewModel;
        private CountrySelectorViewModel _countrySelectorViewModel;

        private AlbumItem _album;
        private AlbumDetails _albumDetails;
        private CountryItem _country;
        private SongItem _editSong;
        private ObservableCollection<SongItem> _songs;

        private double _countryFlagHeight;
        private double _sleeveImageSize;
        private double _stampImageSize;
        
        private String _headlineAlbum;
        private String _wikipediaLanguage;

        private RelayCommand<object> _manageImagesCommand;
        private RelayCommand<object> _coverImageCommand;
        private RelayCommand<object> _countryImageCommand;
        private RelayCommand<object> _playNowCommand;
        private RelayCommand<object> _playNextCommand;
        private RelayCommand<object> _playLastCommand;
        private RelayCommand<object> _toolsCommand;
        private RelayCommand<object> _deleteCommand;
        private RelayCommand<object> _wikipediaCommand;

        #endregion // Fields

        #region Commands    
        public ICommand ManageImagesCommand
        {
            get
            {
                if (null == _manageImagesCommand)
                    _manageImagesCommand = new RelayCommand<object>(ExecuteManageImagesCommand, CanManageImagesCommand);

                return _manageImagesCommand;
            }
        }
        private async void ExecuteManageImagesCommand(object notUsed)
        {
            CoverImageTools coverTools = new CoverImageTools(_conInfo);
            await coverTools.ManageImages(_album.AlbumPath);

            Album = await _dataServiceAlbums.GetAlbum(_album.AlbumId);

            AlbumItemEventArgs args = new AlbumItemEventArgs(_album);
            CoverImageChanged(this, args);
        }
        private bool CanManageImagesCommand(object notUsed)
        {
            return (_album != null);
        }

        public ICommand CoverImageCommand
        {
            get
            {
                if (null == _coverImageCommand)
                    _coverImageCommand = new RelayCommand<object>(ExecuteCoverImageCommand, CanCoverImageCommand);

                return _coverImageCommand;
            }
        }
        private void ExecuteCoverImageCommand(object notUsed)
        {
            frmCoverImage frmCover = new frmCoverImage(_album);
            frmCover.Show();
        }
        private bool CanCoverImageCommand(object notUsed)
        {
            if ((_album != null) && (String.IsNullOrEmpty(_album.FrontImageFileName) == false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ICommand CountryImageCommand
        {
            get
            {
                if (null == _countryImageCommand)
                    _countryImageCommand = new RelayCommand<object>(ExecuteCountryImageCommand, CanCountryImageCommand);

                return _countryImageCommand;
            }
        }
        private void ExecuteCountryImageCommand(object notUsed)
        {
            _countrySelectorViewModel = new CountrySelectorViewModel(_country);

            frmCountryImageSelector frmCountry = new frmCountryImageSelector(_countrySelectorViewModel);
            frmCountry.ShowDialog();

            if ((frmCountry.DialogResult.HasValue == true) && (frmCountry.DialogResult == true))
            {
                CountryItem selectedCountry = _countrySelectorViewModel.SelectedCountry;
                if (File.Exists(selectedCountry.FlagPath) == true) 
                {
                    _dataServiceCountries.AddCountry(selectedCountry);
                }
            }
        }
        private bool CanCountryImageCommand(object notUsed)
        {
            if ((_album != null) && (String.IsNullOrEmpty(_album.FrontImageFileName) == false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ICommand PlayNowCommand
        {
            get
            {
                if (null == _playNowCommand)
                    _playNowCommand = new RelayCommand<object>(ExecutePlayNowCommand, CanPlayNowCommand);

                return _playNowCommand;
            }
        }
        private void ExecutePlayNowCommand(object notUsed)
        {
            ObservableCollection<SongItem> songs = _songsViewModel.GetSelectedOrAllSongs();
            PlaylistEventArgs args = new PlaylistEventArgs(songs, QueueMode.Replace, _album.AlbumName, false);
            OnPlaylistChanged(this, args);
        }
        private bool CanPlayNowCommand(object notUsed)
        {
            return (_album != null);
        }

        public ICommand PlayNextCommand
        {
            get
            {
                if (null == _playNextCommand)
                    _playNextCommand = new RelayCommand<object>(ExecutePlayNextCommand, CanPlayNextCommand);

                return _playNextCommand;
            }
        }
        private void ExecutePlayNextCommand(object notUsed)
        {
            ObservableCollection<SongItem> songs = _songsViewModel.GetSelectedOrAllSongs();
            PlaylistEventArgs args = new PlaylistEventArgs(songs, QueueMode.Insert, "", false);
            OnPlaylistChanged(this, args);
        }
        private bool CanPlayNextCommand(object notUsed)
        {
            return (_album != null);
        }

        public ICommand PlayLastCommand
        {
            get
            {
                if (null == _playLastCommand)
                    _playLastCommand = new RelayCommand<object>(ExecutePlayLastCommand, CanPlayLastCommand);

                return _playLastCommand;
            }
        }
        private void ExecutePlayLastCommand(object notUsed)
        {
            ObservableCollection<SongItem> songs = _songsViewModel.GetSelectedOrAllSongs();
            PlaylistEventArgs args = new PlaylistEventArgs(songs, QueueMode.Append, "", false);
            OnPlaylistChanged(this, args);
        }
        private bool CanPlayLastCommand(object notUsed)
        {
            return (_album != null);
        }

        public ICommand ToolsCommand
        {
            get
            {
                if (null == _toolsCommand)
                    _toolsCommand = new RelayCommand<object>(ExecuteToolsCommand, CanToolsCommand);

                return _toolsCommand;
            }
        }
        private void ExecuteToolsCommand(object notUsed)
        {
            Tools(); 
        }
        private bool CanToolsCommand(object notUsed)
        {
            return (_album != null);
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (null == _deleteCommand)
                    _deleteCommand = new RelayCommand<object>(ExecuteDeleteCommand, CanDeleteCommand);

                return _deleteCommand;
            }
        }
        private void ExecuteDeleteCommand(object notUsed)
        {
            _messageBoxViewModel.MessageText = "Are you sure that you want to delete this album?";
            _messageBoxViewModel.OkAction = new Action(DeleteAlbum);
            _messageBoxViewModel.MessageBoxVisible = true;
        }
        private bool CanDeleteCommand(object notUsed)
        {
            return (_album != null);
        }
        private void DeleteAlbum()
        {
            _dataServiceAlbums.DeleteAlbum(_album);
            Album = null;
        }

        public ICommand WikipediaCommand
        {
            get
            {
                if (null == _wikipediaCommand)
                    _wikipediaCommand = new RelayCommand<object>(ExecuteWikipediaCommand, CanWikipediaCommand);

                return _wikipediaCommand;
            }
        }
        private void ExecuteWikipediaCommand(object notUsed)
        {
            String webLink = WikipediaLink(_album.BandName);
            Process.Start(webLink);
        }
        private bool CanWikipediaCommand(object notUsed)
        {
            return ((_album != null) && (_album.ArtistType == ArtistType.SingleArtist) && (String.IsNullOrEmpty(_album.BandName) == false));
        }
        #endregion // Command

        #region Presentation Properties
        public AlbumItem Album
        {
            get { return _album; }
            set
            {
                if (value == _album)
                    return;

                _album = value;

                OnAlbumChanged();
             
                RaisePropertyChanged("Album");
            }
        }
        public SongsViewModel SongsViewModel
        {
            get { return _songsViewModel; }
            set
            {
                if (value == _songsViewModel)
                    return;

                _songsViewModel = value;

                RaisePropertyChanged("SongsViewModel");
            }
        }
        public MessageBoxViewModel MessageBoxViewModel
        {
            get { return _messageBoxViewModel; }
            set
            {
                if (value == _messageBoxViewModel)
                    return;

                _messageBoxViewModel = value;

                RaisePropertyChanged("MessageBoxViewModel");
            }
        }
        public AlbumDetails AlbumDetails
        {
            get { return _albumDetails; }
            set
            {
                if (value == _albumDetails)
                    return;

                _albumDetails = value;

                RaisePropertyChanged("AlbumDetails");
            }
        }
        public CountryItem Country
        {
            get { return _country; }
            set
            {
                if (value == _country)
                    return;

                _country = value;

                RaisePropertyChanged("Country");
            }
        }
        public double CountryFlagHeight
        {
            get { return _countryFlagHeight; }
            set 
            {
                if (value == _countryFlagHeight)
                    return;

                _countryFlagHeight = value;
                RaisePropertyChanged("CountryFlagHeight");
            }
        }
        public SongItem EditSong
        {
            get { return _editSong; }
            set
            {
                _editSong = value;
                RaisePropertyChanged("EditSong");

                Task.Run(() => OnEditSong());
            }
        }

        public double StampImageSize
        {
            get { return _stampImageSize; }
            set 
            {
                 if (value == _stampImageSize)
                    return;

                 _stampImageSize = value;
                RaisePropertyChanged("StampImageSize");
            }
        }
        public double SleeveImageSize
        {
            get { return _sleeveImageSize; }
            set 
            {
                if (value == _sleeveImageSize)
                    return;

                _sleeveImageSize = value;
                RaisePropertyChanged("SleeveImageSize");
            }
        }
        public String HeadlineAlbum
        {
            get { return _headlineAlbum; }
            set
            {
                if (value == _headlineAlbum)
                    return;

                _headlineAlbum = value;
                RaisePropertyChanged("HeadlineAlbum");
            }
        }
        public String WikipediaLanguage
        {
            get { return _wikipediaLanguage; }
            set
            {
                if (value == _wikipediaLanguage)
                    return;

                _wikipediaLanguage = value;
                RaisePropertyChanged("WikipediaLanguage");
            }
        }
       
        #endregion // Presentation Properties

        #region Constructor
        public AlbumViewModel(ConnectionInfo conInfo)
        {
            _conInfo = conInfo;
            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceAlbums = new DataServiceAlbums_SQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_SQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceAlbums = new DataServiceAlbums_MYSQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_MYSQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }
            
            _messageBoxViewModel = new MessageBoxViewModel();
            _songsViewModel = new SongsViewModel(conInfo);
            Localize();
        }
        #endregion  // Constructor


        #region public
        public ObservableCollection<SongItem> GetSongs()
        {
            ObservableCollection<SongItem> _songs = new ObservableCollection<SongItem>();
            return _songs;
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            ClearView();
            _conInfo = conInfo;
            _dataServiceAlbums.ChangeDatabase(conInfo);
            _dataServiceCountries.ChangeDatabase(conInfo);
            _dataServiceSongs.ChangeDatabase(conInfo);
            _songsViewModel.ChangeDatabase(conInfo);
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            ClearView();
            _conInfo = conInfo;
            _songsViewModel.ChangeDatabaseService(conInfo);

            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Dispose();
            }
            if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Dispose();
            }

            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceAlbums = new DataServiceAlbums_SQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_SQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceAlbums = new DataServiceAlbums_MYSQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_MYSQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }
        }
        public void Close()
        {
            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Close();
            }

            if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Close();
            }
        }
        public async Task RefreshView()
        {
            if (Album != null)
            {
                Album = await _dataServiceAlbums.GetAlbum(Album.AlbumId);
            }
        }
        public void ClearView()
        {
            Album = null;
        }
        public void AskOverwritePlaylist()
        {
            MessageBoxViewModel = new MessageBoxViewModel(MessageBoxButtons.OK_Cancel);
            _messageBoxViewModel.MessageText = "Replace the actual Playlist?";
            _messageBoxViewModel.OkAction = new Action(ReplacePlaylist);
            _messageBoxViewModel.MessageBoxVisible = true;
        }
        public void NotifyFileAccessDenied(String fullFileName)
        {
            MessageBoxViewModel = new MessageBoxViewModel(MessageBoxButtons.OK_only);
            _messageBoxViewModel.MessageText = "Write Access Denied: " + fullFileName;
            _messageBoxViewModel.OkAction = new Action(DoNothing);
            _messageBoxViewModel.MessageBoxVisible = true;
        }
        #endregion  // public

        #region private helper
        private void DoNothing()
        { 
        
        }
        private void OnAlbumChanged()
        {
           var task1 = Task.Run(async () => {
                if (_album != null)
                {
                    await SongsViewModel.GetSongs(_album);

                    if (_songsViewModel.Songs.Count > 0)
                    {
                        _country = await _dataServiceCountries.GetCountry(_songsViewModel.Songs[0].CountryId);
                    }
                    await DefineAlbumDetails();
                    RaisePropertyChanged("Country");
                    RaisePropertyChanged("CountryFlagHeight");
                }
                else
                {
                    SongsViewModel.Songs = new ObservableCollection<SongItem>();
                }

                AlbumItemEventArgs args = new AlbumItemEventArgs(_album);
                OnAlbumChanged(this, args);
            });
        }
        private async Task OnEditSong()
        {
            Mp3Metaedit metaData = new Mp3Metaedit(_editSong.SongFullPath);
            metaData.UpdateMetadata(_editSong);

            await _dataServiceSongs.AddSong(_editSong);
        }
        private async Task DefineAlbumDetails()
        {
            await Task.Run(() =>
            {
                double samplingRate = 0;
                Int32 bitrate = 0;
                float seconds = 0;
                foreach (SongItem song in _songsViewModel.Songs)
                {
                    samplingRate += song.SampleRate;
                    bitrate += song.Bitrate;
                    seconds += song.Seconds;
                }

                _albumDetails = new AlbumDetails(_album);
                _albumDetails.SongCount = _songsViewModel.Songs.Count;
                if (_songsViewModel.Songs.Count > 0)
                {
                    _albumDetails.SamplingRate = samplingRate / _songsViewModel.Songs.Count / 1000;
                    _albumDetails.Bitrate = bitrate / _songsViewModel.Songs.Count / 1000;
                    _albumDetails.WebsiteArtist = _songsViewModel.Songs[0].WebsiteArtist;
                    _albumDetails.WebsiteUser = _songsViewModel.Songs[0].WebsiteUser;
                }
                RaisePropertyChanged("AlbumDetails");
            });
        }
        private String WikipediaLink(String BandName)
        {
            StringBuilder wiki = new StringBuilder();
            wiki.Append("http://");

            LanguagesWikipedia LanguageWikipedia = (LanguagesWikipedia)Enum.Parse(typeof(LanguagesWikipedia), _wikipediaLanguage);

            switch (LanguageWikipedia)
            {
                case LanguagesWikipedia.Dutch:
                    wiki.Append("nl");
                    break;
                case LanguagesWikipedia.English:
                    wiki.Append("en");
                    break;
                case LanguagesWikipedia.French:
                    wiki.Append("fr");
                    break;
                case LanguagesWikipedia.German:
                    wiki.Append("de");
                    break;
                case LanguagesWikipedia.Italian:
                    wiki.Append("it");
                    break;
                case LanguagesWikipedia.Polish:
                    wiki.Append("pl");
                    break;
                case LanguagesWikipedia.Russian:
                    wiki.Append("ru");
                    break;
                case LanguagesWikipedia.Spanish:
                    wiki.Append("es");
                    break;
                default:
                    wiki.Append("en");
                    break;
            }

            wiki.Append(".wikipedia.org/wiki/");
            String WikiBandName = BandName.Replace(' ', '_');
            wiki.Append(WikiBandName);
            return wiki.ToString();
        }
        private void ReplacePlaylist()
        {
            ObservableCollection<SongItem> songs = _songsViewModel.GetSelectedOrAllSongs();
            PlaylistEventArgs args = new PlaylistEventArgs(songs, QueueMode.Replace, "", true);
            OnPlaylistChanged(this, args);
        }
        private void Tools()
        {
            ToolsCallbackDelegate callback = Tools_Callback;

            if (_songsViewModel.SelectedSongs.Count > 0)
            {
                frmTools toolsForm = new frmTools(_conInfo, _songsViewModel.SelectedSongs, callback);
                toolsForm.ShowDialog();

                if (toolsForm.DialogResult == true)
                {
                    ChangedPropertiesListEventArgs args = new ChangedPropertiesListEventArgs(toolsForm.ToolType, toolsForm.ChangedProperties, toolsForm.Songs);
                    OnPropertiesChangedRequested(this, args);
                }
                else
                {
                    Task.Run(() => GetAlbumSongs());
                }
            }
            else
            {
                frmTools toolsForm = new frmTools(_conInfo, _songsViewModel.Songs, callback);
                toolsForm.ShowDialog();

                if (toolsForm.DialogResult == true)
                {
                    ChangedPropertiesListEventArgs args = new ChangedPropertiesListEventArgs(toolsForm.ToolType, toolsForm.ChangedProperties, toolsForm.Songs);
                    OnPropertiesChangedRequested(this, args);
                }
                else
                {
                    Task.Run(() => GetAlbumSongs());
                }
            }
        }
        private async Task GetAlbumSongs()
        {
            _songs = await _dataServiceSongs.GetSongs(_album);
        }
        private void Tools_Callback(ToolType toolType, ChangedPropertiesList changedProperties, ObservableCollection<SongItem> songs)
        {
            ChangedPropertiesListEventArgs args = new ChangedPropertiesListEventArgs(toolType, changedProperties, songs);
            OnPropertiesChangedRequested(this, args);
        }
        #endregion // private helper

        #region Events
        public delegate void AlbumChangedEventHandler(object sender, AlbumItemEventArgs e);
        public event AlbumChangedEventHandler AlbumChanged;
        protected virtual void OnAlbumChanged(object sender, AlbumItemEventArgs e)
        {
            if (this.AlbumChanged != null)
            {
                this.AlbumChanged(this, e);
            }
        }

        public delegate void CoverImageChangedEventHandler(object sender, AlbumItemEventArgs e);
        public event CoverImageChangedEventHandler CoverImageChanged;
        protected virtual void OnCoverImageChanged(object sender, AlbumItemEventArgs e)
        {
            if (this.CoverImageChanged != null)
            {
                this.CoverImageChanged(this, e);
            }
        }

        public delegate void PlaylistChangedEventHandler(object sender, PlaylistEventArgs e);
        public event PlaylistChangedEventHandler PlaylistChanged;
        protected virtual void OnPlaylistChanged(object sender, PlaylistEventArgs e)
        {
            if (this.PlaylistChanged != null)
            {
                this.PlaylistChanged(this, e);
            }
        }

        public delegate void PropertiesChangedRequestedEventHandler(object sender, ChangedPropertiesListEventArgs e);
        public event PropertiesChangedRequestedEventHandler PropertiesChangedRequested;
        protected virtual void OnPropertiesChangedRequested(object sender, ChangedPropertiesListEventArgs e)
        {
            if (this.PropertiesChangedRequested != null)
            {
                this.PropertiesChangedRequested(this, e);
            }
        }       
       
        #endregion // Events

        #region Localization

        private String _cmd_Delete_ToolTip;
        private String _cmd_PlayNow_ToolTip;
        private String _cmd_PlayNext_ToolTip;
        private String _cmd_PlayLast_ToolTip;
        private String _cmd_Properties_ToolTip;
        private String _cmd_Wikipedia_ToolTip;
        private String _cmd_CoverImages_ToolTip;


        public String Cmd_Delete_ToolTip
        {
            get { return _cmd_Delete_ToolTip; }
            set
            {
                if (value == _cmd_Delete_ToolTip)
                    return;

                _cmd_Delete_ToolTip = value;

                RaisePropertyChanged("Cmd_Delete_ToolTip");
            }
        }
        public String Cmd_PlayNow_ToolTip
        {
            get { return _cmd_PlayNow_ToolTip; }
            set
            {
                if (value == _cmd_PlayNow_ToolTip)
                    return;

                _cmd_PlayNow_ToolTip = value;

                RaisePropertyChanged("Cmd_PlayNow_ToolTip");
            }
        }
        public String Cmd_PlayNext_ToolTip
        {
            get { return _cmd_PlayNext_ToolTip; }
            set
            {
                if (value == _cmd_PlayNext_ToolTip)
                    return;

                _cmd_PlayNext_ToolTip = value;

                RaisePropertyChanged("Cmd_PlayNext_ToolTip");
            }
        }
        public String Cmd_PlayLast_ToolTip
        {
            get { return _cmd_PlayLast_ToolTip; }
            set
            {
                if (value == _cmd_PlayLast_ToolTip)
                    return;

                _cmd_PlayLast_ToolTip = value;

                RaisePropertyChanged("Cmd_PlayLast_ToolTip");
            }
        }
        public String Cmd_Properties_ToolTip
        {
            get { return _cmd_Properties_ToolTip; }
            set
            {
                if (value == _cmd_Properties_ToolTip)
                    return;

                _cmd_Properties_ToolTip = value;

                RaisePropertyChanged("Cmd_Properties_ToolTip");
            }
        }
        public String Cmd_Wikipedia_ToolTip
        {
            get { return _cmd_Wikipedia_ToolTip; }
            set
            {
                if (value == _cmd_Wikipedia_ToolTip)
                    return;

                _cmd_Wikipedia_ToolTip = value;

                RaisePropertyChanged("Cmd_Wikipedia_ToolTip");
            }
        }
        public String Cmd_CoverImages_ToolTip
        {
            get { return _cmd_CoverImages_ToolTip; }
            set
            {
                if (value == _cmd_CoverImages_ToolTip)
                    return;

                _cmd_CoverImages_ToolTip = value;

                RaisePropertyChanged("Cmd_CoverImages_ToolTip");
            }
        }

        public void Localize()
        {

            Cmd_Delete_ToolTip = AmmLocalization.GetLocalizedString("cmd_DeleteAlbum_Tooltip");
            Cmd_PlayNow_ToolTip = AmmLocalization.GetLocalizedString("cmd_PlayNow_ToolTip");
            Cmd_PlayNext_ToolTip = AmmLocalization.GetLocalizedString("cmd_PlayNext_ToolTip");
            Cmd_PlayLast_ToolTip = AmmLocalization.GetLocalizedString("cmd_PlayLast_ToolTip");
            Cmd_Properties_ToolTip = AmmLocalization.GetLocalizedString("cmd_Properties_ToolTip");
            Cmd_Wikipedia_ToolTip = AmmLocalization.GetLocalizedString("cmd_Wikipedia_ToolTip");
            Cmd_CoverImages_ToolTip = AmmLocalization.GetLocalizedString("cmd_CoverImages_ToolTip");
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

        ~AlbumViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Dispose();
            }

            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Dispose();
            }

            if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Dispose();
            }

            if (_songsViewModel != null)
            {
                _songsViewModel.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
