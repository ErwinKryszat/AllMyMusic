using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AllMyMusic.QueryBuilder;

namespace AllMyMusic.DataService
{
    public class DataServiceAlbums_MYSQL : IDataServiceAlbums, IDisposable
    {
        #region Properties
        private MySqlConnection _connection;
        public MySqlConnection Connection
        {
            get { return _connection; }
        }

        private Boolean _showVariousArtists = true;
        public Boolean ShowVariousArtists
        {
            get { return _showVariousArtists; }
            set
            {
                _showVariousArtists = value;
                QueryBuilderAlbums.ShowVariousArtists = value;
            }
        }
        public String VariousArtists
        {
            get { return "Various Artists"; }
        }

        public Int32 VariousArtistID 
        {
            get { return 0; }
        }

        public ServerType SelectedServerType
        {
            get { return ServerType.MySql; }
        }
        #endregion

        #region Constructor
        public DataServiceAlbums_MYSQL(ConnectionInfo conInfo)
        {
            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderAlbums.ServerType = conInfo.ServerType;
        }
        #endregion

        #region Public
        public async Task<Int32> AddAlbum(AlbumItem album)
        {
            try
            {
                MySqlParameter param = null;

                MySqlCommand cmd = new MySqlCommand("AddAlbum", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("var_IDAlbum", MySqlDbType.Int32);
                param.Value = album.AlbumId;

                param = cmd.Parameters.Add("var_IDBand", MySqlDbType.Int32);
                param.Value = album.BandId;

                param = cmd.Parameters.Add("var_Name", MySqlDbType.VarString, 100);
                param.Value = album.AlbumName.Substring(0, Math.Min(album.AlbumName.Length, 100));

                param = cmd.Parameters.Add("var_SortName", MySqlDbType.VarString, 100);
                param.Value = album.AlbumSortName.Substring(0, Math.Min(album.AlbumSortName.Length, 100));

                param = cmd.Parameters.Add("var_Year", MySqlDbType.VarChar, 4);
                param.Value = album.Year;

                param = cmd.Parameters.Add("var_AlbumVA", MySqlDbType.Int32);
                param.Value = (Int32)album.ArtistType;

                param = cmd.Parameters.Add("var_AlbumGenre", MySqlDbType.VarString, 50);
                param.Value = album.AlbumGenre;

                param = cmd.Parameters.Add("var_AlbumPath", MySqlDbType.VarString, 200);
                param.Value = album.AlbumPath;

                param = cmd.Parameters.Add("var_ID", MySqlDbType.Int32);
                param.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                album.AlbumId = (int)param.Value;

                return album.AlbumId;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in AddAlbum";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task AddImage(AlbumItem album)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("AddImage", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlParameter param;

                if (album.AlbumId == 0)
                {
                    return;
                }

                //  Note: Param Names in Stored Procedure have to match this code!
                param = cmd.Parameters.Add("var_IDAlbum", MySqlDbType.Int32);
                param.Value = album.AlbumId;
                param = cmd.Parameters.Add("var_IDBand", MySqlDbType.Int32);
                param.Value = album.BandId;
                param = cmd.Parameters.Add("var_Front", MySqlDbType.VarString, 250);
                param.Value = album.FrontImageFileName;
                param = cmd.Parameters.Add("var_Back", MySqlDbType.VarString, 250);
                param.Value = album.BackImageFileName;
                param = cmd.Parameters.Add("var_Stamp", MySqlDbType.VarString, 250);
                param.Value = album.StampImageFileName;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in AddImage";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task<AlbumItem> GetAlbumByPath(String albumPath)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.Folder_MYSQL();
                MySqlParameter sqlParam = new MySqlParameter("@PathPart", SqlDbType.NVarChar);
                sqlParam.Value = albumPath;

                AlbumItem album = await GetAlbumDB(strSQL, sqlParam);

                return album;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbumByPath";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task<ObservableCollection<AlbumItem>> GetAlbums(BandItem band)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.AlbumsByBand(band);
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(AlbumGenreItem albumGenre)
        {
            try
            {
                String strSQL = QueryBuilderAlbumsVA.Albums(albumGenre);
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task<ObservableCollection<AlbumItem>> GetAlbums(ComposerItem composer)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.AlbumsByComposer(composer);
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(ConductorItem conductor)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.AlbumsByConductor(conductor);
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }         
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(CountryItem country)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.AlbumsByCountry(country);
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(GenreItem genre)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.AlbumsByGenre(genre);
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(LanguageItem language)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.AlbumsByLanguage(language);
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);

            }
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbumsByPath(String albumFolderPath)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.Folder_MYSQL();

                MySqlParameter sqlParam = new MySqlParameter("var_PathPart", MySqlDbType.VarString);
                sqlParam.Value = albumFolderPath;

                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL, sqlParam);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbumsByPath";
                throw new DatabaseLayerException(errorMessage, Err);

            }
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(LeadPerformerItem leadPerformer)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.AlbumsByLeadPerformer(leadPerformer);
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(ObservableCollection<String> playList)
        {
            try
            {
                ObservableCollection<AlbumItem> albums = new ObservableCollection<AlbumItem>();
                for (int i = 0; i < playList.Count; i++)
                {
                    String strSQL = QueryBuilderAlbums.Folder_MYSQL();
                    MySqlParameter searchParam = new MySqlParameter("var_PathPart", SqlDbType.NVarChar);
                    searchParam.Value = playList[i];

                    AlbumItem album = await GetAlbumDB(strSQL, searchParam);
                }
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<AlbumItem>> SearchAlbums(String searchString)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.SearchAlbums();
                MySqlParameter searchParam = new MySqlParameter("var_Name", MySqlDbType.VarString);
                searchParam.Value = searchString;

                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL, searchParam);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in SearchAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbumsByCondition(String condition)
        {
            try
            {
                String strSQL = QueryBuilderAlbums.AlbumsByCondition(condition);

                ObservableCollection<AlbumItem> albums = await Task.Run(() => GetAlbumsDB(strSQL));
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbumsByCondition";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<AlbumItem> GetAlbum(Int32 albumID)
        {
            try
            {
                AlbumItem album = await Task<AlbumItem>.Run(() =>
                {
                    AlbumItem album2 = null;
                    String strSQL = QueryBuilderAlbums.Album(albumID);
                    MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        album2 = ReadAlbum(reader);
                    }
                    reader.Close();

                    return album2;
                });

                return album;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbum";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task<ObservableCollection<AlbumItem>> GetAlbums(String strSQL)
        {
            try
            {
                ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
                return albums;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in GetAlbums";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task DeleteAlbum(AlbumItem album)
        {
            try
            {

                MySqlCommand cmd = new MySqlCommand("DeleteAlbum", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlParameter param = cmd.Parameters.Add("var_AlbumId", MySqlDbType.Int32);
                param.Value = album.AlbumId;

                param = cmd.Parameters.Add("var_Rowcount", MySqlDbType.Int32);
                param.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in DeleteAlbum";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task DeleteAlbumGenre(AlbumGenreItem albumGenre)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("DeleteAlbumGenre", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlParameter param = cmd.Parameters.Add("var_AlbumGenreID", MySqlDbType.Int32);
                param.Value = albumGenre.AlbumGenreId;

                param = cmd.Parameters.Add("var_Rowcount", MySqlDbType.Int32);
                param.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in DeleteAlbumGenre";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task DeleteBand(BandItem band)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("DeleteBand", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlParameter param = cmd.Parameters.Add("var_BandID", MySqlDbType.Int32);
                param.Value = band.BandId;

                param = cmd.Parameters.Add("var_Rowcount", MySqlDbType.Int32);
                param.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceAlbums_MYSQL, Error in DeleteBand";
                throw new DatabaseLayerException(errorMessage, Err);

            }
        }

        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderAlbums.ServerType = conInfo.ServerType;
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
        private void DeleteAlbums(String strSQL)
        { 
        
        }
        private async Task<ObservableCollection<AlbumItem>> GetAlbumsDB(String strSQL)
        {
            ObservableCollection<AlbumItem> albums = new ObservableCollection<AlbumItem>();
            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;
            
            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    AlbumItem item = ReadAlbum(reader);
                    albums.Add(item);
                }
            }

            reader.Close();
            return albums;
        }
        private async Task<ObservableCollection<AlbumItem>> GetAlbumsDB(String strSQL, MySqlParameter sqlParam)
        {
            ObservableCollection<AlbumItem> albums = new ObservableCollection<AlbumItem>();

            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            if (sqlParam != null)
            {
                cmd.Parameters.Add(sqlParam);
            }

            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    AlbumItem item = ReadAlbum(reader);
                    albums.Add(item);
                }
            }

            reader.Close();
            return albums;
        }
        private async Task<AlbumItem> GetAlbumDB(String strSQL, MySqlParameter sqlParam)
        {

            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            if (sqlParam != null)
            {
                cmd.Parameters.Add(sqlParam);
            }

            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            AlbumItem album = ReadAlbum(reader);

            reader.Close();
            return album;
        }
        private AlbumItem ReadAlbum(System.Data.Common.DbDataReader reader)
        {
            AlbumItem item = new AlbumItem();

            // albumName
            if (!reader.IsDBNull(0))
            {
                item.AlbumName = reader.GetString(0).TrimEnd();
            }
            else { item.AlbumName = String.Empty; }

            // album ID
            if (!reader.IsDBNull(1)) { item.AlbumId = (Int32)reader.GetInt32(1); }
            else { item.AlbumId = 0; }


            // SongCount
            if (!reader.IsDBNull(2)) { item.SongCount = (Int32)reader.GetInt32(2); }
            else { item.SongCount = 0; }

            // BandName
            if (!reader.IsDBNull(3))
            {
                item.BandName = reader.GetString(3).TrimEnd();
            }
            else { item.BandName = String.Empty; }

            // Band ID
            if (!reader.IsDBNull(4)) { item.BandId = (Int32)reader.GetInt32(4); }
            else { item.BandId = 0; }


            // albumPath
            if (!reader.IsDBNull(5))
            {
                item.AlbumPath = reader.GetString(5).TrimEnd();
            }
            else { item.AlbumPath = String.Empty; }

            // Year
            if (!reader.IsDBNull(6))
            {
                item.Year = reader.GetString(6).TrimEnd();
            }
            else { item.Year = ""; }

            // albumGenre
            if (!reader.IsDBNull(7))
            {
                item.AlbumGenre = reader.GetString(7).TrimEnd();
            }
            else { item.AlbumGenre = String.Empty; }


            // VariousArtists
            if (!reader.IsDBNull(8)) { item.ArtistType = (ArtistType)reader.GetInt32(8); }
            else { item.ArtistType = ArtistType.SingleArtist; }

            //SELECT  albumName, albumID, SongCount, BandName, albumPath, Year, albumGenre, VariousArtists, Front, Back, Stamp,albumGenreID

            // Front Image
            if (!reader.IsDBNull(9))
            {
                item.FrontImageFileName = reader.GetString(9).TrimEnd();
            }
            else { item.FrontImageFileName = String.Empty; }

            // Back Image
            if (!reader.IsDBNull(10))
            {
                item.BackImageFileName = reader.GetString(10).TrimEnd();
            }
            else { item.BackImageFileName = String.Empty; }

            // Stamp Image
            if (!reader.IsDBNull(11))
            {
                item.StampImageFileName = reader.GetString(11).TrimEnd();
            }
            else { item.StampImageFileName = String.Empty; }

            // albumGenre ID
            if (!reader.IsDBNull(12)) { item.AlbumGenreId = (Int32)reader.GetInt32(12); }
            else { item.AlbumGenreId = 0; }

            // TotalLength
            if (!reader.IsDBNull(13)) { item.TotalLength = (Int32)reader.GetInt32(13); }
            else { item.TotalLength = 0; }

            return item;
        }
        private AlbumItem ReadAlbum(MySqlDataReader reader)
        {
            AlbumItem item = new AlbumItem();

            // albumName
            if (!reader.IsDBNull(0))
            {
                item.AlbumName = reader.GetString(0).TrimEnd();
            }
            else { item.AlbumName = String.Empty; }

            // album ID
            if (!reader.IsDBNull(1)) { item.AlbumId = (Int32)reader.GetInt32(1); }
            else { item.AlbumId = 0; }


            // SongCount
            if (!reader.IsDBNull(2)) { item.SongCount = (Int32)reader.GetInt32(2); }
            else { item.SongCount = 0; }

            // BandName
            if (!reader.IsDBNull(3))
            {
                item.BandName = reader.GetString(3).TrimEnd();
            }
            else { item.BandName = String.Empty; }

            // Band ID
            if (!reader.IsDBNull(4)) { item.BandId = (Int32)reader.GetInt32(4); }
            else { item.BandId = 0; }


            // albumPath
            if (!reader.IsDBNull(5))
            {
                item.AlbumPath = reader.GetString(5).TrimEnd();
            }
            else { item.AlbumPath = String.Empty; }

            // Year
            if (!reader.IsDBNull(6))
            {
                item.Year = reader.GetString(6).TrimEnd();
            }
            else { item.Year = ""; }

            // albumGenre
            if (!reader.IsDBNull(7))
            {
                item.AlbumGenre = reader.GetString(7).TrimEnd();
            }
            else { item.AlbumGenre = String.Empty; }


            // VariousArtists
            if (!reader.IsDBNull(8)) { item.ArtistType = (ArtistType)reader.GetInt32(8); }
            else { item.ArtistType = ArtistType.SingleArtist; }

            //SELECT  albumName, albumID, SongCount, BandName, albumPath, Year, albumGenre, VariousArtists, Front, Back, Stamp,albumGenreID

            // Front Image
            if (!reader.IsDBNull(9))
            {
                item.FrontImageFileName = reader.GetString(9).TrimEnd();
            }
            else { item.FrontImageFileName = String.Empty; }

            // Back Image
            if (!reader.IsDBNull(10))
            {
                item.BackImageFileName = reader.GetString(10).TrimEnd();
            }
            else { item.BackImageFileName = String.Empty; }

            // Stamp Image
            if (!reader.IsDBNull(11))
            {
                item.StampImageFileName = reader.GetString(11).TrimEnd();
            }
            else { item.StampImageFileName = String.Empty; }

            // albumGenre ID
            if (!reader.IsDBNull(12)) { item.AlbumGenreId = (Int32)reader.GetInt32(12); }
            else { item.AlbumGenreId = 0; }

            // TotalLength
            if (!reader.IsDBNull(13)) { item.TotalLength = (Int32)reader.GetInt32(13); }
            else { item.TotalLength = 0; }

            return item;
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

        ~DataServiceAlbums_MYSQL()
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