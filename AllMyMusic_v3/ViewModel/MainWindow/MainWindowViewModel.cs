using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using AllMyMusic.Settings;
using AllMyMusic.DataService;


namespace AllMyMusic.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.

    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ConnectionInfo _conInfo;
        private ViewModelBase _currentViewModel;
        private AudioPlayerViewModel _vmAudioPlayer;
        private PlaylistViewModel _vmPlayList;
        private AlbumAndAlbumListViewModel _vmAlbums;
        private AlbumGenreViewModel _vmAlbumGenres;
        private BandListViewModel _vmBands;
        private DatabasesViewModel _vmDatabases;
        private RibbonViewModel _vmRibbon;
        private TaskQueueViewModel _vmTaskQueue;
        private ToolbarViewModel _vmToolbar;
        private IDataServiceSongs _dataServiceSongs;

        private AddSongsBackgroundWorker _addSongsWorker;
        private CoverImagesBackgroundWorker _coverImagesWorker;

        private UpdateSongsBackgroundWorker _updateSongsWorker;
        private RenameBackgroundWorker _renameWorker;
        private TaskQueue _taskQueue;

        private double _albumAndAlbumlistViewWidth;
        private CommandSourceViewModel _lastPlayCommandSource;
        private CommandSourceViewModel _lastUpdatePropertiesCommandSource;
        #endregion // Fields

        #region Presentation Properties
        public AudioPlayerViewModel AudioPlayerViewModel
        {
            get
            {
                return _vmAudioPlayer;
            }
            set
            {
                if (_vmAudioPlayer == value)
                    return;

                _vmAudioPlayer = value;
                RaisePropertyChanged("AudioPlayerViewModel");
            }
        }
        public PlaylistViewModel PlayListViewModel
        {
            get { return _vmPlayList; }
            set
            {
                _vmPlayList = value;

                RaisePropertyChanged("PlayListViewModel");
            }
        }
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }
        public RibbonViewModel RibbonViewModel
        {
            get
            {
                return _vmRibbon;
            }
            set
            {
                if (_vmRibbon == value)
                    return;
                _vmRibbon = value;
                RaisePropertyChanged("RibbonViewModel");
            }
        }
        public ToolbarViewModel ToolbarViewModel
        {
            get
            {
                return _vmToolbar;
            }
            set
            {
                if (_vmToolbar == value)
                    return;
                _vmToolbar = value;
                RaisePropertyChanged("ToolbarViewModel");
            }
        }
        public TaskQueueViewModel TaskQueueViewModel
        {
            get
            {
                return _vmTaskQueue;
            }
            set
            {
                if (_vmTaskQueue == value)
                    return;

                _vmTaskQueue = value;
                RaisePropertyChanged("TaskQueueViewModel");
            }
        }
        public double AlbumAndAlbumlistViewWidth
        {
            get { return _albumAndAlbumlistViewWidth; }
            set
            {
                if (value == _albumAndAlbumlistViewWidth)
                    return;

                _albumAndAlbumlistViewWidth = value;

                RaisePropertyChanged("AlbumAndAlbumlistViewWidth");
            }
        }
        #endregion //Presentation Properties

        #region Constructor
        public MainWindowViewModel()
        {

        }
        #endregion

        #region Public
        public Boolean Initialize()
        {
            Boolean bResult = InitDirectories();
            if (bResult != true) { return false; }

            AmmLocalization.Initialize(AppSettings.GeneralSettings.LanguageGUI);
            AlbumAndAlbumlistViewWidth = AppSettings.FormSettings.FrmMain_AlbumAndAlbumListViewWidth;

            if (AlbumAndAlbumlistViewWidth > AppSettings.FormSettings.FrmMain_Size.Width )
            {
                AlbumAndAlbumlistViewWidth = AppSettings.FormSettings.FrmMain_Size.Width - 100; 
            }

            _taskQueue = new TaskQueue();
            _taskQueue.AllJobsCompleted += new TaskQueue.AllJobsCompletedEventHandler(TaskQueue_WorkDone);

            if (AppSettings.DatabaseSettings.DefaultDatabase != null)
            {
                bResult = Create_vmDatabases();
            }
            else
            {
                _vmDatabases = new DatabasesViewModel();

                frmConnectServer frmDbConnect = new frmConnectServer(_vmDatabases);
                frmDbConnect.ShowDialog();

                if (_vmDatabases.DatabaseConnectionInformation != null) 
                {
                    _vmDatabases.SelectedServerTypeChanged += new DatabasesViewModel.ServerTypeChangedEventHandler(ServerTypeChanged);
                    _vmDatabases.DatabaseChanged += new DatabasesViewModel.DatabaseChangedEventHandler(DatabaseChanged);
                    _vmDatabases.DatabasePurged += new DatabasesViewModel.DatabaseChangedEventHandler(DatabasePurged);
                }
                else
                {
                    OnCloseWindowRequest(this, new EventArgs());
                }
            }

            if (_vmDatabases.DatabaseConnectionInformation.ServerType == ServerType.SqlServer)
            {
                _dataServiceSongs = new DataServiceSongs_SQL(_vmDatabases.DatabaseConnectionInformation);
            }
            if (_vmDatabases.DatabaseConnectionInformation.ServerType == ServerType.MySql)
            {
                _dataServiceSongs = new DataServiceSongs_MYSQL(_vmDatabases.DatabaseConnectionInformation);
            }

            bResult = CreateViewModels(AppSettings.DatabaseSettings.DefaultDatabase);
            return bResult;
        }
        private Boolean InitDirectories()
        {
            Boolean bResult;
            Bootstraper _boot = new Bootstraper();

            try
            {               
                bResult = _boot.CreateApplicationDirectories();
                if (bResult != true) { return false; }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing Directories";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }

            try
            {
                _boot.CopyResources();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing CopyResources";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }

            try
            {
                _boot.CopyPartybuttonImages();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing CopyPartybuttonImages";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }

            try
            {
                _boot.LoadApplicationSettings();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing LoadApplicationSettings";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }
        private Boolean Create_vmDatabases()
        {
            try
            {
                _vmDatabases = new DatabasesViewModel(AppSettings.DatabaseSettings.DatabaseConnections);
                _vmDatabases.SelectedServerTypeChanged += new DatabasesViewModel.ServerTypeChangedEventHandler(ServerTypeChanged);
                _vmDatabases.DatabaseChanged += new DatabasesViewModel.DatabaseChangedEventHandler(DatabaseChanged);
                _vmDatabases.DatabasePurged += new DatabasesViewModel.DatabaseChangedEventHandler(DatabasePurged);
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing ViewModel vmDatabases";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }
        private Boolean CreateViewModels(ConnectionInfo conInfo)
        {
            _conInfo = conInfo;
            Boolean bResult = Create_vmBands(conInfo);
            if (bResult != true) { return false; }

            bResult = Create_vmAlbums(conInfo);
            if (bResult != true) { return false; }

            bResult = Create_vmAlbumGenres(conInfo);
            if (bResult != true) { return false; }

            bResult = Create_vmRibbon();
            if (bResult != true) { return false; }

            bResult = Create_vmToolbar(conInfo);
            if (bResult != true) { return false; }

            bResult = Create_vmPlayList(conInfo);
            if (bResult != true) { return false; }

            bResult = Create_vmAudioPlayer(conInfo, _vmPlayList);
            if (bResult != true) { return false; }

            return true;
        }
        private Boolean Create_vmBands(ConnectionInfo conInfo)
        {
            try
            {
                _vmBands = new BandListViewModel(conInfo);
                _vmBands.BandItemSelected += new BandListViewModel.BandItemSelectedEventHandler(_vmBands_BandItemSelected);
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing ViewModel vmBands";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }
        private Boolean Create_vmAlbums(ConnectionInfo conInfo)
        {
            try
            {
                _vmAlbums = new AlbumAndAlbumListViewModel(conInfo);
                _vmAlbums.ManageCoverImagesRequested += new AlbumAndAlbumListViewModel.ManageCoverImagesRequestedEventHandler(_vmAlbums_ManageCoverImagesRequested);
                _vmAlbums.PropertiesChangedRequested += new AlbumAndAlbumListViewModel.PropertiesChangedRequestedEventHandler(_vmAlbums_PropertiesChangedRequested);
                _vmAlbums.PlaylistChanged += new AlbumAndAlbumListViewModel.PlaylistChangedEventHandler(_vmAlbums_PlaylistChanged);
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing ViewModel vmAlbums";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }
        private Boolean Create_vmAlbumGenres(ConnectionInfo conInfo)
        {
            try
            {
                _vmAlbumGenres = new AlbumGenreViewModel(conInfo);
                _vmAlbumGenres.AlbumGenreSelected += new AlbumGenreViewModel.AlbumGenreSelectedEventHandler(_vmAlbumGenres_AlbumGenreSelected);
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing ViewModel vmAlbumGenres";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }
        private Boolean Create_vmRibbon()
        {
            try
            {
                _vmRibbon = new RibbonViewModel(_vmDatabases);
                _vmRibbon.AddSongsRequest += new ViewModel.RibbonViewModel.AddSongsRequestEventHandler(_ribbonViewModel_AddSongsRequest);
                _vmRibbon.ManageImagesRequest += new ViewModel.RibbonViewModel.ManageImagesRequestEventHandler(_ribbonViewModel_ManageImagesRequest);
                _vmRibbon.PartyButtonDesignerRequest += new ViewModel.RibbonViewModel.PartyButtonDesignerRequestEventHandler(_vmRibbon_PartyButtonDesignerRequest);
                _vmRibbon.PlaylistChanged += new ViewModel.RibbonViewModel.PlaylistChangedEventHandler(_vmRibbon_PlaylistChanged);
                _vmRibbon.PartyButton_Click += new ViewModel.RibbonViewModel.PartyButton_ClickEventHandler(_vmRibbon_PartyButton_Click);
                _vmRibbon.LanguageChanged += new ViewModel.RibbonViewModel.LanguageChangedEventHandler(_vmRibbon_LanguageChanged);
                _vmRibbon.WikipediaChanged += new ViewModel.RibbonViewModel.WikipediaChangedEventHandler(_vmRibbon_WikipediaChanged);
                RaisePropertyChanged("RibbonViewModel");
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing ViewModel vmRibbon";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }
        private Boolean Create_vmToolbar(ConnectionInfo conInfo)
        {
            try
            {
                _vmToolbar = new ToolbarViewModel(conInfo, AppSettings.FormSettings.FrmMain_ViewVaBands);
                _vmToolbar.AlphabetButtonClicked += new ToolbarViewModel.AlphabetButtonClickedEventHandler(_toolbarViewModel_AlphabetButtonClicked);
                _vmToolbar.VaGenresButtonClicked += new ToolbarViewModel.VaGenresButtonClickedEventHandler(_toolbarViewModel_VaGenresButtonClicked);
                _vmToolbar.ShowVaBandsChanged += new ToolbarViewModel.ShowVaBandsChangedEventHandler(_toolbarViewModel_ShowVaBandsChanged);
                _vmToolbar.Search += new ToolbarViewModel.SearchEventHandler(_toolbarViewModel_Search);
                _vmToolbar.SearchBands += new ToolbarViewModel.SearchEventHandler(_toolbarViewModel_SearchBands);
                _vmToolbar.AlbumListViewExpandedClicked += new ToolbarViewModel.AlbumListViewExpandedClickedEventHandler(_toolbarViewModel_AlbumListViewExpandedClicked);
                _vmToolbar.AlbumListViewExpanded = AppSettings.FormSettings.FrmMain_AlbumListViewExpanded;
                RaisePropertyChanged("ToolbarViewModel");
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing ViewModel vmToolbar";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }
        private Boolean Create_vmAudioPlayer(ConnectionInfo conInfo, PlaylistViewModel vmPlayList)
        {
            try
            {
                _vmAudioPlayer = new AudioPlayerViewModel(conInfo, vmPlayList);
                _vmAudioPlayer.Volume = AppSettings.AudioSettings.Volume;
                RaisePropertyChanged("AudioPlayerViewModel");
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing ViewModel vmAudioPlayer";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }
        private Boolean Create_vmPlayList(ConnectionInfo conInfo)
        {
            try
            {
                _vmPlayList = new PlaylistViewModel(conInfo);
                _vmPlayList.PlaylistReady += new PlaylistViewModel.PlaylistReadyEventHandler(_vmPlayList_PlaylistReady);
                _vmPlayList.TryOverwritedLockedPlaylist += new PlaylistViewModel.TryOverwritedLockedPlaylistEventHandler(_vmPlayList_TryOverwritedLockedPlaylist);
                RaisePropertyChanged("PlayListViewModel");
            }
            catch (Exception Err)
            {
                String errorMessage = "Error when initializing ViewModel vmPlayList";
                ShowError.ShowAndLog(Err, errorMessage, 1);
                return false;
            }
            return true;
        }

        private void ServerTypeChanged(object sender, ConnectionInfoEventArgs e)
        {
            ClearView();

            _conInfo = e.DbConInfo;
            _vmToolbar.ChangeDatabaseService(e.DbConInfo);
            _vmBands.ChangeDatabaseService(e.DbConInfo);
            _vmAlbums.ChangeDatabaseService(e.DbConInfo);
            _vmAlbumGenres.ChangeDatabaseService(e.DbConInfo);
            _vmAudioPlayer.ChangeDatabaseService(e.DbConInfo);
            _vmRibbon.ChangeDatabaseService(e.DbConInfo);

            if (e.DbConInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceSongs = new DataServiceSongs_SQL(e.DbConInfo);
            }
            if (e.DbConInfo.ServerType == ServerType.MySql)
            {
                _dataServiceSongs = new DataServiceSongs_MYSQL(e.DbConInfo);
            }
        }
        private void DatabaseChanged(object sender, ConnectionInfoEventArgs e)
        {
            ClearView();

            _conInfo = e.DbConInfo;
            _vmToolbar.ChangeDatabase(e.DbConInfo);
            _vmBands.ChangeDatabase(e.DbConInfo);
            _vmAlbums.ChangeDatabase(e.DbConInfo);
            _vmAlbumGenres.ChangeDatabase(e.DbConInfo);
            _vmAudioPlayer.ChangeDatabase(e.DbConInfo);
            _vmRibbon.ChangeDatabase(e.DbConInfo);
            _dataServiceSongs.ChangeDatabase(e.DbConInfo);
        }
        public void CloseViewModels()
        {
            AppSettings.FormSettings.FrmMain_AlbumAndAlbumListViewWidth = _albumAndAlbumlistViewWidth;

            if (_vmAudioPlayer != null)
            {
                _vmAudioPlayer.Unload();
                AppSettings.AudioSettings.Volume = _vmAudioPlayer.Volume;
            }

            if (_vmAlbums != null)
            {
                _vmAlbums.Close();
            }

            if (_vmAlbumGenres != null)
            {
                _vmAlbumGenres.Close();
            }

            if (_vmBands != null)
            {
                _vmBands.Close();
            }
          
            if (_vmRibbon != null)
            {
                _vmRibbon.Close();
            }

            if (_vmTaskQueue != null)
            {
                //  _vmTaskQueue.Close();
            }

            if (_vmToolbar != null)
            {
                AppSettings.FormSettings.FrmMain_ViewVaBands = _vmToolbar.ViewVaBands;
                AppSettings.FormSettings.FrmMain_AlbumListViewExpanded = _vmToolbar.AlbumListViewExpanded;
                _vmToolbar.Close();
            }
        }
        #endregion

        #region events
        public delegate void CloseWindowRequestEventHandler(object sender, EventArgs e);
        public event CloseWindowRequestEventHandler CloseWindowRequest;
        protected virtual void OnCloseWindowRequest(object sender, EventArgs e)
        {
            if (this.CloseWindowRequest != null)
            {
                this.CloseWindowRequest(this, e);
            }
        }
        #endregion

        private void DatabasePurged(object sender, ConnectionInfoEventArgs e)
        {
            _vmToolbar.Initialize();
            ClearView();
        }

        private void ClearView()
        {
            if (CurrentViewModel is BandListViewModel)
            {
                CurrentViewModel = _vmBands;
                _vmBands.ClearView();
            }

            if (CurrentViewModel is AlbumAndAlbumListViewModel)
            {
                CurrentViewModel = _vmAlbums;
                _vmAlbums.ClearView();
            }

            if (CurrentViewModel != null)
            {
                RaisePropertyChanged("CurrentViewModel");
            }
        }
        private async Task RefreshView()
        {
            if (CurrentViewModel is BandListViewModel)
            {
                try
                {
                    await _vmBands.RefreshView();
                }
                catch (Exception Err)
                {
                    String errorMessage = "Error when refreshing vmBands";
                    ShowError.ShowAndLog(Err, errorMessage, 1);
                }
            }

            if (CurrentViewModel is AlbumAndAlbumListViewModel)
            {
                try
                {
                    await _vmAlbums.RefreshView();
                }
                catch (Exception Err)
                {
                    String errorMessage = "Error when refreshing vmAlbums";
                    ShowError.ShowAndLog(Err, errorMessage, 1);
                }
            }
        }

        private void _vmPlayList_PlaylistReady(object sender, EventArgs e)
        {

                if (_vmAudioPlayer.Song == null)
                {
                    _vmAudioPlayer.Play();
                }
                else
                {
                    if (_vmAudioPlayer.Song != _vmPlayList.GetCurrentSong())
                    {
                        _vmAudioPlayer.Play();
                    }
                }

        }
        private void _vmPlayList_TryOverwritedLockedPlaylist(object sender, EventArgs e)
        {
            _vmAlbums.AskOverwritePlaylist(_lastPlayCommandSource);
        }

        private async void _vmBands_BandItemSelected(object sender, BandItemEventArgs e)
        {
            if (e.Band != null)
            {
                _vmToolbar.AlbumListViewExpanded = true;

                try
                {
                    await _vmAlbums.GetAlbumList(e.Band);
                }
                catch (Exception Err)
                {
                    String errorMessage = "Error GetAlbumList from vmAlbums";
                    ShowError.ShowAndLog(Err, errorMessage, 1);
                }

                try
                {
                    await _vmToolbar.UnselectAllButtons();
                }
                catch (Exception Err)
                {
                    String errorMessage = "Error UnselectAllButtons from vmToolbar";
                    ShowError.ShowAndLog(Err, errorMessage, 1);
                }
                
                CurrentViewModel = _vmAlbums;
            }
        }

        private async void _vmAlbumGenres_AlbumGenreSelected(object sender, AlbumGenreEventArgs e)
        {
            if (e.AlbumGenre != null)
            {
                _vmToolbar.AlbumListViewExpanded = true;

                try
                {
                    await _vmAlbums.GetAlbumList(e.AlbumGenre);
                }
                catch (Exception Err)
                {
                    String errorMessage = "Error GetAlbumList from vmAlbums";
                    ShowError.ShowAndLog(Err, errorMessage, 1);
                }

                
                CurrentViewModel = _vmAlbums;

                _vmToolbar.ViewVaGenres = false;
            }
        }

        private void _vmAlbums_ManageCoverImagesRequested(object sender, AlbumListEventArgs e)
        {
            TaskQueueViewModel = new TaskQueueViewModel(_taskQueue);
            ReportProgress_Callback progressCallback = new ReportProgress_Callback(TaskQueue_Progress);
            CancellationTokenSource cts = new CancellationTokenSource();

            _coverImagesWorker = new CoverImagesBackgroundWorker(cts.Token);
            TaskQueueItem manageImages = new TaskQueueItem(_coverImagesWorker, e.AlbumList, progressCallback, cts);
            _taskQueue.Enqueue(manageImages);
        }
        private void _vmAlbums_PropertiesChangedRequested(object sender, ChangedPropertiesListEventArgs e)
        {
            if (sender is AlbumViewModel)
            {
                _lastUpdatePropertiesCommandSource = CommandSourceViewModel.AlbumViewModel;
            }
            else
            {
                _lastUpdatePropertiesCommandSource = CommandSourceViewModel.AlbumListViewModel;
            }

            if ((e.ToolType == ToolType.PropertiesTool) || (e.ToolType == ToolType.AutoTagTool))
            {
                TaskQueueViewModel = new TaskQueueViewModel(_taskQueue);
                ReportProgress_Callback progressCallback = new ReportProgress_Callback(TaskQueue_Progress);
                CancellationTokenSource cts = new CancellationTokenSource();
                _updateSongsWorker = new UpdateSongsBackgroundWorker(_conInfo, cts.Token);
                TaskQueueItem updateSongs = new TaskQueueItem(_updateSongsWorker, e, progressCallback, cts);
                _taskQueue.Enqueue(updateSongs);
            }

            if (e.ToolType == ToolType.RenameTool)
            {
                TaskQueueViewModel = new TaskQueueViewModel(_taskQueue);
                ReportProgress_Callback progressCallback = new ReportProgress_Callback(TaskQueue_Progress);
                CancellationTokenSource cts = new CancellationTokenSource();
                _renameWorker = new RenameBackgroundWorker(cts.Token);
                TaskQueueItem renameFiles = new TaskQueueItem(_renameWorker, e, progressCallback, cts);
                _taskQueue.Enqueue(renameFiles);
            }
        }
        private void _vmAlbums_PlaylistChanged(object sender, PlaylistEventArgs e)
        {
            if (e.QueueMode == QueueMode.Replace)
            {
                if ((_vmPlayList.LockOverwrite == false) || (e.UnlockPlaylist == true))
                {
                    AddSongsToPlaylist(sender, e);
                }
                else
                {
                    _vmAlbums.AskOverwritePlaylist(_lastPlayCommandSource);
                }  
            }
            else
            {
                // QueueMode = Inser or Append
                AddSongsToPlaylist(sender, e);
            }
             
        }
        private void AddSongsToPlaylist(object sender, PlaylistEventArgs e)
        {
            if (sender is AlbumViewModel)
            {
                _lastPlayCommandSource = CommandSourceViewModel.AlbumViewModel;
            }
            else
            {
                _lastPlayCommandSource = CommandSourceViewModel.AlbumListViewModel;
            }

            if (e.Songs.Count > 0)
            {
                _vmRibbon.UnselectPartyButton();
                var task1 = Task.Run(() => _vmPlayList.AddSongs(e));

                try
                {
                    task1.Wait();
                }
                catch (AggregateException ae)
                {
                    String errorMessage = "Error in AddSongs from _vmPlayList";
                    ShowError.ShowAndLog(ae, errorMessage, 1);
                }
            }
        }

        private void _toolbarViewModel_AlphabetButtonClicked(object sender, AlphabetItemEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Item.Character) == false)
            {
                var task1 = Task.Run(() => _vmBands.GetBandList(e.Item.Character));

                try
                {
                    task1.Wait();
                }
                catch (AggregateException ae)
                {
                    String errorMessage = "Error in GetBandList from vmBands";
                    ShowError.ShowAndLog(ae, errorMessage, 1);
                }

                CurrentViewModel = _vmBands;
            }
        }
        private void _toolbarViewModel_VaGenresButtonClicked(object sender, EventArgs e)
        {
            var task1 = Task.Run(() => _vmAlbumGenres.GetAlbumGenreList());

            try
            {
                task1.Wait();
            }
            catch (AggregateException ae)
            {
                String errorMessage = "Error in GetAlbumGenreList from vmAlbumGenres";
                ShowError.ShowAndLog(ae, errorMessage, 1);
            }

            CurrentViewModel = _vmAlbumGenres;
        }
        private void _toolbarViewModel_ShowVaBandsChanged(object sender, BooleanEventArgs e)
        {
            Task.Run(() => RefreshView());
        }
        private async void _toolbarViewModel_Search(object sender, StringEventArgs e)
        {
            try
            {
                await _vmAlbums.GetAlbumList(e.Name);
            }
            catch (Exception Err)
            {
                String errorMessage = "Error GetAlbumList from vmAlbums";
                ShowError.ShowAndLog(Err, errorMessage, 1);
            }
            
            CurrentViewModel = _vmAlbums;
        }
        private void _toolbarViewModel_SearchBands(object sender, StringEventArgs e)
        {
            var task1 = Task.Run(() => _vmBands.SearchBands(e.Name));

            try
            {
                task1.Wait();
            }
            catch (AggregateException ae)
            {
                String errorMessage = "Error in SearchBands from vmBands";
                ShowError.ShowAndLog(ae, errorMessage, 1);
            }

            CurrentViewModel = _vmBands;
        }
        private void _toolbarViewModel_AlbumListViewExpandedClicked(object sender, BooleanEventArgs e)
        {
            _vmAlbums.AlbumListViewExpanded = e.Value;
        }
        private void _ribbonViewModel_AddSongsRequest(object sender, FolderListEventArgs e)
        {
            TaskQueueViewModel = new TaskQueueViewModel(_taskQueue);
            ReportProgress_Callback progressCallback = new ReportProgress_Callback(TaskQueue_Progress);
            CancellationTokenSource cts = new CancellationTokenSource();

            _addSongsWorker = new AddSongsBackgroundWorker(cts.Token);
            TaskQueueItem addSongs = new TaskQueueItem(_addSongsWorker, e.FolderList, progressCallback, cts);
            _taskQueue.Enqueue(addSongs);
        }
        private void _ribbonViewModel_ManageImagesRequest(object sender, FolderListEventArgs e)
        {
            TaskQueueViewModel = new TaskQueueViewModel(_taskQueue);
            ReportProgress_Callback progressCallback = new ReportProgress_Callback(TaskQueue_Progress);
            CancellationTokenSource cts = new CancellationTokenSource();

            _coverImagesWorker = new CoverImagesBackgroundWorker(cts.Token);
            TaskQueueItem manageImages = new TaskQueueItem(_coverImagesWorker, e.FolderList, progressCallback, cts);
            _taskQueue.Enqueue(manageImages);
        }
        private void _vmRibbon_PartyButtonDesignerRequest(object sender, EventArgs e)
        {
            PartyButtonDesignerViewModel vmPartyButton = new PartyButtonDesignerViewModel(AppSettings.DatabaseSettings.DefaultDatabase, _vmRibbon.PlaylistConfigurations);
            frmPartyButtonDesigner frmParty = new frmPartyButtonDesigner(vmPartyButton);
            frmParty.ShowDialog();

            if ((frmParty.DialogResult.HasValue == true) && (frmParty.DialogResult == true))
            {
                _vmRibbon.PlaylistConfigurationsChanged = true;
            }
        }
        private void _vmRibbon_PlaylistChanged(object sender, PlaylistEventArgs e)
        {
            _lastPlayCommandSource = CommandSourceViewModel.RibbonPartyButton;
            if (e.Songs.Count > 0)
            {
                var task1 = Task.Run(() => _vmPlayList.AddSongs(e));

                try
                {
                    task1.Wait();
                }
                catch (AggregateException ae)
                {
                    String errorMessage = "Error in AddSongs from vmAudioPlayer";
                    ShowError.ShowAndLog(ae, errorMessage, 1);
                }
            }
        }
        private void _vmRibbon_PartyButton_Click(object sender, PartyButtonConfigEventArgs e)
        {
            if (e.PartyButtonConfig.ButtonLabel != _vmPlayList.Headline)
            {
                Task.Run(() => _vmPlayList.LoadPlaylist(e.PartyButtonConfig));
            }
        }
        private void _vmRibbon_LanguageChanged(object sender, EventArgs e)
        {
            if (_vmAudioPlayer != null)
            {
                _vmAudioPlayer.Localize();
            }

            if (_vmAlbums != null)
            {
                _vmAlbums.Localize();
            }

            if (_vmAlbumGenres != null)
            {
                _vmAlbumGenres.Localize();
            }

            if (_vmBands != null)
            {
                _vmBands.Localize();
            }

            if (_vmDatabases != null)
            {
                _vmDatabases.Localize();
            }

            if (_vmPlayList != null)
            {
                _vmPlayList.Localize();
            }

            if (_vmRibbon != null)
            {
                _vmRibbon.Localize();
            }

            if (_vmTaskQueue != null)
            {
                _vmTaskQueue.Localize();
            }

            if (_vmToolbar != null)
            {
                _vmToolbar.Localize();
            }
        }
        private void _vmRibbon_WikipediaChanged(object sender, StringEventArgs e)
        {
            if (_vmAlbums != null)
            {
                _vmAlbums.SetWikipediaLanguage(e.Name);
            }
        }
        private void TaskQueue_Progress(ProgressDataViewModel progress)
        {
            if (_vmTaskQueue != null)
            {
                _vmTaskQueue.ProgressData = progress;
            }

            if (progress.FileWriteAccessDenied == true)
            {
                _vmAlbums.NotifyFileAccessDenied(_lastUpdatePropertiesCommandSource, progress.CurrentFolder);
            }
        }
        private async void TaskQueue_WorkDone(object sender, EventArgs e)
        {
            if (_vmTaskQueue.ProgressData != null)
            {
                if (_vmTaskQueue.ProgressData.ActionName == "Add Songs")
                {
                    _vmToolbar.Initialize();

                    try
                    {
                        await RefreshView();
                    }
                    catch (Exception Err)
                    {
                        String errorMessage = "Error when calling RefreshView";
                        ShowError.ShowAndLog(Err, errorMessage, 1);
                    }
                }

                if (_vmTaskQueue.ProgressData.ActionName == "Manage Images")
                {
                    try
                    {
                        await RefreshView();
                    }
                    catch (Exception Err)
                    {
                        String errorMessage = "Error when calling RefreshView";
                        ShowError.ShowAndLog(Err, errorMessage, 1);
                    }
                }

                if (_vmTaskQueue.ProgressData.ActionName == "Update Songs")
                {
                    try
                    {
                        await RefreshView();
                    }
                    catch (Exception Err)
                    {
                        String errorMessage = "Error when calling RefreshView";
                        ShowError.ShowAndLog(Err, errorMessage, 1);
                    }
                }

                if (_vmTaskQueue.ProgressData.ActionName == "Rename Files")
                {
                    try
                    {
                        await RefreshView();
                    }
                    catch (Exception Err)
                    {
                        String errorMessage = "Error when calling RefreshView";
                        ShowError.ShowAndLog(Err, errorMessage, 1);
                    }
                }
            }


            _vmTaskQueue.Dispose();
            TaskQueueViewModel = null;
        }
        private void _statusbarViewModel_CancelRequest(object sender, EventArgs e)
        {
            if (_taskQueue != null)
            {
                _taskQueue.CancelAll();
            }
        }



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

        ~MainWindowViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_vmAudioPlayer != null)
            {
                _vmAudioPlayer.Dispose();
            }
           
            if (_vmAlbums != null)
            {
                _vmAlbums.Dispose();
            }

            if (_vmAlbumGenres != null)
            {
                _vmAlbumGenres.Dispose();
            }

            if (_vmBands != null)
            {
                _vmBands.Dispose();
            }

            if (_vmDatabases != null)
            {
                _vmDatabases.Dispose();
            }

            if (_vmPlayList != null)
            {
                _vmPlayList.Dispose();
            }

            if (_vmRibbon != null)
            {
                _vmRibbon.Dispose();
            }

            if (_vmTaskQueue != null)
            {
                _vmTaskQueue.Dispose();
            }

            if (_vmToolbar != null)
            {
                _vmToolbar.Dispose();
            }

            if (_addSongsWorker != null)
            {
                _addSongsWorker.Dispose();
            }

            if (_coverImagesWorker != null)
            {
                _coverImagesWorker.Dispose();
            }

            if (_updateSongsWorker != null)
            {
                _updateSongsWorker.Dispose();
            }

            if (_renameWorker != null)
            {
                _renameWorker.Dispose();
            }

            if (_taskQueue != null)
            {
                _taskQueue.Dispose();
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