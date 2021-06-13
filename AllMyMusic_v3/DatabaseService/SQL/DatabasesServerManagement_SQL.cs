using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using AllMyMusic.Database;
using AllMyMusic.QueryBuilder;
using AllMyMusic.Settings;


namespace AllMyMusic.DataService
{
    public class DatabasesServerManagement_SQL : IDatabasesServerManagement, IDisposable
    {
        #region Fields
        private SqlConnection _connection;
        private ConnectionInfo _connectionInfo;
        private ServerType _selectedServerType;
        private String _dbCurrentVersion;
        private String _applicationDataPath;
        #endregion // Fields

        #region Presentation Properties
        public ConnectionInfo DatabaseConnectionInformation
        {
            get { return _connectionInfo; }
            set
            {
                if (value == _connectionInfo)
                    return;

                _connectionInfo = value;
                OnSqlConnectionInfoChanged();
            }
        }
        public SqlConnection Connection
        {
            get { return _connection; }
        }
        public ServerType SelectedServerType
        {
            get { return _selectedServerType; }
            set
            {
                if (value == _selectedServerType)
                    return;

                _selectedServerType = value;
            }
        }
        #endregion

        #region Constructor
        public DatabasesServerManagement_SQL(String dbCurrentVersion, String applicationDataPath)
        {
            _dbCurrentVersion = dbCurrentVersion;
            _applicationDataPath = applicationDataPath;
            _selectedServerType = ServerType.SqlServer;
        }
        public DatabasesServerManagement_SQL(ConnectionInfo dbCI, String dbCurrentVersion, String applicationDataPath)
        {
             _connection = new SqlConnection(AppSettings.DatabaseSettings.GetConnectionString());
            _dbCurrentVersion = dbCurrentVersion;
            _applicationDataPath = applicationDataPath;
            _selectedServerType = ServerType.SqlServer;

            ConnectDefaultDatabase();
        }
        #endregion

        #region Public 
        public void ConnectServer(ConnectionInfo dbCI)
        {
            if (dbCI.IsValid == true)
            {
                try
                {
                    if (_connection != null)
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                            SqlConnection.ClearPool(_connection);
                        }
                    }
                    _connection = new SqlConnection(dbCI.GetConnectionString());
                    _connection.Open();
                    _selectedServerType = dbCI.ServerType;

                    _connectionInfo = dbCI;

                    MessageBox.Show("Successfully connected to server: " + dbCI.ServerName,
                         "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            dbCI.ConnectionTested = true;

                }
                catch (Exception Err)
                {
                    String errorMessage = "Unable to connect server: " + dbCI.ServerName;
                    MessageBox.Show(errorMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    ShowError.ShowAndLog(Err, errorMessage, 1000);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        public Boolean ConnectDatabase(ConnectionInfo dbCI, Boolean _silent)
        {
            if (dbCI.IsValid == false)
                return false;

            Boolean _success = false;

            if ((_connection != null) && (_connection.State == System.Data.ConnectionState.Open))
            {
                _connection.Close();
                SqlConnection.ClearPool(_connection);
            }

            try
            {
                _connection = new SqlConnection(dbCI.GetConnectionString());
                _connection.Open();
                _success = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Could not connect database: " + dbCI.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 1001);
            }

            dbSchemaValidation schemaValidation = new dbSchemaValidation(dbCI);
            _success = schemaValidation.CheckDatabaseSchema();

            if (_success == true)
            {
                if (_silent == false)
                {
                    MessageBox.Show("Successfully connected to database: " + dbCI.DatabaseName,
                        "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (dbCI.DatabaseName != "master")
                {
                    dbCI.ConnectionTested = true;
                }

                _connectionInfo = dbCI;
            }
            else
            {
                if (_silent == false)
                {
                    MessageBox.Show("This is not an AllMyMusic database: " + dbCI.DatabaseName + " Connection is not possible.",
                            "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            return _success;
        }
        public Boolean TestConnection(ConnectionInfo dbCI)
        {
            Boolean _success = false;
            try
            {
                _connection = new SqlConnection(dbCI.GetConnectionString());
                _connection.Open();
                _success = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Could not connect database: " + dbCI.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 1002);
            }

            _connectionInfo = dbCI;
            return _success;

        }
        public ObservableCollection<String> GetUserDatabases()
        {
            SqlConnection tempConnection = null;
            String databaseName = _connection.Database;
            ObservableCollection<String> listDatabases = new ObservableCollection<String>();
            ConnectionInfo tempDbCI = new ConnectionInfo(_connectionInfo, "master");

            try
            {
                tempConnection = new SqlConnection(tempDbCI.GetConnectionString());
                tempConnection.Open();
                DataTable database = tempConnection.GetSchema("Databases");
                foreach (DataRow item in database.Rows)
                {
                    String dbName = item.ItemArray[0].ToString();
                    switch (dbName)
                    {
                        case "master":
                            break;
                        case "model":
                            break;
                        case "msdb":
                            break;
                        case "tempdb":
                            break;
                        default:
                            listDatabases.Add(dbName);
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                listDatabases = null;
                String errorMessage = "Error getting list of databases in SQL-Server: " + tempDbCI.ServerName;
                ShowError.ShowAndLog(Err, errorMessage, 1003);
                throw;
            }
            finally
            {
                tempConnection.Close();
                SqlConnection.ClearPool(tempConnection);
            }
            return listDatabases;
        }
        public Boolean CreateDatabase(ConnectionInfo dbCI)
        {
            Boolean result = false;
            String databaseName = dbCI.DatabaseName;

           
            // Verify if the databaseName already exists; Only create a new database if not yet exists
            ObservableCollection<String> databaseNames = GetUserDatabases();

            if (databaseNames.IndexOf(dbCI.DatabaseName) == -1)
            {
               result = CreateDatabaseName(dbCI);              
            }
         
            if (result == true)
	        {
		        result = CreateDatabaseSchema(dbCI);
	        }

            if (result == true)
            {
                result = InitializeDatabase(dbCI);
            }

            return result;
        }
        private Boolean CreateDatabaseName(ConnectionInfo dbCI)
        {
            Boolean result = false;
            String strSQL = CreateDatabaseQuery(dbCI);
            
            String databaseName = dbCI.DatabaseName;
            dbCI.DatabaseName = "master";
            SqlConnection con = null;

            try
            {
                con = new SqlConnection(dbCI.GetConnectionString());
                con.Open();

                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Error creating database in SQL-Server: " + databaseName;
                ShowError.ShowAndLog(Err, errorMessage, 1004);
            }
            finally
            {
                con.Close();
            }

            dbCI.DatabaseName = databaseName;
            return result;
        }
        private Boolean CreateDatabaseSchema(ConnectionInfo dbCI)
        {
            Boolean result = false;
            try
            {
                dbSchema_SQL schema = new dbSchema_SQL(dbCI, _dbCurrentVersion);
                schema.CreateSchema();
                result = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Error creating database schema for database: " + dbCI.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 1005);
                throw;
            }

            return result;
        }

        private Boolean InitializeDatabase(ConnectionInfo dbCI)
        {
            Boolean result = false;
            SqlConnection con = null;

            try
            {
                con = new SqlConnection(dbCI.GetConnectionString());
                con.Open();
                SqlCommand cmd = new SqlCommand("InitializeCountries", con);
                cmd.CommandType = CommandType.StoredProcedure;
                Int32 resultInt = cmd.ExecuteNonQuery();
                if (resultInt > 0)
                {
                    result = true;
                }
                result = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Could not Initialize Countries" + dbCI.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 1006);
            }
            finally
            {
                con.Close();
            }

            try
            {
                con = new SqlConnection(dbCI.GetConnectionString());
                con.Open();
                SqlCommand cmd = new SqlCommand("InitializeLanguages", con);
                cmd.CommandType = CommandType.StoredProcedure;
                Int32 resultInt = cmd.ExecuteNonQuery();
                if (resultInt > 0)
                {
                    result = true;
                }
                result = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Could not Initialize Languages " + dbCI.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 1006);
            }
            finally
            {
                con.Close();
            }

            return result;
        }

        private String CreateDatabaseQuery(ConnectionInfo dbCI)
        {
            String strSQL;
            String databaseName = dbCI.DatabaseName;

            if (dbCI.ServerName.StartsWith(@".\") || (dbCI.ServerName.StartsWith(Environment.MachineName)))
            {
                strSQL = "DECLARE @data_path nvarchar(256);" +
                " SET @data_path = (SELECT SUBSTRING(physical_name, 1, CHARINDEX(N'master.mdf', LOWER(physical_name)) - 1)" +
                " FROM master.sys.master_files" +
                " WHERE database_id = 1 AND file_id = 1);" +


                "EXECUTE ('CREATE DATABASE " + databaseName +
                " ON PRIMARY (" +
                " NAME = " + databaseName + "_data " +
                " ,FILENAME = ''' + @data_path + '" + databaseName + "_data.mdf'' " +
                " ,SIZE = 50MB" +
                " ,MAXSIZE = UNLIMITED" +
                " ,FILEGROWTH = 10MB)" +

                "LOG ON (" +
                " NAME = " + databaseName + "_log " +
                " ,FILENAME = ''' + @data_path + '" + databaseName + "_log.ldf'' " +
                " ,SIZE = 10MB " +
                " ,FILEGROWTH = 10MB)" +
                " COLLATE " + dbCI.Collation + "');";

            }
            else
            {
                strSQL = "CREATE DATABASE " + databaseName
                + " COLLATE " + dbCI.Collation;
            }

            return strSQL;
        }
        public Boolean PurgeDatabase(ConnectionInfo dbCI)
        {
            Boolean _success = false;
            if (dbCI.IsValid == true)
            {
                try
                {
                    MessageBoxResult res = MessageBox.Show("Are You Sure?", "Delete all data from the Database", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
                    if (res == MessageBoxResult.OK)
                    {
                        SqlCommand cmd = new SqlCommand("PurgeDatabase", _connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    _success = true;

                    if (_success == true)
                    {
                        _success = InitializeDatabase(dbCI);
                    }
                }
                catch (Exception Err)
                {
                    String errorMessage = "Could not purge database: " + dbCI.DatabaseName;
                    ShowError.ShowAndLog(Err, errorMessage, 1006);
                }
            }

            return _success;
        }
        public Boolean DeleteDatabase(ConnectionInfo dbCI)
        {
            String databaseName = dbCI.DatabaseName;

            Boolean _success = false;
            MessageBoxResult res = MessageBox.Show("Are You Sure?", "Delete the Database", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
            if (res == MessageBoxResult.OK)
            {
                try
                {
                    if (_connection != null)
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                            SqlConnection.ClearPool(_connection);
                        }
                    }

                    ConnectionInfo tempConnectionInfo = null;

                    if (dbCI.ServerType == ServerType.SqlServer)
                    {
                        tempConnectionInfo = new ConnectionInfo(dbCI, "master");

                    }

                    if (dbCI.ServerType == ServerType.MySql)
                    {
                        tempConnectionInfo = new ConnectionInfo(dbCI, "mysql");
                    }

                    SqlConnection tempConnection = new SqlConnection(tempConnectionInfo.GetConnectionString());

                    tempConnection.Open();
                    String strSQL = "DROP DATABASE " + databaseName;
                    SqlCommand cmd = new SqlCommand(strSQL, tempConnection);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    tempConnection.Close();

                    MessageBox.Show("The database has been deleted successfully",
                                                "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    _success = true;
                }
                catch (Exception Err)
                {
                    String errorMessage = "Could not delete database: " + databaseName;
                    ShowError.ShowAndLog(Err, errorMessage, 1007);
                }
            }

            return _success;
        }

        public StatisticsItem GetStatistics(ConnectionInfo dbCI)
        {
            StatisticsItem statistics = new StatisticsItem();

            if (dbCI.IsValid == true)
            {
                try
                { 
                    SqlCommand cmd = new SqlCommand("GetStatistics", _connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) { statistics.CountSong = (Int32)reader.GetInt32(0); }
                            else { statistics.CountSong = 0; }

                            if (!reader.IsDBNull(1)) { statistics.CountAlbums = (Int32)reader.GetInt32(1); }
                            else { statistics.CountAlbums = 0; }

                            if (!reader.IsDBNull(2)) { statistics.CountBands = (Int32)reader.GetInt32(2); }
                            else { statistics.CountBands = 0; }
                        }
                    }
                    reader.Close();
                  
                }
                catch (Exception Err)
                {
                    String errorMessage = "Could not GetStatistics " + dbCI.DatabaseName;
                    ShowError.ShowAndLog(Err, errorMessage, 1006);
                }
            }

            return statistics;
        }
        public void Close()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    SqlConnection.ClearPool(_connection);
                }
                _connection.Dispose();
                _connection = null;
            }
        }
        #endregion
    
        #region Events
        public delegate void DatabaseChangedEventHandler(object sender, StringEventArgs e);

        public event DatabaseChangedEventHandler DatabaseChanged;
        protected virtual void OnDatabaseChanged(object sender, StringEventArgs e)
        {
            if (this.DatabaseChanged != null)
            {
                this.DatabaseChanged(this, e);
            }
        }

        public event DatabaseChangedEventHandler DatabasePurged;
        protected virtual void OnDatabasePurged(object sender, StringEventArgs e)
        {
            if (this.DatabasePurged != null)
            {
                this.DatabasePurged(this, e);
            }
        }

        public event DatabaseChangedEventHandler DatabaseDeleted;
        protected virtual void OnDatabaseDeleted(object sender, StringEventArgs e)
        {
            if (this.DatabaseDeleted != null)
            {
                this.DatabaseDeleted(this, e);
            }
        }
        #endregion // Events

        #region Private Helpers
        private void ConnectDefaultDatabase()
        {
            if ((_connection != null) && (AppSettings.DatabaseSettings.DefaultDatabase.IsValid))
            {
                Boolean _success = ConnectDatabase(AppSettings.DatabaseSettings.DefaultDatabase, true);
                if (_success == true)
                {
                    AppSettings.DatabaseSettings.DefaultDatabase.ConnectionTested = true;
                }
            }
        }    
        private void OnSqlConnectionInfoChanged()
        {
            if ((_connection != null) && (_connection.State == System.Data.ConnectionState.Open))
            {
                _connection.Close();
                SqlConnection.ClearPool(_connection);
                _connection.Dispose();
            }

            try
            {
                _connection = new SqlConnection(_connectionInfo.GetConnectionString());
                _connection.Open();
            }
            catch (Exception Err)
            {
                String errorMessage = "Could not connect database: " + _connectionInfo.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        
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

        ~DatabasesServerManagement_SQL()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            Close();
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    } 
}
