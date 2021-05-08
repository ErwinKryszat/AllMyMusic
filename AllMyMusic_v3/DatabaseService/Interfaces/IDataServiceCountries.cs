using System;
using System.Collections.ObjectModel;
using System.Data;

using AllMyMusic.QueryBuilder;
using AllMyMusic.Settings;

using System.Threading;
using System.Threading.Tasks;

namespace AllMyMusic.DataService
{
    public interface IDataServiceCountries 
    {
        ServerType SelectedServerType { get; }

        Task<CountryItem> GetCountry(Int32 countryId);

        Task<CountryItem> GetCountry(String countryName);

        Task<ObservableCollection<CountryItem>> GetCountries();

        Task<Int32> AddCountry(CountryItem country);

        Task UpdateCountries(ObservableCollection<CountryItem> countries);

        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();

        void Dispose();
    }
}