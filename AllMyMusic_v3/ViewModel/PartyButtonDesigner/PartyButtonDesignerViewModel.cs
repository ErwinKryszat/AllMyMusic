using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using AllMyMusic_v3.Settings;
using AllMyMusic_v3.DataService;
using Microsoft.Win32;

namespace AllMyMusic_v3.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.

    public class PartyButtonDesignerViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ConnectionInfo _conInfo;
        private IDataServiceAlbums _dataServiceAlbums;
        private IDataServiceSongs _dataServiceSongs;
        private ViewModelBase _currentViewModel;

        private ObservableCollection<PartyButtonConfigViewModel> _playlistConfigurations;
        private PartyButtonConfigViewModel _selectedPlaylistConfiguration;
        private ObservableCollection<String> _playlistTypes;
        private String _selectedPlaylistType;
        private String _buttonLabel;
        private String _toolTipText;
        private BitmapImage _toolTipImage; 
        private BitmapImage _sampleButtonImage;
        private String _sampleButtonLabel;

        private Boolean _randomPlaylist;
        
        private SqlPlaylistViewModel _sqlPlaylistViewModel;
        private AlbumPlaylistViewModel _albumPlaylistViewModel;
        private SongPlaylistViewModel _songPlaylistViewModel;

        private RelayCommand<object> _deleteSelectedButtonCommand;
        private RelayCommand<object> _addButtonCommand;

        private RelayCommand<object> _selectButtonImageCommand;
        private RelayCommand<object> _selectTooltipImageCommand;

        #endregion // Fields

        #region Commands
        public ICommand DeleteSelectedButtonCommand
        {
            get
            {
                if (null == _deleteSelectedButtonCommand)
                    _deleteSelectedButtonCommand = new RelayCommand<object>(ExecuteDeleteSelectedButtonCommand, CanDeleteSelectedButtonCommand);

                return _deleteSelectedButtonCommand;
            }
        }
        private void ExecuteDeleteSelectedButtonCommand(object notUsed)
        {
            Int32 removeIndex = -1;
            for (int i = 0; i < _playlistConfigurations.Count; i++)
            {
                if (_playlistConfigurations[i].ButtonLabel == _selectedPlaylistConfiguration.ButtonLabel)
                {
                    removeIndex = i;
                }
            }
            if (removeIndex >= 0)
            {
                _playlistConfigurations.RemoveAt(removeIndex);
            }   
        }
        private bool CanDeleteSelectedButtonCommand(object notUsed)
        {
            return (_selectedPlaylistConfiguration != null);
        }

        public ICommand AddButtonCommand
        {
            get
            {
                if (null == _addButtonCommand)
                    _addButtonCommand = new RelayCommand<object>(ExecuteAddButtonCommand, CanAddButtonCommand);

                return _addButtonCommand;
            }
        }
        private void ExecuteAddButtonCommand(object notUsed)
        {
            AddNewConfig();         
        }
        private bool CanAddButtonCommand(object notUsed)
        {
            return true;
        }

        public ICommand SelectButtonImageCommand
        {
            get
            {
                if (null == _selectButtonImageCommand)
                    _selectButtonImageCommand = new RelayCommand<object>(ExecuteSelectButtonImageCommand, CanSelectButtonImageCommand);

                return _selectButtonImageCommand;
            }
        }
        private void ExecuteSelectButtonImageCommand(object notUsed)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select a picture";
            openFile.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            openFile.InitialDirectory = Global.PartyButtonImages;
            if (openFile.ShowDialog() == true)
            {
                _sampleButtonImage = new BitmapImage();
                SampleButtonImage = ResourceHelper.GetImage(openFile.FileName);

                _selectedPlaylistConfiguration.ButtonImagePath = openFile.FileName;
                _selectedPlaylistConfiguration.SmallButtonImage = SampleButtonImage;
                _selectedPlaylistConfiguration.LargeButtonImage = SampleButtonImage;
            }
        }
        private bool CanSelectButtonImageCommand(object notUsed)
        {
            return true;
        }

        public ICommand SelectTooltipImageCommand
        {
            get
            {
                if (null == _selectTooltipImageCommand)
                    _selectTooltipImageCommand = new RelayCommand<object>(ExecuteSelectTooltipImageCommand, CanSelectTooltipImageCommand);

                return _selectTooltipImageCommand;
            }
        }
        private void ExecuteSelectTooltipImageCommand(object notUsed)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select a picture";
            openFile.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            openFile.InitialDirectory = Global.PartyButtonImages;
            if (openFile.ShowDialog() == true)
            {
                _toolTipImage = new BitmapImage();
                ToolTipImage = ResourceHelper.GetImage(openFile.FileName);

                _selectedPlaylistConfiguration.TooltipImagePath = openFile.FileName;
                _selectedPlaylistConfiguration.TooltipImage = _toolTipImage;
            }
        }
        private bool CanSelectTooltipImageCommand(object notUsed)
        {
            return true;
        }
        #endregion

        #region Presentation Properties  
        public SqlPlaylistViewModel SqlPlaylistViewModel
        {
            get { return _sqlPlaylistViewModel; }
            set
            {
                if (value == _sqlPlaylistViewModel)
                    return;

                _sqlPlaylistViewModel = value;
                RaisePropertyChanged("SqlPlaylistViewModel");
            }
        }
        public AlbumPlaylistViewModel AlbumPlaylistViewModel
        {
            get { return _albumPlaylistViewModel; }
            set
            {
                if (value == _albumPlaylistViewModel)
                    return;

                _albumPlaylistViewModel = value;
                RaisePropertyChanged("AlbumPlaylistViewModel");
            }
        }
        public SongPlaylistViewModel SongPlaylistViewModel
        {
            get { return _songPlaylistViewModel; }
            set
            {
                if (value == _songPlaylistViewModel)
                    return;

                _songPlaylistViewModel = value;
                RaisePropertyChanged("SongPlaylistViewModel");
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
        public ObservableCollection<PartyButtonConfigViewModel> PlaylistConfigurations
        {
            get { return _playlistConfigurations; }
            set
            {
                if (value == _playlistConfigurations)
                    return;

                _playlistConfigurations = value;
                RaisePropertyChanged("PlaylistConfigurations");
            }
        }
        public PartyButtonConfigViewModel SelectedPlaylistConfiguration
        {
            get { return _selectedPlaylistConfiguration; }
            set
            {
                _selectedPlaylistConfiguration = value;
                RaisePropertyChanged("SelectedPlaylistConfiguration");
                OnSelectedPlaylistConfigurationChanged();
            }
        }
        public ObservableCollection<String> PlaylistTypes
        {
            get { return _playlistTypes; }
            set
            {
                if (value == _playlistTypes)
                    return;

                _playlistTypes = value;
                RaisePropertyChanged("PlaylistTypes");
            }
        }
        public String SelectedPlaylistType
        {
            get { return _selectedPlaylistType; }
            set
            {
                if (value == _selectedPlaylistType)
                    return;

                _selectedPlaylistType = value;
                RaisePropertyChanged("SelectedPlaylistType");
                OnSelectedPlaylistTypeChaned();
            }
        }
        public String ButtonLabel
        {
            get { return _buttonLabel; }
            set
            {
                if (value == _buttonLabel)
                    return;

                _buttonLabel = value;
                RaisePropertyChanged("ButtonLabel");
                OnButtonLabelChanged();
            }
        }
        public String ToolTipText
        {
            get { return _toolTipText; }
            set
            {
                if (value == _toolTipText)
                    return;

                _toolTipText = value;
                RaisePropertyChanged("TooltipText");
                OnToolTipTextChanged();
            }
        }
        public Boolean RandomPlaylist
        {
            get { return _randomPlaylist; }
            set
            {
                if (value == _randomPlaylist)
                    return;

                _randomPlaylist = value;
                RaisePropertyChanged("RandomPlaylist");
                OnPlayRandomChanged();
            }
        }
        public BitmapImage ToolTipImage
        {
            get { return _toolTipImage; }
            set
            {
                if (value == _toolTipImage)
                    return;

                _toolTipImage = value;
                RaisePropertyChanged("TooltipImage");
            }
        }
        public BitmapImage SampleButtonImage
        {
            get { return _sampleButtonImage; }
            set
            {
                if (value == _sampleButtonImage)
                    return;

                _sampleButtonImage = value;
                RaisePropertyChanged("SampleButtonImage");
            }
        }
        public String SampleButtonLabel
        {
            get { return _sampleButtonLabel; }
            set
            {
                if (value == _sampleButtonLabel)
                    return;

                _sampleButtonLabel = value;
                RaisePropertyChanged("SampleButtonLabel");
            }
        }
        #endregion //Presentation Properties

        #region Constructor
        public PartyButtonDesignerViewModel(ConnectionInfo conInfo, ObservableCollection<PartyButtonConfigViewModel> playlistConfigurations)
        {
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

            _playlistConfigurations = playlistConfigurations;

            Boolean res = CreateViewModels(conInfo);

             _playlistTypes = new ObservableCollection<string>();
            _playlistTypes.Add(PartyButtonType.Query.ToString());
            _playlistTypes.Add(PartyButtonType.Albumlist.ToString());
            _playlistTypes.Add(PartyButtonType.Songlist.ToString());

            Localize();
        }
        #endregion

        #region public
        public void Close()
        {
            AppSettings.FormSettings.FrmPartyButtonDesigner_LeftAlbumsWidth = _albumPlaylistViewModel.AlbumsLeftWidth;
            AppSettings.FormSettings.FrmPartyButtonDesigner_LeftSongsWidth = _songPlaylistViewModel.SongsLeftWidth;
        }
        #endregion


        #region Private
        private Boolean CreateViewModels(ConnectionInfo conInfo)
        {
            _conInfo = conInfo;

            _sqlPlaylistViewModel = new SqlPlaylistViewModel(_conInfo);
            _sqlPlaylistViewModel.SqlPlaylistQueryChanged += new ViewModel.SqlPlaylistViewModel.SqlPlaylistQueryChangedEventHandler(_sqlPlaylistViewModel_SqlPlaylistQueryChanged);

            _albumPlaylistViewModel = new AlbumPlaylistViewModel(_conInfo);
            _albumPlaylistViewModel.AlbumsLeftWidth = AppSettings.FormSettings.FrmPartyButtonDesigner_LeftAlbumsWidth;

            _songPlaylistViewModel = new SongPlaylistViewModel(_conInfo);
            _songPlaylistViewModel.SongsLeftWidth = AppSettings.FormSettings.FrmPartyButtonDesigner_LeftSongsWidth;

            return true;
        }
        private void OnSelectedPlaylistConfigurationChanged()
        {
            if (_selectedPlaylistConfiguration != null)
            {
                ButtonLabel = SelectedPlaylistConfiguration.ButtonLabel;
                ToolTipText = SelectedPlaylistConfiguration.ToolTipText;
                RandomPlaylist = SelectedPlaylistConfiguration.Randomize;
                SampleButtonImage = SelectedPlaylistConfiguration.LargeButtonImage;
                SampleButtonLabel = SelectedPlaylistConfiguration.ButtonLabel;
               

                if (SelectedPlaylistType != SelectedPlaylistConfiguration.ButtonType.ToString())
                {
                    SelectedPlaylistType = SelectedPlaylistConfiguration.ButtonType.ToString();
                }
                else
                {
                    AssignPlaylistConfig();
                }
            }
            else
            {
                SelectedPlaylistType = String.Empty;
                ButtonLabel = String.Empty;
                ToolTipText = String.Empty;
                RandomPlaylist = false;
                SampleButtonImage = null;

                SampleButtonLabel = String.Empty;
            }
        }
        private void OnSelectedPlaylistTypeChaned()
        {
            if (_selectedPlaylistConfiguration != null)
            {
                _selectedPlaylistConfiguration.ButtonType = (PartyButtonType)Enum.Parse(typeof(PartyButtonType), _selectedPlaylistType);
                AssignPlaylistConfig();

                switch (_selectedPlaylistConfiguration.ButtonType)
                {
                    case PartyButtonType.Songlist:
                        CurrentViewModel = _songPlaylistViewModel;
                        break;
                    case PartyButtonType.Albumlist:
                        CurrentViewModel = _albumPlaylistViewModel;
                        break;
                    case PartyButtonType.Query:
                        CurrentViewModel = _sqlPlaylistViewModel;
                        break;
                    case PartyButtonType.Undefined:
                        break;
                    default:
                        break;
                }
            }
        }

        private void AssignPlaylistConfig()
        {
            switch (_selectedPlaylistConfiguration.ButtonType)
            {
                case PartyButtonType.Songlist:
                    _songPlaylistViewModel.PlaylistConfiguration = _selectedPlaylistConfiguration;
                    break;
                case PartyButtonType.Albumlist:
                    _albumPlaylistViewModel.PlaylistConfiguration = _selectedPlaylistConfiguration;
                    break;
                case PartyButtonType.Query:
                    _sqlPlaylistViewModel.PlaylistConfiguration = _selectedPlaylistConfiguration;
                    break;
                case PartyButtonType.Undefined:
                    break;
                default:
                    break;
            }
        }
        private void OnButtonLabelChanged()
        {
            if (_selectedPlaylistConfiguration != null)
            {
                _selectedPlaylistConfiguration.ButtonLabel = _buttonLabel;
                SampleButtonLabel = _buttonLabel;
            }
        }
        private void OnToolTipTextChanged()
        {
            if (_selectedPlaylistConfiguration != null)
            {
                _selectedPlaylistConfiguration.ToolTipText = _toolTipText;
            }
        }
        private void OnPlayRandomChanged()
        {
            if (_selectedPlaylistConfiguration != null)
            {
                _selectedPlaylistConfiguration.Randomize = _randomPlaylist;
            }
        }

        private void AddNewConfig()
        {
            PartyButtonConfigViewModel newConfig = new PartyButtonConfigViewModel();

            newConfig.ButtonLabel = "My Playlist Name";
            newConfig.ButtonType = PartyButtonType.Query;
            newConfig.Randomize = true;
            newConfig.ButtonImagePath = Global.PartyButtonImages + "\\g1.png";
            newConfig.ToolTipText = "AllMyMusic Playlist";
            newConfig.SqlQuery = "SELECT * FROM viewSongs WHERE ";

            _playlistConfigurations.Add(newConfig);
            SelectedPlaylistConfiguration = newConfig;
        }


        private void _sqlPlaylistViewModel_SqlPlaylistQueryChanged(object sender, StringEventArgs e)
        {
            _selectedPlaylistConfiguration.SqlQuery = e.Name;
        }
        
        #endregion

        #region Localization

        private String _formTitle = "Party Button Designer";
        private String _formPuropose = "This dialog helps you designing your Playlists";
        private String _addNewCommandLabel = "Add New";
        private String _deleteSelectedCommandLabel = "Delete Selected";
        private String _groupSelectPlaylist = "Select a Playlist";
        private String _groupDesignPlaylist = "Design your Playlist";
        private String _groupPreviewPlaylist = "Preview";

        public String FormTitle
        {
            get { return _formTitle; }
            set
            {
                if (value == _formTitle)
                    return;

                _formTitle = value;

                RaisePropertyChanged("FormTitle");
            }
        }

        public String FormPuropose
        {
            get { return _formPuropose; }
            set
            {
                if (value == _formPuropose)
                    return;

                _formPuropose = value;

                RaisePropertyChanged("FormPuropose");
            }
        }

        public String AddNewCommandLabel
        {
            get { return _addNewCommandLabel; }
            set
            {
                if (value == _addNewCommandLabel)
                    return;

                _addNewCommandLabel = value;

                RaisePropertyChanged("AddNewCommandLabel");
            }
        }

        public String DeleteSelectedCommandLabel
        {
            get { return _deleteSelectedCommandLabel; }
            set
            {
                if (value == _deleteSelectedCommandLabel)
                    return;

                _deleteSelectedCommandLabel = value;

                RaisePropertyChanged("DeleteSelectedCommandLabel");
            }
        }

        public String GroupSelectPlaylist
        {
            get { return _groupSelectPlaylist; }
            set
            {
                if (value == _groupSelectPlaylist)
                    return;

                _groupSelectPlaylist = value;

                RaisePropertyChanged("GroupSelectPlaylist");
            }
        }

        public String GroupDesignPlaylist
        {
            get { return _groupDesignPlaylist; }
            set
            {
                if (value == _groupDesignPlaylist)
                    return;

                _groupDesignPlaylist = value;

                RaisePropertyChanged("GroupDesignPlaylist");
            }
        }

        public String GroupPreviewPlaylist
        {
            get { return _groupPreviewPlaylist; }
            set
            {
                if (value == _groupPreviewPlaylist)
                    return;

                _groupPreviewPlaylist = value;

                RaisePropertyChanged("_groupPreviewPlaylist");
            }
        }
        public void Localize()
        {
            if (_sqlPlaylistViewModel != null)
            {
                _sqlPlaylistViewModel.Localize();
            }

            if (_albumPlaylistViewModel != null)
            {
                _albumPlaylistViewModel.Localize();
            }

            if (_songPlaylistViewModel != null)
            {
                _songPlaylistViewModel.Localize();
            }

            FormTitle = AmmLocalization.GetLocalizedString("frmPartyButton_Title");
            FormPuropose = AmmLocalization.GetLocalizedString("frmPartyButton_Purpose");
            AddNewCommandLabel = AmmLocalization.GetLocalizedString("frmPartyButton_AddNew");
            DeleteSelectedCommandLabel = AmmLocalization.GetLocalizedString("frmPartyButton_DeleteSelected");
            GroupSelectPlaylist = AmmLocalization.GetLocalizedString("frmPartyButton_GrpSelected");
            GroupDesignPlaylist = AmmLocalization.GetLocalizedString("frmPartyButton_GrpDesign");
            GroupPreviewPlaylist = AmmLocalization.GetLocalizedString("frmPartyButton_Grppreview");
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

        ~PartyButtonDesignerViewModel()
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
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}