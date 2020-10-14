using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AllMyMusic_v3.DataService
{
    interface IDataServiceAlbums 
    {
        ServerType SelectedServerType { get; }
        String VariousArtists { get; }
        Int32 VariousArtistID { get; }


        Task<Int32> AddAlbum(AlbumItem album);

        Task AddImage(AlbumItem album);

        Task<AlbumItem> GetAlbumByPath(String albumPath);

        Task<ObservableCollection<AlbumItem>> GetAlbums(BandItem band);

        Task<ObservableCollection<AlbumItem>> GetAlbums(AlbumGenreItem albumGenre);

        Task<ObservableCollection<AlbumItem>> GetAlbums(ComposerItem composer);

        Task<ObservableCollection<AlbumItem>> GetAlbums(ConductorItem conductor);

        Task<ObservableCollection<AlbumItem>> GetAlbums(CountryItem country);

        Task<ObservableCollection<AlbumItem>> GetAlbums(GenreItem genre);

        Task<ObservableCollection<AlbumItem>> GetAlbums(LanguageItem language);

        Task<ObservableCollection<AlbumItem>> GetAlbumsByPath(String albumFolderPath);

        Task<ObservableCollection<AlbumItem>> GetAlbums(LeadPerformerItem leadPerformer);

        Task<ObservableCollection<AlbumItem>> GetAlbums(ObservableCollection<String> playList);

        Task<ObservableCollection<AlbumItem>> SearchAlbums(String searchString);

        Task<ObservableCollection<AlbumItem>> GetAlbumsByCondition(String condition);

        Task<AlbumItem> GetAlbum(Int32 albumID);

        Task<ObservableCollection<AlbumItem>> GetAlbums(String strSQL);

        Task DeleteAlbum(AlbumItem album);

        Task DeleteAlbumGenre(AlbumGenreItem albumGenre);

        Task DeleteBand(BandItem band);

        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();

        void Dispose();
    }
}