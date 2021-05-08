using System;
using System.Collections.ObjectModel;
using System.Data;

using AllMyMusic.QueryBuilder;
using AllMyMusic.Settings;

using System.Threading;
using System.Threading.Tasks;

namespace AllMyMusic.DataService
{
    public interface IDataServiceBands 
    {
        ServerType SelectedServerType { get; }
        
        Task<Int32> AddBand(BandItem band);

        Task<ObservableCollection<BandItem>> GetBandsByAlphabet(String firstCharacter);

        Task<ObservableCollection<BandItem>> SearchBands(String searchText);

        Task<BandItem> GetBand(Int32 bandID);

        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();

        void Dispose();
    }
}