using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AllMyMusic.ViewModel
{
    public class PartyButtonConfigViewModel : ViewModelBase
    {
        #region Fields
        private String _buttonLabel = String.Empty;
        private String _buttonImagePath = String.Empty;
        private String _sqlQuery = String.Empty;
        private String _toolTipText = String.Empty;
        private String _tooltipImagePath = String.Empty;
        
        private Boolean _randomize;
        private bool _isSelected;

        private PartyButtonType _buttonType = PartyButtonType.Undefined;
        private BitmapImage _smallButtonImage;
        private BitmapImage _largeButtonImage;
        private BitmapImage _tooltipImage; 

        private ObservableCollection<AlbumItem> _albumList;
        private ObservableCollection<String> _albumPathNames;
        private ObservableCollection<SongItem> _playlist;
        private ObservableCollection<String> _songPathNames;

        private RelayCommand<object> _playPlaylistCommand;
        #endregion // Fields

        #region Commands
        public ICommand PlayPlaylistCommand
        {
            get
            {
                if (null == _playPlaylistCommand)
                    _playPlaylistCommand = new RelayCommand<object>(ExecutePlayPlaylistCommand, CanPlayPlaylistCommand);

                return _playPlaylistCommand;
            }
        }
        private void ExecutePlayPlaylistCommand(object _notUsed)
        {
            PartyButtonConfigEventArgs args = new PartyButtonConfigEventArgs(this);
            OnPartyButton_Click(this, args);
        }
        private bool CanPlayPlaylistCommand(object _notUsed)
        {
            return true;
        }
        #endregion

        #region Presentation Properties
        public String ButtonLabel
        {
            get { return _buttonLabel; }
            set
            {
                if (value == _buttonLabel)
                    return;

                _buttonLabel = value;
                RaisePropertyChanged("ButtonLabel");
            }
        }   
        public String ButtonImagePath
        {
            get { return _buttonImagePath; }
            set
            {
                if (value == _buttonImagePath)
                    return;

                _buttonImagePath = value;
                RaisePropertyChanged("ButtonImagePath");
            }
        } 
        public Boolean Randomize
        {
            get { return _randomize; }
            set
            {
                if (value == _randomize)
                    return;

                _randomize = value;
                RaisePropertyChanged("Randomize");

                //if (this.playlist != null)
                //{
                //    this.playlist.RandomPlaylist = this._randomize;
                //}
            }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                RaisePropertyChanged("IsSelected");
            }
        }
        public PartyButtonType ButtonType
        {
            get { return _buttonType; }
            set
            {
                if (value == _buttonType)
                    return;

                _buttonType = value;
                RaisePropertyChanged("ButtonType");
            }
        }
        public String SqlQuery
        {
            get { return _sqlQuery; }
            set
            {
                if (value == _sqlQuery)
                    return;

                _sqlQuery = value;
                RaisePropertyChanged("SqlQuery");
            }
        }
        public String ToolTipText
        {
            get { return _toolTipText; }
            set
            {
                if (value == _toolTipText)
                    return;

                _toolTipText = value;
                RaisePropertyChanged("TooltipText");
            }
        }
        public String TooltipImagePath
        {
            get { return _tooltipImagePath; }
            set
            {
                if (value == _tooltipImagePath)
                    return;

                _tooltipImagePath = value;
                RaisePropertyChanged("TooltipImagePath");
            }
        } 
        public ObservableCollection<AlbumItem> AlbumList
        {
            get { return _albumList; }
            set
            {
                if (value == _albumList)
                    return;

                _albumList = value;
                RaisePropertyChanged("AlbumList");
            }
        }
        public ObservableCollection<String> AlbumPathNames
        {
            get { return _albumPathNames; }
            set
            {
                if (value == _albumPathNames)
                    return;

                _albumPathNames = value;
                RaisePropertyChanged("AlbumNames");
            }
        }
        public ObservableCollection<SongItem> Playlist
        {
            get { return _playlist; }
            set
            {
                if (value == _playlist)
                    return;

                _playlist = value;
                RaisePropertyChanged("Playlist");
            }
        }
        public ObservableCollection<String> SongPathNames
        {
            get { return _songPathNames; }
            set
            {
                if (value == _songPathNames)
                    return;

                _songPathNames = value;
                RaisePropertyChanged("SongTitles");
            }
        }

        public BitmapImage SmallButtonImage
        {
            get { return _smallButtonImage; }
            set
            {
                if (value == _smallButtonImage)
                    return;

                _smallButtonImage = value;
                RaisePropertyChanged("SmallButtonImage");
            }
        }
        public BitmapImage LargeButtonImage
        {
            get { return _largeButtonImage; }
            set
            {
                if (value == _largeButtonImage)
                    return;

                _largeButtonImage = value;
                RaisePropertyChanged("LargeButtonImage");
            }
        }
        public BitmapImage TooltipImage
        {
            get { return _tooltipImage; }
            set
            {
                if (value == _tooltipImage)
                    return;

                _tooltipImage = value;
                RaisePropertyChanged("TooltipImage");
            }
        }
        #endregion

        #region AlbumPathNames
        public void AddAllAlbums(ObservableCollection<AlbumItem> allAlbums)
        {
            for (int i = 0; i < allAlbums.Count; i++)
            {
                _albumPathNames.Add(allAlbums[i].AlbumPath);
            }
            RaisePropertyChanged("AlbumNames");
        }

        public void AddSelectedAlbums(ObservableCollection<AlbumItem> selectedAlbums)
        {
            for (int i = 0; i < selectedAlbums.Count; i++)
            {
                _albumPathNames.Add(selectedAlbums[i].AlbumPath);
            }
            RaisePropertyChanged("AlbumNames");
        }

        public void RemoveSelectedAlbums(ObservableCollection<AlbumItem> selectedAlbums)
        {
            for (int i = 0; i < selectedAlbums.Count; i++)
            {
                _albumPathNames.Remove(selectedAlbums[i].AlbumPath);
            }
            RaisePropertyChanged("AlbumNames");
        }

        public void RemoveAllAlbums()
        {
            _albumPathNames.Clear();
            RaisePropertyChanged("AlbumNames");
        }
        #endregion

        #region SongPathNames
        public void AddAllSongs(ObservableCollection<SongItem> allSongs)
        {
            for (int i = 0; i < allSongs.Count; i++)
            {
                _songPathNames.Add(allSongs[i].SongFullPath);
            }
            RaisePropertyChanged("SongTitles");
        }

        public void AddSelectedSongs(IList selectedSongs)
        {
            for (int i = 0; i < selectedSongs.Count; i++)
            {
                _songPathNames.Add(((SongItem)selectedSongs[i]).SongFullPath);
            }
            RaisePropertyChanged("SongTitles");
        }

        public void RemoveSelectedSongs(IList selectedSongs)
        {
            for (int i = 0; i < selectedSongs.Count; i++)
            {
                _songPathNames.Remove(((SongItem)selectedSongs[i]).SongFullPath);
            }
            RaisePropertyChanged("SongTitles");
        }

        public void RemoveAllSongs()
        {
            _songPathNames.Clear();
            RaisePropertyChanged("SongTitles");
        }
        #endregion

        #region Events
        public delegate void PartyButton_ClickEventHandler(object sender, PartyButtonConfigEventArgs e);
        public event PartyButton_ClickEventHandler PartyButton_Click;
        protected virtual void OnPartyButton_Click(object sender, PartyButtonConfigEventArgs e)
        {
            if (this.PartyButton_Click != null)
            {
                this.PartyButton_Click(this, e);
            }
        }
        #endregion
    }
}
