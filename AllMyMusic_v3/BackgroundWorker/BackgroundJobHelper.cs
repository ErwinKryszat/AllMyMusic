using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using AllMyMusic.DataService;

namespace AllMyMusic
{
    public class BackgroundJobHelper : IDisposable
    {
        #region Fields
        private IDataServiceSongs _dataServiceSongs;
        private IDataServiceBands _dataServiceBands;
        private IDataServiceAlbums _dataServiceAlbums;
        private IDataServiceCountries _dataServiceCountries;
        #endregion

        public BackgroundJobHelper(ConnectionInfo conInfo)
        {
            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceBands = new DataServiceBands_SQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
                _dataServiceAlbums = new DataServiceAlbums_SQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceBands = new DataServiceBands_MYSQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
                _dataServiceAlbums = new DataServiceAlbums_MYSQL(conInfo);
                _dataServiceCountries = new DataServiceCountries_MYSQL(conInfo);
            }
        }

        public void AddSong(SongItem song)
        {
            try
            {
                _dataServiceSongs.AddSong(song);
            }
            catch (Exception Err)
            {
                String errorMessage = "Error adding song: " + song.SongFullPath;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        public async Task<SongItem> AddOneBandAlbumSong(SongItem song, SongItem lastSong)
        {
            try
            {
                if ((String.IsNullOrEmpty(song.BandName) == true) && (String.IsNullOrEmpty(song.LeadPerformer) == false))
                {
                    song.BandName = song.LeadPerformer;
                }

                if (lastSong != null)
                {
                    song.UpdateItemIDs(lastSong);

                    if (song.AlbumId == lastSong.AlbumId)
                    {
                        song.ArtistType = lastSong.ArtistType;
                    }
                    if ((song.AlbumId == lastSong.AlbumId) && (song.BandName != lastSong.BandName) && lastSong.ArtistType == ArtistType.SingleArtist)
                    {
                        song.ArtistType = ArtistType.VariousArtist;
                        song.MustUpdateAlbum = true;
                    }

                    if (song.ArtistType == ArtistType.VariousArtist)
                    {
                        String albumGenre = GetFolder(song.SongPath, 3);
                        song.AlbumGenre = albumGenre.Substring(0, Math.Min(albumGenre.Length, 50));
                    }
                    else
                    {
                        String albumGenre = GetFolder(song.SongPath, 4);
                        song.AlbumGenre = albumGenre.Substring(0, Math.Min(albumGenre.Length, 50));
                    }

                    if (song.AlbumGenre == lastSong.AlbumGenre)
                    {
                        song.AlbumGenreId = lastSong.AlbumGenreId;
                    }

                    song.MustUpdateAlbum = SongsFromDifferentAlbums(lastSong, song);
                    song.MustUpdateBand = BandCompare(lastSong, song);


                }
                else
                {
                    song.MustUpdateBand = true;
                    song.MustUpdateAlbum = true;
                }



                if ((song.MustUpdateBand == true) || (song.ArtistType == ArtistType.VariousArtist))
                {
                    if (song.BandName != String.Empty)
                    {
                        try
                        {
                            BandItem _band = song.GetBand();
                            song.BandId = await _dataServiceBands.AddBand(_band);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }


                if (song.MustUpdateAlbum == true)
                {
                    AlbumItem album = new AlbumItem(song);

                    if (song.ArtistType == ArtistType.VariousArtist)
                    {
                        album.BandName = _dataServiceAlbums.VariousArtists;
                        album.BandId = _dataServiceAlbums.VariousArtistID;
                    }

                    song.AlbumId = await _dataServiceAlbums.AddAlbum(album);

                    await _dataServiceAlbums.AddImage(album);

                }


                song.SongId =  await _dataServiceSongs.AddSong(song);

            }
            catch (Exception Err)
            {
                String errorMessage = null;
                if ((String.IsNullOrEmpty(song.BandName) == false) && (String.IsNullOrEmpty(song.AlbumName) == false) && (String.IsNullOrEmpty(song.SongTitle) == false))
                {
                    errorMessage = "Error Band, Album and Song "
                    + "Band: " + song.BandName + "\\n"
                    + "Album: " + song.AlbumName + "\\n"
                    + "Song: " + song.SongTitle + "\\n";
                }
                else
                {
                    errorMessage = "Error updating Band, Album and Song: No song data available!";
                }


                
                ShowError.ShowAndLog(Err, errorMessage, 2001);
        
            }

            return song;
        }
        public async Task  UpdateCountryTable()
        {
            ObservableCollection<CountryItem> _countries = await _dataServiceCountries.GetCountries();
            await _dataServiceCountries.UpdateCountries(_countries);
        }
        public void Close()
        {
            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Close();
            }

            if (_dataServiceBands != null)
            {
                _dataServiceBands.Close();
            }

            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Close();
            }

            if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Close();
            }
        }

        private static String GetFolder(String folder, Int32 reverseLevel)
        {
            // ReverseLevel is the folder Level started counting from the last subdirectory
            // Path = "Z:\Music\Hardrock\Danzig\1988 Danzig"
            // 1998 Danzig is Level 1
            // Danzig is Level 2
            // Hardrock is Level 3

            if ((reverseLevel > 10) || (reverseLevel < 0)) { return null; }
            int[] PosSeparator = new int[10];
            int CountSeparator = 0;

            try
            {
                // Identify position of the folder separators
                for (int i = folder.Length; i > 1; i--)
                {
                    if (folder[i - 1] == Path.DirectorySeparatorChar)
                    {
                        PosSeparator[CountSeparator] = i;
                        CountSeparator++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }


            // Extract the substring between two separartors
            reverseLevel--;
            if (reverseLevel > 0)
            {
                if ((PosSeparator[reverseLevel] != 0) && (PosSeparator[reverseLevel - 1] != 0))
                {
                    int len = PosSeparator[reverseLevel - 1] - PosSeparator[reverseLevel] - 1;
                    return folder.Substring(PosSeparator[reverseLevel], len);
                }
            }
            else
            {
                int len = folder.Length - PosSeparator[reverseLevel];
                string subpath = folder.Substring(PosSeparator[reverseLevel], len);
                return folder.Substring(PosSeparator[reverseLevel], len);
            }

            return "Unknown";
        }
        private static Boolean SongsFromDifferentAlbums(SongItem lastSong, SongItem song)
        {
            if ((lastSong.AlbumName != String.Empty) && (song.AlbumName != String.Empty) && (lastSong.AlbumName != song.AlbumName))
            {
                return true;
            }

            if ((lastSong.AlbumSortName != String.Empty) && (song.AlbumSortName != String.Empty) && (lastSong.AlbumSortName != song.AlbumSortName))
            {
                return true;
            }

            if ((lastSong.BandName != String.Empty) && (song.BandName != String.Empty) && (lastSong.BandName != song.BandName) && (lastSong.ArtistType == ArtistType.SingleArtist))
            {
                return true;
            }

            if (lastSong.Year != song.Year)
            {
                return true;
            }

            if (lastSong.Country != song.Country)
            {
                return true;
            }

            if (lastSong.AlbumGenre != song.AlbumGenre)
            {
                return true;
            }

            if (lastSong.SongPath != song.SongPath)
            {
                return true;
            }

            return false;
        }
        private static Boolean BandCompare(SongItem lastSong, SongItem song)
        {
            if ((lastSong.BandName != String.Empty) && (song.BandName != String.Empty) && (lastSong.BandName != song.BandName))
            {
                return true;
            }

            if ((lastSong.BandSortName != String.Empty) && (song.BandSortName != String.Empty) && (lastSong.BandSortName != song.BandSortName))
            {
                return true;
            }

            return false;
        }
       
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

        ~BackgroundJobHelper()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Dispose();
            }

            if (_dataServiceBands != null)
            {
                _dataServiceBands.Dispose();
            }

            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Dispose();
            }

            if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Dispose();
            }  
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
