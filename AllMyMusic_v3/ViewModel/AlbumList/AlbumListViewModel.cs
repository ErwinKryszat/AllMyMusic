using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

using AllMyMusic.DataService;
using AllMyMusic.QueryBuilder;
using AllMyMusic.Settings;

namespace AllMyMusic.ViewModel
{
    public class AlbumListViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ConnectionInfo _conInfo;
        private IDataServiceAlbums _dataServiceAlbums;
        private IDataServiceSongs _dataServiceSongs;

        private PagerViewModel _pagingHelper;
        private MessageBoxViewModel _messageBoxViewModel;

        private frmTools _toolsForm;

        private ObservableCollection<AlbumItem> _allAlbumItemCollection;
        private ObservableCollection<AlbumItem> _pageAlbumItemCollection;
        private ObservableCollection<AlbumItem> _selectedAlbumItemCollection;
        private ObservableCollection<SongItem> _songs;

        private AlbumItem _selectedAlbum;
        private SelectionMode _selectionMode;
        

        private double _sleeveImageSize;
        private double _albumListViewWidth;
        private double _stampImageSize;

        private Boolean _bandNameVisbility;
        private Boolean _enableWikipediaButton;

        private String _headlineAlbumList;
        private String _searchText;
        private String _wikipediaLanguage;

        private object _lastQueryItem = null;

        private String _strSongsQuery = String.Empty;
        private String _strSongsQueryParameterName = String.Empty;
        private String _strSongsQueryParameterValue = String.Empty;

        private static Task _addCoverImagesTask = null;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;

        private RelayCommand<object> _manageImagesCommand;
        private RelayCommand<object> _deleteCommand;
        private RelayCommand<object> _playNowCommand;
        private RelayCommand<object> _playNextCommand;
        private RelayCommand<object> _playLastCommand;
        private RelayCommand<object> _toolsCommand;
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
        private void ExecuteManageImagesCommand(object notUsed)
        {
            AlbumListEventArgs args = new AlbumListEventArgs(_allAlbumItemCollection);
            OnManageCoverImagesRequested(this, args);
        }
        private bool CanManageImagesCommand(object notUsed)
        {
            return (_allAlbumItemCollection != null);
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
            _messageBoxViewModel.MessageText = "Are you sure that you want to delete all albums listed above?";
            _messageBoxViewModel.OkAction = new Action(DeleteAlbums);
            _messageBoxViewModel.MessageBoxVisible = true;
        }
        private bool CanDeleteCommand(object notUsed)
        {
            return (_allAlbumItemCollection != null);
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
            Task.Run(()=>EnqueueSongs(QueueMode.Replace));
            
        }
        private bool CanPlayNowCommand(object notUsed)
        {
            return (_allAlbumItemCollection != null);
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
            Task.Run(() => EnqueueSongs(QueueMode.Insert));
        }
        private bool CanPlayNextCommand(object notUsed)
        {
            return (_allAlbumItemCollection != null);
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
            Task.Run(() => EnqueueSongs(QueueMode.Append));
        }
        private bool CanPlayLastCommand(object notUsed)
        {
            return (_allAlbumItemCollection != null);
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
            return (_allAlbumItemCollection != null);
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
            String webLink = WikipediaLink(_allAlbumItemCollection[0].BandName);
            Process.Start(webLink);
        }
        private bool CanWikipediaCommand(object notUsed)
        {
            return _enableWikipediaButton;
        }
        #endregion // Command

        #region Presentation Properties

        public ObservableCollection<AlbumItem> AllAlbumItemCollection
        {
            get { return _allAlbumItemCollection; }
            set
            {
                _allAlbumItemCollection = value;
                OnAllAlbumItemCollectionChanged();
                
                //RaisePropertyChanged("AllAlbumItemCollection");
            }
        }
        public ObservableCollection<AlbumItem> PageAlbumItemCollection
        {
            get { return _pageAlbumItemCollection; }
            set
            {
                if (value == _pageAlbumItemCollection)
                    return;

                _pageAlbumItemCollection = value;

                RaisePropertyChanged("PageAlbumItemCollection");
            }
        }
        public ObservableCollection<AlbumItem> SelectedAlbumItemCollection
        {
            get { return _selectedAlbumItemCollection; }
            set
            {
                if (value == _selectedAlbumItemCollection)
                    return;

                _selectedAlbumItemCollection = value;

                RaisePropertyChanged("SelectedAlbumItemCollection");
            }
        }
        
        public PagerViewModel Pager
        {
            get { return _pagingHelper; }
            set
            {
                if (value == _pagingHelper)
                    return;

                _pagingHelper = value;

                RaisePropertyChanged("Pager");
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
        public AlbumItem SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                if ((value == _selectedAlbum) || (value == null))
                    return;

                _selectedAlbum = value;

                if (_selectedAlbum != null)
                {
                    AlbumItemEventArgs args = new AlbumItemEventArgs(_selectedAlbum);
                    OnAlbumItemSelected(this, args);
                }
               
                RaisePropertyChanged("SelectedAlbum");
            }
        }
        public SelectionMode SelectionMode
        {
            get { return _selectionMode; }
            set
            {
                _selectionMode = value;

                RaisePropertyChanged("SelectionMode");
            }
        }

        // Binding Mode.ToWay required for this property to function
        public double AlbumListViewWidth
        {
            get { return _albumListViewWidth; }
            set
            {
                if (value == _albumListViewWidth)
                    return;

                _albumListViewWidth = value;

                RaisePropertyChanged("AlbumListViewWidth");
            }
        }
        public Boolean BandNameVisbility
        {
            get { return _bandNameVisbility; }
            set
            {
                if (value == _bandNameVisbility)
                    return;

                _bandNameVisbility = value;

                RaisePropertyChanged("BandVisbility");
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
        public String HeadlineAlbumList
        {
            get { return _headlineAlbumList; }
            set
            {
                if (value == _headlineAlbumList)
                    return;

                _headlineAlbumList = value;
                RaisePropertyChanged("HeadlineAlbumList");
            }
        }
        public String SearchText
        {
            get { return _searchText; }
            set
            {
                if (value == _searchText)
                    return;

                _searchText = value;
                RaisePropertyChanged("SearchText");
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
        public AlbumListViewModel(ConnectionInfo conInfo)
        {
            _conInfo = conInfo;
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.SqlServer)
            {
                _dataServiceAlbums = new DataServiceAlbums_SQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.MySql)
            {
                _dataServiceAlbums = new DataServiceAlbums_MYSQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }

            _messageBoxViewModel = new MessageBoxViewModel();

            _stampImageSize = 80;

            _allAlbumItemCollection = new ObservableCollection<AlbumItem>();
            _pagingHelper = new PagerViewModel("Albums", _allAlbumItemCollection.Count, 50);

            Localize();
        }
        #endregion  // Constructor

        #region public
        public async Task GetAlbumList(BandItem band)
        {
            HeadlineAlbumList = band.BandName;

            _bandNameVisbility = false;
            _enableWikipediaButton = true;

            _strSongsQuery = QueryBuilderSongs.SongsByBand(band.BandId);
            _strSongsQueryParameterName = String.Empty;
            _strSongsQueryParameterValue = String.Empty;

            AllAlbumItemCollection = await _dataServiceAlbums.GetAlbums(band);
            _lastQueryItem = band;
        }
        public async Task GetAlbumList(AlbumGenreItem albumGenre)
        {
            HeadlineAlbumList = albumGenre.Name;

            _bandNameVisbility = true;
            _enableWikipediaButton = false;

            _strSongsQuery = QueryBuilderSongs.SongsByAlbumGenreVA(albumGenre.AlbumGenreId);
            _strSongsQueryParameterName = String.Empty;
            _strSongsQueryParameterValue = String.Empty;

            AllAlbumItemCollection = await _dataServiceAlbums.GetAlbums(albumGenre);
            

            _lastQueryItem = albumGenre;
        }
        public async Task GetAlbumList(String searchText)
        {          
            HeadlineAlbumList = "Search: " + searchText;

            _searchText = searchText;

            _bandNameVisbility = true;
            _enableWikipediaButton = false;

            _strSongsQuery = QueryBuilderSongs.SearchSongs();
            _strSongsQueryParameterName = "Name";
            _strSongsQueryParameterValue = searchText;

            AllAlbumItemCollection = await _dataServiceAlbums.SearchAlbums(searchText);
            
            _lastQueryItem = searchText;
        }
        public async Task RefreshView()
        {
            if (_lastQueryItem != null)
            {
                if (_lastQueryItem.GetType() == typeof(BandItem))
                {
                    await GetAlbumList((BandItem)_lastQueryItem);
                }

                if (_lastQueryItem.GetType() == typeof(AlbumGenreItem))
                {
                    await GetAlbumList((AlbumGenreItem)_lastQueryItem);
                }

                if (_lastQueryItem.GetType() == typeof(String))
                {
                    await GetAlbumList((String)_lastQueryItem);
                }
            }
        }
        public void ClearView()
        {
            AllAlbumItemCollection = new ObservableCollection<AlbumItem>();
            HeadlineAlbumList = String.Empty;
            _lastQueryItem = null;
        }
        public void AddAlbums(ObservableCollection<AlbumItem> newAlbums)
        {
            for (int i = 0; i < newAlbums.Count; i++)
            {
                AlbumItem album = new AlbumItem();
                _allAlbumItemCollection.Add(newAlbums[i]);
            }
            OnAllAlbumItemCollectionChanged();
        }
        public void RemoveAlbums(ObservableCollection<AlbumItem> removeAlbums)
        {
            for (int i = 0; i < removeAlbums.Count; i++)
            {
                _allAlbumItemCollection.Remove(removeAlbums[i]);
            }
            OnAllAlbumItemCollectionChanged();
        }
        public void RemoveAllAlbums()
        {
            _allAlbumItemCollection.Clear();
            OnAllAlbumItemCollectionChanged();
        }
        public void UnselectAll()
        {
            for (int i = 0; i < _allAlbumItemCollection.Count; i++)
            {
                _allAlbumItemCollection[i].IsSelected = false;
            }
        }
        public void MoveSelectedAlbumsUp()
        {
            for (int i = 0; i < _selectedAlbumItemCollection.Count; i++)
            {
                Int32 selectedIndex = GetItemIndex(_selectedAlbumItemCollection[i]);
                MoveItem(-1, selectedIndex);
            }
        }
        public void MoveSelectedAlbumsDown()
        {
            for (int i = 0; i < _selectedAlbumItemCollection.Count; i++)
            {
                Int32 selectedIndex = GetItemIndex(_selectedAlbumItemCollection[i]);
                MoveItem(1, selectedIndex);
            }
        }
        private Int32 GetItemIndex(AlbumItem ai)
        {
            for (int i = 0; i < _allAlbumItemCollection.Count; i++)
            {
                if (_allAlbumItemCollection[i].AlbumId == ai.AlbumId)
                {
                    return i;
                }
            }
            return -1;
        }
        private void MoveItem(int direction, int selectedIndex)
        {
            // Calculate new index using move direction
            int newIndex = selectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= _allAlbumItemCollection.Count)
                return; // Index out of range - nothing to do

            AlbumItem selectedAlbum = _allAlbumItemCollection[selectedIndex];

            // Removing removable element
            _allAlbumItemCollection.RemoveAt(selectedIndex);

            // Insert it in new position
            _allAlbumItemCollection.Insert(newIndex, selectedAlbum);

            OnAllAlbumItemCollectionChanged();
        }

        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            ClearView();
            _conInfo = conInfo;
            _dataServiceAlbums.ChangeDatabase(conInfo);
            _dataServiceSongs.ChangeDatabase(conInfo);
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            ClearView();
            _conInfo = conInfo;
            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Dispose();
            }
            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Dispose();
            }

            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceAlbums = new DataServiceAlbums_SQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceAlbums = new DataServiceAlbums_MYSQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }
        }
        public void Close()
        {
            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Close();
            }

            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Close();
            }
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
        private async Task EnqueueSongs(QueueMode queueMode)
        {
            _songs = await GetSongsAsync();
            PlaylistEventArgs args = new PlaylistEventArgs(_songs, queueMode, "", false);
            OnPlaylistChanged(this, args);
        }
        private async Task <ObservableCollection<SongItem>> GetSongsAsync()
        {
            ObservableCollection<SongItem> songs = null;
            try
            {
                songs = await _dataServiceSongs.GetSongs(_strSongsQuery);
            }
            catch (Exception)
            {
                
                throw;
            }

            return songs;
        }
        private void OnAllAlbumItemCollectionChanged()
        {
            if ((_allAlbumItemCollection != null) && (_allAlbumItemCollection.Count > 0))
            {
                _pagingHelper = new PagerViewModel("Albums", _allAlbumItemCollection.Count, 50);
                _pagingHelper.PageChanged += new PagerViewModel.PageChangedEventHandler(_pagingHelper_PageChanged);
                RaisePropertyChanged("Pager");

                SelectedAlbum = null;

                LoadCoverImages();

                _searchText = String.Empty;
            }
            else
            {
                _pagingHelper = new PagerViewModel("Albums", 0, 50);
                RaisePropertyChanged("Pager");
                LoadCoverImages();
            }
            AlbumListEventArgs args = new AlbumListEventArgs(_allAlbumItemCollection);
            OnAlbumListChanged(this, args);
        }     
        private void _pagingHelper_PageChanged(object sender, EventArgs e)
        {
            LoadCoverImages();
        }
        private void LoadCoverImages()
        {
            if (_allAlbumItemCollection.Count > 0)
            {
                if ((_addCoverImagesTask != null) && (_addCoverImagesTask.Status == TaskStatus.Running))
                {
                    if (_tokenSource != null)
                    {
                        _tokenSource.Cancel();
                    }

                    while (_addCoverImagesTask.Status == TaskStatus.Running) { }
                }

                _tokenSource = new CancellationTokenSource();
                _token = _tokenSource.Token;

                
                _addCoverImagesTask = Task.Factory.StartNew(() => Background_LoadPage(_token, _allAlbumItemCollection), _token);
            }
            else
            {
                PageAlbumItemCollection = _allAlbumItemCollection;
            }
        }
        private void Background_LoadPage(CancellationToken ct, ObservableCollection<AlbumItem> albums)
        {
            ObservableCollection<AlbumItem> itemList = new ObservableCollection<AlbumItem>();
            for (int i = _pagingHelper.StartIndex; i <= _pagingHelper.EndIndex; i++)
            {
                if (ct.IsCancellationRequested == true)
                {
                    break;
                }

                AlbumItem album = albums[i];
                String imagePath = album.StampImageFullpath;

                if ((File.Exists(imagePath) == true) && (album.StampImage == null))
                {
                    using (FileStream imageFileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        Byte[] imageBytes = new byte[imageFileStream.Length];
                        Int32 bytesRead = imageFileStream.Read(imageBytes, 0, (int)imageFileStream.Length);

                        if (bytesRead == imageFileStream.Length)
                        {
                            using (MemoryStream imageMemStream = new MemoryStream(imageBytes))
                            {
                                BitmapImage thumbNailImage = new BitmapImage();
                                thumbNailImage.BeginInit();
                                thumbNailImage.StreamSource = imageMemStream;
                                thumbNailImage.CacheOption = BitmapCacheOption.OnLoad;
                                thumbNailImage.EndInit();
                                thumbNailImage.Freeze();
                                album.StampImage = thumbNailImage;
                                
                            }
                        }
                    }
                }

                if (album.StampImage == null)
                {
                    Uri imageReference = new Uri(Global.Images + "cover.jpg", UriKind.Relative);
                    StreamResourceInfo sri = System.Windows.Application.GetResourceStream(imageReference);
                    BitmapImage bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.StreamSource = sri.Stream;
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.EndInit();
                    bmp.Freeze();
                    album.StampImage = bmp;
                }

                itemList.Add(album);
                
            }
            
            PageAlbumItemCollection = itemList;
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
            PlaylistEventArgs args = new PlaylistEventArgs(_songs, QueueMode.Replace, "", true);
            OnPlaylistChanged(this, args);
        }
        private void DoNothing()
        {

        }
        private void DeleteAlbums()
        {
            if (_lastQueryItem != null)
            {
                if (_lastQueryItem.GetType() == typeof(BandItem))
                {
                    _dataServiceAlbums.DeleteBand((BandItem)_lastQueryItem);
                }

                if (_lastQueryItem.GetType() == typeof(AlbumGenreItem))
                {
                    _dataServiceAlbums.DeleteAlbumGenre((AlbumGenreItem)_lastQueryItem);
                }
            }

            AllAlbumItemCollection = new ObservableCollection<AlbumItem>();
            PageAlbumItemCollection = new ObservableCollection<AlbumItem>();
            HeadlineAlbumList = String.Empty;
        }

        //private ToolsCallbackDelegate callback;
        private void Tools()
        {
            ToolsCallbackDelegate callback = Tools_Callback;

            _toolsForm = new frmTools(_conInfo, _strSongsQuery, callback);
            _toolsForm.ShowDialog();

            if (_toolsForm.DialogResult == true)
            {
                ChangedPropertiesListEventArgs args = new ChangedPropertiesListEventArgs(_toolsForm.ToolType, _toolsForm.ChangedProperties, _toolsForm.Songs);
                OnPropertiesChangedRequested(this, args);
            }
            else
            {
                // Reload songs when user pressed Cancel button
                // _dataServiceSongs.GetSongs(_strSongsQuery)
                GetSongs();
            }
        }
        private void GetSongs()
        {
            Task.Run(async () =>
                {
                    _songs = await _dataServiceSongs.GetSongs(_strSongsQuery);
                }
            );
        }

        private void Tools_Callback(ToolType toolType, ChangedPropertiesList changedProperties, ObservableCollection<SongItem> songs)
        {
            ChangedPropertiesListEventArgs args = new ChangedPropertiesListEventArgs(toolType, changedProperties, songs);
            OnPropertiesChangedRequested(this, args);
        }
        #endregion // private helper

        #region Events
        public delegate void AlbumListChangedEventHandler(object sender, AlbumListEventArgs e);
        public event AlbumListChangedEventHandler AlbumListChanged;
        protected virtual void OnAlbumListChanged(object sender, AlbumListEventArgs e)
        {
            if (this.AlbumListChanged != null)
            {
                this.AlbumListChanged(this, e);
            }
        }

        public delegate void AlbumItemSelectedEventHandler(object sender, AlbumItemEventArgs e);
        public event AlbumItemSelectedEventHandler AlbumItemSelected;
        protected virtual void OnAlbumItemSelected(object sender, AlbumItemEventArgs e)
        {
            if (this.AlbumItemSelected != null)
            {
                this.AlbumItemSelected(this, e);
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

        public delegate void ManageCoverImagesRequestedEventHandler(object sender, AlbumListEventArgs e);
        public event ManageCoverImagesRequestedEventHandler ManageCoverImagesRequested;
        protected virtual void OnManageCoverImagesRequested(object sender, AlbumListEventArgs e)
        {
            if (this.ManageCoverImagesRequested != null)
            {
                this.ManageCoverImagesRequested(this, e);
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
            Cmd_Delete_ToolTip = AmmLocalization.GetLocalizedString("cmd_DeleteAlbumList_Tooltip");
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

        ~AlbumListViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_toolsForm != null)
            {
                _toolsForm.Dispose();
            }
            if (_tokenSource != null)
            {
                _tokenSource.Dispose();
            }

            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Dispose();
            }

            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
