using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AllMyMusic_v3.QueryBuilder;


namespace AllMyMusic_v3.DataService
{
    public class DataServiceBands_MYSQL : IDataServiceBands, IDisposable
    {
        #region Properties
        private MySqlConnection _connection;
        public MySqlConnection Connection
        {
            get { return _connection; }
        }

        private Boolean showVariousArtists = true;
        public Boolean ShowVariousArtists
        {
            get { return showVariousArtists; }
            set
            {
                showVariousArtists = value;
                QueryBuilderBands.ShowVariousArtists = value;
            }
        }

        public ServerType SelectedServerType
        {
            get { return ServerType.MySql; }
        }
        #endregion

        #region Constructor
        public DataServiceBands_MYSQL(ConnectionInfo conInfo)
        {
            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderBands.ServerType = conInfo.ServerType;
        }

        #endregion

        #region Public
        public async Task<Int32> AddBand(BandItem band)
        {
            MySqlParameter param = null;

            MySqlCommand cmd = new MySqlCommand("AddBand", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("var_Name", MySqlDbType.VarString, 100);
            param.Value = band.BandName.Substring(0, Math.Min(band.BandName.Length, 100));

            param = cmd.Parameters.Add("var_SortName", MySqlDbType.VarString, 100);
            param.Value = band.SortName.Substring(0, Math.Min(band.SortName.Length, 100));

            param = cmd.Parameters.Add("var_ID", MySqlDbType.Int32);
            param.Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            band.BandId = (int)param.Value;

            return band.BandId;
        }
        public async Task<ObservableCollection<BandItem>> GetBandsByAlphabet(String firstCharacter)
        {
            String strSQL = String.Empty;
            if (firstCharacter == "0_9")
            {
                strSQL = QueryBuilderBands.BandsByDigit();
            }
            else if (firstCharacter == "#")
            {
                strSQL = QueryBuilderBands.BandsBySpecialCharacter();
            }
            else
            {
                strSQL = QueryBuilderBands.BandsByAlphabet();
            }

            MySqlParameter sqlParam = new MySqlParameter("var_FirstChar", MySqlDbType.VarString);
            sqlParam.Value = firstCharacter;

            ObservableCollection<BandItem> bands = await GetBandsDB(strSQL, sqlParam);
            return bands;
        }
        public async Task<ObservableCollection<BandItem>> SearchBands(String searchText)
        {
            String strSQL = QueryBuilderBands.SearchBands();
            MySqlParameter searchParam = new MySqlParameter("var_Name", MySqlDbType.VarString);
            searchParam.Value = searchText;

            ObservableCollection<BandItem> bands = await GetBandsDB(strSQL, searchParam);
            return bands;
        }
        public async Task<BandItem> GetBand(Int32 bandID)
        {
            BandItem band = new BandItem();

            String strSQL = QueryBuilderBands.BandById(bandID);

            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        band.BandName = reader.GetString(0).TrimEnd();
                    }
                    else { band.BandName = String.Empty; }

                    // bandID (bandID, LeadPerformerID, ComposerID, ConductorID)
                    if (!reader.IsDBNull(1)) { band.BandId = (Int32)reader.GetInt32(1); }
                    else { band.BandId = 0; }

                    if (!reader.IsDBNull(2))
                    {
                        band.SortName = reader.GetString(2).TrimEnd();
                    }
                    else { band.SortName = String.Empty; }

                    // album Count
                    if (!reader.IsDBNull(3)) { band.AlbumCount = (Int32)reader.GetInt32(3); }
                    else { band.AlbumCount = 0; }

                    // Bookmarked Flag
                    if (!reader.IsDBNull(4)) { band.BookmarkedBand = (Int32)reader.GetInt32(4); }
                    else { band.BookmarkedBand = 0; }

                }
            }
            reader.Close();

            return band;
        }

        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderBands.ServerType = conInfo.ServerType;
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

        private async Task<ObservableCollection<BandItem>> GetBandsDB(String strSQL, MySqlParameter sqlParam)
        {
            ObservableCollection<BandItem> bandList = new ObservableCollection<BandItem>();
            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            if (sqlParam != null)
            {
                cmd.Parameters.Add(sqlParam);
            }

            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                // dbo.Bands.Name AS BandName, 
                // dbo.Bands.ID AS IDBand, 
                // dbo.Bands.SortName, 
                // COUNT(dbo.Bands.Name) AS AlbumCount, 
                // dbo.Bands.Bookmarked,     

                while (reader.Read())
                {
                    BandItem band = new BandItem();
                    if (!reader.IsDBNull(0))
                    {
                        band.BandName = reader.GetString(0).TrimEnd();
                    }
                    else { band.BandName = String.Empty; }

                    if (!reader.IsDBNull(1)) { band.BandId = (Int32)reader.GetInt32(1); }
                    else { band.BandId = 0; }

                    if (!reader.IsDBNull(2))
                    {
                        band.SortName = reader.GetString(2).TrimEnd();
                    }
                    else { band.SortName = String.Empty; }

                    // album Count
                    if (!reader.IsDBNull(3)) { band.AlbumCount = (Int32)reader.GetInt32(3); }
                    else { band.AlbumCount = 0; }

                    // Bookmarked Flag
                    if (!reader.IsDBNull(4)) { band.BookmarkedBand = (Int32)reader.GetInt32(4); }
                    else { band.BookmarkedBand = 0; }

                    // VA Flag
                    if (!reader.IsDBNull(5)) { band.ArtistType = (ArtistType)reader.GetInt32(5); }
                    else { band.ArtistType = ArtistType.SingleArtist; }

                    bandList.Add(band);
                }
            }
            reader.Close();
            return bandList;
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

        ~DataServiceBands_MYSQL()
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