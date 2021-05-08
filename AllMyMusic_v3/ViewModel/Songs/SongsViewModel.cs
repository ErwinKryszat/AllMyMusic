using AllMyMusic.View;

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading.Tasks;

using AllMyMusic.DataService;



namespace AllMyMusic.ViewModel
{
    public class SongsViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private IDataServiceSongs _dataServiceSongs;
        private ObservableCollection<SongItem> _songs;
        private IList _selectedSongs = new ArrayList();
        private String _headline;
        private String _searchText;
        private SongItem _changedSong;
        private double _songListViewWidth;
        private Boolean _bandColumnVisible;
        #endregion // Fields

        #region Commands 
       
        #endregion // Command


        #region Presentation Properties
        public ObservableCollection<SongItem> Songs
        {
            get { return _songs; }
            set
            {
                if (value == _songs)
                    return;

                _songs = value;

                RaisePropertyChanged("Songs");
            }
        }
        public IList SelectedSongs
        {
            get { return _selectedSongs; }
            set
            {
                if (value == _selectedSongs)
                    return;

                _selectedSongs = value;
                RaisePropertyChanged("SelectedSongs");
            }
        }

        public SongItem ChangedSong
        {
            get { return _changedSong; }
            set
            {
                if (value == _changedSong)
                    return;

                _changedSong = value;

                RaisePropertyChanged("ChangedSong");
            }
        }

        public String Headline
        {
            get { return _headline; }
            set
            {
                if (value == _headline)
                    return;

                _headline = value;

                RaisePropertyChanged("Headline");
            }
        }
        // Binding Mode.ToWay required for this property to function
        public double SongListViewWidth
        {
            get { return _songListViewWidth; }
            set
            {
                if (value == _songListViewWidth)
                    return;

                _songListViewWidth = value;

                RaisePropertyChanged("SongListViewWidth");
            }
        }
        public Boolean BandColumnVisible
        {
            get { return _bandColumnVisible; }
            set
            {
                _bandColumnVisible = value;

                RaisePropertyChanged("BandColumnVisible");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public SongsViewModel(ConnectionInfo conInfo)
        {
            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }
            _songs = new ObservableCollection<SongItem>();

            _songListViewWidth = 300;

            Localize();
        }
        #endregion  // Constructor


        #region public
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            _dataServiceSongs.ChangeDatabase(conInfo);
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }
        }

        public void Close()
        {
            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Close();
            }
        }
        public async Task GetSongs(BandItem band)
        {
            _headline = "Band: " + band.BandName;
            RaisePropertyChanged("Headline");

            Songs = await _dataServiceSongs.GetSongs(band);
        }
        public async Task GetSongs(AlbumItem album)
        {
            _headline = "Album: " + album.AlbumName;
            RaisePropertyChanged("Headline");

            Songs = await _dataServiceSongs.GetSongs(album);

            if (Songs[0].ArtistType == ArtistType.SingleArtist)
            {
                BandColumnVisible = false;
            }
            else
            {
                BandColumnVisible = true;
            }
        }
        public async Task GetSongs(AlbumItem album, String searchText)
        {
            _searchText = searchText;
            _headline = "Album: " + album.AlbumName;
            RaisePropertyChanged("Headline");

            Songs = await _dataServiceSongs.GetSongs(album);
        }
        public async Task GetSongs(String _strSongsQuery)
        {
            _headline = String.Empty;
            RaisePropertyChanged("Headline");

            Songs = await _dataServiceSongs.GetSongs(_strSongsQuery);
        }
        public ObservableCollection<SongItem> GetSelectedOrAllSongs()
        {
            if ((_selectedSongs != null) && (_selectedSongs.Count >0))
            {
                ObservableCollection<SongItem> _selectedSongs2 = new ObservableCollection<SongItem>();

                for (int i = 0; i < _selectedSongs.Count; i++)
                {
                    _selectedSongs2.Add((SongItem)_selectedSongs[i]);
                }
                return _selectedSongs2;
            }
            else
            {
                return _songs;
            }
        }

        public void AddSongs(IList newSongs)
        {
            for (int i = 0; i < newSongs.Count; i++)
            {
                _songs.Add((SongItem)newSongs[i]);
            }
            OnSongsChanged();
        }
        public void RemoveSongs(IList removeSongs)
        {
            for (int i = 0; i < removeSongs.Count; i++)
            {
                _songs.Remove((SongItem)removeSongs[i]);
            }
            OnSongsChanged();
        }
        public void RemoveAllSongs()
        {
            _songs.Clear();
            OnSongsChanged();
        }
        public void UnselectAll()
        {
            for (int i = 0; i < _songs.Count; i++)
            {
                _songs[i].IsSelected = false;
            }
        }
        public void MoveSelectedSongsUp()
        {
            for (int i = 0; i < _selectedSongs.Count; i++)
            {
                Int32 selectedIndex = GetItemIndex((SongItem)_selectedSongs[i]);
                MoveItem(-1, selectedIndex);
            }
        }
        public void MoveSelectedSongsDown()
        {
            for (int i = 0; i < _selectedSongs.Count; i++)
            {
                Int32 selectedIndex = GetItemIndex((SongItem)_selectedSongs[i]);
                MoveItem(1, selectedIndex);
            }
        }
        private Int32 GetItemIndex(SongItem ai)
        {
            for (int i = 0; i < _songs.Count; i++)
            {
                if (_songs[i].SongId == ai.SongId)
                {
                    return i;
                }
            }
            return -1;
        }
        private void MoveItem(int direction, int selectedIndex)
        {
            // Calculate new index using move direction
            int newIndex = selectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= _songs.Count)
                return; // Index out of range - nothing to do

            SongItem selectedSong = _songs[selectedIndex];

            // Removing removable element
            _songs.RemoveAt(selectedIndex);

            // Insert it in new position
            _songs.Insert(newIndex, selectedSong);

            OnSongsChanged();
        }

        #endregion  // public

        #region private helper
        private void OnSongsChanged()
        {
            RaisePropertyChanged("Songs");
        }

        #endregion // private helper


        #region Localization
        private String _dadaGridToolTip = "Reorder columns with Drag and Drop";

       
        public String DadaGridToolTip
        {
            get { return _dadaGridToolTip; }
            set { _dadaGridToolTip = value; }
        }


        public void Localize()
        {
            DadaGridToolTip = AmmLocalization.GetLocalizedString("frmTools_DadaGridToolTip");
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

        ~SongsViewModel()
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
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
