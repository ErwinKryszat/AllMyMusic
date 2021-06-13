using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AllMyMusic.QueryBuilder;

namespace AllMyMusic.DataService
{
    public class DataServiceAlbumGenre_MYSQL : IDataServiceAlbumGenre, IDisposable
    {

        #region Properties
        private MySqlConnection _connection;
        public MySqlConnection Connection
        {
            get { return _connection; }
        }

        public ServerType SelectedServerType
        {
            get { return ServerType.MySql; }
        }
        #endregion

        #region Constructor
        public DataServiceAlbumGenre_MYSQL(ConnectionInfo conInfo)
        {

            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderAlbumGenre.ServerType = conInfo.ServerType;
        }
        #endregion

        #region Public
        public async Task<ObservableCollection<AlbumGenreItem>> GetAlbumGenres()
        {
            try
            {
                String strSQL = QueryBuilderAlbumGenre.VariousArtistsGenres();
                ObservableCollection<AlbumGenreItem> albumGenres = await Task.Run(() => GetAlbumGenresDB(strSQL));
                return albumGenres;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbumGenre_MYSQL, Error in GetAlbumGenres";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<Int32> AddAlbumGenre(AlbumGenreItem AlbumGenre)
        {
            try
            {
                MySqlParameter param = null;

                MySqlCommand cmd = new MySqlCommand("AddAlbumGenre", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("var_Name", MySqlDbType.VarChar, 100);
                param.Value = AlbumGenre.Name.Substring(0, Math.Min(AlbumGenre.Name.Length, 100));

                param = cmd.Parameters.Add("var_ID", MySqlDbType.Int32);
                param.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                return (int)param.Value;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbumGenre_MYSQL, Error in AddAlbumGenre";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderAlbumGenre.ServerType = conInfo.ServerType;
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

        #region private
       

        private ObservableCollection<AlbumGenreItem> GetAlbumGenresDB(String strSQL)
        {
            ObservableCollection<AlbumGenreItem> albumGenreList = new ObservableCollection<AlbumGenreItem>();

            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    AlbumGenreItem albumGenre = new AlbumGenreItem();
                    if (!reader.IsDBNull(0))
                    {
                        albumGenre.Name = reader.GetString(0).TrimEnd();
                    }
                    else { albumGenre.Name = String.Empty; }

                    if (!reader.IsDBNull(1)) { albumGenre.AlbumGenreId = (Int32)reader.GetInt32(1); }
                    else { albumGenre.AlbumGenreId = 0; }

                    if (!reader.IsDBNull(2)) { albumGenre.AlbumCount = (Int32)reader.GetInt32(2); }
                    else { albumGenre.AlbumCount = 0; }

                    albumGenreList.Add(albumGenre);
                }
            }
            reader.Close();

            return albumGenreList;
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

        ~DataServiceAlbumGenre_MYSQL()
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