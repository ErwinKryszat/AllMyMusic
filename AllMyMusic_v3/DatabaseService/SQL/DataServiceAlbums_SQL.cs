using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AllMyMusic.QueryBuilder;


namespace AllMyMusic.DataService
{
    public class DataServiceAlbums_SQL : IDataServiceAlbums, IDisposable
    {
        #region Properties
        private SqlConnection _connection;
        public SqlConnection Connection
        {
            get { return _connection; }
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
            get { return ServerType.SqlServer; }
        }
        #endregion

        #region Constructor
        public DataServiceAlbums_SQL(ConnectionInfo conInfo)
        {
            _connection = new SqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderAlbums.ServerType = conInfo.ServerType;
            QueryBuilderAlbums.ShowVariousArtists = Global.ViewVaBands;
        }
        #endregion

        #region Public
        public async Task<Int32> AddAlbum( AlbumItem album)
        {
            SqlParameter param = null;

            SqlCommand cmd = new SqlCommand("AddAlbum", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IDAlbum", SqlDbType.Int);
            param.Value = album.AlbumId;

            param = cmd.Parameters.Add("@IDBand", SqlDbType.Int);
            param.Value = album.BandId;

            param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
            param.Value = album.AlbumName.Substring(0, Math.Min(album.AlbumName.Length, 100));

            param = cmd.Parameters.Add("@SortName", SqlDbType.NVarChar, 100);
            param.Value = album.AlbumSortName.Substring(0, Math.Min(album.AlbumSortName.Length, 100));

            param = cmd.Parameters.Add("@Year", SqlDbType.Char, 4);
            param.Value = album.Year;

            param = cmd.Parameters.Add("@AlbumVA", SqlDbType.Int);
            param.Value = (Int32)album.ArtistType;

            param = cmd.Parameters.Add("@AlbumGenre", SqlDbType.NVarChar, 50);
            param.Value = album.AlbumGenre;

            param = cmd.Parameters.Add("@AlbumPath", SqlDbType.NVarChar, 200);
            param.Value = album.AlbumPath;

            param = cmd.Parameters.Add("@ID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            album.AlbumId = (int)param.Value;

            return album.AlbumId;
        }
        public async Task AddImage(AlbumItem album)
        {
            SqlCommand cmd = new SqlCommand("AddImage", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param;

            if (album.AlbumId == 0)
            {
                return;
            }

            //  Note: Param Names in Stored Procedure have to match this code!
            param = cmd.Parameters.Add("@IDAlbum", SqlDbType.Int);
            param.Value = album.AlbumId;
            param = cmd.Parameters.Add("@IDBand", SqlDbType.Int);
            param.Value = album.BandId;
            param = cmd.Parameters.Add("@Front", SqlDbType.NVarChar, 250);
            param.Value = album.FrontImageFileName;
            param = cmd.Parameters.Add("@Back", SqlDbType.NVarChar, 250);
            param.Value = album.BackImageFileName;
            param = cmd.Parameters.Add("@Stamp", SqlDbType.NVarChar, 250);
            param.Value = album.StampImageFileName;

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<AlbumItem> GetAlbumByPath(String albumPath)
        {
            String strSQL = QueryBuilderAlbums.Folder_SQL();
            SqlParameter sqlParam = new SqlParameter("@PathPart", SqlDbType.NVarChar);
            sqlParam.Value = albumPath;

            AlbumItem album = await GetAlbumDB(strSQL, sqlParam);

            return album;
        }

        public async Task<ObservableCollection<AlbumItem>> GetAlbums(BandItem band)
        {
            String strSQL = QueryBuilderAlbums.AlbumsByBand(band);
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(AlbumGenreItem albumGenre)
        {
            String strSQL = QueryBuilderAlbumsVA.Albums(albumGenre);
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(ComposerItem composer)
        {
            String strSQL = QueryBuilderAlbums.AlbumsByComposer(composer);
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(ConductorItem conductor)
        {
            String strSQL = QueryBuilderAlbums.AlbumsByConductor(conductor);
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(CountryItem country)
        {
            String strSQL = QueryBuilderAlbums.AlbumsByCountry(country);
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(GenreItem genre)
        {
            String strSQL = QueryBuilderAlbums.AlbumsByGenre(genre);
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(LanguageItem language)
        {
            String strSQL = QueryBuilderAlbums.AlbumsByLanguage(language);
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(ObservableCollection<String> playList)
        {
            ObservableCollection<AlbumItem> albums = new ObservableCollection<AlbumItem>();
            for (int i = 0; i < playList.Count; i++)
            {
                String strSQL = QueryBuilderAlbums.Folder_SQL();
                SqlParameter searchParam = new SqlParameter("@PathPart", SqlDbType.NVarChar);
                searchParam.Value = playList[i];

                AlbumItem album = await GetAlbumDB(strSQL, searchParam);
                albums.Add(album);
            }
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbumsByPath(String albumFolderPath)
        {
            String strSQL = QueryBuilderAlbums.Folder_SQL();

            SqlParameter sqlParam = new SqlParameter("@PathPart", SqlDbType.NVarChar);
            sqlParam.Value = albumFolderPath;

            ObservableCollection<AlbumItem> albums = await Task.Run(() => GetAlbumsDB(strSQL, sqlParam));
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(LeadPerformerItem leadPerformer)
        {
            String strSQL = QueryBuilderAlbums.AlbumsByLeadPerformer(leadPerformer);
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> SearchAlbums(String searchString)
        {
            String strSQL = QueryBuilderAlbums.SearchAlbums();
            SqlParameter searchParam = new SqlParameter("@Name", SqlDbType.NVarChar);
            searchParam.Value = searchString;

            ObservableCollection<AlbumItem> albums = await Task.Run(() => GetAlbumsDB(strSQL, searchParam));
            return albums;
        }
        public async Task<ObservableCollection<AlbumItem>> GetAlbumsByCondition(String condition)
        {
            String strSQL = QueryBuilderAlbums.AlbumsByCondition(condition);

            ObservableCollection<AlbumItem> albums = await Task.Run(() => GetAlbumsDB(strSQL));
            return albums;
        }

        public async Task<AlbumItem> GetAlbum(Int32 albumID)
        {
            AlbumItem album = await Task<AlbumItem>.Run(() => 
            {
                AlbumItem album2 = null;
                String strSQL = QueryBuilderAlbums.Album(albumID);
                SqlCommand cmd = new SqlCommand(strSQL, _connection);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

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
        public async Task<ObservableCollection<AlbumItem>> GetAlbums(String strSQL)
        {
            ObservableCollection<AlbumItem> albums = await GetAlbumsDB(strSQL);
            return albums;
        }
        public async Task DeleteAlbum(AlbumItem album)
        {
            SqlCommand cmd = new SqlCommand("DeleteAlbum", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param = cmd.Parameters.Add("@AlbumId", SqlDbType.Int);
            param.Value = album.AlbumId;

            param = cmd.Parameters.Add("@Rowcount", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task DeleteAlbumGenre(AlbumGenreItem albumGenre)
        {
            SqlCommand cmd = new SqlCommand("DeleteAlbumGenre", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param = cmd.Parameters.Add("@AlbumGenreID", SqlDbType.Int);
            param.Value = albumGenre.AlbumGenreId;

            param = cmd.Parameters.Add("@Rowcount", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task DeleteBand(BandItem band)
        {
            SqlCommand cmd = new SqlCommand("DeleteBand", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param = cmd.Parameters.Add("@BandID", SqlDbType.Int);
            param.Value = band.BandId;

            param = cmd.Parameters.Add("@Rowcount", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
        }

        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new SqlConnection(conInfo.GetConnectionString());
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
                    SqlConnection.ClearPool(_connection);
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
        private async Task< ObservableCollection<AlbumItem>> GetAlbumsDB(String strSQL)
        {
            ObservableCollection<AlbumItem> albums = new ObservableCollection<AlbumItem>();
            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
        private async Task<ObservableCollection<AlbumItem>> GetAlbumsDB(String strSQL, SqlParameter sqlParam)
        {
            ObservableCollection<AlbumItem> albums = new ObservableCollection<AlbumItem>();

            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            if (sqlParam != null)
            {
                cmd.Parameters.Add(sqlParam);
            }
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
        private async Task<AlbumItem> GetAlbumDB(String strSQL, SqlParameter sqlParam)
        {

            SqlCommand cmd = new SqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            if (sqlParam != null)
            {
                cmd.Parameters.Add(sqlParam);
            }
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                reader.Read();
            }

            AlbumItem album = ReadAlbum(reader);

            reader.Close();
            return album;
        }
        private AlbumItem ReadAlbum(SqlDataReader reader)
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

        ~DataServiceAlbums_SQL()
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