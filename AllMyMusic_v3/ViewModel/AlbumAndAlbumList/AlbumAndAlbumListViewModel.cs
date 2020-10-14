using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

//using AllMyMusic_v3.AllSql;
using AllMyMusic_v3.Settings;

namespace AllMyMusic_v3.ViewModel
{
    public class AlbumAndAlbumListViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private AlbumViewModel _vmAlbum;
        private AlbumListViewModel _vmAlbumList;   
        private Boolean _albumViewVisible;
        private Boolean _albumListViewExpanded;
        #endregion // Fields

        #region Presentation Properties
        public AlbumViewModel AlbumViewModel
        {
            get
            {
                return _vmAlbum;
            }
            set
            {
                if (_vmAlbum == value)
                    return;

                _vmAlbum = value;
                RaisePropertyChanged("AlbumViewModel");
            }
        }
        public AlbumListViewModel AlbumListViewModel
        {
            get
            {
                return _vmAlbumList;
            }
            set
            {
                if (_vmAlbumList == value)
                    return;
                _vmAlbumList = value;
                RaisePropertyChanged("AlbumListViewModel");
            }
        }
        public Boolean AlbumListViewExpanded
        {
            get { return _albumListViewExpanded; }
            set
            {
                if (value == _albumListViewExpanded)
                    return;

                _albumListViewExpanded = value;

                RaisePropertyChanged("AlbumListViewExpanded");
            }
        }
        public Boolean AlbumViewVisible
        {
            get { return _albumViewVisible; }
            set
            {
                _albumViewVisible = value;

                RaisePropertyChanged("AlbumViewVisible");
            }
        }
       
        #endregion // Presentation Properties

        #region Constructor
        public AlbumAndAlbumListViewModel(ConnectionInfo conInfo)
        {
            _albumListViewExpanded = true;

            Create_vmAlbumList(conInfo);
            Create_vmAlbum(conInfo);
        }
        private void Create_vmAlbumList(ConnectionInfo conInfo)
        {
            _vmAlbumList = new AlbumListViewModel(conInfo);
            _vmAlbumList.AlbumItemSelected += new AlbumListViewModel.AlbumItemSelectedEventHandler(_vmAlbumList_AlbumItemSelected);
            _vmAlbumList.ManageCoverImagesRequested += new AlbumListViewModel.ManageCoverImagesRequestedEventHandler(_vmAlbumList_ManageCoverImagesRequested);
            _vmAlbumList.PropertiesChangedRequested += new AlbumListViewModel.PropertiesChangedRequestedEventHandler(PropertiesChanged);
            _vmAlbumList.AlbumListChanged += new AlbumListViewModel.AlbumListChangedEventHandler(_vmAlbumList_AlbumListChanged);
            _vmAlbumList.PlaylistChanged += new AlbumListViewModel.PlaylistChangedEventHandler(OnPlaylistChanged);
            _vmAlbumList.StampImageSize = AppSettings.GeneralSettings.StampImageSize;
            _vmAlbumList.SleeveImageSize = AppSettings.GeneralSettings.SleeveImageSize;
            _vmAlbumList.WikipediaLanguage = AppSettings.GeneralSettings.WikipediaLanguage;
            _vmAlbumList.AlbumListViewWidth = AppSettings.FormSettings.FrmMain_AlbumListViewWidth;
            _vmAlbumList.SelectionMode = System.Windows.Controls.SelectionMode.Single;
        }
        private void Create_vmAlbum(ConnectionInfo conInfo)
        {
            _vmAlbum = new AlbumViewModel(conInfo);
            _vmAlbum.PlaylistChanged += new AlbumViewModel.PlaylistChangedEventHandler(OnPlaylistChanged);
            _vmAlbum.PropertiesChangedRequested += new AlbumViewModel.PropertiesChangedRequestedEventHandler(PropertiesChanged);
            _vmAlbum.CoverImageChanged += new ViewModel.AlbumViewModel.CoverImageChangedEventHandler(_vmAlbum_CoverImageChanged);
            _vmAlbum.StampImageSize = AppSettings.GeneralSettings.StampImageSize;
            _vmAlbum.SleeveImageSize = AppSettings.GeneralSettings.SleeveImageSize;
            _vmAlbum.CountryFlagHeight = AppSettings.GeneralSettings.CountryFlagHeight;
            _vmAlbum.WikipediaLanguage = AppSettings.GeneralSettings.WikipediaLanguage;
        }
        #endregion  // Constructor

        #region public
        public async Task GetAlbumList(BandItem band)
        {
            await _vmAlbumList.GetAlbumList(band);

            if (_vmAlbum.Album == null)
            {
                AlbumViewVisible = false;
            }
            else
            {
                AlbumViewVisible = true;
            }
        }
        public async Task GetAlbumList(String searchText)
        {
            await _vmAlbumList.GetAlbumList(searchText);

            if (_vmAlbum.Album == null)
            {
                AlbumViewVisible = false;
            }
            else
            {
                AlbumViewVisible = true;
            }
        }
        public async Task GetAlbumList(AlbumGenreItem albumGenre)
        {
            await _vmAlbumList.GetAlbumList(albumGenre);

            if (_vmAlbum.Album == null)
            {
                AlbumViewVisible = false;
            }
            else
            {
                AlbumViewVisible = true;
            }
        }
        public async Task RefreshView()
        {
            await _vmAlbumList.RefreshView();
            await _vmAlbum.RefreshView();
        }
        public void ClearView()
        {
            _vmAlbumList.ClearView();
            _vmAlbum.ClearView();
        }
        public void Close()
        {
            AppSettings.FormSettings.FrmMain_AlbumListViewWidth = _vmAlbumList.AlbumListViewWidth;

            if (_vmAlbum != null)
            {
                _vmAlbum.Close();
            }

            if (_vmAlbumList != null)
            {
                _vmAlbumList.Close();
            }
        }
        public void AskOverwritePlaylist(CommandSourceViewModel cmdSource)
        {
            if (cmdSource == CommandSourceViewModel.AlbumViewModel)
            {
                _vmAlbum.AskOverwritePlaylist();
            }
            else
            {
                _vmAlbumList.AskOverwritePlaylist();
            }
        }
        public void NotifyFileAccessDenied(CommandSourceViewModel cmdSource, String fullFileName)
        {
            if (cmdSource == CommandSourceViewModel.AlbumViewModel)
            {
                _vmAlbum.NotifyFileAccessDenied(fullFileName);
            }
            else
            {
                _vmAlbumList.NotifyFileAccessDenied(fullFileName);
            }
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            _vmAlbum.ChangeDatabase(conInfo);
            _vmAlbumList.ChangeDatabase(conInfo);
            AlbumViewVisible = false;
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            _vmAlbum.ChangeDatabaseService(conInfo);
            _vmAlbumList.ChangeDatabaseService(conInfo);
            AlbumViewVisible = false;
        }
        public void SetWikipediaLanguage(String wikipediaLanguage)
        {
            _vmAlbum.WikipediaLanguage = wikipediaLanguage;
            _vmAlbumList.WikipediaLanguage = wikipediaLanguage;
        }
        #endregion  // public

        #region private helper
        private void _vmAlbumList_AlbumItemSelected(object sender, AlbumItemEventArgs e)
        {
            _vmAlbum.Album = e.Album;

            if (_vmAlbumList.BandNameVisbility == true)
            {
                _vmAlbum.HeadlineAlbum = e.Album.Headline;
            }
            else
            {
                _vmAlbum.HeadlineAlbum = e.Album.AlbumName;
            }
            AlbumViewVisible = true;
        }
        private void _vmAlbumList_ManageCoverImagesRequested(object sender, AlbumListEventArgs e)
        {
            OnManageCoverImagesRequested(this, e);
        }
        private void PropertiesChanged(object sender, ChangedPropertiesListEventArgs e)
        {
            OnPropertiesChangedRequested(sender, e);
        }
        private void _vmAlbum_AlbumChanged(object sender, AlbumItemEventArgs e)
        {
            
        }
        private void _vmAlbum_CoverImageChanged(object sender, AlbumItemEventArgs e)
        {
            Task.Run(() => _vmAlbumList.RefreshView());
        }
        private void _vmAlbumList_AlbumListChanged(object sender, AlbumListEventArgs e)
        {
            if (e.AlbumList ==null)
	        {
                AlbumListViewExpanded = false;
	        }
        }
        #endregion // private helper


        #region Events
        public delegate void PlaylistChangedEventHandler(object sender, PlaylistEventArgs e);
        public event PlaylistChangedEventHandler PlaylistChanged;
        protected virtual void OnPlaylistChanged(object sender, PlaylistEventArgs e)
        {
            if (this.PlaylistChanged != null)
            {
                this.PlaylistChanged(sender, e);
            }
        }

        public delegate void ManageCoverImagesRequestedEventHandler(object sender, AlbumListEventArgs e);
        public event ManageCoverImagesRequestedEventHandler ManageCoverImagesRequested;
        protected virtual void OnManageCoverImagesRequested(object sender, AlbumListEventArgs e)
        {
            if (this.ManageCoverImagesRequested != null)
            {
                this.ManageCoverImagesRequested(this, e);
            }
        }

        public delegate void PropertiesChangedRequestedEventHandler(object sender, ChangedPropertiesListEventArgs e);
        public event PropertiesChangedRequestedEventHandler PropertiesChangedRequested;
        protected virtual void OnPropertiesChangedRequested(object sender, ChangedPropertiesListEventArgs e)
        {
            if (this.PropertiesChangedRequested != null)
            {
                this.PropertiesChangedRequested(sender, e);
            }
        }       
        #endregion // Events

        #region Localization
        public void Localize()
        {
            _vmAlbum.Localize();
            _vmAlbumList.Localize();
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

        ~AlbumAndAlbumListViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_vmAlbum != null)
            {
                _vmAlbum.Dispose();
            }

            if (_vmAlbumList != null)
            {
                _vmAlbumList.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
