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

using AllMyMusic_v3.Settings;
using Metadata.Mp3;
using Metadata.ID3;

namespace AllMyMusic_v3.ViewModel
{
    public class ToolsViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ConnectionInfo _conInfo;
        private PropertiesToolViewModel _propertiesToolVM;
        private AutoTagToolViewModel _autoTagToolVM;
        private RenameToolViewModel _renameToolVM;
        private SongsViewModel _songsVM;
        private Tokenizer _tokenizer;
        private ToolType _toolType;
        private Int32 _tabSelectedIndex = 0;
        private Boolean _viewColumnSelector;
        private Boolean _tooltipsEnabled;


        private double _propertiesViewWidth;
        #endregion // Fields

        #region Commands

        #endregion

        #region Presentation Properties
        public PropertiesToolViewModel PropertiesToolVM
        {
            get { return _propertiesToolVM; }
            set
            {
                _propertiesToolVM = value;

                RaisePropertyChanged("PropertiesToolVM");
            }
        }
        public AutoTagToolViewModel AutoTagToolVM
        {
            get { return _autoTagToolVM; }
            set
            {
                if (value == _autoTagToolVM)
                    return;

                _autoTagToolVM = value;

                RaisePropertyChanged("AutoTagToolVM");
            }
        }
        public RenameToolViewModel RenameToolVM
        {
            get { return _renameToolVM; }
            set
            {
                if (value == _renameToolVM)
                    return;

                _renameToolVM = value;

                RaisePropertyChanged("RenameToolVM");
            }
        }
        public SongsViewModel SongsVM
        {
            get { return _songsVM; }
            set
            {
                if (value == _songsVM)
                    return;

                _songsVM = value;

                RaisePropertyChanged("SongsVM");
            }
        }
        public ChangedPropertiesList ChangedProperties
        {
            get 
            {
                if (_toolType == AllMyMusic_v3.ToolType.PropertiesTool)
                {
                    return _propertiesToolVM.ChangedProperties; 
                }

                if (_toolType == AllMyMusic_v3.ToolType.AutoTagTool)
                {
                    return _autoTagToolVM.ChangedProperties;
                }

                return _propertiesToolVM.ChangedProperties; 
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
        public Boolean ViewColumnSelector
        {
            get { return _viewColumnSelector; }
            set
            {
                //if (value == _viewColumnSelector)
                //    return;

                _viewColumnSelector = value;
                
                RaisePropertyChanged("ViewColumnSelector");
            }
        }
        public ToolType ToolType
        {
            get { return _toolType; }
            set 
            {
                if (value == _toolType)
                    return;

                _toolType = value;
                OnToolTypeChanged();
                RaisePropertyChanged("ToolType");
            }
        }
        public double PropertiesViewWidth
        {
            get { return _propertiesViewWidth; }
            set
            {
                if (value == _propertiesViewWidth)
                    return;

                _propertiesViewWidth = value;

                RaisePropertyChanged("PropertiesViewWidth");
            }
        }
        public Int32 TabSelectedIndex
        {
            get { return _tabSelectedIndex; }
            set
            {
                if (value == _tabSelectedIndex)
                    return;

                _tabSelectedIndex = value;
                OnTabSelectedIndexChanged();

                RaisePropertyChanged("TabSelectedIndex");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public ToolsViewModel(ConnectionInfo conInfo)
        {
            _conInfo = conInfo;
            // used onl for DesignTime VM
        }   
        public ToolsViewModel(ConnectionInfo conInfo, ObservableCollection<SongItem> songs)
        {
            _conInfo = conInfo;
            _songsVM = new SongsViewModel(_conInfo);
            _songsVM.Songs = songs;

            _propertiesToolVM = new PropertiesToolViewModel(_conInfo, songs);

            _autoTagToolVM = new AutoTagToolViewModel();
            _autoTagToolVM.PreviewRequested += new AutoTagToolViewModel.PreviewRequestedEventHandler(AutoTagTool_PreviewRequested);
            _autoTagToolVM.Init();

            _renameToolVM = new RenameToolViewModel();
            _renameToolVM.PreviewRequested += new RenameToolViewModel.PreviewRequestedEventHandler(RenameTool_PreviewRequested);
            _renameToolVM.Init();

            PropertiesViewWidth = AppSettings.FormSettings.FrmTools_PropertiesViewWidth;
            ViewColumnSelector = AppSettings.FormSettings.FrmTools_ColumnSelectorVisible;
            TooltipsEnabled = AppSettings.FormSettings.FrmTools_TooltipsEnabled;
            ViewColumnSelector = true;

            Localize();
        }
        public ToolsViewModel(ConnectionInfo conInfo, String strSongsQuery)
        {
            _conInfo = conInfo;
            _songsVM = new SongsViewModel(_conInfo);
            
            Task.Run(() => LoadSongs(strSongsQuery));

            _autoTagToolVM = new AutoTagToolViewModel();
            _autoTagToolVM.PreviewRequested += new AutoTagToolViewModel.PreviewRequestedEventHandler(AutoTagTool_PreviewRequested);
            _autoTagToolVM.Init();

            _renameToolVM = new RenameToolViewModel();
            _renameToolVM.PreviewRequested += new RenameToolViewModel.PreviewRequestedEventHandler(RenameTool_PreviewRequested);
            _renameToolVM.Init();

            PropertiesViewWidth = AppSettings.FormSettings.FrmTools_PropertiesViewWidth;
            ViewColumnSelector = AppSettings.FormSettings.FrmTools_ColumnSelectorVisible;
            TooltipsEnabled = AppSettings.FormSettings.FrmTools_TooltipsEnabled;
            ViewColumnSelector = true;

            Localize();
        }
        public ToolsViewModel(ConnectionInfo conInfo, SongItem song)
        {
            SongItem _song = song;

            Mp3Metaedit mp3Metaedit = new Mp3Metaedit(_song.SongFullPath);
            _song = mp3Metaedit.ReadMetadata();

            _propertiesToolVM = new PropertiesToolViewModel(conInfo, _song);

            _autoTagToolVM = new AutoTagToolViewModel();
            _autoTagToolVM.PreviewRequested += new AutoTagToolViewModel.PreviewRequestedEventHandler(AutoTagTool_PreviewRequested);
            _autoTagToolVM.Init();

            _renameToolVM = new RenameToolViewModel();
            _renameToolVM.PreviewRequested += new RenameToolViewModel.PreviewRequestedEventHandler(RenameTool_PreviewRequested);
            _renameToolVM.Init();

            PropertiesViewWidth = 800;

            Localize();
        }
       
        public void Close()
        {
            AppSettings.FormSettings.FrmTools_TooltipsEnabled = TooltipsEnabled;

            if (_propertiesToolVM != null)
            {
                _propertiesToolVM.Close();
            }
            if (_songsVM != null)
            {
                _songsVM.Close();
            }

            if (_autoTagToolVM != null)
            {
                _autoTagToolVM.Close();
            }
            if (_renameToolVM != null)
            {
                _renameToolVM.Close();
            }
        }
        #endregion  // Constructor

        #region private 
        private async Task LoadSongs(String strSongsQuery)
        {
            await _songsVM.GetSongs(strSongsQuery);

            PropertiesToolVM = new PropertiesToolViewModel(_conInfo, _songsVM.Songs);
        }
        private void AutoTagTool_PreviewRequested(object sender, EventArgs e)
        {
            if (_toolType == AllMyMusic_v3.ToolType.AutoTagTool)
            {
                if (_songsVM.Songs.Count > 0)
                {
                    _tokenizer = new Tokenizer(_songsVM.Songs, _autoTagToolVM.ReplaceUnderscores, _autoTagToolVM.UndoUpperCase);
                    _tokenizer.ParseFilename(_autoTagToolVM.AutoTagPattern);
                }
            }
        }
        private void RenameTool_PreviewRequested(object sender, EventArgs e)
        {
            if (_toolType == AllMyMusic_v3.ToolType.RenameTool)
            {
                if (_songsVM.Songs.Count > 0)
                {
                    _tokenizer = new Tokenizer(_songsVM.Songs);
                    _tokenizer.AssembleFilename(_renameToolVM.RenamePattern);
                }
            }
        }
        private void OnTabSelectedIndexChanged()
        {
            switch (_tabSelectedIndex)
            {
                case 0:
                    ShowProperties();
                    break;

                case 1:
                    ShowRenameTool();
                    break;

                case 2:
                    ShowAutoTagTool();
                    break;

                default:
                    ShowProperties();
                    break;
            }
        }
        private void ShowProperties()
        {
            ToolType = ToolType.PropertiesTool;
        }
        private void ShowAutoTagTool()
        {
            ToolType = ToolType.AutoTagTool;
            AutoTagTool_PreviewRequested(this, EventArgs.Empty);
        }
        private void ShowRenameTool()
        {
            ToolType = ToolType.RenameTool;

            _tokenizer = new Tokenizer(_songsVM.Songs);
            _tokenizer.AssembleFilename(_renameToolVM.RenamePattern);
        }
        private void OnToolTypeChanged()
        {
            ToolTypeEventArgs args = new ToolTypeEventArgs(_toolType);
            OnToolTypeChanged(this, args);
        }
        #endregion

        #region Events
        public delegate void ToolTypeChangedEventHandler(object sender, ToolTypeEventArgs e);
        public event ToolTypeChangedEventHandler ToolTypeChanged;
        protected virtual void OnToolTypeChanged(object sender, ToolTypeEventArgs e)
        {
            if (this.ToolTypeChanged != null)
            {
                this.ToolTypeChanged(this, e);
            }
        }
        #endregion

        #region Localization
        private String _autoTagToolLabel = "Auto Tag from Filename";
        private String _propertiesToolLabel = "Properties";
        private String _renameToolLabel = "Rename MP3 Files";
        private String _formTitle = "Tools";

        private String _titleTip = "This form helps organizing your MP3 files";
        private String _toolTipsLabel = "Tools Tips";


        private String _columnSelectorToolTip = "Select the columns you want to see in the table to the right";
        private String _dadaGridToolTip = "Reorder columns with Drag and Drop";

        public String ColumnSelectorToolTip
        {
            get { return _columnSelectorToolTip; }
            set
            { _columnSelectorToolTip = value; }
        }
        public String DadaGridToolTip
        {
            get { return _dadaGridToolTip; }
            set { _dadaGridToolTip = value; }
        }
        

        public String AutoTagToolLabel
        {
            get { return _autoTagToolLabel; }
            set
            {
                if (value == _autoTagToolLabel)
                    return;

                _autoTagToolLabel = value;

                RaisePropertyChanged("AutoTagToolLabel");
            }
        }
        public String PropertiesToolLabel
        {
            get { return _propertiesToolLabel; }
            set
            {
                if (value == _propertiesToolLabel)
                    return;

                _propertiesToolLabel = value;

                RaisePropertyChanged("PropertiesToolLabel");
            }
        }
        public String RenameToolLabel
        {
            get { return _renameToolLabel; }
            set
            {
                if (value == _renameToolLabel)
                    return;

                _renameToolLabel = value;

                RaisePropertyChanged("RenameToolLabel");
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
        public String ToolTipsLabel
        {
            get { return _toolTipsLabel; }
            set
            {
                if (value == _toolTipsLabel)
                    return;

                _toolTipsLabel = value;

                RaisePropertyChanged("ToolTipsLabel");
            }
        }


     
        public String Title
        {
            get { return _formTitle; }
            set
            {
                if (value == _formTitle)
                    return;

                _formTitle = value;

                RaisePropertyChanged("Title");
            }

        }
        public void Localize()
        {
            AutoTagToolLabel = AmmLocalization.GetLocalizedString("frmAutoTag_Title");
            PropertiesToolLabel = AmmLocalization.GetLocalizedString("frmProperties_Title");
            RenameToolLabel = AmmLocalization.GetLocalizedString("frmRename_Title");
            Title = AmmLocalization.GetLocalizedString("frmAllMyMusic_Tools");

            TitleTip = AmmLocalization.GetLocalizedString("frmTools_TitleTip");
            ToolTipsLabel = AmmLocalization.GetLocalizedString("frmTools_ToolTipsLabel");

            ColumnSelectorToolTip = AmmLocalization.GetLocalizedString("frmTools_ColumnSelectorToolTip");
            DadaGridToolTip = AmmLocalization.GetLocalizedString("frmTools_DadaGridToolTip");
           
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

        ~ToolsViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {

            if (_songsVM != null)
            {
                _songsVM.Dispose();
            }

            if (_autoTagToolVM != null)
            {
                _autoTagToolVM.Dispose();
            }

            if (_renameToolVM != null)
            {
                _renameToolVM.Dispose();
            }

            if (_propertiesToolVM != null)
            {
                _propertiesToolVM.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
