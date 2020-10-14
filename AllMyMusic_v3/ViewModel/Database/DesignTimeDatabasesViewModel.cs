using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AllMyMusic_v3.ViewModel
{
    public class DesignTimeDatabasesViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private String _activeDatabaseName;
        
        private ObservableCollection<String> _allUserDatabaseNames;
        private String _allUserDatabasesSelectedDatabase;
        
        private String _createDatabaseName;
        private String _createDatabaseCollation;
        private String _selectedServerName;
        private String _userName;
        private String _password;
        private Boolean _showUserName;

        private ObservableCollection<DatabaseViewModel> _databaseConnections;
        private Int32 _selectedDatabaseConnectionIndex;
        private DatabaseViewModel _selectedDatabaseConnection;
        
        private ObservableCollection<String> _serverNames;
        private ObservableCollection<String> _serverTypes;
        private ObservableCollection<String> _collations;
        #endregion // Fields

        #region Presentation Properties

        
        public ObservableCollection<DatabaseViewModel> DatabaseConnections
        {
            get { return _databaseConnections; }
            set
            {
                if (value == _databaseConnections)
                    return;

                _databaseConnections = value;

                RaisePropertyChanged("DatabaseConnections");
            }
        }

        public ObservableCollection<String> AllUserDatabaseNames
        {
            get { return _allUserDatabaseNames; }
            set
            {
                if (value == _allUserDatabaseNames)
                    return;

                _allUserDatabaseNames = value;

                RaisePropertyChanged("AllUserDatabaseNames");
            }
        }

        public String AllUserDatabasesSelectedDatabase
        {
            get { return _allUserDatabasesSelectedDatabase; }
            set
            {
                if (value == _allUserDatabasesSelectedDatabase)
                    return;

                _allUserDatabasesSelectedDatabase = value;

                RaisePropertyChanged("AllUserDatabasesSelectedDatabase");
            }
        }

        public ObservableCollection<String> Collations
        {
            get { return _collations; }
            set
            {
                if (value == _collations)
                    return;

                _collations = value;

                RaisePropertyChanged("Collations");
            }
        }   

        public String SelectedServerName
        {
            get { return _selectedServerName; }
            set
            {
                if (value == _selectedServerName)
                    return;

                _selectedServerName = value;

                RaisePropertyChanged("SelectedServerName");
            }
        }

        private ServerType _selectedServerType = ServerType.SqlServer;
        public ServerType SelectedServerType
        {
            get { return ServerType.SqlServer; }
            set { _selectedServerType = value; }
        }

        public String UserName
        {
            get { return _userName; }
            set
            {
                if (value == _userName)
                    return;

                _userName = value;

                RaisePropertyChanged("UserName");
            }
        }

        public String Password
        {
            get { return _password; }
            set
            {
                if (value == _password)
                    return;

                _password = value;

                RaisePropertyChanged("Password");
            }
        }

        public Boolean ShowUserName
        {
            get { return _showUserName; }
            set
            {
                if (value == _showUserName)
                    return;

                _showUserName = value;

                RaisePropertyChanged("ShowUserName");
            }
        }

        public ObservableCollection<String> ServerNames
        {
            get { return _serverNames; }
            set
            {
                if (value == _serverNames)
                    return;

                _serverNames = value;

                RaisePropertyChanged("ServerNames");
            }
        }

        public ObservableCollection<String> ServerTypes
        {
            get { return _serverTypes; }
            set
            {
                if (value == _serverTypes)
                    return;

                _serverTypes = value;

                RaisePropertyChanged("ServerTypes");
            }
        }

        public DatabaseViewModel SelectedDatabaseConnection
        {
            get { return _selectedDatabaseConnection; }
            set
            {
                if (value == _selectedDatabaseConnection)
                    return;

                _selectedDatabaseConnection = value;

                RaisePropertyChanged("SelectedDatabaseConnection");
            }
        }

        public Int32 SelectedDatabaseConnectionIndex
        {
            get { return _selectedDatabaseConnectionIndex; }
            set
            {
                if (value == _selectedDatabaseConnectionIndex)
                    return;

                _selectedDatabaseConnectionIndex = value;

                RaisePropertyChanged("SelectedDatabaseConnectionIndex");
            }
        }
        
        public String ActiveDatabaseName
        {
            get { return _activeDatabaseName; }
            set
            {
                if (value == _activeDatabaseName)
                    return;

                _activeDatabaseName = value;

                RaisePropertyChanged("ActiveDatabaseName");
            }
        }

        public String CreateDatabaseName
        {
            get { return _createDatabaseName; }
            set
            {
                if (value == _createDatabaseName)
                    return;

                _createDatabaseName = value;

                if (_createDatabaseCollation == "abc")
                {
                    Int32 i = 0;
                    i++;
                }

                RaisePropertyChanged("CreateDatabaseName");
            }
        }

        public String CreateDatabaseCollation
        {
            get { return _createDatabaseCollation; }
            set
            {
                if (value == _createDatabaseCollation)
                    return;

                _createDatabaseCollation = value;

                RaisePropertyChanged("CreateDatabaseCollation");
            }
        }
        #endregion

        #region Constructor
        public DesignTimeDatabasesViewModel()
        {

        }
       
        #endregion

        #region Localization
        private String _cmd_ConnectServer = "ConnectServer";
        private String _cmd_ConnectDatabase = "Connect Database";
        private String _cmd_PurgeDatabase = "Purge Database";
        private String _cmd_DeleteDatabase = "Delete Database";
        private String _cmd_CreateDatabase = "Create Database";

        private String _cmd_ConnectServer_ToolTip;
        private String _cmd_ConnectDatabase_ToolTip;
        private String _cmd_PurgeDatabase_ToolTip;
        private String _cmd_DeleteDatabase_ToolTip;
        private String _cmd_DeleteConnection_ToolTip;
        private String _cmd_Connection_Up_ToolTip;
        private String _cmd_Connection_Down_ToolTip;
        private String _listbox_DatabaseNames_ToolTip;
        private String _listbox_Connections_ToolTip;

        private String _tbDatabaseName = "Database Name";
        private String _combo_Collation = "Collation";

        private String _combo_ServerType = "Server Type";
        private String _combo_ServerName = "Server Name";
        private String _tbUser = "Username";
        private String _tbPassword = "Password";

        private String _combo_ServerType_ToolTip;
        private String _combo_ServerName_ToolTip;
        private String _tbUser_ToolTip;
        private String _tbPassword_ToolTip;

        private String _grpDatabaseServer = "Database Server";
        private String _grpDatabaseList = "Databases";
        private String _grpCreateDatabase = "Create Database";

        private String _title = "Connect Server";
        private String _titleTip = "This dialog helps you connecting to a database on your computer or in the network";

        public String Cmd_ConnectServer
        {
            get { return _cmd_ConnectServer; }
            set
            {
                if (value == _cmd_ConnectServer)
                    return;

                _cmd_ConnectServer = value;

                RaisePropertyChanged("Cmd_ConnectServer");
            }
        }
        public String Cmd_ConnectDatabase
        {
            get { return _cmd_ConnectDatabase; }
            set
            {
                if (value == _cmd_ConnectDatabase)
                    return;

                _cmd_ConnectDatabase = value;

                RaisePropertyChanged("Cmd_ConnectDatabase");
            }
        }
        public String Cmd_PurgeDatabase
        {
            get { return _cmd_PurgeDatabase; }
            set
            {
                if (value == _cmd_PurgeDatabase)
                    return;

                _cmd_PurgeDatabase = value;

                RaisePropertyChanged("Cmd_PurgeDatabase");
            }
        }
        public String Cmd_DeleteDatabase
        {
            get { return _cmd_DeleteDatabase; }
            set
            {
                if (value == _cmd_DeleteDatabase)
                    return;

                _cmd_DeleteDatabase = value;

                RaisePropertyChanged("Cmd_DeleteDatabase");
            }
        }
        public String Cmd_CreateDatabase
        {
            get { return _cmd_CreateDatabase; }
            set
            {
                if (value == _cmd_CreateDatabase)
                    return;

                _cmd_CreateDatabase = value;

                RaisePropertyChanged("Cmd_CreateDatabase");
            }
        }

        public String Cmd_ConnectServer_ToolTip
        {
            get { return _cmd_ConnectServer_ToolTip; }
            set
            {
                if (value == _cmd_ConnectServer_ToolTip)
                    return;

                _cmd_ConnectServer_ToolTip = value;

                RaisePropertyChanged("Cmd_ConnectServer_ToolTip");
            }
        }
        public String Cmd_ConnectDatabase_ToolTip
        {
            get { return _cmd_ConnectDatabase_ToolTip; }
            set
            {
                if (value == _cmd_ConnectDatabase_ToolTip)
                    return;

                _cmd_ConnectDatabase_ToolTip = value;

                RaisePropertyChanged("Cmd_ConnectDatabase_ToolTip");
            }
        }
        public String Cmd_PurgeDatabase_ToolTip
        {
            get { return _cmd_PurgeDatabase_ToolTip; }
            set
            {
                if (value == _cmd_PurgeDatabase_ToolTip)
                    return;

                _cmd_PurgeDatabase_ToolTip = value;

                RaisePropertyChanged("Cmd_PurgeDatabase_ToolTip");
            }
        }
        public String Cmd_DeleteDatabase_ToolTip
        {
            get { return _cmd_DeleteDatabase_ToolTip; }
            set
            {
                if (value == _cmd_DeleteDatabase_ToolTip)
                    return;

                _cmd_DeleteDatabase_ToolTip = value;

                RaisePropertyChanged("Cmd_DeleteDatabase_ToolTip");
            }
        }
        public String Cmd_DeleteConnection_ToolTip
        {
            get { return _cmd_DeleteConnection_ToolTip; }
            set
            {
                if (value == _cmd_DeleteConnection_ToolTip)
                    return;

                _cmd_DeleteConnection_ToolTip = value;

                RaisePropertyChanged("Cmd_DeleteConnection_ToolTip");
            }
        }
        public String Cmd_Connection_Up_ToolTip
        {
            get { return _cmd_Connection_Up_ToolTip; }
            set
            {
                if (value == _cmd_Connection_Up_ToolTip)
                    return;

                _cmd_Connection_Up_ToolTip = value;

                RaisePropertyChanged("Cmd_Connection_Up_ToolTip");
            }
        }
        public String Cmd_Connection_Down_ToolTip
        {
            get { return _cmd_Connection_Down_ToolTip; }
            set
            {
                if (value == _cmd_Connection_Down_ToolTip)
                    return;

                _cmd_Connection_Down_ToolTip = value;

                RaisePropertyChanged("Cmd_Connection_Down_ToolTip");
            }
        }
        public String Listbox_DatabaseNames_ToolTip
        {
            get { return _listbox_DatabaseNames_ToolTip; }
            set
            {
                if (value == _listbox_DatabaseNames_ToolTip)
                    return;

                _listbox_DatabaseNames_ToolTip = value;

                RaisePropertyChanged("Listbox_DatabaseNames_ToolTip");
            }
        }
        public String Listbox_Connections_ToolTip
        {
            get { return _listbox_Connections_ToolTip; }
            set
            {
                if (value == _listbox_Connections_ToolTip)
                    return;

                _listbox_Connections_ToolTip = value;

                RaisePropertyChanged("Listbox_Connections_ToolTip");
            }
        }

        public String TbDatabaseName
        {
            get { return _tbDatabaseName; }
            set
            {
                if (value == _tbDatabaseName)
                    return;

                _tbDatabaseName = value;

                RaisePropertyChanged("TbDatabaseName");
            }
        }
        public String Combo_Collation
        {
            get { return _combo_Collation; }
            set
            {
                if (value == _combo_Collation)
                    return;

                _combo_Collation = value;

                RaisePropertyChanged("Combo_Collation");
            }
        }
        public String Combo_ServerType
        {
            get { return _combo_ServerType; }
            set
            {
                if (value == _combo_ServerType)
                    return;

                _combo_ServerType = value;

                RaisePropertyChanged("Combo_ServerType");
            }
        }
        public String Combo_ServerName
        {
            get { return _combo_ServerName; }
            set
            {
                if (value == _combo_ServerName)
                    return;

                _combo_ServerName = value;

                RaisePropertyChanged("Combo_ServerName");
            }
        }
        public String TbUser
        {
            get { return _tbUser; }
            set
            {
                if (value == _tbUser)
                    return;

                _tbUser = value;

                RaisePropertyChanged("TbUser");
            }
        }
        public String TbPassword
        {
            get { return _tbPassword; }
            set
            {
                if (value == _tbPassword)
                    return;

                _tbPassword = value;

                RaisePropertyChanged("TbPassword");
            }
        }

        public String Combo_ServerType_ToolTip
        {
            get { return _combo_ServerType_ToolTip; }
            set
            {
                if (value == _combo_ServerType_ToolTip)
                    return;

                _combo_ServerType_ToolTip = value;

                RaisePropertyChanged("Combo_ServerType_ToolTip");
            }
        }
        public String Combo_ServerName_ToolTip
        {
            get { return _combo_ServerName_ToolTip; }
            set
            {
                if (value == _combo_ServerName_ToolTip)
                    return;

                _combo_ServerName_ToolTip = value;

                RaisePropertyChanged("Combo_ServerName_ToolTip");
            }
        }
        public String TbUser_ToolTip
        {
            get { return _tbUser_ToolTip; }
            set
            {
                if (value == _tbUser_ToolTip)
                    return;

                _tbUser_ToolTip = value;

                RaisePropertyChanged("TbUser_ToolTip");
            }
        }
        public String TbPassword_ToolTip
        {
            get { return _tbPassword_ToolTip; }
            set
            {
                if (value == _tbPassword_ToolTip)
                    return;

                _tbPassword_ToolTip = value;

                RaisePropertyChanged("TbPassword_ToolTip");
            }
        }

        public String GrpDatabaseServer
        {
            get { return _grpDatabaseServer; }
            set
            {
                if (value == _grpDatabaseServer)
                    return;

                _grpDatabaseServer = value;

                RaisePropertyChanged("GrpDatabaseServer");
            }
        }
        public String GrpDatabaseList
        {
            get { return _grpDatabaseList; }
            set
            {
                if (value == _grpDatabaseList)
                    return;

                _grpDatabaseList = value;

                RaisePropertyChanged("GrpDatabaseList");
            }
        }
        public String GrpCreateDatabase
        {
            get { return _grpCreateDatabase; }
            set
            {
                if (value == _grpCreateDatabase)
                    return;

                _grpCreateDatabase = value;

                RaisePropertyChanged("GrpCreateDatabase");
            }
        }

        public String Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                    return;

                _title = value;

                RaisePropertyChanged("Title");
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

        public void Localize()
        {
            Cmd_ConnectServer = AmmLocalization.GetLocalizedString("cmd_ConnectServer");
            Cmd_ConnectDatabase = AmmLocalization.GetLocalizedString("cmd_ConnectDatabase");
            Cmd_PurgeDatabase = AmmLocalization.GetLocalizedString("cmd_PurgeDatabase");
            Cmd_DeleteDatabase = AmmLocalization.GetLocalizedString("cmd_DeleteDatabase");
            Cmd_CreateDatabase = AmmLocalization.GetLocalizedString("cmd_CreateDatabase");

            Cmd_ConnectServer_ToolTip = AmmLocalization.GetLocalizedString("cmd_ConnectServer_Tooltip");
            Cmd_ConnectDatabase_ToolTip = AmmLocalization.GetLocalizedString("cmd_ConnectDatabase_Tooltip");
            Cmd_PurgeDatabase_ToolTip = AmmLocalization.GetLocalizedString("cmd_PurgeDatabase_Tooltip");
            Cmd_DeleteDatabase_ToolTip = AmmLocalization.GetLocalizedString("cmd_DeleteDatabase_Tooltip");
            Cmd_DeleteConnection_ToolTip = AmmLocalization.GetLocalizedString("cmd_DeleteConnection_Tooltip");
            Cmd_Connection_Up_ToolTip = AmmLocalization.GetLocalizedString("cmd_Connection_Up_Tooltip");
            Cmd_Connection_Down_ToolTip = AmmLocalization.GetLocalizedString("cmd_Connection_Down_Tooltip");           

            Listbox_DatabaseNames_ToolTip = AmmLocalization.GetLocalizedString("listbox_Databases_Tooltip");
            Listbox_Connections_ToolTip = AmmLocalization.GetLocalizedString("listbox_Connections_Tooltip");

            TbDatabaseName = AmmLocalization.GetLocalizedString("tbDatabaseName");
            Combo_Collation = AmmLocalization.GetLocalizedString("combo_Collation");

            Combo_ServerType = AmmLocalization.GetLocalizedString("frmDbSetup_Label1");
            Combo_ServerName = AmmLocalization.GetLocalizedString("frmDbSetup_Label2");
            TbUser = AmmLocalization.GetLocalizedString("frmDbSetup_Label3");
            TbPassword = AmmLocalization.GetLocalizedString("frmDbSetup_Label4");

            Combo_ServerType_ToolTip = AmmLocalization.GetLocalizedString("frmDbSetup_Tooltip1");
            Combo_ServerName_ToolTip = AmmLocalization.GetLocalizedString("frmDbSetup_Tooltip2");
            TbUser_ToolTip = AmmLocalization.GetLocalizedString("frmDbSetup_Tooltip3");
            TbPassword_ToolTip = AmmLocalization.GetLocalizedString("frmDbSetup_Tooltip4");

            GrpDatabaseServer = AmmLocalization.GetLocalizedString("frmDbSetup_grpDatabaseServer");
            GrpDatabaseList = AmmLocalization.GetLocalizedString("frmDbSetup_grpDatabaseList");
            GrpCreateDatabase = AmmLocalization.GetLocalizedString("frmDbSetup_grpCreateDatabase");

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

        ~DesignTimeDatabasesViewModel()
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
