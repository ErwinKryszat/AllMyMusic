using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using AllMyMusic.QueryBuilder;


namespace AllMyMusic.DataService
{
    public class DataServiceListItems_SQL : IDataServiceListItems, IDisposable
    {
        #region Properties
        private SqlConnection _connection;
        public SqlConnection Connection
        {
            get { return _connection; }
        }

        public ServerType SelectedServerType
        {
            get { return ServerType.SqlServer; }
        }
        #endregion

        #region Constructor
        public DataServiceListItems_SQL(ConnectionInfo conInfo)
        {
            _connection = new SqlConnection(conInfo.GetConnectionString());
            _connection.Open();
        }
        #endregion

        #region Public
        public async Task<ObservableCollection<String>> GetListItems(String strSQL)
        {
            ObservableCollection<String> listItems = new ObservableCollection<String>();

            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    String listItem;
                    if (!reader.IsDBNull(0))
                    {
                        listItem = reader.GetString(0).TrimEnd();
                    }
                    else { listItem = String.Empty; }

                    listItems.Add(listItem);
                }
            }
            reader.Close();

            return listItems;
        }
        public async Task<ObservableCollection<String>> GetListItemsIntByColumn(String columName)
        {
            ObservableCollection<String> listItems = new ObservableCollection<String>();
            String strSQL = QueryBuilderItems.GetIntItemsByColumn(columName);

            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    String listItem;
                    if (!reader.IsDBNull(0))
                    {
                        listItem = reader.GetInt32(0).ToString();
                    }
                    else { listItem = "0"; }

                    listItems.Add(listItem);
                }
            }
            reader.Close();

            return listItems;
        }
        public async Task<ObservableCollection<String>> GetListItemsByColumn(String columName)
        {
            ObservableCollection<String> listItems = new ObservableCollection<String>();
            String strSQL = QueryBuilderItems.GetStringItemsByColumn(columName);

            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    String listItem;
                    if (!reader.IsDBNull(0))
                    {
                        listItem = reader.GetString(0);
                    }
                    else { listItem = String.Empty; }

                    listItems.Add(listItem);
                }
            }
            reader.Close();

            return listItems;
        }
        public async Task<ObservableCollection<String>> GetStringItemsByAlphabet(String columName, String firstCharacter)
        {
            ObservableCollection<String> listItems = new ObservableCollection<String>();
            String strSQL = QueryBuilderItems.GetStringItemsByAlphabet(columName, firstCharacter);

            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    String listItem;
                    if (!reader.IsDBNull(0))
                    {
                        listItem = reader.GetString(0);
                    }
                    else { listItem = String.Empty; }

                    listItems.Add(listItem);
                }
            }
            reader.Close();

            return listItems;
        }
        public async Task<ObservableCollection<String>> GetCountries()
        {
            String strSQL = QueryBuilderItems.Counties();
            ObservableCollection<String> listItems = await GetListItems(strSQL);
            return listItems;
        }
        public async Task<ObservableCollection<String>> GetLanguages()
        {
            String strSQL = QueryBuilderItems.Languages();
            ObservableCollection<String> listItems = await GetListItems(strSQL);
            return listItems;
        }
        public async Task<ObservableCollection<String>> GetGenres()
        {
            String strSQL = QueryBuilderItems.Genres();
            ObservableCollection<String> listItems = await GetListItems(strSQL);
            return listItems;
        }
        public async Task<ObservableCollection<String>> GetAlphabet(TreeviewCategory tvCategory)
        {
            String strSQL = QueryBuilderItems.GetAlphabet(tvCategory); ;
            ObservableCollection<String>  alphabetItems = await GetListItems(strSQL);
            return alphabetItems;
        }

        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new SqlConnection(conInfo.GetConnectionString());
            _connection.Open();
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

        #region private
        

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

        ~DataServiceListItems_SQL()
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