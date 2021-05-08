using System;
using System.Windows;
using AllMyMusic.ViewModel;
using AllMyMusic.Settings;

namespace AllMyMusic
{
    /// <summary>
    /// Interaction logic for frmConnectDatabase.xaml
    /// </summary>
    public partial class frmConnectServer : Window
    {
        private DatabasesViewModel _databasesViewModel;

        public frmConnectServer(DatabasesViewModel databasesViewModel)
        {
            InitializeComponent();

            _databasesViewModel = databasesViewModel;
            _databasesViewModel.SelectedServerTypeChanged += new DatabasesViewModel.ServerTypeChangedEventHandler(SelectedServerTypeChanged);

            DataContext = _databasesViewModel;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Settings();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AppSettings.FormSettings.FrmConnectServer_Position = new Point(this.Left, this.Top);
            AppSettings.FormSettings.FrmConnectServer_Size = new Size(this.Width, this.Height);
        }

        private void SelectedServerTypeChanged(object sender, ConnectionInfoEventArgs e)
        {
            this.Close();
        }

        private void Settings()
        {
            this.Left = AppSettings.FormSettings.FrmConnectServer_Position.X;
            this.Top = AppSettings.FormSettings.FrmConnectServer_Position.Y;

            if (AppSettings.FormSettings.FrmConnectServer_Size != new Size(0, 0))
            {
                this.Width = AppSettings.FormSettings.FrmConnectServer_Size.Width;
                this.Height = AppSettings.FormSettings.FrmConnectServer_Size.Height;
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

        ~frmConnectServer()
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
