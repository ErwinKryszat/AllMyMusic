using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AllMyMusic.QueryBuilder;


namespace AllMyMusic.DataService
{
    public class DataServiceBands_SQL : IDataServiceBands, IDisposable
    {
        #region Properties
        private SqlConnection _connection;
        public SqlConnection Connection
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
            get { return ServerType.SqlServer; }
        }

        #endregion

        #region Constructor
        public DataServiceBands_SQL(ConnectionInfo conInfo)
        {
            _connection = new SqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderBands.ServerType = conInfo.ServerType;
        }

        #endregion

        #region Public
        public async Task<Int32> AddBand(BandItem band)
        {
            try
            {
                SqlParameter param = null;

                SqlCommand cmd = new SqlCommand("AddBand", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
                param.Value = band.BandName.Substring(0, Math.Min(band.BandName.Length, 100));

                param = cmd.Parameters.Add("@SortName", SqlDbType.NVarChar, 100);
                param.Value = band.SortName.Substring(0, Math.Min(band.SortName.Length, 100));

                param = cmd.Parameters.Add("@ID", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                band.BandId = (int)param.Value;

                return band.BandId;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceBands_SQL, Error in AddBand";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<BandItem>> GetBandsByAlphabet(String firstCharacter)
        {
            try
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

                SqlParameter sqlParam = new SqlParameter("@FirstChar", SqlDbType.NVarChar);
                sqlParam.Value = firstCharacter;

                ObservableCollection<BandItem> bands = await GetBandsDB(strSQL, sqlParam);
                return bands;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceBands_SQL, Error in GetBandsByAlphabet";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<BandItem>> SearchBands(String searchText)
        {
            try
            {
                String strSQL = QueryBuilderBands.SearchBands();
                SqlParameter searchParam = new SqlParameter("@Name", SqlDbType.NVarChar);
                searchParam.Value = searchText;

                ObservableCollection<BandItem> bands = await GetBandsDB(strSQL, searchParam);
                return bands;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceBands_SQL, Error in SearchBands";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<BandItem> GetBand(Int32 bandID)
        {
            try
            {
                BandItem band = new BandItem();

                String strSQL = QueryBuilderBands.BandById(bandID);

                SqlCommand cmd = new SqlCommand(strSQL, _connection);
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
            catch (Exception Err)
            {
                String errorMessage = "DataServiceBands_SQL, Error in GetBand";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }


        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new SqlConnection(conInfo.GetConnectionString());
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
                    SqlConnection.ClearPool(_connection);
                }
                _connection.Dispose();
                _connection = null;
            }
        }
        #endregion

        #region private


        private async Task<ObservableCollection<BandItem>> GetBandsDB(String strSQL, SqlParameter sqlParam)
        {
            ObservableCollection<BandItem> bandList = new ObservableCollection<BandItem>();
            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            if (sqlParam != null)
            {
                cmd.Parameters.Add(sqlParam);
            }

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

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

        ~DataServiceBands_SQL()
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