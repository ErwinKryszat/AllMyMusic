using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllMyMusic.ViewModel
{
    public class DatabaseViewModel : ViewModelBase
    {
        #region Fields
        private ConnectionInfo _dbConInfo;
        private bool _isSelected;
        private RelayCommand<String> _selectDatabaseCommand;
        #endregion // Fields

        #region Customer Properties

        public ConnectionInfo DatabaseConnectionInformation
        {
            get { return _dbConInfo; }
            set
            {
                if (value == _dbConInfo)
                    return;

                _dbConInfo = value;

                RaisePropertyChanged("DatabaseConnectionInformation");
            }
        }

        public string DatabaseName
        {
            get { return _dbConInfo.DatabaseName; }
            set
            {
                if (value == _dbConInfo.DatabaseName)
                    return;

                _dbConInfo.DatabaseName = value;

                RaisePropertyChanged("DatabaseName");
            }
        }

        public string ServerName
        {
            get { return _dbConInfo.ServerName; }
            set
            {
                if (value == _dbConInfo.ServerName)
                    return;

                _dbConInfo.ServerName = value;

                RaisePropertyChanged("ServerName");
            }
        }

        public ServerType ServerType
        {
            get { return _dbConInfo.ServerType; }
            set
            {
                if (value == _dbConInfo.ServerType)
                    return;

                _dbConInfo.ServerType = value;

                RaisePropertyChanged("ServerType");
            }
        }

        public string ProductName
        {
            get { return _dbConInfo.ProductName; }
            set
            {
                if (value == _dbConInfo.ProductName)
                    return;

                _dbConInfo.ProductName = value;

                RaisePropertyChanged("ProductName");
            }
        }

        public string User
        {
            get { return _dbConInfo.User; }
            set
            {
                if (value == _dbConInfo.User)
                    return;

                _dbConInfo.User = value;

                RaisePropertyChanged("User");
            }
        }

        public string Password
        {
            get { return _dbConInfo.Password; }
            set
            {
                if (value == _dbConInfo.Password)
                    return;

                _dbConInfo.Password = value;

                RaisePropertyChanged("Password");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                RaisePropertyChanged("IsSelected");
            }
        }

        #endregion

        #region Constructor
        public DatabaseViewModel(ConnectionInfo dbConInfo)
        {
            if (dbConInfo == null)
                throw new ArgumentNullException("dbConInfo");

            _dbConInfo = dbConInfo;
        }
        #endregion

        #region Commands

        public ICommand SelectDatabaseCommand
        {
            get
            {
                if (null == _selectDatabaseCommand)
                    _selectDatabaseCommand = new RelayCommand<String>(ExecuteSelectDatabase, CanSelectDatabase);

                return _selectDatabaseCommand;
            }
        }
        private void ExecuteSelectDatabase(object notUsed)
        {
            ConnectionInfoEventArgs args = new ConnectionInfoEventArgs(_dbConInfo);
            OnDatabaseSelected(this, args);
        }
        private bool CanSelectDatabase(object notUsed)
        {
            return true;
        }

        #endregion // Commands

        #region Presentation Properties


        #endregion // Presentation Properties

        #region Public Methods

        #endregion // Public Methods

        #region Private Helpers

        

        #endregion // Private Helpers

        #region Events
        public delegate void DatabaseSelectedEventHandler(object sender, ConnectionInfoEventArgs e);

        public event DatabaseSelectedEventHandler DatabaseSelected;
        protected virtual void OnDatabaseSelected(object sender, ConnectionInfoEventArgs e)
        {
            if (this.DatabaseSelected != null)
            {
                this.DatabaseSelected(this, e);
            }
        }
   
        #endregion // Events

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

        ~DatabaseViewModel()
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
