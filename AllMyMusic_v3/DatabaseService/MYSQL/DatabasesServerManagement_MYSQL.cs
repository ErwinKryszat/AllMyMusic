using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using AllMyMusic.Database;
using AllMyMusic.QueryBuilder;
using AllMyMusic.Settings;

namespace AllMyMusic.DataService
{
    public class DatabasesServerManagement_MYSQL : IDatabasesServerManagement, IDisposable
    {
        #region Fields
        private MySqlConnection _connection;
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
                OnMySqlConnectionInfoChanged();
            }
        }
        public MySqlConnection Connection
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
        public DatabasesServerManagement_MYSQL(String dbCurrentVersion, String applicationDataPath)
        {
            _dbCurrentVersion = dbCurrentVersion;
            _applicationDataPath = applicationDataPath;
            _selectedServerType = ServerType.MySql;
        }
        public DatabasesServerManagement_MYSQL(ConnectionInfo dbCI, String dbCurrentVersion, String applicationDataPath)
        {
             _connection = new MySqlConnection(AppSettings.DatabaseSettings.GetConnectionString());
            _dbCurrentVersion = dbCurrentVersion;
            _applicationDataPath = applicationDataPath;
            _selectedServerType = ServerType.MySql;

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
                            MySqlConnection.ClearPool(_connection);
                        }
                    }
                    _connection = new MySqlConnection(dbCI.GetConnectionString());
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
                    ShowError.ShowAndLog(Err, errorMessage, 2000);
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
                MySqlConnection.ClearPool(_connection);
            }

            try
            {
                _connection = new MySqlConnection(dbCI.GetConnectionString());
                _connection.Open();
                _success = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Could not connect database: " + dbCI.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }


            if (_success == true)
            {
                dbSchemaValidation schemaValidation = new dbSchemaValidation(dbCI);
                _success = schemaValidation.CheckDatabaseSchema();

                if (_silent == false)
                {
                    String errorMessage = "Successfully connected to database: " + dbCI.DatabaseName;
                    MessageBox.Show(errorMessage,"Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (dbCI.DatabaseName != "mysql")
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
                _connection = new MySqlConnection(dbCI.GetConnectionString());
                _connection.Open();
                _success = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Could not connect database: " + dbCI.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 2002);
            }

            _connectionInfo = dbCI;
            return _success;
        }
        public void GenerateHashString(string password, ref string hashValue)
        {
            byte[] encodedpassword;
            byte[] sha1hash;
           
            System.Security.Cryptography.SHA1Managed hash = new System.Security.Cryptography.SHA1Managed();

            // get the byte representation of the password
            encodedpassword = Encoding.ASCII.GetBytes(password);

            // Compute the SHA1 hash of the password.
            sha1hash = hash.ComputeHash(encodedpassword);

            // String builder to create the final Hex encoded hash string
            StringBuilder hashedkey = new StringBuilder(sha1hash.Length);

            // Convert to Hex encoded string
            foreach (byte b in sha1hash)
            {
                // This generates the hex encoded string in lower case. Use "X2" to get hash string in upper-case.
                hashedkey.Append(b.ToString("x2"));
            }
            hashValue = hashedkey.ToString();  // Final Hex encoded SHA1 hash string
            return;
        }

        public ObservableCollection<String> GetUserDatabases()
        {
            MySqlConnection tempConnection = null;
            String databaseName = _connection.Database;
            ObservableCollection<String> listDatabases = new ObservableCollection<String>();
            ConnectionInfo tempDbCI = new ConnectionInfo(_connectionInfo, "mysql");

            try
            {
                tempConnection = new MySqlConnection(tempDbCI.GetConnectionString());
                tempConnection.Open();
                DataTable database = tempConnection.GetSchema("Databases");
                foreach (DataRow item in database.Rows)
                {
                    String dbName = item.ItemArray[1].ToString();
                    switch (dbName)
                    {
                        case "information_schema":
                            break;
                        case "performance_schema":
                            break;
                        case "phpmyadmin":
                            break;
                        case "mysql":
                            break;
                        case "webauth":
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
                ShowError.ShowAndLog(Err, errorMessage, 2003);
                throw;
            }
            finally
            {
                tempConnection.Close();
                MySqlConnection.ClearPool(tempConnection);
            }
            return listDatabases;
        }
        public Boolean CreateDatabase(ConnectionInfo dbCI)
        {
            Boolean result = false;
            String databaseName = dbCI.DatabaseName;

           
            // Verify if the databaseName already exists; Only create a new database if not yet exists
            ObservableCollection<String> databaseNames = GetUserDatabases();

            if (databaseNames.IndexOf(dbCI.DatabaseName.ToLower()) == -1)
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
            dbCI.DatabaseName = "mysql";
            MySqlConnection con = null;

            try
            {
                con = new MySqlConnection(dbCI.GetConnectionString());
                con.Open();

                MySqlCommand cmd = new MySqlCommand(strSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Error creating database in SQL-Server: " + databaseName;
                ShowError.ShowAndLog(Err, errorMessage, 2004);
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
                dbSchema_MYSQL schema = new dbSchema_MYSQL(dbCI, _dbCurrentVersion);
                schema.CreateSchema();
                result = true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Error creating database schema for database: " + dbCI.DatabaseName;
                ShowError.ShowAndLog(Err, errorMessage, 2005);
                throw;
            }

            return result;
        }

        private Boolean InitializeDatabase(ConnectionInfo dbCI)
        {
            Boolean result = false;
            MySqlConnection con = null;

            try
            {
                con = new MySqlConnection(dbCI.GetConnectionString());
                con.Open();
                MySqlCommand cmd = new MySqlCommand("InitializeCountries", con);
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
                con = new MySqlConnection(dbCI.GetConnectionString());
                con.Open();
                MySqlCommand cmd = new MySqlCommand("InitializeLanguages", con);
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
            String databaseName = dbCI.DatabaseName;

            String strSQL = "CREATE DATABASE " + databaseName
                      + " CHARACTER SET " + dbCI.Characterset
                      + " COLLATE " + dbCI.Collation
                      + "; ";

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
                        MySqlCommand cmd = new MySqlCommand("PurgeDatabase", _connection);
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
                    ShowError.ShowAndLog(Err, errorMessage, 2006);
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
                            MySqlConnection.ClearPool(_connection);
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

                    MySqlConnection tempConnection = new MySqlConnection(tempConnectionInfo.GetConnectionString());

                    tempConnection.Open();
                    String strSQL = "DROP DATABASE " + databaseName;
                    MySqlCommand cmd = new MySqlCommand(strSQL, tempConnection);
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
                    ShowError.ShowAndLog(Err, errorMessage, 2007);
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
                    MySqlCommand cmd = new MySqlCommand("GetStatistics", _connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    MySqlDataReader reader = cmd.ExecuteReader();

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
                    MySqlConnection.ClearPool(_connection);
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
        private void OnMySqlConnectionInfoChanged()
        {
            if ((_connection != null) && (_connection.State == System.Data.ConnectionState.Open))
            {
                _connection.Close();
                MySqlConnection.ClearPool(_connection);
                _connection.Dispose();
            }

            try
            {
                _connection = new MySqlConnection(_connectionInfo.GetConnectionString());
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

        ~DatabasesServerManagement_MYSQL()
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
