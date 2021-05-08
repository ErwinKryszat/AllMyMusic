using System;
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

using Microsoft.Win32;
using AllMyMusic.Settings;
using AllMyMusic.DataService;

namespace AllMyMusic.ViewModel
{
    public class SongPlaylistViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ConnectionInfo _conInfo;
        private IDataServiceSongs _dataServiceSongs;
        private IDataServiceListItems _dataServiceListItems;
        private PartyButtonConfigViewModel _playlistConfiguration;

        private QueryComposerViewModel _queryComposerViewModel;

        private SongsViewModel _songsLeft;
        private SongsViewModel _songsRight;

        private AsyncObservableCollection<SongItem> _importSongs = new AsyncObservableCollection<SongItem>();

        private Double _progressValue;
        private Boolean _progressBarVisible;

        private RelayCommand<object> _searchSongsCommand;

        private RelayCommand<object> _addAllSongsCommand;
        private RelayCommand<object> _addSelectedSongsCommand;
        private RelayCommand<object> _removeSelectedSongsCommand;
        private RelayCommand<object> _removeAllSongsCommand;

        private RelayCommand<object> _moveSongsUpCommand;
        private RelayCommand<object> _moveSongsDownCommand;
        #endregion // Fields

        #region Commands
        public ICommand SearchSongsCommand
        {
            get
            {
                if (null == _searchSongsCommand)
                    _searchSongsCommand = new RelayCommand<object>(ExecuteSearchSongsCommand, CanSearchSongsCommand);

                return _searchSongsCommand;
            }
        }
        private void ExecuteSearchSongsCommand(object notUsed)
        {
            Task.Run(() => SearchSongs());
        }
        private bool CanSearchSongsCommand(object notUsed)
        {
            return (String.IsNullOrEmpty(_queryComposerViewModel.SqlPlaylistQuery) == false);
        }

        public ICommand AddAllSongsCommand
        {
            get
            {
                if (null == _addAllSongsCommand)
                    _addAllSongsCommand = new RelayCommand<object>(ExecuteAddAllSongsCommand, CanAddAllSongsCommand);

                return _addAllSongsCommand;
            }
        }
        private void ExecuteAddAllSongsCommand(object notUsed)
        {
            AddAllSongs();
        }
        private bool CanAddAllSongsCommand(object notUsed)
        {
            return ((_songsLeft.Songs != null) && (_songsLeft.Songs.Count > 0));
        }

        public ICommand AddSelectedSongsCommand
        {
            get
            {
                if (null == _addSelectedSongsCommand)
                    _addSelectedSongsCommand = new RelayCommand<object>(ExecuteAddSelectedSongsCommand, CanAddSelectedSongsCommand);

                return _addSelectedSongsCommand;
            }
        }
        private void ExecuteAddSelectedSongsCommand(object notUsed)
        {
            AddSelectedSongs();
        }
        private bool CanAddSelectedSongsCommand(object notUsed)
        {
            return ((_songsLeft.Songs != null) && (_songsLeft.SelectedSongs.Count > 0));
        }

        public ICommand RemoveAllSongsCommand
        {
            get
            {
                if (null == _removeAllSongsCommand)
                    _removeAllSongsCommand = new RelayCommand<object>(ExecuteRemoveAllSongsCommand, CanRemoveAllSongsCommand);

                return _removeAllSongsCommand;
            }
        }
        private void ExecuteRemoveAllSongsCommand(object notUsed)
        {
            RemoveAllSongs();
        }
        private bool CanRemoveAllSongsCommand(object notUsed)
        {
            return ((_songsRight.Songs != null) && (_songsRight.Songs.Count > 0));
        }

        public ICommand RemoveSelectedSongsCommand
        {
            get
            {
                if (null == _removeSelectedSongsCommand)
                    _removeSelectedSongsCommand = new RelayCommand<object>(ExecuteRemoveSelectedSongsCommand, CanRemoveSelectedSongsCommand);

                return _removeSelectedSongsCommand;
            }
        }
        private void ExecuteRemoveSelectedSongsCommand(object notUsed)
        {
            RemoveSelectedSongs();
        }
        private bool CanRemoveSelectedSongsCommand(object notUsed)
        {
            return ((_songsRight.Songs != null) && (_songsRight.SelectedSongs.Count > 0));
        }


        public ICommand MoveSongsUpCommand
        {
            get
            {
                if (null == _moveSongsUpCommand)
                    _moveSongsUpCommand = new RelayCommand<object>(ExecuteMoveSongsUpCommand, CanMoveSongsUpCommand);

                return _moveSongsUpCommand;
            }
        }
        private void ExecuteMoveSongsUpCommand(object notUsed)
        {
            MoveSongsUp();
        }
        private bool CanMoveSongsUpCommand(object notUsed)
        {
            return ((_songsRight != null) && (_songsRight.SelectedSongs.Count > 0));
        }

        public ICommand MoveSongsDownCommand
        {
            get
            {
                if (null == _moveSongsDownCommand)
                    _moveSongsDownCommand = new RelayCommand<object>(ExecuteMoveSongsDownCommand, CanMoveSongsDownCommand);

                return _moveSongsDownCommand;
            }
        }
        private void ExecuteMoveSongsDownCommand(object notUsed)
        {
            MoveSongsDown();
        }
        private bool CanMoveSongsDownCommand(object notUsed)
        {
            return ((_songsRight != null) && (_songsRight.SelectedSongs.Count > 0));
        }

        #endregion

        #region Presentation Properties
        public QueryComposerViewModel QueryComposerViewModel
        {
            get { return _queryComposerViewModel; }
            set
            {
                _queryComposerViewModel = value;

                RaisePropertyChanged("QueryComposerViewModel");
            }
        }
        public PartyButtonConfigViewModel PlaylistConfiguration
        {
            get { return _playlistConfiguration; }
            set
            {
                _playlistConfiguration = value;
                Task.Run(() => OnPlaylistConfigurationChanged());
            }
        }

        public SongsViewModel SongsLeft
        {
            get { return _songsLeft; }
            set
            {
                _songsLeft = value;

                RaisePropertyChanged("SongsLeft");
            }
        }
        public SongsViewModel SongsRight
        {
            get { return _songsRight; }
            set
            {
                _songsRight = value;

                RaisePropertyChanged("SongsRight");
            }
        }
        public double SongsLeftWidth
        {
            get { return _songsLeft.SongListViewWidth; }
            set
            {
                if (value == _songsLeft.SongListViewWidth)
                    return;

                _songsLeft.SongListViewWidth = value;

                RaisePropertyChanged("SongsLeftWidth");
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
        #endregion // Presentation Properties

        #region Constructor
        public SongPlaylistViewModel(ConnectionInfo conInfo)
        {
            _conInfo = conInfo;
            _queryComposerViewModel = new QueryComposerViewModel(_conInfo);

            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
                _dataServiceListItems = new DataServiceListItems_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
                _dataServiceListItems = new DataServiceListItems_MYSQL(conInfo);
            }

            _songsLeft = new SongsViewModel(conInfo);
            _songsRight = new SongsViewModel(conInfo);
        }
        #endregion  // Constructor



        #region Private
        private async Task SearchSongs()
        {
            String condition = " WHERE " + _queryComposerViewModel.SqlPlaylistQuery;
            _songsLeft.Songs = await _dataServiceSongs.GetSongsByCondition(condition);
        }
        private async Task OnPlaylistConfigurationChanged()
        {
            if ((_playlistConfiguration.Playlist != null) && (_playlistConfiguration.Playlist.Count > 0))
            {
                _songsRight.Songs = _playlistConfiguration.Playlist;
                RaisePropertyChanged("SongsRight");
            }
            else 
            {
                if ((_playlistConfiguration.SongPathNames != null) && (_playlistConfiguration.SongPathNames.Count > 0))
                {
                    await ImportSongs(_playlistConfiguration.SongPathNames);
                }
                else
                {
                    _playlistConfiguration.SongPathNames = new ObservableCollection<string>();
                }
            }
        }

        private async Task ImportSongs(ObservableCollection<String> songList)
        {
            _importSongs.Clear();

            ProgressBarVisible = true;
            Int32 done = 0;
            

            for (int i = 0; i < songList.Count; i++)
            {
                SongItem song = await _dataServiceSongs.GetSongByPath(songList[i]);
                _importSongs.Add(song);
                done++;
                if (done > 9)
                {
                    done = 0;

                    ProgressValue = (Double)i / songList.Count * 100;
                }
            }

            _playlistConfiguration.Playlist = _importSongs;
            _songsRight.Songs = _playlistConfiguration.Playlist;
            RaisePropertyChanged("SongsRight");
            ProgressValue = 0;
            ProgressBarVisible = false;
        }

        private void AddAllSongs()
        {
            ObservableCollection<SongItem> songsLeft = _songsLeft.Songs;
            _songsRight.AddSongs(songsLeft);
            _songsLeft.RemoveAllSongs();

            _playlistConfiguration.AddAllSongs(songsLeft);
        }

        private void AddSelectedSongs()
        {
            IList selectedSongs = _songsLeft.SelectedSongs;
            _songsRight.AddSongs(selectedSongs);
            _songsLeft.RemoveSongs(selectedSongs);
            
            _playlistConfiguration.AddSelectedSongs(selectedSongs);
            _songsRight.UnselectAll();
        }

        private void RemoveSelectedSongs()
        {
            IList selectedSongs = _songsRight.SelectedSongs;
            _songsLeft.AddSongs(selectedSongs);
            _songsRight.RemoveSongs(selectedSongs);

            _playlistConfiguration.RemoveSelectedSongs(selectedSongs);
            _songsLeft.UnselectAll();
        }

        private void RemoveAllSongs()
        {
            _songsLeft.AddSongs(_songsRight.Songs);
            _songsRight.RemoveAllSongs();
            _playlistConfiguration.RemoveAllSongs();
        }

        private void MoveSongsUp()
        {
            _songsRight.MoveSelectedSongsUp();
        }

        private void MoveSongsDown()
        {
            _songsRight.MoveSelectedSongsDown();
        }
        #endregion  // Private

        #region Localization
        public void Localize()
        {

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

        ~SongPlaylistViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Dispose();
            }

            if (_dataServiceListItems != null)
            {
                _dataServiceListItems.Dispose();
            }


            if (_queryComposerViewModel != null)
            {
                _queryComposerViewModel.Dispose();
            }

            if (_songsLeft != null)
            {
                _songsLeft.Dispose();
            }

            if (_songsRight != null)
            {
                _songsRight.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
