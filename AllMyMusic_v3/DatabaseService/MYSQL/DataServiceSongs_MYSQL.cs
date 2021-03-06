﻿using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AllMyMusic.QueryBuilder;


namespace AllMyMusic.DataService
{
    public class DataServiceSongs_MYSQL : IDataServiceSongs, IDisposable
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
                QueryBuilderSongs.ShowVariousArtists = value;
            }
        }

        public ServerType SelectedServerType
        {
            get { return ServerType.MySql; }
        }
        #endregion

        #region Constructor
        public DataServiceSongs_MYSQL(ConnectionInfo conInfo)
        {
            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderSongs.ServerType = conInfo.ServerType;
        }
        #endregion

        #region Public
        public async Task<int> AddSong(SongItem song)
        {
            try
            {
                Int32 SongId = -1;

                MySqlCommand cmd = new MySqlCommand("AddSong", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlParameter param;

                // When adding songs to the database, add 5s so other tasks can complete writing the file and we not think its new the next time we find it
                song.DateAdded = DateTime.Now.AddSeconds(5);

                // Add the Song ********************************************
                param = cmd.Parameters.Add("var_IDSong", MySqlDbType.Int32);
                param.Value = song.SongId;

                param = cmd.Parameters.Add("var_IDBand", MySqlDbType.Int32);
                param.Value = song.BandId;

                param = cmd.Parameters.Add("var_IDAlbum", MySqlDbType.Int32);
                param.Value = song.AlbumId;

                param = cmd.Parameters.Add("var_IDGenre", MySqlDbType.Int32);
                param.Value = song.GenreId;

                param = cmd.Parameters.Add("var_IDLanguage", MySqlDbType.Int32);
                param.Value = song.LanguageId;

                param = cmd.Parameters.Add("var_IDCountry", MySqlDbType.Int32);
                param.Value = song.CountryId;

                param = cmd.Parameters.Add("var_IDComposer", MySqlDbType.Int32);
                param.Value = song.ComposerId;

                param = cmd.Parameters.Add("var_IDConductor", MySqlDbType.Int32);
                param.Value = song.ConductorId;

                param = cmd.Parameters.Add("var_IDLeadPerformer", MySqlDbType.Int32);
                param.Value = song.LeadPerformerId;

                param = cmd.Parameters.Add("var_SongTitle", MySqlDbType.VarString, 100);
                param.Value = song.SongTitle.Substring(0, Math.Min(song.SongTitle.Length, 100));

                param = cmd.Parameters.Add("var_Path", MySqlDbType.VarString, 250);
                param.Value = song.SongPath;

                param = cmd.Parameters.Add("var_Filename", MySqlDbType.VarString, 250);
                param.Value = song.SongFilename;

                param = cmd.Parameters.Add("var_Language", MySqlDbType.VarString, 50);
                param.Value = song.Language;

                param = cmd.Parameters.Add("var_Country", MySqlDbType.VarString, 50);
                param.Value = song.Country;

                param = cmd.Parameters.Add("var_Track", MySqlDbType.Int32);
                if (String.IsNullOrEmpty(song.Track) == true)
                {
                    param.Value = -1;
                }
                else
                {
                    param.Value = Convert.ToInt32(song.Track);
                }


                param = cmd.Parameters.Add("var_Rating", MySqlDbType.Int32);
                param.Value = song.Rating;

                param = cmd.Parameters.Add("var_Genre", MySqlDbType.VarString, 50);
                param.Value = song.Genre.Substring(0, Math.Min(song.Genre.Length, 50));

                param = cmd.Parameters.Add("var_LengthInteger", MySqlDbType.Int32);
                param.Value = song.Seconds;

                param = cmd.Parameters.Add("var_LengthString", MySqlDbType.VarString, 8);
                param.Value = song.Duration;

                param = cmd.Parameters.Add("var_BitRate", MySqlDbType.Int32);
                param.Value = song.Bitrate;

                param = cmd.Parameters.Add("var_SampleRate", MySqlDbType.Int32);
                param.Value = song.SampleRate;

                param = cmd.Parameters.Add("var_CBR_VBR", MySqlDbType.Int32);
                param.Value = (Int32)song.BitrateType;

                param = cmd.Parameters.Add("var_ComposerName", MySqlDbType.VarString, 100);
                param.Value = song.ComposerName.Substring(0, Math.Min(song.ComposerName.Length, 100));

                param = cmd.Parameters.Add("var_ConductorName", MySqlDbType.VarString, 100);
                param.Value = song.ConductorName.Substring(0, Math.Min(song.ConductorName.Length, 100));

                param = cmd.Parameters.Add("var_LeadPerformerName", MySqlDbType.VarString, 100);
                param.Value = song.LeadPerformer.Substring(0, Math.Min(song.LeadPerformer.Length, 100));

                param = cmd.Parameters.Add("var_VA_Flag", MySqlDbType.Int32);
                param.Value = (Int32)song.ArtistType;

                param = cmd.Parameters.Add("var_DateAdded", MySqlDbType.DateTime);
                if (song.DateAdded != DateTime.MinValue)
                {
                    param.Value = (DateTime)song.DateAdded;
                }
                else
                {
                    param.Value = new DateTime(1900, 1, 1);
                }

                param = cmd.Parameters.Add("var_DatePlayed", MySqlDbType.DateTime);
                if (song.DatePlayed != DateTime.MinValue)
                {
                    param.Value = (DateTime)song.DatePlayed;
                }
                else
                {
                    param.Value = new DateTime(1900, 1, 1);
                }


                param = cmd.Parameters.Add("var_Comment", MySqlDbType.VarString, 250);
                param.Value = song.Comment.Substring(0, Math.Min(song.Comment.Length, 250));

                param = cmd.Parameters.Add("var_WebsiteUser", MySqlDbType.VarString, 250);
                param.Value = song.WebsiteUser.Substring(0, Math.Min(song.WebsiteUser.Length, 250));

                param = cmd.Parameters.Add("var_WebsiteArtist", MySqlDbType.VarString, 250);
                param.Value = song.WebsiteArtist.Substring(0, Math.Min(song.WebsiteArtist.Length, 100));


                param = cmd.Parameters.Add("var_ID", MySqlDbType.Int32);
                param.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                SongId = (Int32)param.Value;

                return SongId;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in AddSong";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<SongItem> GetSong(Int32 songID)
        {
            try
            {
                SongItem song = new SongItem();
                String strSQL = "SELECT * FROM viewSongs WHERE IDSong = " + songID.ToString(); ;
                MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
                cmd.CommandType = CommandType.Text;

                System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
                //MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                if (reader.HasRows)
                {
                    song = ReadSongDB(reader);
                }

                reader.Close();

                return song;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSong";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<SongItem> GetSongByPath(String songPath)
        {
            try
            {
                SongItem song = new SongItem();
                MySqlParameter sqlParam = new MySqlParameter("var_FullPath", SqlDbType.NVarChar);
                sqlParam.Value = songPath.Replace("\\", "\\\\");

                String strSQL = QueryBuilderSongs.FullPath();

                MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
                cmd.Parameters.Add(sqlParam);
                cmd.CommandType = CommandType.Text;

                System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
                //MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                if (reader.HasRows)
                {
                    song = ReadSongDB(reader);
                }
                reader.Close();

                return song;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongByPath";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task<ObservableCollection<SongItem>> GetSongs(BandItem band)
        {
            try
            {
                String strSQL = QueryBuilderSongs.SongsByBand(band.BandId);
                ObservableCollection<SongItem> songs = await GetSongs(strSQL, null);

                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<SongItem>> GetSongs(AlbumItem album)
        {
            try
            {
                String strSQL = QueryBuilderSongs.SongsByAlbum(album.AlbumId);
                ObservableCollection<SongItem> songs = await GetSongs(strSQL, null);
                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<SongItem>> GetSongs(AlbumGenreItem albumGenre)
        {
            try
            {
                String strSQL = QueryBuilderSongs.AlbumGenreVA(albumGenre.AlbumGenreId);
                ObservableCollection<SongItem> songs = await GetSongs(strSQL, null);
                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<SongItem>> GetSongs(ObservableCollection<String> playList)
        {
            try
            {
                ObservableCollection<SongItem> songs = new ObservableCollection<SongItem>();

                for (int i = 0; i < playList.Count; i++)
                {
                    String strSQL = QueryBuilderSongs.FullPath();
                    MySqlParameter searchParam = new MySqlParameter("var_FullPath", SqlDbType.NVarChar);
                    searchParam.Value = playList[i];
                    SongItem song = await GetSongDB(strSQL, searchParam);
                    if (song != null)
                    {
                        songs.Add(song);
                    }
                }
                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<SongItem>> GetSongs(String strSQL)
        {
            try
            {
                ObservableCollection<SongItem> songs = await GetSongs(strSQL, null);
                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<SongItem>> GetSongsByPathPart(String albumPath)
        {
            try
            {
                MySqlParameter sqlParam = new MySqlParameter("var_PathPart", SqlDbType.NVarChar);
                sqlParam.Value = albumPath;

                String strSQL = QueryBuilderSongs.Folder();

                ObservableCollection<SongItem> songs = await GetSongs(strSQL, sqlParam);
                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        private async Task<ObservableCollection<SongItem>> GetSongs(String strSQL, MySqlParameter sqlParam)
        {
            try
            {
                ObservableCollection<SongItem> songs = new ObservableCollection<SongItem>();

                MySqlCommand cmd = new MySqlCommand(strSQL, _connection);

                if (sqlParam != null)
                {
                    cmd.Parameters.Add(sqlParam);
                }

                cmd.CommandType = CommandType.Text;

                System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
                //MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SongItem song = ReadSongDB(reader);
                        songs.Add(song);
                    }
                }
                reader.Close();

                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        private async Task<ObservableCollection<SongItem>> GetSongs(String strSQL, MySqlParameter sqlParam, ObservableCollection<SongItem> songs)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, _connection);

                if (sqlParam != null)
                {
                    cmd.Parameters.Add(sqlParam);
                }

                cmd.CommandType = CommandType.Text;

                System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
                //MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SongItem song = ReadSongDB(reader);
                        songs.Add(song);
                    }
                }
                reader.Close();

                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public async Task<ObservableCollection<SongItem>> SearchSongs(String searchString)
        {
            try
            {
                String strSQL = QueryBuilderSongs.SearchSongs();
                MySqlParameter searchParam = new MySqlParameter("@Name", SqlDbType.NVarChar);
                searchParam.Value = searchString;

                ObservableCollection<SongItem> songs = await GetSongs(strSQL, searchParam);
                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in SearchSongs";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<SongItem>> GetSongsByCondition(String condition)
        {
            try
            {
                String strSQL = QueryBuilderSongs.SongsByCondition(condition);

                ObservableCollection<SongItem> songs = await GetSongs(strSQL);
                return songs;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceSongs_MYSQL, Error in GetSongsByCondition";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

            QueryBuilderSongs.ServerType = conInfo.ServerType;
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
        private async Task<SongItem> GetSongDB(String strSQL, MySqlParameter sqlParam)
        {
            SongItem song = null;
            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);

            if (sqlParam != null)
            {
                cmd.Parameters.Add(sqlParam);
            }

            cmd.CommandType = CommandType.Text;

            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    song = ReadSongDB(reader);
                }
            }
            reader.Close();

            return song;
        }
        private SongItem ReadSongDB(System.Data.Common.DbDataReader reader)
        {
            SongItem song = new SongItem();

            // BandName
            if (!reader.IsDBNull(0))
            {
                song.BandName = reader.GetString(0).TrimEnd();
            }
            else { song.BandName = String.Empty; }

            // AlbumName
            if (!reader.IsDBNull(1))
            {
                song.AlbumName = reader.GetString(1).TrimEnd();
            }
            else { song.AlbumName = String.Empty; }

            // SongTitle
            if (!reader.IsDBNull(2))
            {
                song.SongTitle = reader.GetString(2).TrimEnd();
            }
            else { song.SongTitle = String.Empty; }

            // LeadPerformer
            if (!reader.IsDBNull(3))
            {
                song.LeadPerformer = reader.GetString(3).TrimEnd();
            }
            else { song.LeadPerformer = String.Empty; }

            // ComposerName
            if (!reader.IsDBNull(4))
            {
                song.ComposerName = reader.GetString(4).TrimEnd();
            }
            else { song.ComposerName = String.Empty; }

            // ConductorName
            if (!reader.IsDBNull(5))
            {
                song.ConductorName = reader.GetString(5).TrimEnd();
            }
            else { song.ConductorName = String.Empty; }


            // *******************************************************************

            // Track
            if (!reader.IsDBNull(6))
            {
                Int32 nTrack = reader.GetInt32(6);
                song.Track = nTrack.ToString();
            }
            else { song.Track = ""; }

            // Year
            if (!reader.IsDBNull(7))
            {
                song.Year = reader.GetString(7).TrimEnd();
            }
            else { song.Year = ""; }

            // Genre
            if (!reader.IsDBNull(8))
            {
                song.Genre = reader.GetString(8).TrimEnd();
            }
            else { song.Genre = String.Empty; }

            // albumGenre
            if (!reader.IsDBNull(9))
            {
                song.AlbumGenre = reader.GetString(9).TrimEnd();
            }
            else { song.AlbumGenre = String.Empty; }

            // Rating
            if (!reader.IsDBNull(10)) { song.Rating = (Int32)reader.GetInt32(10); }
            else { song.Rating = 0; }
            //song.RatingDecimal = RatingDecimal(song.Rating);  // Number of Image to show the Stars

            // *******************************************************************

            // Country
            if (!reader.IsDBNull(11))
            {
                song.Country = reader.GetString(11).TrimEnd();
            }
            else { song.Country = String.Empty; }

            // Language
            if (!reader.IsDBNull(12))
            {
                song.Language = reader.GetString(12).TrimEnd();
            }
            else { song.Language = String.Empty; }

            // *******************************************************************

            // SongPath
            if (!reader.IsDBNull(13))
            {
                song.SongPath = reader.GetString(13).TrimEnd();
            }
            else { song.SongPath = String.Empty; }

            // SongFilename
            if (!reader.IsDBNull(14))
            {
                song.SongFilename = reader.GetString(14).TrimEnd();
            }
            else { song.SongFilename = String.Empty; }

            // Song Length as integer
            if (!reader.IsDBNull(15)) { song.Seconds = (Int32)reader.GetInt32(15); }
            else { song.Seconds = 0; }

            // BitRate
            if (!reader.IsDBNull(17)) { song.Bitrate = (Int32)reader.GetInt32(17); }
            else { song.Bitrate = 0; }

            // SampleRate
            if (!reader.IsDBNull(18)) { song.SampleRate = (Int32)reader.GetInt32(18); }
            else { song.SampleRate = 0; }

            // CBR_VBR
            if (!reader.IsDBNull(19)) { song.BitrateType = (BitrateType)reader.GetInt32(19); }
            else { song.BitrateType = BitrateType.CBR; }

            // VA_Flag
            if (!reader.IsDBNull(20)) { song.ArtistType = (ArtistType)reader.GetInt32(20); }
            else { song.ArtistType = 0; }

            // *******************************************************************

            // bandID
            if (!reader.IsDBNull(21)) { song.BandId = (Int32)reader.GetInt32(21); }
            else { song.BandId = 0; }

            // albumID
            if (!reader.IsDBNull(22)) { song.AlbumId = (Int32)reader.GetInt32(22); }
            else { song.AlbumId = 0; }

            // SongId
            if (!reader.IsDBNull(23)) { song.SongId = (Int32)reader.GetInt32(23); }
            else { song.SongId = 0; }

            // LeadPerformerID
            if (!reader.IsDBNull(24)) { song.LeadPerformerId = (Int32)reader.GetInt32(24); }
            else { song.LeadPerformerId = 0; }

            // ComposerID
            if (!reader.IsDBNull(25)) { song.ComposerId = (Int32)reader.GetInt32(25); }
            else { song.ComposerId = 0; }

            // ConductorID
            if (!reader.IsDBNull(26)) { song.ConductorId = (Int32)reader.GetInt32(26); }
            else { song.ConductorId = 0; }

            // CountryID
            if (!reader.IsDBNull(27)) { song.CountryId = (Int32)reader.GetInt32(27); }
            else { song.CountryId = 0; }

            // LanguageID
            if (!reader.IsDBNull(28)) { song.LanguageId = (Int32)reader.GetInt32(28); }
            else { song.LanguageId = 0; }

            // albumGenreID
            if (!reader.IsDBNull(29)) { song.AlbumGenreId = (Int32)reader.GetInt32(29); }
            else { song.AlbumGenreId = 0; }

            // GenreID
            if (!reader.IsDBNull(30)) { song.GenreId = (Int32)reader.GetInt32(30); }
            else { song.GenreId = 0; }

            // DateAdded 
            if (!reader.IsDBNull(34)) { song.DateAdded = (DateTime)reader.GetDateTime(34); }
            else { song.DateAdded = DateTime.MinValue; }

            // DatePlayed
            if (!reader.IsDBNull(35)) { song.DatePlayed = (DateTime)reader.GetDateTime(35); }
            else { song.DatePlayed = DateTime.MinValue; }

            // BandSortName
            if (!reader.IsDBNull(36))
            {
                song.BandSortName = reader.GetString(36).TrimEnd();
            }
            else { song.BandSortName = String.Empty; }

            // AlbumSortName
            if (!reader.IsDBNull(37))
            {
                song.AlbumSortName = reader.GetString(37).TrimEnd();
            }
            else { song.AlbumSortName = String.Empty; }

            // FrontImage
            if (!reader.IsDBNull(38))
            {
                song.FrontImageFileName = reader.GetString(38).TrimEnd();
            }
            else { song.FrontImageFileName = String.Empty; }

            // BackImage
            if (!reader.IsDBNull(39))
            {
                song.BackImageFileName = reader.GetString(39).TrimEnd();
            }
            else { song.BackImageFileName = String.Empty; }

            // StampImage
            if (!reader.IsDBNull(40))
            {
                song.StampImageFileName = reader.GetString(40).TrimEnd();
            }
            else { song.StampImageFileName = String.Empty; }

            // Comment
            if (!reader.IsDBNull(41))
            {
                song.Comment = reader.GetString(41).TrimEnd();
            }
            else { song.Comment = String.Empty; }

            // WebsiteUser
            if (!reader.IsDBNull(42))
            {
                song.WebsiteUser = reader.GetString(42).TrimEnd();
            }
            else { song.WebsiteUser = String.Empty; }

            // WebsiteArtist
            if (!reader.IsDBNull(43))
            {
                song.WebsiteArtist = reader.GetString(43).TrimEnd();
            }
            else { song.WebsiteArtist = String.Empty; }
            return song;
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

        ~DataServiceSongs_MYSQL()
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