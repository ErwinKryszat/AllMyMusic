using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

using System.Threading;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using AllMyMusic.Database;
using AllMyMusic.ViewModel;
using AllMyMusic.QueryBuilder;


namespace AllMyMusic.DataService
{
    public class DataServiceToolbar_SQL : IDataServiceToolbar, IDisposable
    {
        #region Fields
        private SqlConnection _connection;
        #endregion // Fields

        #region Presentation Properties
        public ServerType SelectedServerType
        {
            get { return ServerType.SqlServer; }
        }
        #endregion

        #region Constructor
        public DataServiceToolbar_SQL(ConnectionInfo conInfo)
        {
            _connection = new SqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderItems.ServerType = conInfo.ServerType;
        }
        #endregion

        #region Public 
        public async Task<ObservableCollection<AlphabetItem>> GetAlphabetButtons()
        {
            ObservableCollection<AlphabetItem> alphabetItems = new ObservableCollection<AlphabetItem>();
            Boolean digitButtonDone = false;
            Boolean specialCharacterButtonDone = false;

            String strSQL = QueryBuilderItems.GetAlphabet(TreeviewCategory.Band);
            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    AlphabetItem item = new AlphabetItem(reader.GetString(0));
                    alphabetItems.Add(item);
                }
            }
            reader.Close();

            ObservableCollection<AlphabetItem> buttonLabels = new ObservableCollection<AlphabetItem>();

            foreach (AlphabetItem item in alphabetItems)
            {
                if ((item.Character.CompareTo("0") >= 0) && (item.Character.CompareTo("9") <= 0) && (digitButtonDone == false))
                {
                    if (digitButtonDone == false)
                    {
                        buttonLabels.Add(new AlphabetItem("0_9"));
                        digitButtonDone = true;
                    }
                }
                else if ((item.Character.CompareTo("A") >= 0) && (item.Character.CompareTo("Z") <= 0))
                {
                    buttonLabels.Add(item);
                }
                else if ((item.Character.CompareTo("a") >= 0) && (item.Character.CompareTo("z") <= 0))
                {
                    buttonLabels.Add(item);
                }
                else if (specialCharacterButtonDone == false)                   
                {
                    buttonLabels.Add(new AlphabetItem("#"));
                    specialCharacterButtonDone = true;
                }
            }
            return buttonLabels;
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new SqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderItems.ServerType = conInfo.ServerType;
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

       
        #endregion // Events

        #region Private Helpers

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

        ~DataServiceToolbar_SQL()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_connection != null) 
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
                
                _connection.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }


     
}
