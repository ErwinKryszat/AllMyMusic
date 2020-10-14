using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using AllMyMusic_v3.Settings;
using AllMyMusic_v3.ViewModel;

namespace AllMyMusic_v3
{
    /// <summary>
    /// Interaction logic for frmFolderSelect.xaml
    /// </summary>
    public partial class frmFolderSelect : Window, IDisposable
    {
        #region Fields
        private MultiFolderSelectViewModel _multiFolderSelectViewModel;
        private List<String> _expandedDrives;
        private List<String> _expandedFolders;
        private FilesystemAction _fsAction;
        private List<String> _folderList;
        #endregion

        #region Properties
        public List<String> FolderList
        {
            get { return _folderList; }
        }



        #endregion

        #region Form
        public frmFolderSelect()
        {
            InitializeComponent();

            try
            {
                _multiFolderSelectViewModel = new MultiFolderSelectViewModel();
            }
            catch (SettingsException Err)
            {
                String errorMessage = "Error creating MultiFolderSelectViewModel";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            
            this.DataContext = _multiFolderSelectViewModel;
        }
        public frmFolderSelect(FilesystemAction fileSystemAction)
        {
            _fsAction = fileSystemAction;
            InitializeComponent();

            try
            {
                _multiFolderSelectViewModel = new MultiFolderSelectViewModel();
            }
            catch (SettingsException Err)
            {
                String errorMessage = "Error creating MultiFolderSelectViewModel";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            
            this.DataContext = _multiFolderSelectViewModel;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Settings();
            Localize();
            InitControlls();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _folderList = _multiFolderSelectViewModel.GetAllCheckedFolders();
                _expandedDrives = _multiFolderSelectViewModel.ExpandedDrives;
                _expandedFolders = _multiFolderSelectViewModel.ExpandedFolders;
                _multiFolderSelectViewModel.Unload();

                AppSettings.FormSettings.FrmFolderSelect_Position = new Point(this.Left, this.Top);
                AppSettings.FormSettings.FrmFolderSelect_Size = new Size(this.Width, this.Height);
                AppSettings.FormSettings.frmFolderSelect_ExpandedDrives = _expandedDrives;
                AppSettings.FormSettings.frmFolderSelect_ExpandedFolders = _expandedFolders;
            }
            catch (SettingsException Err)
            {
                String errorMessage = "Error saving Form Settings ";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        private void Settings()
        {
            try
            {
                this.Left = AppSettings.FormSettings.FrmFolderSelect_Position.X;
                this.Top = AppSettings.FormSettings.FrmFolderSelect_Position.Y;

                if (AppSettings.FormSettings.FrmFolderSelect_Size != new Size(0, 0))
                {
                    this.Width = AppSettings.FormSettings.FrmFolderSelect_Size.Width;
                    this.Height = AppSettings.FormSettings.FrmFolderSelect_Size.Height;
                }

                if (AppSettings.FormSettings.frmFolderSelect_ExpandedDrives != null)
                {
                    _multiFolderSelectViewModel.ExpandedDrives = AppSettings.FormSettings.frmFolderSelect_ExpandedDrives;
                }

                if (AppSettings.FormSettings.frmFolderSelect_ExpandedFolders != null)
                {
                    _multiFolderSelectViewModel.ExpandedFolders = AppSettings.FormSettings.frmFolderSelect_ExpandedFolders;
                }
            }
            catch (SettingsException Err)
            {
                String errorMessage = "Error loading Form Settings ";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        private void Localize()
        {
            
            this.Title = AmmLocalization.GetLocalizedString("frmFolderSelction_Title1");
            infoArea.Content = AmmLocalization.GetLocalizedString("frmFolderSelction_Info");
            cmdOK.Content = AmmLocalization.GetLocalizedString("Common_OK");
            cmdCancel.Content = AmmLocalization.GetLocalizedString("Common_Cancel");  
        }
        private void InitControlls()
        {

        }
        #endregion

        #region Commands
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

        ~frmFolderSelect()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_multiFolderSelectViewModel != null)
            {
                _multiFolderSelectViewModel.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion

     
    }
}
