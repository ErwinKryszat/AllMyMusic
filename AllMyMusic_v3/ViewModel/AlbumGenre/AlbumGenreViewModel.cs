
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AllMyMusic.DataService;

namespace AllMyMusic.ViewModel
{
    public class AlbumGenreViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ObservableCollection<AlbumGenreItem> _albumGenreCollection;
        private AlbumGenreItem _selectedAlbumGenre;
        private IDataServiceAlbumGenre _dataServiceAlbumGenre;
        #endregion // Fields

        #region Commands
  
        #endregion // Commands

        #region Presentation Properties
        public ObservableCollection<AlbumGenreItem> AlbumGenreCollection
        {
            get { return _albumGenreCollection; }
            set
            {
                if (value == _albumGenreCollection)
                    return;

                _albumGenreCollection = value;

                RaisePropertyChanged("AlbumGenreCollection");
            }
        }
        public AlbumGenreItem SelectedAlbumGenre
        {
            get { return _selectedAlbumGenre; }
            set
            {
                if (value == _selectedAlbumGenre)
                    return;

                _selectedAlbumGenre = value;

                AlbumGenreEventArgs args = new AlbumGenreEventArgs(value);
                OnAlbumGenreSelected(this, args);

                RaisePropertyChanged("SelectedAlbumGenre");
            }
        }
        #endregion

        #region Constructor
        public AlbumGenreViewModel(ConnectionInfo conInfo)
        {
            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceAlbumGenre = new DataServiceAlbumGenre_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceAlbumGenre = new DataServiceAlbumGenre_MYSQL(conInfo);
            }
        }
        #endregion

        #region public
        public async Task GetAlbumGenreList()
        {
            AlbumGenreCollection = await _dataServiceAlbumGenre.GetAlbumGenres();
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            _dataServiceAlbumGenre.ChangeDatabase(conInfo);
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            if (_dataServiceAlbumGenre != null)
            {
                _dataServiceAlbumGenre.Dispose();
            }

            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceAlbumGenre = new DataServiceAlbumGenre_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceAlbumGenre = new DataServiceAlbumGenre_MYSQL(conInfo);
            }
        }

        public void Close()
        {
            if (_dataServiceAlbumGenre != null)
            {
                _dataServiceAlbumGenre.Close();
            }
        }
        #endregion

        #region Events
        public delegate void AlbumGenreSelectedEventHandler(object sender, AlbumGenreEventArgs e);

        public event AlbumGenreSelectedEventHandler AlbumGenreSelected;
        protected virtual void OnAlbumGenreSelected(object sender, AlbumGenreEventArgs e)
        {
            if (this.AlbumGenreSelected != null)
            {
                this.AlbumGenreSelected(this, e);
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

        ~AlbumGenreViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_dataServiceAlbumGenre != null)
            {
                _dataServiceAlbumGenre.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
