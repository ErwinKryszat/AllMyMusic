using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AllMyMusic.DataService
{
    public interface IDataServiceSongs 
    {
        ServerType SelectedServerType { get; }

        Task<int> AddSong(SongItem song);

        Task<SongItem> GetSong(Int32 songID);

        Task<SongItem> GetSongByPath(String songPath);

        Task<ObservableCollection<SongItem>> GetSongs(BandItem band);
       
        Task<ObservableCollection<SongItem>> GetSongs(AlbumItem album);
        
        Task<ObservableCollection<SongItem>> GetSongs(AlbumGenreItem albumGenre);
        
        Task<ObservableCollection<SongItem>> GetSongs(String strSQL);

        Task<ObservableCollection<SongItem>> GetSongs(ObservableCollection<String> playList);

        Task<ObservableCollection<SongItem>> GetSongsByPathPart(String albumPath);

        Task<ObservableCollection<SongItem>> SearchSongs(String searchString);

        Task<ObservableCollection<SongItem>> GetSongsByCondition(String condition);

        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();

        void Dispose();
    }
}