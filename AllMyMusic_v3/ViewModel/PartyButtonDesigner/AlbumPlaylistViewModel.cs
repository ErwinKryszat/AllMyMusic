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
using AllMyMusic_v3.Settings;
using AllMyMusic_v3.DataService;

namespace AllMyMusic_v3.ViewModel
{
    public class AlbumPlaylistViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ConnectionInfo _conInfo;
        private IDataServiceAlbums _dataServiceAlbums;
        private IDataServiceSongs _dataServiceSongs;
        private IDataServiceListItems _dataServiceListItems;
        private PartyButtonConfigViewModel _playlistConfiguration;

        private QueryComposerViewModel _queryComposerViewModel;
        
        private AlbumListViewModel _albumListLeft;
        private AlbumListViewModel _albumListRight;

        private ObservableCollection<AlbumItem> _importAlbums = new AsyncObservableCollection<AlbumItem>();

        private Double _progressValue;
        private Boolean _progressBarVisible;

        private RelayCommand<object> _searchAlbumsCommand;

        private RelayCommand<object> _addAllAlbumsCommand;
        private RelayCommand<object> _addSelectedAlbumsCommand;
        private RelayCommand<object> _removeSelectedAlbumsCommand;
        private RelayCommand<object> _removeAllAlbumsCommand;

        private RelayCommand<object> _moveAlbumsUpCommand;
        private RelayCommand<object> _moveAlbumsDownCommand;
        #endregion // Fields

        #region Commands
        public ICommand SearchAlbumsCommand
        {
            get
            {
                if (null == _searchAlbumsCommand)
                    _searchAlbumsCommand = new RelayCommand<object>(ExecuteSearchAlbumsCommand, CanSearchAlbumsCommand);

                return _searchAlbumsCommand;
            }
        }
        private void ExecuteSearchAlbumsCommand(object notUsed)
        {
            Task.Run(() => SearchAlbums());
        }
        private bool CanSearchAlbumsCommand(object notUsed)
        {
            return (String.IsNullOrEmpty(_queryComposerViewModel.SqlPlaylistQuery) == false);
        }

        public ICommand AddAllAlbumsCommand
        {
            get
            {
                if (null == _addAllAlbumsCommand)
                    _addAllAlbumsCommand = new RelayCommand<object>(ExecuteAddAllAlbumsCommand, CanAddAllAlbumsCommand);

                return _addAllAlbumsCommand;
            }
        }
        private void ExecuteAddAllAlbumsCommand(object notUsed)
        {
            AddAllAlbums();
        }
        private bool CanAddAllAlbumsCommand(object notUsed)
        {
            return ((_albumListLeft.AllAlbumItemCollection != null) && (_albumListLeft.AllAlbumItemCollection.Count > 0));
        }

        public ICommand AddSelectedAlbumsCommand
        {
            get
            {
                if (null == _addSelectedAlbumsCommand)
                    _addSelectedAlbumsCommand = new RelayCommand<object>(ExecuteAddSelectedAlbumsCommand, CanAddSelectedAlbumsCommand);

                return _addSelectedAlbumsCommand;
            }
        }
        private void ExecuteAddSelectedAlbumsCommand(object notUsed)
        {
            AddSelectedAlbums();
        }
        private bool CanAddSelectedAlbumsCommand(object notUsed)
        {
            return ((_albumListLeft.SelectedAlbumItemCollection != null) && (_albumListLeft.SelectedAlbumItemCollection.Count > 0));
        }

        public ICommand RemoveAllAlbumsCommand
        {
            get
            {
                if (null == _removeAllAlbumsCommand)
                    _removeAllAlbumsCommand = new RelayCommand<object>(ExecuteRemoveAllAlbumsCommand, CanRemoveAllAlbumsCommand);

                return _removeAllAlbumsCommand;
            }
        }
        private void ExecuteRemoveAllAlbumsCommand(object notUsed)
        {
            RemoveAllAlbums();
        }
        private bool CanRemoveAllAlbumsCommand(object notUsed)
        {
            return ((_albumListRight.AllAlbumItemCollection != null) && (_albumListRight.AllAlbumItemCollection.Count > 0));
        }

        public ICommand RemoveSelectedAlbumsCommand
        {
            get
            {
                if (null == _removeSelectedAlbumsCommand)
                    _removeSelectedAlbumsCommand = new RelayCommand<object>(ExecuteRemoveSelectedAlbumsCommand, CanRemoveSelectedAlbumsCommand);

                return _removeSelectedAlbumsCommand;
            }
        }
        private void ExecuteRemoveSelectedAlbumsCommand(object notUsed)
        {
            RemoveSelectedAlbums();
        }
        private bool CanRemoveSelectedAlbumsCommand(object notUsed)
        {
            return ((_albumListRight.SelectedAlbumItemCollection != null) && (_albumListRight.SelectedAlbumItemCollection.Count > 0));
        }

        public ICommand MoveAlbumsUpCommand
        {
            get
            {
                if (null == _moveAlbumsUpCommand)
                    _moveAlbumsUpCommand = new RelayCommand<object>(ExecuteMoveAlbumsUpCommand, CanMoveAlbumsUpCommand);

                return _moveAlbumsUpCommand;
            }
        }
        private void ExecuteMoveAlbumsUpCommand(object notUsed)
        {
            MoveAlbumsUp();
        }
        private bool CanMoveAlbumsUpCommand(object notUsed)
        {
            return ((_albumListRight.AllAlbumItemCollection != null) && (_albumListRight.AllAlbumItemCollection.Count > 0));
        }

        public ICommand MoveAlbumsDownCommand
        {
            get
            {
                if (null == _moveAlbumsDownCommand)
                    _moveAlbumsDownCommand = new RelayCommand<object>(ExecuteMoveAlbumsDownCommand, CanMoveAlbumsDownCommand);

                return _moveAlbumsDownCommand;
            }
        }
        private void ExecuteMoveAlbumsDownCommand(object notUsed)
        {
            MoveAlbumsDown();
        }
        private bool CanMoveAlbumsDownCommand(object notUsed)
        {
            return ((_albumListRight.AllAlbumItemCollection != null) && (_albumListRight.AllAlbumItemCollection.Count > 0));
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

        public AlbumListViewModel AlbumListLeft
        {
            get { return _albumListLeft; }
            set
            {
                _albumListLeft = value;

                RaisePropertyChanged("AlbumListLeft");
            }
        }
        public AlbumListViewModel AlbumListRight
        {
            get { return _albumListRight; }
            set
            {
                _albumListRight = value;

                RaisePropertyChanged("AlbumListRight");
            }
        }

        public double AlbumsLeftWidth
        {
            get { return _albumListLeft.AlbumListViewWidth; }
            set
            {
                if (value == _albumListLeft.AlbumListViewWidth)
                    return;

                _albumListLeft.AlbumListViewWidth = value;

                RaisePropertyChanged("AlbumsLeftWidth");
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
        public AlbumPlaylistViewModel(ConnectionInfo conInfo)
        {
            _conInfo = conInfo;
            _queryComposerViewModel = new QueryComposerViewModel(_conInfo);

            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceAlbums = new DataServiceAlbums_SQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
                _dataServiceListItems = new DataServiceListItems_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceAlbums = new DataServiceAlbums_MYSQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
                _dataServiceListItems = new DataServiceListItems_MYSQL(conInfo);
            }
            _albumListLeft = new AlbumListViewModel(_conInfo);
            _albumListRight = new AlbumListViewModel(_conInfo);

            _albumListLeft.StampImageSize = AppSettings.GeneralSettings.StampImageSize;
            _albumListRight.StampImageSize = AppSettings.GeneralSettings.StampImageSize;

            _albumListLeft.AlbumListViewWidth = AppSettings.FormSettings.FrmMain_AlbumListViewWidth;
            _albumListRight.AlbumListViewWidth = AppSettings.FormSettings.FrmMain_AlbumListViewWidth;

            _albumListLeft.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
            _albumListRight.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
        }
        #endregion  // Constructor


        #region Private
        private async Task SearchAlbums()
        {
            String condition = " WHERE " + _queryComposerViewModel.SqlPlaylistQuery;
            _albumListLeft.AllAlbumItemCollection = await _dataServiceAlbums.GetAlbumsByCondition(condition);
        }
        private async Task OnPlaylistConfigurationChanged()
        {
            if ((_playlistConfiguration.AlbumList != null) && (_playlistConfiguration.AlbumList.Count > 0))
            {
                _albumListRight.AllAlbumItemCollection = _playlistConfiguration.AlbumList;
            }
            else
	        {
                if ((_playlistConfiguration.AlbumPathNames != null) && (_playlistConfiguration.AlbumPathNames.Count > 0))
                {
                    await ImportAlbums(_playlistConfiguration.AlbumPathNames);
                }

                if (_playlistConfiguration.AlbumPathNames == null)
                {
                    _playlistConfiguration.AlbumPathNames = new ObservableCollection<string>();
                }
	        }
        }

        private async Task ImportAlbums(ObservableCollection<String> albumPathList)
        {
            _importAlbums.Clear();

            ProgressBarVisible = true;
            Int32 done = 0;


            for (int i = 0; i < albumPathList.Count; i++)
            {
                AlbumItem album = await _dataServiceAlbums.GetAlbumByPath(albumPathList[i]);
                _importAlbums.Add(album);
                done++;
                if (done > 9)
                {
                    done = 0;

                    _playlistConfiguration.AlbumList = _importAlbums;
                    _albumListRight.AllAlbumItemCollection = _playlistConfiguration.AlbumList;
                    RaisePropertyChanged("AlbumListRight");

                    ProgressValue = (Double)i / albumPathList.Count * 100;
                }
            }

            if (done > 0)
            {
                ProgressValue = 100;
                _playlistConfiguration.AlbumList = _importAlbums;
                _albumListRight.AllAlbumItemCollection = _playlistConfiguration.AlbumList;
                RaisePropertyChanged("AlbumListRight");
            }

            ProgressValue = 0;
            ProgressBarVisible = false;
        }

        private void AddAllAlbums()
        {
            ObservableCollection<AlbumItem> albumsLeft = _albumListLeft.AllAlbumItemCollection;
            _albumListRight.AddAlbums(albumsLeft);
            _albumListLeft.RemoveAllAlbums();
            _playlistConfiguration.AddAllAlbums(albumsLeft);
        }

        private void AddSelectedAlbums()
        {
            ObservableCollection<AlbumItem> selectedAlbums = _albumListLeft.SelectedAlbumItemCollection;
            _albumListRight.AddAlbums(selectedAlbums);
            _albumListLeft.RemoveAlbums(selectedAlbums);

            _playlistConfiguration.AddSelectedAlbums(selectedAlbums);
            _albumListRight.UnselectAll();
        }

        private void RemoveSelectedAlbums()
        {
            ObservableCollection<AlbumItem> selectedAlbums = _albumListRight.SelectedAlbumItemCollection;
            _albumListLeft.AddAlbums(selectedAlbums);
            _albumListRight.RemoveAlbums(selectedAlbums);
            
            
            _playlistConfiguration.RemoveSelectedAlbums(selectedAlbums);
            _albumListLeft.UnselectAll();
        }

        private void RemoveAllAlbums()
        {
            _albumListLeft.AddAlbums(_albumListRight.AllAlbumItemCollection);
            _albumListRight.RemoveAllAlbums();
            _playlistConfiguration.RemoveAllAlbums();
        }

        private void MoveAlbumsUp()
        {
            _albumListRight.MoveSelectedAlbumsUp();
        }

        private void MoveAlbumsDown()
        {
            _albumListRight.MoveSelectedAlbumsDown();
        }  

        #endregion  // Private

        #region Localization
        public void Localize()
        {
            if (_queryComposerViewModel != null)
            {
                _queryComposerViewModel.Localize();
            }
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

        ~AlbumPlaylistViewModel()
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

            if (_dataServiceListItems != null)
            {
                _dataServiceListItems.Dispose();
            }


            if (_queryComposerViewModel != null)
            {
                _queryComposerViewModel.Dispose();
            }

            if (_albumListLeft != null)
            {
                _albumListLeft.Dispose();
            }

            if (_albumListRight != null)
            {
                _albumListRight.Dispose();
            }

        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
