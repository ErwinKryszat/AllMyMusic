using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;

using AllMyMusic_v3.Settings;
using AllMyMusic_v3.DataService;

namespace AllMyMusic_v3.ViewModel
{
    public class RibbonViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private Int32 _tabIndex;
        private ObservableCollection<PartyButtonConfigViewModel> _playlistConfigurations;
        private Boolean _playlistConfigurationsChanged;

        private DatabasesViewModel _databasesViewModel;

        private ObservableCollection<String> _guiLanguages;
        private ObservableCollection<String> _wikipediaLanguages;
        private String _selectedGuiLanguage;
        private String _selectedWikipediaLanguage;

        private LanguagesGUI _languageGUI;
        private LanguagesWikipedia _languageWikipedia;

        private RelayCommand<object> _addSongsCommand;
        private RelayCommand<object> _connectServerCommand;
        private RelayCommand<object> _manageImagesCommand;
        private RelayCommand<object> _renameFileCommand;
        private RelayCommand<object> _tagFilesCommand;
        private RelayCommand<object> _partyButtonDesignerCommand;
        #endregion // Fields

        #region Commands
        public ICommand AddSongsCommand
        {
            get
            {
                if (null == _addSongsCommand)
                    _addSongsCommand = new RelayCommand<object>(ExecuteAddSongsCommand, CanAddSongsCommand);

                return _addSongsCommand;
            }
        }
        private void ExecuteAddSongsCommand(object _notUsed)
        {
            frmFolderSelect frmFolderSelect = new frmFolderSelect(FilesystemAction.AddSongs);
            frmFolderSelect.ShowDialog();


            List<String> folderList = null;
            if ((frmFolderSelect.DialogResult.HasValue == true) && (frmFolderSelect.DialogResult == true))
            {
                // user clicked OK button
                folderList = frmFolderSelect.FolderList;

                FolderListEventArgs args = new FolderListEventArgs(folderList);
                OnAddSongsRequest(this, args);
            }

        }
        private bool CanAddSongsCommand(object _notUsed)
        {
            return true;
        }

        public ICommand ConnectServerCommand
        {
            get
            {
                if (null == _connectServerCommand)
                    _connectServerCommand = new RelayCommand<object>(ExecuteConnectServerCommand, CanConnectServerCommand);

                return _connectServerCommand;
            }
        }
        private void ExecuteConnectServerCommand(object _notUsed)
        {
            

        }
        private bool CanConnectServerCommand(object _notUsed)
        {
            return true;
        }

        public ICommand RenameFilesCommand
        {
            get
            {
                if (null == _renameFileCommand)
                    _renameFileCommand = new RelayCommand<object>(ExecuteRenameFilesCommand, CanRenameFilesCommand);

                return _renameFileCommand;
            }
        }
        private void ExecuteRenameFilesCommand(object _notUsed)
        {

        }
        private bool CanRenameFilesCommand(object _notUsed)
        {
            return true;
        }

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
            frmManageCoverImages frmManageImages = new frmManageCoverImages();
            frmManageImages.ShowDialog();

            List<String> folderList = null;
            if ((frmManageImages.DialogResult.HasValue == true) && (frmManageImages.DialogResult == true))
            {
                // user clicked OK button
                folderList = frmManageImages.FolderList;

                FolderListEventArgs args = new FolderListEventArgs(folderList);
                OnManageImagesRequest(this, args);
            }
        }
        private bool CanManageImagesCommand(object notUsed)
        {
            return true;
        }

        public ICommand TagFilesCommand
        {
            get
            {
                if (null == _tagFilesCommand)
                    _tagFilesCommand = new RelayCommand<object>(ExecuteTagFilesCommand, CanTagFilesCommand);

                return _tagFilesCommand;
            }
        }
        private void ExecuteTagFilesCommand(object _notUsed)
        {

        }
        private bool CanTagFilesCommand(object _notUsed)
        {
            return true;
        }

        public ICommand PartyButtonDesignerCommand
        {
            get
            {
                if (null == _partyButtonDesignerCommand)
                    _partyButtonDesignerCommand = new RelayCommand<object>(ExecutePartyButtonDesignerCommand, CanPartyButtonDesignerCommand);

                return _partyButtonDesignerCommand;
            }
        }
        private void ExecutePartyButtonDesignerCommand(object _notUsed)
        {
            EventArgs args = new EventArgs();
            OnPartyButtonDesignerRequest(this, args);
        }
        private bool CanPartyButtonDesignerCommand(object _notUsed)
        {
            return true;
        }

        #endregion

        #region Presentation Properties
        public DatabasesViewModel DatabasesViewModel
        {
            get { return _databasesViewModel; }
            set
            {
                if (value == _databasesViewModel)
                    return;

                _databasesViewModel = value;

                RaisePropertyChanged("DatabasesViewModel");
            }
        }
        public Int32 TabIndex
        {
            get { return _tabIndex; }
            set
            {
                if (value == _tabIndex)
                    return;

                _tabIndex = value;

                RaisePropertyChanged("TabIndex");
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
                OnPlaylistConfigurationsChanged();
            }
        }
        public Boolean PlaylistConfigurationsChanged
        {
            get { return _playlistConfigurationsChanged; }
            set
            {
                _playlistConfigurationsChanged = value;

                RaisePropertyChanged("PlaylistConfigurationsChanged");
                OnPlaylistConfigurationsChanged();
            }
        }
        public ObservableCollection<String> GuiLanguages
        {
            get { return _guiLanguages; }
            set
            {
                _guiLanguages = value;

                RaisePropertyChanged("GuiLanguages");
            }
        }
        public ObservableCollection<String> WikipediaLanguages
        {
            get { return _wikipediaLanguages; }
            set
            {
                _wikipediaLanguages = value;

                RaisePropertyChanged("WikipediaLanguages");
            }
        }
        public String SelectedGuiLanguage
        {
            get { return _selectedGuiLanguage; }
            set
            {
                if (_selectedGuiLanguage == value)
                    return;

                _selectedGuiLanguage = value;
                
                _languageGUI = ConvertLanguageGUI(_selectedGuiLanguage);
                AppSettings.GeneralSettings.LanguageGUI = _languageGUI.ToString();
                AmmLocalization.Initialize(AppSettings.GeneralSettings.LanguageGUI);
                RaisePropertyChanged("SelectedGuiLanguage");
                OnLanguageChanged(this, new EventArgs());
            }
        }
        public String SelectedWikipediaLanguage
        {
            get { return _selectedWikipediaLanguage; }
            set
            {
                if (_selectedWikipediaLanguage == value)
                    return;

                _selectedWikipediaLanguage = value;

                _languageWikipedia = ConvertLanguageWIKI(_selectedWikipediaLanguage);
                AppSettings.GeneralSettings.WikipediaLanguage = _languageWikipedia.ToString();
                RaisePropertyChanged("SelectedWikipediaLanguage");
                StringEventArgs args = new StringEventArgs(AppSettings.GeneralSettings.WikipediaLanguage);
                OnWikipediaChanged(this, args);
            }
        }
        #endregion

        #region Constructor
        public RibbonViewModel(DatabasesViewModel databasesViewModel)
        {
            _databasesViewModel = databasesViewModel;
            _tabIndex = AppSettings.FormSettings.FrmMain_RibbonTab;

            Localize();
            LoadLocalizedLanguages();

            PlaylistConfigurations = PartyButtonXml.Load(Global.PartyButtonConfigFile, _databasesViewModel.ActiveDatabaseName);
        }
        #endregion

        #region Public
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            if (_playlistConfigurationsChanged)
            {
                PartyButtonXml.Save(PlaylistConfigurations, Global.PartyButtonConfigFile, _databasesViewModel.ActiveDatabaseName);
            }

            PlaylistConfigurations = PartyButtonXml.Load(Global.PartyButtonConfigFile, _databasesViewModel.ActiveDatabaseName);
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            if (_playlistConfigurationsChanged)
            {
                PartyButtonXml.Save(PlaylistConfigurations, Global.PartyButtonConfigFile, _databasesViewModel.ActiveDatabaseName);
            }

            PlaylistConfigurations = PartyButtonXml.Load(Global.PartyButtonConfigFile, _databasesViewModel.ActiveDatabaseName);
        }
        public void Close()
        {
            AppSettings.FormSettings.FrmMain_RibbonTab = _tabIndex;
            
            if (_playlistConfigurationsChanged)
            {
                PartyButtonXml.Save(PlaylistConfigurations, Global.PartyButtonConfigFile, _databasesViewModel.ActiveDatabaseName);
            }
        }
        public void UnselectPartyButton()
        {
            for (int i = 0; i < _playlistConfigurations.Count; i++)
            {
                _playlistConfigurations[i].IsSelected = false;
            }
        }
        #endregion


        #region Private 
        private void OnPlaylistConfigurationsChanged()
        {
            for (int i = 0; i < _playlistConfigurations.Count; i++)
            {
                _playlistConfigurations[i].PartyButton_Click += OnPartyButton_Click;
            }
        }
        private void LoadLocalizedLanguages()
        {
            String[] guiLanguages = { "Deutsch", "English", "Français", "Nederlands", "Polski" };
            String[] wikipediaLanguages = { "Deutsch", "English", "Español", "Français", "Italiano", "Nederlands", "Polski", "Русский" };

            GuiLanguages = new ObservableCollection<string>(guiLanguages);
            WikipediaLanguages = new ObservableCollection<string>(wikipediaLanguages);

            _languageGUI = ConvertLanguageGUI(AppSettings.GeneralSettings.LanguageGUI);
            _languageWikipedia = ConvertLanguageWIKI(AppSettings.GeneralSettings.WikipediaLanguage);

            SelectedGuiLanguage = TranslateLanguageInternation(AppSettings.GeneralSettings.LanguageGUI);
            SelectedWikipediaLanguage = TranslateLanguageInternation(AppSettings.GeneralSettings.WikipediaLanguage);
        }
        private LanguagesGUI ConvertLanguageGUI(String language)
        {
            switch (language)
            {
                case "Deutsch":
                    return LanguagesGUI.German;
                case "English":
                    return LanguagesGUI.English;
                case "Français":
                    return LanguagesGUI.French;
                case "Nederlands":
                    return LanguagesGUI.Dutch;
                case "Polski":
                    return LanguagesGUI.Polish;
                default:
                    return LanguagesGUI.English;
            }
        }
        private LanguagesWikipedia ConvertLanguageWIKI(String language)
        {
            switch (language)
            {
                case "Deutsch":
                    return LanguagesWikipedia.German;
                case "English":
                    return LanguagesWikipedia.English;
                case "Español":
                    return LanguagesWikipedia.Spanish;
                case "Français":
                    return LanguagesWikipedia.French;
                case "Italiano":
                    return LanguagesWikipedia.Italian;
                case "Nederlands":
                    return LanguagesWikipedia.Dutch;
                case "Polski":
                    return LanguagesWikipedia.Polish;
                case "Русский":
                    return LanguagesWikipedia.Russian;
                default:
                    return LanguagesWikipedia.English;
            }
        }
        private String TranslateLanguageInternation(String languageInternation)
        {
            switch (languageInternation)
            {
                case "Dutch":
                    return "Nederlands";
                case "English":
                    return "English";
                case "French":
                    return "Français";
                case "Italian":
                    return "Italiano";
                case "German":
                    return "Deutsch";
                case "Polish":
                    return "Polski";
                case "Russian":
                    return "Русский";
                case "Spanish":
                    return "Español";
                default:
                    return "English";
            }
        }
        #endregion

        #region Events
        public delegate void AddSongsRequestEventHandler(object sender, FolderListEventArgs e);
        public event AddSongsRequestEventHandler AddSongsRequest;
        protected virtual void OnAddSongsRequest(object sender, FolderListEventArgs e)
        {
            if (this.AddSongsRequest != null)
            {
                this.AddSongsRequest(this, e);
            }
        }

        public delegate void ManageImagesRequestEventHandler(object sender, FolderListEventArgs e);
        public event ManageImagesRequestEventHandler ManageImagesRequest;
        protected virtual void OnManageImagesRequest(object sender, FolderListEventArgs e)
        {
            if (this.ManageImagesRequest != null)
            {
                this.ManageImagesRequest(this, e);
            }
        }

        public delegate void PartyButtonDesignerRequestEventHandler(object sender, EventArgs e);
        public event PartyButtonDesignerRequestEventHandler PartyButtonDesignerRequest;
        protected virtual void OnPartyButtonDesignerRequest(object sender, EventArgs e)
        {
            if (this.PartyButtonDesignerRequest != null)
            {
                this.PartyButtonDesignerRequest(this, e);
            }
        }

        public delegate void PartyButton_ClickEventHandler(object sender, PartyButtonConfigEventArgs e);
        public event PartyButton_ClickEventHandler PartyButton_Click;
        private void OnPartyButton_Click(object sender, PartyButtonConfigEventArgs e)
        {
            for (int i = 0; i < _playlistConfigurations.Count; i++)
            {
                if (e.PartyButtonConfig != _playlistConfigurations[i])
                {
                    _playlistConfigurations[i].IsSelected = false;
                }
            }

            if (this.PartyButton_Click != null)
            {
                this.PartyButton_Click(this, e);
            }
        }

        // Currently not used
        public delegate void PlaylistChangedEventHandler(object sender, PlaylistEventArgs e);
        public event PlaylistChangedEventHandler PlaylistChanged;
        protected virtual void OnPlaylistChanged(object sender, PlaylistEventArgs e)
        {
            if (this.PlaylistChanged != null)
            {
                this.PlaylistChanged(this, e);
            }
        }
        #endregion // Events



        #region Localization
        private String _tab_Database;
        private String _tab_Collection;
        private String _tab_Tools;
        private String _tab_Party;

        private String _grp_AddSongs;
        private String _grp_SelectDatabase;
        private String _grp_Server;
        private String _grp_ExistingDatabases;
        private String _grp_NewDatabase;
        private String _grp_DatabaseConnections;
        private String _grp_CoverImages;
        private String _grp_Language;
        private String _grp_Wikipedia;
        private String _grp_PartyButtonTool;
        private String _grp_PartyButton;

        private String _cmd_Rename_ToolTip;
        private String _cmd_AutoTag_ToolTip;
        private String _cmd_CoverImages_ToolTip;

        private String _cmd_AddSongs;
        private String _cmd_PartyButtonTool;
        


        public String Cmd_Rename_ToolTip
        {
            get { return _cmd_Rename_ToolTip; }
            set
            {
                if (value == _cmd_Rename_ToolTip)
                    return;

                _cmd_Rename_ToolTip = value;

                RaisePropertyChanged("Cmd_Rename_ToolTip");
            }
        }
        public String Cmd_AutoTag_ToolTip
        {
            get { return _cmd_AutoTag_ToolTip; }
            set
            {
                if (value == _cmd_AutoTag_ToolTip)
                    return;

                _cmd_AutoTag_ToolTip = value;

                RaisePropertyChanged("Cmd_AutoTag_ToolTip");
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
        public String Cmd_AddSongs
        {
            get { return _cmd_AddSongs; }
            set
            {
                if (value == _cmd_AddSongs)
                    return;

                _cmd_AddSongs = value;

                RaisePropertyChanged("Cmd_AddSongs");
            }
        }
        public String Cmd_PartyButtonTool
        {
            get { return _cmd_PartyButtonTool; }
            set
            {
                if (value == _cmd_PartyButtonTool)
                    return;

                _cmd_PartyButtonTool = value;

                RaisePropertyChanged("Cmd_PartyButtonTool");
            }
        }

        public String Tab_Database
        {
            get { return _tab_Database; }
            set
            {
                if (value == _tab_Database)
                    return;

                _tab_Database = value;

                RaisePropertyChanged("Tab_Database");
            }
        }
        public String Tab_Collection
        {
            get { return _tab_Collection; }
            set
            {
                if (value == _tab_Collection)
                    return;

                _tab_Collection = value;

                RaisePropertyChanged("Tab_Collection");
            }
        }
        public String Tab_Tools
        {
            get { return _tab_Tools; }
            set
            {
                if (value == _tab_Tools)
                    return;

                _tab_Tools = value;

                RaisePropertyChanged("Tab_Tools");
            }
        }
        public String Tab_Party
        {
            get { return _tab_Party; }
            set
            {
                if (value == _tab_Party)
                    return;

                _tab_Party = value;

                RaisePropertyChanged("Tab_Party");
            }
        }

        public String Grp_AddSongs
        {
            get { return _grp_AddSongs; }
            set
            {
                if (value == _grp_AddSongs)
                    return;

                _grp_AddSongs = value;

                RaisePropertyChanged("Grp_AddSongs");
            }
        }
        public String Grp_SelectDatabase
        {
            get { return _grp_SelectDatabase; }
            set
            {
                if (value == _grp_SelectDatabase)
                    return;

                _grp_SelectDatabase = value;

                RaisePropertyChanged("Grp_SelectDatabase");
            }
        }
        public String Grp_Server
        {
            get { return _grp_Server; }
            set
            {
                if (value == _grp_Server)
                    return;

                _grp_Server = value;

                RaisePropertyChanged("Grp_Server");
            }
        }
        public String Grp_ExistingDatabases
        {
            get { return _grp_ExistingDatabases; }
            set
            {
                if (value == _grp_ExistingDatabases)
                    return;

                _grp_ExistingDatabases = value;

                RaisePropertyChanged("Grp_ExistingDatabases");
            }
        }
        public String Grp_NewDatabase
        {
            get { return _grp_NewDatabase; }
            set
            {
                if (value == _grp_NewDatabase)
                    return;

                _grp_NewDatabase = value;

                RaisePropertyChanged("Grp_NewDatabase");
            }
        }
        public String Grp_DatabaseConnections
        {
            get { return _grp_DatabaseConnections; }
            set
            {
                if (value == _grp_DatabaseConnections)
                    return;

                _grp_DatabaseConnections = value;

                RaisePropertyChanged("Grp_DatabaseConnections");
            }
        }
        public String Grp_CoverImages
        {
            get { return _grp_CoverImages; }
            set
            {
                if (value == _grp_CoverImages)
                    return;

                _grp_CoverImages = value;

                RaisePropertyChanged("Grp_CoverImages");
            }
        }
        public String Grp_Language
        {
            get { return _grp_Language; }
            set
            {
                if (value == _grp_Language)
                    return;

                _grp_Language = value;

                RaisePropertyChanged("Grp_Language");
            }
        }
        public String Grp_Wikipedia
        {
            get { return _grp_Wikipedia; }
            set
            {
                if (value == _grp_Wikipedia)
                    return;
                
                _grp_Wikipedia = value;

                RaisePropertyChanged("Grp_Wikipedia");
            }
        }
        public String Grp_PartyButtonTool
        {
            get { return _grp_PartyButtonTool; }
            set
            {
                if (value == _grp_PartyButtonTool)
                    return;
                
                _grp_PartyButtonTool = value;

                RaisePropertyChanged("Grp_PartyButtonTool");
            }
        }
        public String Grp_PartyButton
        {
            get { return _grp_PartyButton; }
            set
            {
                if (value == _grp_PartyButton)
                    return;
                
                _grp_PartyButton = value;

                RaisePropertyChanged("Grp_PartyButton");
            }
        }


        public void Localize()
        {
            _databasesViewModel.Localize();

            Tab_Database  = AmmLocalization.GetLocalizedString("ribbon_Tab_Database");
            Tab_Collection = AmmLocalization.GetLocalizedString("ribbon_Tab_Collection");
            Tab_Tools = AmmLocalization.GetLocalizedString("ribbon_Tab_Tools");
            Tab_Party = AmmLocalization.GetLocalizedString("ribbon_Tab_Party");
            Grp_AddSongs = AmmLocalization.GetLocalizedString("ribbon_Grp_AddSongs");
            Grp_SelectDatabase = AmmLocalization.GetLocalizedString("ribbon_Grp_SelectDatabase");
            Grp_Server = AmmLocalization.GetLocalizedString("ribbon_Grp_Server");
            Grp_ExistingDatabases = AmmLocalization.GetLocalizedString("ribbon_Grp_ExistingDatabases");
            Grp_NewDatabase = AmmLocalization.GetLocalizedString("ribbon_Grp_NewDatabase");
            Grp_DatabaseConnections = AmmLocalization.GetLocalizedString("ribbon_Grp_DatabaseConnections");
            Grp_CoverImages = AmmLocalization.GetLocalizedString("ribbon_Grp_CoverImages");
            Grp_Language = AmmLocalization.GetLocalizedString("ribbon_Grp_Language");
            Grp_Wikipedia = AmmLocalization.GetLocalizedString("ribbon_Grp_Wikipedia");
            Grp_PartyButtonTool = AmmLocalization.GetLocalizedString("ribbon_Grp_PartyButtonTool");
            Grp_PartyButton = AmmLocalization.GetLocalizedString("ribbon_Grp_PartyButton");
            Cmd_AddSongs = AmmLocalization.GetLocalizedString("ribbon_Cmd_AddSongs");
            Cmd_PartyButtonTool = AmmLocalization.GetLocalizedString("ribbon_Grp_PartyButtonTool");

            Cmd_Rename_ToolTip = AmmLocalization.GetLocalizedString("cmd_Rename_ToolTip");
            Cmd_AutoTag_ToolTip = AmmLocalization.GetLocalizedString("cmd_AutoTag_ToolTip");
            Cmd_CoverImages_ToolTip = AmmLocalization.GetLocalizedString("cmd_CoverImages_ToolTip");
        }
        #endregion // Localization

        #region Events
        public delegate void LanguageChangedEventHandler(object sender, EventArgs e);
        public event LanguageChangedEventHandler LanguageChanged;
        protected virtual void OnLanguageChanged(object sender, EventArgs e)
        {
            if (this.LanguageChanged != null)
            {
                this.LanguageChanged(this, e);
            }
        }
        public delegate void WikipediaChangedEventHandler(object sender, StringEventArgs e);
        public event WikipediaChangedEventHandler WikipediaChanged;
        protected virtual void OnWikipediaChanged(object sender, StringEventArgs e)
        {
            if (this.WikipediaChanged != null)
            {
                this.WikipediaChanged(this, e);
            }
        }
        #endregion

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

        ~RibbonViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_databasesViewModel != null)
	        {
                _databasesViewModel.Dispose();
	        }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion

    }


     
}
