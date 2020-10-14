using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

using AllMyMusic_v3.DataService;
using AllMyMusic_v3.Settings;

namespace AllMyMusic_v3.ViewModel
{
    

    public class BandListViewModel : ViewModelBase, IDisposable
    {
        public enum BandQueryType
        {
            Alphabet,
            SearchName
        }

        #region Fields
        private ObservableCollection<BandItem> _bandItemCollection;
        private BandItem _selectedBandItem;
        private IDataServiceBands _dataServiceBands;
        private String _firstCharacter;
        private String _lastQueryItem;
        private BandQueryType _bandQueryType;
        private double _bandListViewWidth;
        #endregion // Fields

        #region Commands
  
        #endregion // Commands

        #region Presentation Properties
        public ObservableCollection<BandItem> BandItemCollection
        {
            get { return _bandItemCollection; }
            set
            {
                if (value == _bandItemCollection)
                    return;

                _bandItemCollection = value;

                RaisePropertyChanged("BandItemCollection");
            }
        }
        public BandItem SelectedBandItem
        {
            get { return _selectedBandItem; }
            set
            {
                if (value == _selectedBandItem)
                    return;

                _selectedBandItem = value;

                BandItemEventArgs args = new BandItemEventArgs(value);
                OnBandItemSelected(this, args);

                RaisePropertyChanged("SelectedBandItem");
            }
        }
        // Binding Mode.ToWay required for this property to function
        public double BandListViewWidth
        {
            get { return _bandListViewWidth; }
            set
            {
                if (value == _bandListViewWidth)
                    return;

                _bandListViewWidth = value;

                RaisePropertyChanged("BandListViewWidth");
            }
        }
        #endregion

        #region Constructor
        public BandListViewModel(ConnectionInfo conInfo)
        {
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.SqlServer)
            {
                _dataServiceBands = new DataServiceBands_SQL(conInfo);
            }
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.MySql)
            {
                _dataServiceBands = new DataServiceBands_MYSQL(conInfo);
            }
        }
        public void Close()
        {
            if (_dataServiceBands != null)
            {
                _dataServiceBands.Close();
            }
        }
        #endregion

        #region public
        public async Task GetBandList(String firstCharacter)
        {
            _firstCharacter = firstCharacter;
            if ((_dataServiceBands != null) && (String.IsNullOrEmpty(firstCharacter) == false))
            {
                BandItemCollection = await _dataServiceBands.GetBandsByAlphabet(firstCharacter);
            }

            _lastQueryItem = firstCharacter;
            _bandQueryType = BandQueryType.Alphabet;
        }

        public async Task SearchBands(String searchText)
        {
            if ((_dataServiceBands != null) && (String.IsNullOrEmpty(searchText) == false))
            {
                BandItemCollection = await _dataServiceBands.SearchBands(searchText);
            }

            _lastQueryItem = searchText;
            _bandQueryType = BandQueryType.SearchName;
        }
        public async Task RefreshView()
        {
            switch (_bandQueryType)
            {
                case BandQueryType.Alphabet:
                    await GetBandList(_lastQueryItem);
                    break;

                case BandQueryType.SearchName:
                    await SearchBands(_lastQueryItem);
                    break;

                default:
                    break;
            }
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            ClearView();
            _dataServiceBands.ChangeDatabase(conInfo);
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            ClearView();

            if (_dataServiceBands != null)
            {
                _dataServiceBands.Dispose();
            }

            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.SqlServer)
            {
                _dataServiceBands = new DataServiceBands_SQL(conInfo);
            }
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.MySql)
            {
                _dataServiceBands = new DataServiceBands_MYSQL(conInfo);
            }
        }
        public void ClearView()
        {
            SelectedBandItem = null;
            BandItemCollection = new ObservableCollection<BandItem>();
        }
        #endregion


        #region Events
        public delegate void BandItemSelectedEventHandler(object sender, BandItemEventArgs e);

        public event BandItemSelectedEventHandler BandItemSelected;
        protected virtual void OnBandItemSelected(object sender, BandItemEventArgs e)
        {
            if (this.BandItemSelected != null)
            {
                this.BandItemSelected(this, e);
            }
        }

        #endregion // Events

        #region Localization

        public void Localize()
        {

        }
        #endregion // Localization

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

        ~BandListViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_dataServiceBands != null)
            {
                _dataServiceBands.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
