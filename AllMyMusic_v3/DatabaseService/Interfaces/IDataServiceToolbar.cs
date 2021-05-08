using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AllMyMusic.ViewModel;

namespace AllMyMusic
{
    public interface IDataServiceToolbar
    {
        ServerType SelectedServerType { get; }

        Task<ObservableCollection<AlphabetItem>> GetAlphabetButtons();

        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();

        void Dispose();
    }
}
