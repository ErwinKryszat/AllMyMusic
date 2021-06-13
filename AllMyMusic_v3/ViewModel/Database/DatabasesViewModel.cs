using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing;

using AllMyMusic.DataService;
using AllMyMusic.Settings;

namespace AllMyMusic.ViewModel
{
    public class DatabasesViewModel : ViewModelBase, IDisposable
    {
        #region Constructor
        public DatabasesViewModel()
        {
            _databaseConnections = new ObservableCollection<DatabaseViewModel>();
            _collations = GetCollations();
            _serverName = GetServerName();
            _serverTypes = GetServerTypes();
            _databaseCollation = "latin";
            DatabaseName = "New-DB-Name";
            Localize();
        }
        public DatabasesViewModel(ObservableCollection<ConnectionInfo> conectionList)
        {
            if (AppSettings.DatabaseSettings.DefaultDatabase != null)
            {
                CreateConnectedDatabaseService(AppSettings.DatabaseSettings.DefaultDatabase.ServerType);
                _selectedServerType = AppSettings.DatabaseSettings.DefaultDatabase.ServerType;
            }

            BuildDatabaseViewModels(conectionList);
            OnDatabaseConnectionListChanged();

            if (_databasesServerManagement != null)
            {
                _databasesServerManagement.DatabaseConnectionInformation = AppSettings.DatabaseSettings.DefaultDatabase;
                _selectedDatabaseConnectionIndex = -1;  // no connection is selected
                OnUserDatabasesChanged();
            }

            _collations = GetCollations();
            _serverName = GetServerName();
            _serverTypes = GetServerTypes();
            _databaseCollation = "latin";
            DatabaseName = "New-DB-Name";
            Localize();
        }
        private void BuildDatabaseViewModels(ObservableCollection<ConnectionInfo> conectionList)
        {
            _databaseConnections = new ObservableCollection<DatabaseViewModel>();

            foreach (ConnectionInfo dbConInfo in conectionList)
            {

                DatabaseViewModel dbVM = new DatabaseViewModel(dbConInfo);
                if (dbConInfo.DatabaseName == AppSettings.DatabaseSettings.DefaultDatabase.DatabaseName)
                {
                    dbVM.IsSelected = true;

                    _serverName = dbConInfo.ServerName;
                    _userName = dbConInfo.User;
                    _password = dbConInfo.Password;
                    _databaseCollation = dbConInfo.Collation;

                    _activeDatabaseName = AppSettings.DatabaseSettings.DefaultDatabase.DatabaseName;
                }
                switch (dbConInfo.DatabaseName)
                {
                    case "master":
                        break;
                    case "model":
                        break;
                    case "msdb":
                        break;
                    case "tempdb":
                        break;
                    case "information_schema":
                        break;
                    case "mysql":
                        break;
                    default:
                        _databaseConnections.Add(dbVM);
                        dbVM.DatabaseSelected += new DatabaseViewModel.DatabaseSelectedEventHandler(dbVM_DatabaseSelected);
                        break;
                }
            }

            RaisePropertyChanged("DatabaseConnections");
        }
        private void dbVM_DatabaseSelected(object sender, ConnectionInfoEventArgs e)
        {
            SelectDatabase(e.DbConInfo);
        }
        #endregion

        #region Fields
        private IDatabasesServerManagement _databasesServerManagement;
        #endregion // Fields

        #region Commands
        private RelayCommand<object> _connectServerCommand;
        public ICommand ConnectServerCommand
        {
            get
            {
                if (null == _connectServerCommand)
                    _connectServerCommand = new RelayCommand<object>(ExecuteConnectServer);

                return _connectServerCommand;
            }
        }
        private void ExecuteConnectServer(object _notUsed)
        {
            ConnectServer();
        }
        private void ConnectServer()
        {
            frmConnectServer frmDbConnect = new frmConnectServer(this);
            frmDbConnect.ShowDialog();

            if ((this.DatabaseConnectionInformation != null) && (this.DatabaseConnectionInformation.ConnectionTested == true))
            {
                ConnectionInfoEventArgs args = new ConnectionInfoEventArgs(this.DatabaseConnectionInformation);
                DatabaseChanged(this, args);
            }
        }
        #endregion

        #region Private Helpers
        private Boolean CreateDatabaseService(ServerType serverType)
        {
            Boolean result = false;

            if (_databasesServerManagement != null)
            {
                _databasesServerManagement.Close();
            }

            if (serverType == ServerType.SqlServer)
            {
                _databasesServerManagement = new DatabasesServerManagement_SQL(
                AppSettings.DatabaseSettings.DbCurrentVersion,
                AppSettings.GeneralSettings.ApplicationDataPath);
                result = true;
            }

            if (serverType == ServerType.MySql)
            {
                _databasesServerManagement = new DatabasesServerManagement_MYSQL(
                AppSettings.DatabaseSettings.DbCurrentVersion,
                AppSettings.GeneralSettings.ApplicationDataPath);
                result = true;
            }

            return result;
        }
        private Boolean CreateConnectedDatabaseService(ServerType serverType)
        {
            Boolean result = false;

            if (serverType == ServerType.SqlServer)
            {
                _databasesServerManagement = new DatabasesServerManagement_SQL(
                AppSettings.DatabaseSettings.DefaultDatabase,
                AppSettings.DatabaseSettings.DbCurrentVersion,
                AppSettings.GeneralSettings.ApplicationDataPath);
                result = true;
            }

            if (serverType == ServerType.MySql)
            {
                _databasesServerManagement = new DatabasesServerManagement_MYSQL(
                AppSettings.DatabaseSettings.DefaultDatabase,
                AppSettings.DatabaseSettings.DbCurrentVersion,
                AppSettings.GeneralSettings.ApplicationDataPath);
                result = true;
            }

            return result;
        }

        private ConnectionInfo GetDatabaseConnectionInfo(String databaseName)
        {
            foreach (DatabaseViewModel dbVM in _databaseConnections)
            {
                if ((dbVM.DatabaseConnectionInformation.ServerType == _selectedServerType) && (dbVM.DatabaseName == databaseName))
                {
                    dbVM.DatabaseConnectionInformation.DatabaseName = databaseName;
                    return dbVM.DatabaseConnectionInformation;
                }
            }

            ConnectionInfo dbCI2 = new ConnectionInfo();
            dbCI2.ServerType = _selectedServerType;
            dbCI2.ServerName = _serverName;
            dbCI2.User = _userName;
            dbCI2.Password = _password;
            dbCI2.Characterset = GetCharacterSetByName();
            dbCI2.Collation = GetCollation();
            dbCI2.DatabaseName = databaseName;
            return dbCI2;
        }
        private String GetCollation()
        {
            if (_databasesServerManagement.SelectedServerType == ServerType.SqlServer)
            {
                if (_databaseCollation == "latin")
                {
                    return "Latin1_General_CI_AS";
                }

                if (_databaseCollation == "cyrillic")
                {
                    return "Cyrillic_General_CI_AS";
                }

            }

            if (_databasesServerManagement.SelectedServerType == ServerType.MySql)
            {
                if (_databaseCollation == "latin")
                {
                    return "latin1_general_ci";
                }

                if (_databaseCollation == "cyrillic")
                {
                    return "cp1251_general_ci";
                }
            }
            return "Latin1_General_CI_AS";
        }
        private String GetCharacterSetByName()
        {
            // mysql> SHOW CHARACTER SET;

            //| big5     | Big5 Traditional Chinese    | big5_chinese_ci     |      2 |
            //| dec8     | DEC West European           | dec8_swedish_ci     |      1 |
            //| cp850    | DOS West European           | cp850_general_ci    |      1 |
            //| hp8      | HP West European            | hp8_english_ci      |      1 |
            //| koi8r    | KOI8-R Relcom Russian       | koi8r_general_ci    |      1 |
            //| latin1   | cp1252 West European        | latin1_swedish_ci   |      1 |
            //| latin2   | ISO 8859-2 Central European | latin2_general_ci   |      1 |
            //| swe7     | 7bit Swedish                | swe7_swedish_ci     |      1 |
            //| ascii    | US ASCII                    | ascii_general_ci    |      1 |
            //| ujis     | EUC-JP Japanese             | ujis_japanese_ci    |      3 |
            //| sjis     | Shift-JIS Japanese          | sjis_japanese_ci    |      2 |
            //| hebrew   | ISO 8859-8 Hebrew           | hebrew_general_ci   |      1 |
            //| tis620   | TIS620 Thai                 | tis620_thai_ci      |      1 |
            //| euckr    | EUC-KR Korean               | euckr_korean_ci     |      2 |
            //| koi8u    | KOI8-U Ukrainian            | koi8u_general_ci    |      1 |
            //| gb2312   | GB2312 Simplified Chinese   | gb2312_chinese_ci   |      2 |
            //| greek    | ISO 8859-7 Greek            | greek_general_ci    |      1 |
            //| cp1250   | Windows Central European    | cp1250_general_ci   |      1 |
            //| gbk      | GBK Simplified Chinese      | gbk_chinese_ci      |      2 |
            //| latin5   | ISO 8859-9 Turkish          | latin5_turkish_ci   |      1 |



            if (_databaseCollation == "latin")
            {
                return "latin1";
            }

            if (_databaseCollation == "cyrillic")
            {
                return "cp1251";
            }

            // Default Charset
            return "latin1";
        }
        #endregion // Private Helpers

        // ************************************************************************************************************

        #region Server ViewModel Fields
        private String _serverName;
        private String _userName;
        private String _password;
        private Boolean _showUserName;
        private ServerType _selectedServerType;
        private ObservableCollection<String> _serverTypes;
        #endregion

        #region Server ViewModel Properties
        public String ServerName
        {
            get { return _serverName; }
            set
            {
                if (value == _serverName)
                    return;

                _serverName = value;

                RaisePropertyChanged("ServerName");
            }
        }    
        public ServerType SelectedServerType
        {
            get { return _selectedServerType; }
            set
            {
                if (value == _selectedServerType)
                    return;

                _selectedServerType = value;

                OnSelectedServerTypeChanged();

                RaisePropertyChanged("SelectedServerType");
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
        #endregion

        #region Server ViewModel Commands
        private RelayCommand<object> _testConnectionCommand;
        public ICommand TestConnectionCommand
        {
            get
            {
                if (null == _testConnectionCommand)
                    _testConnectionCommand = new RelayCommand<object>(ExecuteTestConnection, CanTestConnection);

                return _testConnectionCommand;
            }
        }
        private void ExecuteTestConnection(object _notUsed)
        {
            TestConnection();
        }
        private bool CanTestConnection(object _notUsed)
        {
            return (_selectedServerType != ServerType.Unknown) && (String.IsNullOrEmpty(_serverName) != true);
        }
        private void OnSelectedServerTypeChanged()
        {
            if (_selectedServerType == ServerType.MySql)
            {
                _showUserName = true;
            }
            else
            {
                _showUserName = false;
            }

            ServerName = GetServerName();
            UserDatabaseNames = new ObservableCollection<string>();

            RaisePropertyChanged("ShowUserName");
        }
        #endregion

        #region Server ViewModel Private Helpers
        private ObservableCollection<String> GetServerTypes()
        {
            ObservableCollection<String> serverTypes = new ObservableCollection<String>();
            serverTypes.Add("Sql-Server (Microsoft)");
            serverTypes.Add("MySql (Oracle)");
            return serverTypes;
        }
        private void TestConnection()
        {
            Boolean result = CreateDatabaseService(_selectedServerType);

            if (result == true)
            {
                ConnectionInfo dbConInfo = new ConnectionInfo();
                dbConInfo.ServerType = _selectedServerType;
                dbConInfo.ServerName = _serverName;
                dbConInfo.User = _userName;
                dbConInfo.Password = _password;
                dbConInfo.Collation = GetCollation();

                Boolean res = _databasesServerManagement.TestConnection(dbConInfo);
                if (res == true)
                {
                    _databasesServerManagement.DatabaseConnectionInformation = dbConInfo;
                    UserDatabaseNames = _databasesServerManagement.GetUserDatabases();

                    if (UserDatabaseNames.Count == 0)
                    {
                        MessageBox.Show("Successfully connected to your SQL-Server", "Information", MessageBoxButton.OK);
                    }
                }
                else
                {
                    UserDatabaseNames = new ObservableCollection<string>();
                }
            }
        }
        #endregion

        // ************************************************************************************************************

        #region UserDB ViewModel Fields
        private ObservableCollection<String> _userDatabaseNames;
        private String _selectedDatabase;
        private Int32 _countSongs = 0;
        private Int32 _countAlbums = 0;
        private Int32 _countBands = 0;
        #endregion

        public void GetStatistics()
        {
            if (String.IsNullOrEmpty(_activeDatabaseName) == false)
            {
                ConnectionInfo dbCI = GetDatabaseConnectionInfo(_activeDatabaseName);
                StatisticsItem statistics = _databasesServerManagement.GetStatistics(dbCI);
                CountSongs = statistics.CountSong;
                CountAlbums = statistics.CountAlbums;
                CountBands = statistics.CountBands;
            }
        }

        #region UserDB ViewModel Properties
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
        public ObservableCollection<String> UserDatabaseNames
        {
            get { return _userDatabaseNames; }
            set
            {
                if (value == _userDatabaseNames)
                    return;

                _userDatabaseNames = value;

                RaisePropertyChanged("UserDatabaseNames");
            }
        }
        public String SelectedDatabase
        {
            get { return _selectedDatabase; }
            set
            {
                if (value == _selectedDatabase)
                    return;

                _selectedDatabase = value;

                RaisePropertyChanged("SelectedDatabase");
            }
        }

        public Int32 CountSongs
        {
            get { return _countSongs; }
            set
            {
                _countSongs = value;
                RaisePropertyChanged("CountSongs");
            }
        }
        public Int32 CountAlbums
        {
            get { return _countAlbums; }
            set
            {
                _countAlbums = value;
                RaisePropertyChanged("CountAlbums");
            }
        }
        public Int32 CountBands
        {
            get { return _countBands; }
            set
            {
                _countBands = value;
                RaisePropertyChanged("CountBands");
            }
        }
        #endregion

        #region UserDB ViewModel Commands
        private RelayCommand<String> _connectDatabaseCommand;
        public ICommand ConnectDatabaseCommand
        {
            get
            {
                if (null == _connectDatabaseCommand)
                    _connectDatabaseCommand = new RelayCommand<String>(ExecuteConnectDatabase, CanConnectDatabase);

                return _connectDatabaseCommand;
            }
        }
        private void ExecuteConnectDatabase(String _databaseName)
        {
            if (String.IsNullOrEmpty(_databaseName) == false)
            {
                ConnectDatabase(_databaseName);
            }
        }
        private bool CanConnectDatabase(String _databaseName)
        {
            return (String.IsNullOrEmpty(_selectedDatabase) != true);
        }

        private RelayCommand<String> _purgeDatabaseCommand;
        public ICommand PurgeDatabaseCommand
        {
            get
            {
                if (null == _purgeDatabaseCommand)
                    _purgeDatabaseCommand = new RelayCommand<String>(ExecutePurgeDatabase, CanPurgeDatabase);

                return _purgeDatabaseCommand;
            }
        }
        private void ExecutePurgeDatabase(String _databaseName)
        {
            if (String.IsNullOrEmpty(_databaseName) == false)
            {
                ConnectionInfo dbCI = GetDatabaseConnectionInfo(_databaseName);
                _databasesServerManagement.PurgeDatabase(dbCI);

                ConnectionInfoEventArgs args = new ConnectionInfoEventArgs(dbCI);
                OnDatabasePurged(this, args);
            }
        }
        private bool CanPurgeDatabase(String _databaseName)
        {
            return (String.IsNullOrEmpty(_selectedDatabase) != true);
        }

        private RelayCommand<String> _deleteDatabaseCommand;
        public ICommand DeleteDatabaseCommand
        {
            get
            {
                if (null == _deleteDatabaseCommand)
                    _deleteDatabaseCommand = new RelayCommand<String>(ExecuteDeleteDatabase, CanDeleteDatabase);

                return _deleteDatabaseCommand;
            }
        }
        private void ExecuteDeleteDatabase(String _databaseName)
        {
            if (String.IsNullOrEmpty(_databaseName) == false)
            {
                ConnectionInfo dbCI = GetDatabaseConnectionInfo(_databaseName);
                _databasesServerManagement.DeleteDatabase(dbCI);

                OnUserDatabasesChanged();
                DeleteConnection(_databaseName);
                ConnectionInfoEventArgs args = new ConnectionInfoEventArgs(dbCI);
                OnDatabaseDeleted(this, args);
            }

        }
        private bool CanDeleteDatabase(String _databaseName)
        {
            return (String.IsNullOrEmpty(_selectedDatabase) != true)
                && (String.IsNullOrEmpty(_activeDatabaseName) != true)
                && (_selectedDatabase.ToUpper() != _activeDatabaseName.ToUpper());
        }
        #endregion

        #region UserDB ViewModel Private Helpers
        private String GetServerName()
        {
            if (_databaseConnections != null)
            {
                foreach (DatabaseViewModel dbVM in _databaseConnections)
                {
                    ConnectionInfo dbConInfo = dbVM.DatabaseConnectionInformation;

                    if ((dbConInfo.ServerType == ServerType.MySql) && (_selectedServerType == ServerType.MySql))
                    {
                        return dbConInfo.ServerName;
                    }

                    if ((dbConInfo.ServerType == ServerType.SqlServer) && (_selectedServerType == ServerType.SqlServer))
                    {
                        return dbConInfo.ServerName;
                    }
                }
            }

            if (_selectedServerType == ServerType.MySql)
            {
                return @"localhost";
            }
            else
            {
                return @".\SQLEXPRESS";
            }
        }
        private void ConnectDatabase(String _databaseName)
        {
            ConnectionInfo newConnection = new ConnectionInfo(_databasesServerManagement.DatabaseConnectionInformation, _databaseName);
            _databasesServerManagement.ConnectDatabase(newConnection, true);


            if ((AppSettings.DatabaseSettings.DefaultDatabase == null) || (AppSettings.DatabaseSettings.DefaultDatabase.ServerType != newConnection.ServerType))
            {
                AppSettings.DatabaseSettings.DefaultDatabase = newConnection;
                ConnectionInfoEventArgs args = new ConnectionInfoEventArgs(newConnection);
                OnServerTypeChanged(this, args);
            }
            else
            {
                AppSettings.DatabaseSettings.DefaultDatabase = newConnection;
                ConnectionInfoEventArgs args = new ConnectionInfoEventArgs(newConnection);
                OnDatabaseChanged(this, args);
            }

            UpdateOrAddConnection(newConnection);

            RaisePropertyChanged("DatabaseConnections");           
        }
        private void OnUserDatabasesChanged()
        {
            UserDatabaseNames = _databasesServerManagement.GetUserDatabases();
        }
        #endregion

        #region UserDB ViewModel Events
        public delegate void DatabaseChangedEventHandler(object sender, ConnectionInfoEventArgs e);
        public delegate void ServerTypeChangedEventHandler(object sender, ConnectionInfoEventArgs e);

        public event DatabaseChangedEventHandler DatabasePurged;
        protected virtual void OnDatabasePurged(object sender, ConnectionInfoEventArgs e)
        {
            if (this.DatabasePurged != null)
            {
                this.DatabasePurged(this, e);
            }
        }

        public event DatabaseChangedEventHandler DatabaseDeleted;
        protected virtual void OnDatabaseDeleted(object sender, ConnectionInfoEventArgs e)
        {
            if (this.DatabaseDeleted != null)
            {
                this.DatabaseDeleted(this, e);
            }
        }
       

        public event DatabaseChangedEventHandler DatabaseChanged;
        protected virtual void OnDatabaseChanged(object sender, ConnectionInfoEventArgs e)
        {
            if (this.DatabaseChanged != null)
            {
                this.DatabaseChanged(this, e);
            }
        }

        public event ServerTypeChangedEventHandler SelectedServerTypeChanged;
        protected virtual void OnServerTypeChanged(object sender, ConnectionInfoEventArgs e)
        {
            if (this.SelectedServerTypeChanged != null)
            {
                this.SelectedServerTypeChanged(this, e);
            }
        }    
        #endregion

        // ************************************************************************************************************

        #region Connections ViewModel Fields
        private ObservableCollection<DatabaseViewModel> _databaseConnections;
        private Int32 _selectedDatabaseConnectionIndex;
        private DatabaseViewModel _selectedDatabaseConnection;
        private String _activeDatabaseName;
        #endregion

        #region Connections ViewModel Properties
        public ConnectionInfo DatabaseConnectionInformation
        {
            get { return _databasesServerManagement.DatabaseConnectionInformation; }
            set
            {
                if (value == _databasesServerManagement.DatabaseConnectionInformation)
                    return;

                _databasesServerManagement.DatabaseConnectionInformation = value;
                RaisePropertyChanged("DatabaseConnectionInformation");
            }
        }
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
        #endregion

        #region Connections ViewModel Commands
        private RelayCommand<DatabaseViewModel> _deleteConnection;
        public ICommand DeleteConnectionCommand
        {
            get
            {
                if (null == _deleteConnection)
                    _deleteConnection = new RelayCommand<DatabaseViewModel>(ExecuteDeleteConnection, CanDeleteConnection);

                return _deleteConnection;
            }
        }
        private void ExecuteDeleteConnection(DatabaseViewModel dbVM)
        {
            DeleteConnection(dbVM.DatabaseName);
        }
        private bool CanDeleteConnection(DatabaseViewModel dbVM)
        {
            return (_selectedDatabaseConnection != null);
        }

        private RelayCommand<object> _moveConnectionUp;
        public ICommand MoveConnectionUpCommand
        {
            get
            {
                if (null == _moveConnectionUp)
                    _moveConnectionUp = new RelayCommand<object>(ExecuteMoveConnectionUp, CanMoveConnectionUp);

                return _moveConnectionUp;
            }
        }
        private void ExecuteMoveConnectionUp(object _notUsed)
        {
            MoveConnectionUp();
        }
        private bool CanMoveConnectionUp(object _notUsed)
        {
            return ((_selectedDatabaseConnection != null) && (_databaseConnections.Count > 1) && (_selectedDatabaseConnectionIndex > 0));
        }

        private RelayCommand<object> _moveConnectionDown;
        public ICommand MoveConnectionDownCommand
        {
            get
            {
                if (null == _moveConnectionDown)
                    _moveConnectionDown = new RelayCommand<object>(ExecuteMoveConnectionDown, CanMoveConnectionDown);

                return _moveConnectionDown;
            }
        }
        private void ExecuteMoveConnectionDown(object _notUsed)
        {
            MoveConnectionDown();
        }
        private bool CanMoveConnectionDown(object _notUsed)
        {
            return ((_selectedDatabaseConnection != null) && (_databaseConnections.Count > 1) && (_selectedDatabaseConnectionIndex < _databaseConnections.Count - 1));
        }
        #endregion

        #region Connections ViewModel Private Helpers
        private void MoveConnectionUp()
        {
            if (_selectedDatabaseConnectionIndex > 0)
            {
                _databaseConnections.Move(_selectedDatabaseConnectionIndex, _selectedDatabaseConnectionIndex - 1);
            }
            OnDatabaseConnectionListChanged();
        }
        private void MoveConnectionDown()
        {
            if ((_selectedDatabaseConnectionIndex >= 0) && (_selectedDatabaseConnectionIndex < _databaseConnections.Count - 1))
            {
                _databaseConnections.Move(_selectedDatabaseConnectionIndex, _selectedDatabaseConnectionIndex + 1);
            }
            OnDatabaseConnectionListChanged();
        }
        private void UpdateOrAddConnection(ConnectionInfo dbConInfo)
        {
            DatabaseViewModel dbVM = new DatabaseViewModel(dbConInfo);
            dbVM.DatabaseSelected += new DatabaseViewModel.DatabaseSelectedEventHandler(dbVM_DatabaseSelected);
            dbVM.IsSelected = true;
            
            Boolean vmFound = false;
            
            for (int i = 0; i < _databaseConnections.Count; i++)
            {
                if ((_databaseConnections[i].DatabaseName == dbVM.DatabaseConnectionInformation.DatabaseName) && (_databaseConnections[i].ProductName == dbVM.DatabaseConnectionInformation.ProductName))
                {
                    _databaseConnections[i] = dbVM;
                    _databaseConnections[i].IsSelected = true;
                    vmFound = true;
                }
                else
                {
                    _databaseConnections[i].IsSelected = false;
                }
                AppSettings.DatabaseSettings.DatabaseConnections.Add(dbVM.DatabaseConnectionInformation);
            }

            if (vmFound == false)
            {
                _databaseConnections.Add(dbVM);
                OnUserDatabasesChanged();
                OnDatabaseConnectionListChanged();
            }
        }
        private void DeleteConnection(String _databaseName)
        {
            Int32 removeIndex = -1;
            for (int i = 0; i < _databaseConnections.Count; i++)
            {
                if (_databaseConnections[i].DatabaseName == _databaseName)
                {
                    removeIndex = i;
                    break;
                }
            }


            if (removeIndex >= 0)
            {
                _databaseConnections.RemoveAt(removeIndex);
            }

            OnDatabaseConnectionListChanged();
            RaisePropertyChanged("DatabaseConnections");
        }
        private void OnDatabaseConnectionListChanged()
        {
            AppSettings.DatabaseSettings.DatabaseConnections = new ObservableCollection<ConnectionInfo>();
            foreach (DatabaseViewModel dbVM in _databaseConnections)
            {
                AppSettings.DatabaseSettings.DatabaseConnections.Add(dbVM.DatabaseConnectionInformation);
            }
        }
        #endregion

        // ************************************************************************************************************

        #region NewDB ViewModel Fields
        private String _databaseName;
        private String _databaseCollation;
        private ObservableCollection<String> _collations;
        #endregion

        #region NewDB ViewModel Properties
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
        public String DatabaseName
        {
            get { return _databaseName; }
            set
            {
                if (value == _databaseName)
                    return;

                _databaseName = value;

                RaisePropertyChanged("DatabaseName");
            }
        }
        public String DatabaseCollation
        {
            get { return _databaseCollation; }
            set
            {
                if (value == _databaseCollation)
                    return;

                _databaseCollation = value;

                RaisePropertyChanged("DatabaseCollation");
            }
        }
        #endregion

        #region NewDB ViewModel Commands
        private RelayCommand<String> _createDatabaseCommand;
        public ICommand CreateDatabaseCommand
        {
            get
            {
                if (null == _createDatabaseCommand)
                    _createDatabaseCommand = new RelayCommand<String>(ExecuteCreateDatabase, CanCreateDatabase);

                return _createDatabaseCommand;
            }
        }
        private void ExecuteCreateDatabase(String _databaseName)
        {
            if ((_databaseName.IndexOf(" ") >= 0) == true)
            {
                ShowError.Show("Database name must not contain spaces");
            }
            else
            {
                ConnectionInfo dbCI = GetDatabaseConnectionInfo(_databaseName);
                Boolean _res = _databasesServerManagement.CreateDatabase(dbCI);
                if (_res == true)
                {
                    UpdateOrAddConnection(dbCI);
                    OnUserDatabasesChanged();
                    SelectDatabase(dbCI);
                }
            }
        }
        private bool CanCreateDatabase(String _databaseName)
        {
            return (String.IsNullOrEmpty(_databaseName) != true) &&
                   (String.IsNullOrEmpty(_databaseCollation) != true) &&
                   DatabaseConnectionInformation != null;
        }
        #endregion

        #region NewDB ViewModel Private Helpers
        private ObservableCollection<String> GetCollations()
        {
            ObservableCollection<String> collations = new ObservableCollection<String>();
            collations.Add("latin");
            collations.Add("cyrillic");
            return collations;
        }
        private void SelectDatabase(ConnectionInfo dbConInfo)
        {
            _activeDatabaseName = dbConInfo.DatabaseName;

            foreach (DatabaseViewModel dbVM in DatabaseConnections)
            {
                if (dbVM.DatabaseName != _activeDatabaseName)
                {
                    dbVM.IsSelected = false;
                }
                else
                {
                    dbVM.IsSelected = true;
                    if (_databasesServerManagement.DatabaseConnectionInformation.ServerType == dbVM.DatabaseConnectionInformation.ServerType)
                    {
                        _databasesServerManagement.DatabaseConnectionInformation = dbVM.DatabaseConnectionInformation;
                    }
                    else
                    {
                        CreateDatabaseService(dbVM.DatabaseConnectionInformation.ServerType);

                        _databasesServerManagement.ConnectDatabase(dbVM.DatabaseConnectionInformation, true);
                        //_databasesServerManagement.DatabaseConnectionInformation = dbVM.DatabaseConnectionInformation;
                    }
                }
            }

            if ((AppSettings.DatabaseSettings.DefaultDatabase == null) || (AppSettings.DatabaseSettings.DefaultDatabase.ServerType != dbConInfo.ServerType))
            {
                AppSettings.DatabaseSettings.DefaultDatabase = dbConInfo;
                ConnectionInfoEventArgs args = new ConnectionInfoEventArgs(dbConInfo);
                OnServerTypeChanged(this, args);
            }
            else
            {
                AppSettings.DatabaseSettings.DefaultDatabase = dbConInfo;
                ConnectionInfoEventArgs args = new ConnectionInfoEventArgs(dbConInfo);
                OnDatabaseChanged(this, args);
            }

        }
        #endregion

        // ************************************************************************************************************
        

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

        ~DatabasesViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_databasesServerManagement != null)
            {
                _databasesServerManagement.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
