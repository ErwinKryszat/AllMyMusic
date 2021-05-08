using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace AllMyMusic.DataService
{
    public interface IDataServiceAlbumGenre 
    {
        ServerType SelectedServerType { get; }

        Task<ObservableCollection<AlbumGenreItem>> GetAlbumGenres();

        Task<Int32> AddAlbumGenre(AlbumGenreItem AlbumGenre);

        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();

        void Dispose();
    }
}