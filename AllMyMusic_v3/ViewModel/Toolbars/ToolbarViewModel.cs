using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

using AllMyMusic_v3.DataService;
using AllMyMusic_v3.QueryBuilder;

namespace AllMyMusic_v3.ViewModel
{
    public class ToolbarViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private IDataServiceToolbar _toolbarService;
        private ObservableCollection<AlphabetItem> _buttonLabels;
        private Boolean _viewVaBands;
        private Boolean _viewVaGenres;
        private Boolean _albumListViewExpanded;
        private String _searchText;
        private String _searchBandsText;

        private RelayCommand<AlphabetItem> _buttonClickCommand;
        #endregion // Fields

        #region Commands
        public ICommand ButtonClickCommand
        {
            get
            {
                if (null == _buttonClickCommand)
                    _buttonClickCommand = new RelayCommand<AlphabetItem>(ExecuteButtonClickCommand);

                return _buttonClickCommand;
            }
        }
        private void ExecuteButtonClickCommand(AlphabetItem _buttonLabel)
        {
            AlphabetItemEventArgs args = new AlphabetItemEventArgs(_buttonLabel);
            OnButtonClicked(this, args);
        } 
        #endregion

        #region Presentation Properties
        public ObservableCollection<AlphabetItem> ButtonLabels
        {
            get { return _buttonLabels; }
            set
            {
                if (value == _buttonLabels)
                    return;

                _buttonLabels = value;

                RaisePropertyChanged("ButtonLabels");
            }
        }
        public String SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged("SearchText");

                StringEventArgs args = new StringEventArgs(_searchText);
                OnSearch(this, args);
            }
        }
        public String SearchBandsText
        {
            get { return _searchBandsText; }
            set
            {
                _searchBandsText = value;
                RaisePropertyChanged("SearchBandsText");

                StringEventArgs args = new StringEventArgs(_searchBandsText);
                OnSearchBands(this, args);
            }
        }

        public Boolean ViewVaBands
        {
            get { return _viewVaBands; }
            set
            {
                if (value == _viewVaBands)
                    return;

                _viewVaBands = value;
                RaisePropertyChanged("ViewVaBands");

                BooleanEventArgs args = new BooleanEventArgs(_viewVaBands);
                OnShowVaBandsChanged(this, args);
            }
        }
        public Boolean ViewVaGenres
        {
            get { return _viewVaGenres; }
            set
            {
                if (value == _viewVaGenres)
                    return;

                _viewVaGenres = value;
                RaisePropertyChanged("ViewVaGenres");

                if (_viewVaGenres == true)
                {
                    OnVaGenresClicked(this, EventArgs.Empty);
                }
            }
        }
        public Boolean AlbumListViewExpanded
        {
            get { return _albumListViewExpanded; }
            set
            {
                if (value == _albumListViewExpanded)
                    return;

                _albumListViewExpanded = value;

                BooleanEventArgs args = new BooleanEventArgs(_albumListViewExpanded);
                OnAlbumListViewExpandedClicked(this, args);

                RaisePropertyChanged("AlbumListViewExpanded");
            }
        }

        public ServerType SelectedServerType
        {
            get { return _toolbarService.SelectedServerType; }
        }
        #endregion

        #region Localization
        private String _viewVaGenres_ToolTip;
        private String _viewVaBands_ToolTip;

        public String ViewVaGenres_ToolTip
        {
            get { return _viewVaGenres_ToolTip; }
            set
            {
                if (value == _viewVaGenres_ToolTip)
                    return;

                _viewVaGenres_ToolTip = value;

                RaisePropertyChanged("ViewVaGenres_ToolTip");
            }
        }
        public String ViewVaBands_ToolTip
        {
            get { return _viewVaBands_ToolTip; }
            set
            {
                if (value == _viewVaBands_ToolTip)
                    return;

                _viewVaBands_ToolTip = value;

                RaisePropertyChanged("ViewVaBands_ToolTip");
            }
        }
        public void Localize()
        {
            _viewVaGenres_ToolTip = AmmLocalization.GetLocalizedString("frmAllMyMusic_ToolTip_VA");
            _viewVaBands_ToolTip = AmmLocalization.GetLocalizedString("frmAllMyMusic_ToolTip_ViewVA");
        }
        #endregion

        #region Constructor
        public ToolbarViewModel(ConnectionInfo conInfo, Boolean viewVaBands)
        {
            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _toolbarService = new DataServiceToolbar_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _toolbarService = new DataServiceToolbar_MYSQL(conInfo);
            }

            _viewVaBands = viewVaBands;
            QueryBuilderAlbums.ShowVariousArtists = viewVaBands;
            QueryBuilderBands.ShowVariousArtists = viewVaBands;
            QueryBuilderItems.ShowVariousArtists = viewVaBands;
            QueryBuilderSongs.ShowVariousArtists = viewVaBands;
            Global.ViewVaBands = viewVaBands;

            OnDatabaseChanged();

            Localize();
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            _toolbarService.ChangeDatabase(conInfo);
            OnDatabaseChanged();
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            if (_toolbarService != null)
            {
                _toolbarService.Dispose();
            }
            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _toolbarService = new DataServiceToolbar_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _toolbarService = new DataServiceToolbar_MYSQL(conInfo);
            }

            OnDatabaseChanged();
        }

        private async Task GetButtonLabels()
        {
            ButtonLabels = await _toolbarService.GetAlphabetButtons();
        }

        public void Close()
        {
 
        }

        public void Initialize()
        {
            Task.Run(() => GetButtonLabels());
        }

        public async Task UnselectAllButtons()
        {
            await Task.Run(() => UnselectButtons()); 
            RaisePropertyChanged("ButtonLabels");
        }
        private void UnselectButtons()
        {
            for (int i = 0; i < _buttonLabels.Count; i++)
            {
                _buttonLabels[i].IsSelected = false;
            }
        }

        private void OnDatabaseChanged()
        {
            Task.Run(() => GetButtonLabels());
        }
        #endregion

        #region Events
        public delegate void AlphabetButtonClickedEventHandler(object sender, AlphabetItemEventArgs e);
        public delegate void VaGenresButtonClickedEventHandler(object sender, EventArgs e);
        public delegate void ShowVaBandsChangedEventHandler(object sender, BooleanEventArgs e);
        public delegate void SearchEventHandler(object sender, StringEventArgs e);
        public delegate void AlbumListViewExpandedClickedEventHandler(object sender, BooleanEventArgs e);

        public event AlphabetButtonClickedEventHandler AlphabetButtonClicked;
        protected virtual void OnButtonClicked(object sender, AlphabetItemEventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < _buttonLabels.Count; i++)
                {
                    if (_buttonLabels[i].Character != e.Item.Character)
                    {
                        _buttonLabels[i].IsSelected = false;
                    }
                }
                RaisePropertyChanged("ButtonLabels");

                _viewVaGenres = false;
                RaisePropertyChanged("ViewVaGenres");

                if (this.AlphabetButtonClicked != null)
                {
                    this.AlphabetButtonClicked(this, e);
                }
            });
        }

        public event ShowVaBandsChangedEventHandler ShowVaBandsChanged;
        protected virtual void OnShowVaBandsChanged(object sender, BooleanEventArgs e)
        {
            QueryBuilderAlbums.ShowVariousArtists = e.Value;
            QueryBuilderBands.ShowVariousArtists = e.Value;
            QueryBuilderItems.ShowVariousArtists = e.Value;
            QueryBuilderSongs.ShowVariousArtists = e.Value;
            Global.ViewVaBands = e.Value;

            Task.Run(() => GetButtonLabels());
            
            if (this.ShowVaBandsChanged != null)
            {
                this.ShowVaBandsChanged(this, e);
            }
        }

        public event VaGenresButtonClickedEventHandler VaGenresButtonClicked;
        protected virtual void OnVaGenresClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < _buttonLabels.Count; i++)
            {
                _buttonLabels[i].IsSelected = false;
            }
            RaisePropertyChanged("ButtonLabels");

            if (this.VaGenresButtonClicked != null)
            {
                this.VaGenresButtonClicked(this, e);
            }
        }

        public event SearchEventHandler Search;
        protected virtual void OnSearch(object sender, StringEventArgs e)
        {
            if (this.Search != null)
            {
                this.Search(this, e);
            }
        }

        public event SearchEventHandler SearchBands;
        protected virtual void OnSearchBands(object sender, StringEventArgs e)
        {
            if (this.SearchBands != null)
            {
                this.SearchBands(this, e);
            }
        }

        public event AlbumListViewExpandedClickedEventHandler AlbumListViewExpandedClicked;
        protected virtual void OnAlbumListViewExpandedClicked(object sender, BooleanEventArgs e)
        {
            if (this.AlbumListViewExpandedClicked != null)
            {
                this.AlbumListViewExpandedClicked(this, e);
            }
        }
        #endregion // Events

        #region Private Helpers
       

        #endregion // Private Helpers


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

        ~ToolbarViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {

        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion

    }


     
}
