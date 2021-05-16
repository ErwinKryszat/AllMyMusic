using System;
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

using Microsoft.Win32;
using AllMyMusic.Settings;
using AllMyMusic.DataService;

namespace AllMyMusic.ViewModel
{
    public class PlaylistViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ObservableCollection<SongItem> _nonRandomizedSongs;
        private ObservableCollection<SongItem> _randomizedSongs;

        private ObservableCollection<SongItem> _allSongItemCollection;
        private ObservableCollection<SongItem> _pageSongItemCollection;
        private PagerViewModel _pagingHelper;
        private IDataServiceSongs _dataServiceSongs;

        private Boolean _randomize;
        private Int32 _insertSongsIndex = -1;
        private SongItem _song;
        private String _headline;
        private Boolean _lockOverwrite;
        private Double _progressValue;
        private Boolean _progressBarVisible;
        
        private String _playlistFilename;
        private Boolean _playlistChanged;

        private Int32 _indexCurrentSong;

        private Task _addCoverImagesTask = null;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;

        private RelayCommand<object> _openPlaylistCommand;
        private RelayCommand<object> _savePlaylistCommand;
        private RelayCommand<object> _saveAsPlaylistCommand;
        private RelayCommand<object> _deleteSongsCommand;
        private RelayCommand<object> _lockOverwriteCommand;
        private RelayCommand<object> _coverImageCommand;
        #endregion // Fields

        #region Commands
        public ICommand OpenPlaylistCommand
        {
            get
            {
                if (null == _openPlaylistCommand)
                    _openPlaylistCommand = new RelayCommand<object>(ExecuteOpenPlaylistCommand, CanOpenPlaylistCommand);

                return _openPlaylistCommand;
            }
        }
        private void ExecuteOpenPlaylistCommand(object notUsed)
        {
            OpenFileDialog dlgOpenPlaylist = new OpenFileDialog();
            dlgOpenPlaylist.DefaultExt = ".m3u";
            dlgOpenPlaylist.Filter = "Playlist Files(*.m3u;*.pls;*.xspf)|*.m3u;*.pls;*.xspf|All files (*.*)|*.*";
            dlgOpenPlaylist.FilterIndex = 1;
            dlgOpenPlaylist.InitialDirectory = AppSettings.GeneralSettings.PlaylistPath;
            dlgOpenPlaylist.Title = AmmLocalization.GetLocalizedString("frmAllMyMusic_OpenPlaylist");
            dlgOpenPlaylist.FileName = String.Empty;
            dlgOpenPlaylist.ShowDialog();
            dlgOpenPlaylist.ValidateNames = true;
            dlgOpenPlaylist.Multiselect = false;

            if (dlgOpenPlaylist.FileName != "")
            {
                _playlistFilename = dlgOpenPlaylist.FileName;
                AppSettings.GeneralSettings.PlaylistPath = Path.GetDirectoryName(dlgOpenPlaylist.FileName);
                String headline = Path.GetFileNameWithoutExtension(dlgOpenPlaylist.FileName);

                ObservableCollection<String> songList = PlaylistImport.Import(dlgOpenPlaylist.FileName);

                Task.Run(() => ImportSongs(songList, headline, false));

                LockOverwrite = true;
            }
        }
        private bool CanOpenPlaylistCommand(object notUsed)
        {
            return true;
        }

        public ICommand SavePlaylistCommand
        {
            get
            {
                if (null == _savePlaylistCommand)
                    _savePlaylistCommand = new RelayCommand<object>(ExecuteSavePlaylistCommand, CanSavePlaylistCommand);

                return _savePlaylistCommand;
            }
        }
        private void ExecuteSavePlaylistCommand(object notUsed)
        {
            if (_playlistFilename != "")
            {
                AppSettings.GeneralSettings.PlaylistPath = Path.GetDirectoryName(_playlistFilename);
                Headline = Path.GetFileNameWithoutExtension(_playlistFilename);

                PlaylistExport.Export(_playlistFilename, _allSongItemCollection);

                _playlistChanged = false;
            }
        }
        private bool CanSavePlaylistCommand(object notUsed)
        {
            return ((String.IsNullOrEmpty(_playlistFilename) == false) && (_allSongItemCollection != null) && (_allSongItemCollection.Count > 0) && (_playlistChanged == true));
        }

        public ICommand SaveAsPlaylistCommand
        {
            get
            {
                if (null == _saveAsPlaylistCommand)
                    _saveAsPlaylistCommand = new RelayCommand<object>(ExecuteSaveAsPlaylistCommand, CanSaveAsPlaylistCommand);

                return _saveAsPlaylistCommand;
            }
        }
        private void ExecuteSaveAsPlaylistCommand(object notUsed)
        {
            SaveFileDialog dlgSaveAsPlaylist = new SaveFileDialog();
            dlgSaveAsPlaylist.DefaultExt = ".m3u";
            dlgSaveAsPlaylist.AddExtension = true;
            dlgSaveAsPlaylist.Filter = "Playlist Files(*.m3u;*.pls;*.xspf)|*.m3u;*.pls;*.xspf|All files (*.*)|*.*";
            dlgSaveAsPlaylist.FilterIndex = 1;
            dlgSaveAsPlaylist.InitialDirectory = AppSettings.GeneralSettings.PlaylistPath;
            dlgSaveAsPlaylist.Title = AmmLocalization.GetLocalizedString("frmAllMyMusic_SaveAsPlaylist");
            dlgSaveAsPlaylist.FileName = String.Empty;
            dlgSaveAsPlaylist.ShowDialog();
            dlgSaveAsPlaylist.ValidateNames = true;


            if (dlgSaveAsPlaylist.FileName != "")
            {
                _playlistFilename = dlgSaveAsPlaylist.FileName;
                AppSettings.GeneralSettings.PlaylistPath = Path.GetDirectoryName(dlgSaveAsPlaylist.FileName);
                Headline = Path.GetFileNameWithoutExtension(dlgSaveAsPlaylist.FileName);

                PlaylistExport.Export(dlgSaveAsPlaylist.FileName, _allSongItemCollection);
            }
        }
        private bool CanSaveAsPlaylistCommand(object notUsed)
        {
            return ((_allSongItemCollection != null) && (_allSongItemCollection.Count > 0) && (_playlistChanged == true));
        }

        public ICommand DeleteSongsCommand
        {
            get
            {
                if (null == _deleteSongsCommand)
                    _deleteSongsCommand = new RelayCommand<object>(ExecuteDeleteSongsCommand, CanDeleteSongsCommand);

                return _deleteSongsCommand;
            }
        }
        private void ExecuteDeleteSongsCommand(object notUsed)
        {
            ObservableCollection<SongItem> _selectedSongs = GetSelectedSongs();
            for (int i = 0; i < _selectedSongs.Count; i++)
            {
                _nonRandomizedSongs.Remove(_selectedSongs[i]);
                _randomizedSongs.Remove(_selectedSongs[i]);
                _pageSongItemCollection.Remove(_selectedSongs[i]);
            }
            _playlistChanged = true;
            OnAllSongItemCountChanged();
        }
        private bool CanDeleteSongsCommand(object notUsed)
        {
            return ((_allSongItemCollection != null) && (_allSongItemCollection.Count > 0));
        }

        public ICommand LockOverwriteCommand
        {
            get
            {
                if (null == _lockOverwriteCommand)
                    _lockOverwriteCommand = new RelayCommand<object>(ExecuteLockOverwriteCommand, CanLockOverwriteCommand);

                return _lockOverwriteCommand;
            }
        }
        private void ExecuteLockOverwriteCommand(object notUsed)
        {
          
        }
        private bool CanLockOverwriteCommand(object notUsed)
        {
            return ((_allSongItemCollection != null) && (_allSongItemCollection.Count > 0));
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
            frmCoverImage frmCover = new frmCoverImage(_song);
            frmCover.Show();
        }
        private bool CanCoverImageCommand(object notUsed)
        {
            return (String.IsNullOrEmpty(_song.FrontImageFileName) == false);
        }
        #endregion

        #region Presentation Properties
        public ObservableCollection<SongItem> AllSongItemCollection
        {
            get { return _allSongItemCollection; }
            set
            {
                _allSongItemCollection = value;

                RaisePropertyChanged("AllSongItemCollection");
            }
        }
        public ObservableCollection<SongItem> PageSongItemCollection
        {
            get { return _pageSongItemCollection; }
            set
            {
                _pageSongItemCollection = value;

                RaisePropertyChanged("PageSongItemCollection");
            }
        }
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
        public Boolean Randomize
        {
            get { return _randomize; }
            set
            {
                _randomize = value;

                RandomizeSongs();

                if (_randomize == true)
                {
                    _allSongItemCollection = _randomizedSongs;
                    _indexCurrentSong = IndexOfSongRandomized(_song);
                }
                else
                {
                    _allSongItemCollection = _nonRandomizedSongs;
                    _indexCurrentSong = IndexOfSongNonRandomized(_song);
                }
                LoadPageOfSongItems();
                RaisePropertyChanged("Randomize");
            }
        }
        public Boolean HeadlineVisible
        {
            get { return (String.IsNullOrEmpty(_headline) == false); }
        }
        public Boolean LockOverwrite
        {
            get { return _lockOverwrite; }
            set
            {
                if (value == _lockOverwrite)
                    return;

                _lockOverwrite = value;

                RaisePropertyChanged("LockOverwrite");
            }
        }
        public Double ProgressValue
        {
            get { return _progressValue; }
            set
            {
                if (value == _progressValue)
                    return;

                _progressValue = value;

                RaisePropertyChanged("ProgressValue");
            }
        }
        public Boolean ProgressBarVisible
        {
            get { return _progressBarVisible; }
            set
            {
                if (value == _progressBarVisible)
                    return;

                _progressBarVisible = value;

                RaisePropertyChanged("ProgressBarVisible");
            }
        }
        public String Headline
        {
            get { return _headline; }
            set
            {
                if ((value == _headline) || (value == null))
                    return;

                _headline = value;
                RaisePropertyChanged("Headline");
                RaisePropertyChanged("HeadlineVisible");
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
        public Int32 InsertSongsIndex
        {
            get { return _insertSongsIndex; }
            set
            {
                if (value == _insertSongsIndex)
                    return;

                _insertSongsIndex = value;

                if (_insertSongsIndex >= 0) 
                {
                    DragAndDrop();
                }

                RaisePropertyChanged("InsertSongsIndex");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public PlaylistViewModel(ConnectionInfo conInfo)
        {
            _pagingHelper = new PagerViewModel();
            _nonRandomizedSongs = new ObservableCollection<SongItem>();
            _randomizedSongs = new ObservableCollection<SongItem>();

            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }

            Localize();
        }
        #endregion  // Constructor

        public async Task AddSongs(PlaylistEventArgs args)
        {
            if (String.IsNullOrEmpty(args.Headline) == false)
            {
                 Headline = args.Headline;
            }
           

            if (args.UnlockPlaylist == true)
            {
                LockOverwrite = false;
            }

            switch (args.QueueMode)
            {
                case QueueMode.Replace:
                    _randomize = args.PlayRandom;
                    await ReplaceSongs(args.Songs);
                    Randomize = args.PlayRandom;
                    break;
                case QueueMode.Insert:
                    await InsertSongs(args.Songs);
                    _playlistChanged = true;
                    break;
                case QueueMode.Append:
                    await AppendSongs(args.Songs);
                    _playlistChanged = true;
                    break;
                default:
                    _nonRandomizedSongs = args.Songs;
                    _randomizedSongs = args.Songs;
                    break;
            }
            
            LoadPageOfSongItems();

            _playlistChanged = true;
        }

        public async Task LoadPlaylist(PartyButtonConfigViewModel playlistConfig)
        {
            switch (playlistConfig.ButtonType)
            {
                case PartyButtonType.Songlist:
                    await GetPlaylist_Songlist(playlistConfig);
                    break;
                case PartyButtonType.Albumlist:
                    await GetPlaylist_Albumlist(playlistConfig);
                    break;
                case PartyButtonType.Query:
                    await GetPlaylist_SqlQuery(playlistConfig);
                    break;
                default:
                    break;
            }
        }
        private async Task ImportSongs(ObservableCollection<String> songPathNames, String headline, Boolean randomize)
        {
            _nonRandomizedSongs = new ObservableCollection<SongItem>();
            _randomizedSongs = new ObservableCollection<SongItem>();

            Headline = headline;

            Boolean startedAudioPlayback = false;
            ObservableCollection<SongItem> someSongs = new ObservableCollection<SongItem>();
            Int32 done = 0;

            ClearPlayList();
            ProgressBarVisible = true;
            for (int i = 0; i < songPathNames.Count; i++)
            {
                if (done == 0)
                {
                    someSongs = new ObservableCollection<SongItem>();
                }
                SongItem song = await _dataServiceSongs.GetSongByPath(songPathNames[i]);
                someSongs.Add(song);


                done++;

                if (done > 9)
                {
                    done = 0;
                    PlaylistEventArgs args = new PlaylistEventArgs(someSongs, QueueMode.Append, headline, randomize);
                    await AppendSongs(args.Songs);

                    ProgressValue = (Double)i / songPathNames.Count * 100;

                    if (startedAudioPlayback == false)
                    {
                        LoadPageOfSongItems();

                        startedAudioPlayback = true;
                        OnPlaylistReady(this, new EventArgs());
                    }
                }
            }

            if (done > 0)
            {
                ProgressValue = 100;
                PlaylistEventArgs args = new PlaylistEventArgs(someSongs, QueueMode.Append, headline, randomize);
                await AddSongs(args);
            }
            ProgressValue = 0;
            ProgressBarVisible = false;

            _playlistChanged = false;

            LoadPageOfSongItems();
        }
     
        private void OnAllSongItemCountChanged()
        {
            _pagingHelper.ItemCount = _allSongItemCollection.Count;
            RaisePropertyChanged("PageSongItemCollection");
        }
        private void LoadPageOfSongItems()
        {
            _pagingHelper = new PagerViewModel("Songs", _allSongItemCollection.Count, 100);
            _pagingHelper.PageChanged += new PagerViewModel.PageChangedEventHandler(PagingHelper_PageChanged);
            RaisePropertyChanged("Pager");

            RaisePropertyChanged("AllAlbumItemCollection");

            Load_PageSongItemCollection();
        }
        private void RandomizeSongs()
        {
            if (_randomize == true)
            {
                _randomizedSongs = new ObservableCollection<SongItem>(_nonRandomizedSongs.OrderBy(a => a.SongGuid));
            }
        }
        private async Task ReplaceSongs(ObservableCollection<SongItem> songs)
        {
            await Task.Run(() =>
            {
                _playlistFilename = String.Empty;
                ClearPlayList();

                if (_lockOverwrite == false)
                {
                   
                    for (int i = 0; i < songs.Count; i++)
                    {
                        songs[i].IsSelected = false;
                        _nonRandomizedSongs.Add(songs[i]);
                    }

                    for (int i = 0; i < songs.Count; i++)
                    {
                        _randomizedSongs.Add(songs[i]);
                    }
                }
                else
                {
                    EventArgs args = new EventArgs();
                    OnTryOverwritedLockedPlaylist(this, args);
                }

                RandomizeSongs();

                if (_randomize)
                {
                    _allSongItemCollection = _randomizedSongs;
                }
                else
                {
                    _allSongItemCollection = _nonRandomizedSongs;
                }

            });

            OnPlaylistReady(this, new EventArgs());
        }
        private async Task AppendSongs(ObservableCollection<SongItem> songs)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < songs.Count; i++)
                {
                    songs[i].IsSelected = false;
                    _nonRandomizedSongs.Add(songs[i]);
                }

                for (int i = 0; i < songs.Count; i++)
                {
                    _randomizedSongs.Add(songs[i]);
                }
            } );
        }
        private async Task InsertSongs(ObservableCollection<SongItem> songs)
        {
            await Task.Run(() =>
            {
                Int32 insertPosition = IndexOfSongNonRandomized(_song);
                if (insertPosition < _nonRandomizedSongs.Count)
                {
                    insertPosition += 1;
                }
     
                for (int i = songs.Count -1; i >= 0; i--)
                {
                    _nonRandomizedSongs.Insert(insertPosition, songs[i]);
                }


                insertPosition = IndexOfSongRandomized(_song);
                if (insertPosition < _randomizedSongs.Count)
                {
                    insertPosition += 1;
                }

                for (int i = songs.Count - 1; i >= 0; i--)
                {
                    _randomizedSongs.Insert(insertPosition, songs[i]);
                }
            });
        }

        private async Task GetPlaylist_SqlQuery(PartyButtonConfigViewModel playlistConfig)
        {
            if (playlistConfig.Playlist == null)
            {
                playlistConfig.Playlist = new ObservableCollection<SongItem>();
                if (String.IsNullOrEmpty(playlistConfig.SqlQuery) == false)
                {
                    playlistConfig.Playlist = await _dataServiceSongs.GetSongs(playlistConfig.SqlQuery);

                    PlaylistEventArgs args = new PlaylistEventArgs(playlistConfig.Playlist, QueueMode.Replace, playlistConfig.ButtonLabel, false, playlistConfig.Randomize);
                    await AddSongs(args);
                }
                else
                {
                    //MessageBox.Show("No query defined for this button!");
                }
            }
            else
            {
                PlaylistEventArgs args = new PlaylistEventArgs(playlistConfig.Playlist, QueueMode.Replace, playlistConfig.ButtonLabel, false, playlistConfig.Randomize);
                await AddSongs(args);
            }

            //OnPlaylistReady(this, new EventArgs());
            Headline = playlistConfig.ButtonLabel;
        }
        private async Task GetPlaylist_Songlist(PartyButtonConfigViewModel playlistConfig)
        {
            if (playlistConfig.Playlist == null)
            {
                await ImportSongs(playlistConfig.SongPathNames, playlistConfig.ButtonLabel, playlistConfig.Randomize);
                playlistConfig.Playlist = _allSongItemCollection;
            }
            else
            {
                PlaylistEventArgs args = new PlaylistEventArgs(playlistConfig.Playlist, QueueMode.Replace, playlistConfig.ButtonLabel, playlistConfig.Randomize);
                await AddSongs(args);
            }
            
            //OnPlaylistReady(this, new EventArgs());
            Headline = playlistConfig.ButtonLabel;
        }
        private async Task GetPlaylist_Albumlist(PartyButtonConfigViewModel playlistConfig)
        {
            if (playlistConfig.Playlist == null)
            {
                ClearPlayList();

                ProgressBarVisible = true;
                Boolean startedAudioPlayback = false;
                ObservableCollection<SongItem> songs = new ObservableCollection<SongItem>();

                for (int i = 0; i < playlistConfig.AlbumPathNames.Count; i++)
                {
                    songs = await _dataServiceSongs.GetSongsByPathPart(playlistConfig.AlbumPathNames[i]);

                    PlaylistEventArgs args = new PlaylistEventArgs(songs, QueueMode.Append, playlistConfig.ButtonLabel, playlistConfig.Randomize);
                    await AppendSongs(args.Songs);

                    ProgressValue = i / playlistConfig.AlbumPathNames.Count * 100;

                    if (startedAudioPlayback == false)
                    {
                        LoadPageOfSongItems();
                        OnPlaylistReady(this, new EventArgs());
                        startedAudioPlayback = true;
                    }
                }

                playlistConfig.Playlist = _allSongItemCollection;
                ProgressValue = 0;
                ProgressBarVisible = false;
            }
            else
            {
                PlaylistEventArgs args = new PlaylistEventArgs(playlistConfig.Playlist, QueueMode.Replace, playlistConfig.ButtonLabel, playlistConfig.Randomize);
                await AddSongs(args);

                //OnPlaylistReady(this, new EventArgs());
            }

            
            Headline = playlistConfig.ButtonLabel;
            LoadPageOfSongItems();
        }

        public SongItem GetCurrentSong()
        {
            if (_song != null)
            {
                _song.IsNowPlaying = false;
            }
            Song = _allSongItemCollection[_indexCurrentSong];
            _song.IsNowPlaying = true;
            return _song;   
        }
        public SongItem GetNextSong()
        {
            if (_song != null)
            {
                _song.IsNowPlaying = false;
            }
            _allSongItemCollection[_indexCurrentSong].IsNowPlaying = false;
            if (_indexCurrentSong < (_allSongItemCollection.Count - 1))
            {
                _indexCurrentSong++;
            }
            else
            {
                _indexCurrentSong = 0;
            }

            Song = _allSongItemCollection[_indexCurrentSong];
            _song.IsNowPlaying = true;
            return _song;
        }
        public SongItem GetPreviousSong()
        {
            if (_song != null)
            {
                _song.IsNowPlaying = false;
            }
            _allSongItemCollection[_indexCurrentSong].IsNowPlaying = false;
            if (_indexCurrentSong > 0)
            {
                _indexCurrentSong--;
            }
            else
            {
                _indexCurrentSong = _allSongItemCollection.Count - 1;
            }

            Song = _allSongItemCollection[_indexCurrentSong];
            _song.IsNowPlaying = true;
            return _song;
        }

     
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            _dataServiceSongs.ChangeDatabase(conInfo);
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.SqlServer)
            {
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.MySql)
            {
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }
        }
        private Int32 IndexOfSongNonRandomized(SongItem song)
        {
            if ((_nonRandomizedSongs != null) && (song != null))
            {
                for (int i = 0; i < _nonRandomizedSongs.Count; i++)
                {
                    if (_nonRandomizedSongs[i] == song)
                    {
                        return i;
                    }
                }
            }

            return 0;
        }
        private Int32 IndexOfSongRandomized(SongItem song)
        {
            if ((_randomizedSongs != null) && (song != null))
            {
                for (int i = 0; i < _randomizedSongs.Count; i++)
                {
                    if (_randomizedSongs[i] == song)
                    {
                        return i;
                    }
                }
            }

            return 0;
        }

        #region Drag_Drop
        private ObservableCollection<SongItem> GetSelectedSongs()
        {
            ObservableCollection<SongItem> _selectedSongs = new ObservableCollection<SongItem>();
            for (int i = 0; i < _allSongItemCollection.Count; i++)
            {
                if (_allSongItemCollection[i].IsSelected == true)
                {
                    _selectedSongs.Add(_allSongItemCollection[i]);
                    _allSongItemCollection[i].IsSelected = false;
                }
            }
            return _selectedSongs;
        }
        private void DragAndDrop()
        {
            ObservableCollection<SongItem> _selectedSongs = GetSelectedSongs();

            if ((_selectedSongs != null) && (_selectedSongs.Count > 0))
            {
                int absoluteInsertIndex = _insertSongsIndex + ((_pagingHelper.Page -1 ) * _pagingHelper.ItemsPerPage);
                int indexFirstSelectedSong = IndexOfSong(_selectedSongs[0]);

                SongItem songWherWeInsertSongs = _allSongItemCollection[absoluteInsertIndex];

                for (int i = 0; i < _selectedSongs.Count; i++)
                {
                    _allSongItemCollection.Remove((SongItem)_selectedSongs[i]);
                }
                
                absoluteInsertIndex = IndexOfSong(songWherWeInsertSongs);
                InsertSongs(_selectedSongs, absoluteInsertIndex);
                _indexCurrentSong = IndexOfSong(_song);
                RaisePropertyChanged("AllSongItemCollection");

                Load_PageSongItemCollection();
            }
        }
        private Int32 IndexOfSong(SongItem song)
        {
            if ((_allSongItemCollection != null) && (song != null))
            {
                for (int i = 0; i < _allSongItemCollection.Count; i++)
                {
                    if (_allSongItemCollection[i] == song)
                    {
                        return i;
                    }
                }
            }

            return 0;
        }
        private void InsertSongs(ObservableCollection<SongItem> songs, Int32 insertPosition)
        {
            for (int i = songs.Count - 1; i >= 0; i--)
            {
                _allSongItemCollection.Insert(insertPosition, (SongItem)songs[i]);
            }
        }
        #endregion

        private void ClearPlayList()
        {
            _nonRandomizedSongs = new ObservableCollection<SongItem>();
            _randomizedSongs = new ObservableCollection<SongItem>();
            _indexCurrentSong = 0;
            _playlistChanged = false;

            if (_randomize)
            {
                _allSongItemCollection = _randomizedSongs;
            }
            else
            {
                _allSongItemCollection = _nonRandomizedSongs;
            }
        }

        private void PagingHelper_PageChanged(object sender, EventArgs e)
        {
            Load_PageSongItemCollection();
        }
        private void Load_PageSongItemCollection()
        {
            if (_allSongItemCollection.Count > 0)
            {
                if ((_addCoverImagesTask != null) && (_addCoverImagesTask.Status == TaskStatus.Running))
                {
                    if (_tokenSource != null)
                    {
                        _tokenSource.Cancel();
                    }
                }

                _tokenSource = new CancellationTokenSource();
                _token = _tokenSource.Token;
                _addCoverImagesTask = Task.Factory.StartNew(() => Background_LoadPage(_token, _allSongItemCollection), _token);                
            }
            else
            {
                PageSongItemCollection = _allSongItemCollection;
            }
        }
        private void Background_LoadPage(CancellationToken ct, ObservableCollection<SongItem> songs)
        {
            LoadPageItems(ct, songs);
            LoadCoverImages(ct);
        }
        private void LoadPageItems(CancellationToken ct, ObservableCollection<SongItem> songs)
        {
            Int32 updateFrequency = 10;
            Int32 itemsAddedCounter = 0;

            ObservableCollection<SongItem> itemList = new ObservableCollection<SongItem>();
            for (int i = _pagingHelper.StartIndex; i <= _pagingHelper.EndIndex; i++)
            {
                if (ct.IsCancellationRequested == true)
                {
                    break;
                }
                itemList.Add(songs[i]);
                itemsAddedCounter++;

                if (itemsAddedCounter >= updateFrequency)
                {
                    itemsAddedCounter = 0;
                    PageSongItemCollection = itemList;
                }
            }
            PageSongItemCollection = itemList;
        }
        private void LoadCoverImages(CancellationToken ct)
        {
            Int32 updateFrequency = 50;
            Int32 itemsProcessedCounter = 0;

            for (int i = 0; i < _pageSongItemCollection.Count; i++)
            {
                if (ct.IsCancellationRequested == true)
                {
                    break;
                }

                SongItem song = _pageSongItemCollection[i];
                String imagePath = song.StampImageFullpath;

                if ((File.Exists(imagePath) == true) && (song.StampImage == null))
                {
                    using (FileStream imageFileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        Byte[] imageBytes = new byte[imageFileStream.Length];
                        Int32 bytesRead = imageFileStream.Read(imageBytes, 0, (int)imageFileStream.Length);

                        if (bytesRead == imageFileStream.Length)
                        {
                            using (MemoryStream imageMemStream = new MemoryStream(imageBytes))
                            {
                                Application.Current.Dispatcher.Invoke((Action)(delegate
                                {
                                    BitmapImage thumbNailImage = new BitmapImage();
                                    thumbNailImage.BeginInit();
                                    thumbNailImage.StreamSource = imageMemStream;
                                    thumbNailImage.CacheOption = BitmapCacheOption.OnLoad;
                                    thumbNailImage.EndInit();
                                    thumbNailImage.Freeze();
                                    song.StampImage = thumbNailImage;

                                }));
                            }
                        }
                    }
                }

                if (song.StampImage == null)
                {
                    Application.Current.Dispatcher.Invoke((Action)(delegate
                    {
                        Uri imageReference = new Uri(Global.Images + "cover.jpg", UriKind.Relative);
                        BitmapImage thumbNailImage = new BitmapImage();
                        thumbNailImage.BeginInit();
                        thumbNailImage.UriSource = imageReference;
                        thumbNailImage.EndInit();

                        song.StampImage = thumbNailImage;
                    }));
                }

                itemsProcessedCounter = 0;
                if (itemsProcessedCounter >= updateFrequency)
                {
                    itemsProcessedCounter = 0;
                    //RaisePropertyChanged("PageSongItemCollection");
                }
                RaisePropertyChanged("PageSongItemCollection");
            }
            // RaisePropertyChanged("PageSongItemCollection");
        }

        #region Events
        public delegate void PlaylistReadyEventHandler(object sender, EventArgs e);
        public event PlaylistReadyEventHandler PlaylistReady;
        protected virtual void OnPlaylistReady(object sender, EventArgs e)
        {
            if (this.PlaylistReady != null)
            {
                this.PlaylistReady(this, e);
            }
        }

        public delegate void TryOverwritedLockedPlaylistEventHandler(object sender, EventArgs e);
        public event TryOverwritedLockedPlaylistEventHandler TryOverwritedLockedPlaylist;
        protected virtual void OnTryOverwritedLockedPlaylist(object sender, EventArgs e)
        {
            if (this.TryOverwritedLockedPlaylist != null)
            {
                this.TryOverwritedLockedPlaylist(this, e);
            }
        }
        #endregion // Events

        #region Localization

        private String _cmd_DeleteSongsPL_ToolTip;
        private String _cmd_OpenPlaylist_ToolTip;
        private String _cmd_SavePlaylist_ToolTip;
        private String _cmd_SaveAsPlaylist_ToolTip;
        private String _cmd_LockOverwrite_ToolTip;

        public String Cmd_DeleteSongsPL_ToolTip
        {
            get { return _cmd_DeleteSongsPL_ToolTip; }
            set
            {
                if (value == _cmd_DeleteSongsPL_ToolTip)
                    return;

                _cmd_DeleteSongsPL_ToolTip = value;

                RaisePropertyChanged("Cmd_DeleteSongsPL_ToolTip");
            }
        }
        public String Cmd_OpenPlaylist_ToolTip
        {
            get { return _cmd_OpenPlaylist_ToolTip; }
            set
            {
                if (value == _cmd_OpenPlaylist_ToolTip)
                    return;

                _cmd_OpenPlaylist_ToolTip = value;

                RaisePropertyChanged("Cmd_OpenPlaylist_ToolTip");
            }
        }
        public String Cmd_SavePlaylist_ToolTip
        {
            get { return _cmd_SavePlaylist_ToolTip; }
            set
            {
                if (value == _cmd_SavePlaylist_ToolTip)
                    return;

                _cmd_SavePlaylist_ToolTip = value;

                RaisePropertyChanged("Cmd_SavePlaylist_ToolTip");
            }
        }
        public String Cmd_SaveAsPlaylist_ToolTip
        {
            get { return _cmd_SaveAsPlaylist_ToolTip; }
            set
            {
                if (value == _cmd_SaveAsPlaylist_ToolTip)
                    return;

                _cmd_SaveAsPlaylist_ToolTip = value;

                RaisePropertyChanged("Cmd_SaveAsPlaylist_ToolTip");
            }
        }
        public String Cmd_LockOverwrite_ToolTip
        {
            get { return _cmd_LockOverwrite_ToolTip; }
            set
            {
                if (value == _cmd_LockOverwrite_ToolTip)
                    return;

                _cmd_LockOverwrite_ToolTip = value;

                RaisePropertyChanged("Cmd_LockOverwrite_ToolTip");
            }
        }
        public void Localize()
        {
            Cmd_DeleteSongsPL_ToolTip = AmmLocalization.GetLocalizedString("cmd_DeleteSongsPL_ToolTip");
            Cmd_OpenPlaylist_ToolTip = AmmLocalization.GetLocalizedString("cmd_OpenPlaylist_ToolTip");
            Cmd_SavePlaylist_ToolTip = AmmLocalization.GetLocalizedString("cmd_SavePlaylist_ToolTip");
            Cmd_SaveAsPlaylist_ToolTip = AmmLocalization.GetLocalizedString("cmd_SaveAsPlaylist_ToolTip");
            Cmd_LockOverwrite_ToolTip = AmmLocalization.GetLocalizedString("cmd_LockOverwrite_ToolTip");
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

        ~PlaylistViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
