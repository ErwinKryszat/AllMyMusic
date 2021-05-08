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

using AllMyMusic.Settings;
using AllMyMusic.ViewModel;

namespace AllMyMusic
{
    /// <summary>
    /// Interaction logic for frmFolderSelect.xaml
    /// </summary>
    public partial class frmManageCoverImages : Window, IDisposable
    {
        #region Fields
        private List<String> _folderList;
        private CoverImageToolsViewModel _coverImageToolsViewModel;
        #endregion

        #region Properties    
        public List<String> FolderList
        {
            get { return _folderList; }
        }
        #endregion

        #region Form
        public frmManageCoverImages()
        {
            InitializeComponent();
            _coverImageToolsViewModel = new CoverImageToolsViewModel();
            DataContext = _coverImageToolsViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Settings();
            Localize();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _folderList = _coverImageToolsViewModel.MultiFolderSelectVM.GetAllCheckedFolders();
                _coverImageToolsViewModel.MultiFolderSelectVM.Unload();

                AppSettings.FormSettings.FrmManageCoverImages_Position = new Point(this.Left, this.Top);
                AppSettings.FormSettings.FrmManageCoverImages_Size = new Size(this.Width, this.Height);
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
                this.Left = AppSettings.FormSettings.FrmManageCoverImages_Position.X;
                this.Top = AppSettings.FormSettings.FrmManageCoverImages_Position.Y;

                if (AppSettings.FormSettings.FrmFolderSelect_Size != new Size(0, 0))
                {
                    this.Width = AppSettings.FormSettings.FrmManageCoverImages_Size.Width;
                    this.Height = AppSettings.FormSettings.FrmManageCoverImages_Size.Height;
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
            cmdOK.Content = AmmLocalization.GetLocalizedString("Common_OK");
            cmdCancel.Content = AmmLocalization.GetLocalizedString("Common_Cancel");
        }

        #endregion

        #region Commands
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (_coverImageToolsViewModel != null)
            {
                _coverImageToolsViewModel.Save();
            }
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

        ~frmManageCoverImages()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_coverImageToolsViewModel != null)
            {
                _coverImageToolsViewModel.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion

     
    }
}
