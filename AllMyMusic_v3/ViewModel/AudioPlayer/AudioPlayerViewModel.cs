using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

using System.IO;
using AllMyMusic.DataService;
using AllMyMusic.Controls;
using AllMyMusic.Settings;

using Metadata.Mp3;
//using Metadata.ID3;

namespace AllMyMusic.ViewModel
{
    public class AudioPlayerViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private AmmAudioPlayback _audioPlayback;
        private PlaylistViewModel _vmPlayList;
        private SongItem _song;
        private CountryItem _country;
        private MessageBoxViewModel _messageBoxViewModel;
        private float _volume = -1.0f;

        private System.Timers.Timer _timer;

        //private System.Windows.Forms.Timer _timer;
        private Boolean _sliderIsDragging;
        private Boolean _stopped;
        private Boolean _playbackPaused;
        private IDataServiceCountries _dataServiceCountries;
        private IDataServiceSongs _dataServiceSongs;
        private IVisualizationPlugin selectedVisualization;

        private String _currentTime;
        private Int32 _rating;


        private String _fileName = String.Empty;

        private RelayCommand<SongItem> _coverImageCommand;
        private RelayCommand<object> _playCommand;
        private RelayCommand<object> _pauseCommand;
        private RelayCommand<object> _stopCommand;
        private RelayCommand<object> _nextSongCommand;
        private RelayCommand<object> _previousSongCommand;
        private RelayCommand<Boolean> _randomizeCommand;
        #endregion // Fields

        #region Commands
        public ICommand CoverImageCommand
        {
            get
            {
                if (null == _coverImageCommand)
                    _coverImageCommand = new RelayCommand<SongItem>(ExecuteCoverImageCommand, CanCoverImageCommand);

                return _coverImageCommand;
            }
        }
        private void ExecuteCoverImageCommand(SongItem song)
        {
            frmCoverImage frmCover = new frmCoverImage(song);
            frmCover.Show();
        }
        private bool CanCoverImageCommand(SongItem song)
        {
            if ((song != null) && (String.IsNullOrEmpty(song.FrontImageFullpath) == false))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public ICommand PlayCommand
        {
            get
            {
                if (null == _playCommand)
                    _playCommand = new RelayCommand<object>(ExecutePlayCommand, CanPlayCommand);

                return _playCommand;
            }
        }
        private void ExecutePlayCommand(object notUsed)
        {
            PlaySong();
            //_audioPlayback.Play();
            //PlaybackPaused = false;
        }
        private bool CanPlayCommand(object notUsed)
        {
            return (_song != null);
        }

        public ICommand PauseCommand
        {
            get
            {
                if (null == _pauseCommand)
                    _pauseCommand = new RelayCommand<object>(ExecutePauseCommand, CanPauseCommand);

                return _pauseCommand;
            }
        }
        private void ExecutePauseCommand(object notUsed)
        {
            _audioPlayback.Pause();
        }
        private bool CanPauseCommand(object notUsed)
        {
            return (_song != null);
        }

        public ICommand StopCommand
        {
            get
            {
                if (null == _stopCommand)
                    _stopCommand = new RelayCommand<object>(ExecuteStopCommand, CanStopCommand);

                return _stopCommand;
            }
        }
        private void ExecuteStopCommand(object notUsed)
        {
            StopPlayback();
            //_stopped = true;
            //_audioPlayback.Stop();
        }
        private bool CanStopCommand(object notUsed)
        {
            return (_song != null);
        }

        public ICommand NextSongCommand
        {
            get
            {
                if (null == _nextSongCommand)
                    _nextSongCommand = new RelayCommand<object>(ExecuteNextSongCommand, CanNextSongCommand);

                return _nextSongCommand;
            }
        }
        private void ExecuteNextSongCommand(object notUsed)
        {
            PlayNextSong();
        }
        private bool CanNextSongCommand(object notUsed)
        {
            return (_song != null);
        }

        public ICommand PreviousSongCommand
        {
            get
            {
                if (null == _previousSongCommand)
                    _previousSongCommand = new RelayCommand<object>(ExecutePreviousSongCommand, CanPreviousSongCommand);

                return _previousSongCommand;
            }
        }
        private void ExecutePreviousSongCommand(object notUsed)
        {
            PlayPreviousSong();
        }
        private bool CanPreviousSongCommand(object notUsed)
        {
            return (_song != null);
        }

        public ICommand RandomizeCommand
        {
            get
            {
                if (null == _randomizeCommand)
                    _randomizeCommand = new RelayCommand<Boolean>(ExecuteRandomizeCommand, CanRandomizeCommand);

                return _randomizeCommand;
            }
        }
        private void ExecuteRandomizeCommand(Boolean buttonChecked)
        {
            if (_vmPlayList != null)
            {
                _vmPlayList.Randomize = buttonChecked;
            }
        }
        private bool CanRandomizeCommand(Boolean buttonChecked)
        {
            return true;
        }

        #endregion

        #region Presentation Properties
        public AmmAudioPlayback AudioPlayback
        {
            get { return _audioPlayback; }
            set
            {
                _audioPlayback = value;
                RaisePropertyChanged("AudioPlayback");
            }
        }
        public SongItem Song
        {
            get { return _song; }
            set
            {
                if ((value == _song) || (value == null))
                    return;

                _song = value;
                RaisePropertyChanged("Song");
            }
        }
        public ObservableCollection<SongItem> Songs
        {
            get { return _vmPlayList.AllSongItemCollection; }
        }
        public CountryItem Country
        {
            get { return _country; }
            set
            {
                if ((value == _country) || (value == null))
                    return;

                _country = value;
                RaisePropertyChanged("Country");
            }
        }
        public float Volume
        {
            get { return _volume; }
            set
            {
                if (value == _volume)
                    return;

                _volume = value;

                if (_audioPlayback != null)
                {
                    _audioPlayback.Volume = value;
                }

                RaisePropertyChanged("Volume");
            }
        }
        public long StreamPosition
        {
            get 
            {
                if (_audioPlayback != null)
                {
                    return _audioPlayback.StreamPosition;
                }
                return 0; 
            }
            set
            {
                if (value == _audioPlayback.StreamPosition)
                    return;

                _audioPlayback.StreamPosition = value;

                RaisePropertyChanged("StreamPosition");
            }
        }
        public long StreamLength
        {
            get 
            {
                if (_audioPlayback != null)
                {
                    return _audioPlayback.StreamLength;
                }
                return 0; 
            }
        }
        public String CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (value == _currentTime)
                    return;

                _currentTime = value;
                RaisePropertyChanged("CurrentTime");
            }
        }
        public Int32 Rating
        {
            get { return _rating; }
            set
            {
                if (value == _rating)
                    return;

                _rating = value;
                RaisePropertyChanged("Rating");
            }
        }
        public Boolean Randomize
        {
            get { return _vmPlayList.Randomize; }
            set
            {
                _vmPlayList.Randomize = value;
                RaisePropertyChanged("Randomize");
            }
        }
        public Boolean SliderIsDragging
        {
            get { return _sliderIsDragging; }
            set
            {
                if (value == _sliderIsDragging)
                    return;
                _sliderIsDragging = value;

                if (_sliderIsDragging == true)
                {
                    _audioPlayback.Pause();
                }
                else
                {
                    _audioPlayback.Play();
                }

                
                RaisePropertyChanged("SliderIsDragging");
            }
        }
        public Boolean RandomizedPlaylist
        {
            get 
            {
                if (_vmPlayList != null)
                {
                    return _vmPlayList.Randomize;
                }
                return false; 
            }
            set
            {
                if (_vmPlayList != null)
                {
                    if (value == _vmPlayList.Randomize)
                        return;

                    _vmPlayList.Randomize = value;
                    RaisePropertyChanged("RandomizedPlaylist");
                }
            }
        }
        public IVisualizationPlugin SelectedVisualization
        {
            get
            {
                return this.selectedVisualization;
            }
            set
            {
                if (this.selectedVisualization != value)
                {
                    this.selectedVisualization = value;
                    RaisePropertyChanged("SelectedVisualization");
                    RaisePropertyChanged("Visualization");
                }
            }
        }
        public object Visualization
        {
            get
            {
                return this.selectedVisualization.Content;
            }
        }
        public Boolean PlaybackPaused
        {
            get { return _playbackPaused; }
            set
            {
                if (value == _playbackPaused)
                    return;

                _playbackPaused = value;
                RaisePropertyChanged("PlaybackPaused");
            }
        }
        public MessageBoxViewModel MessageBoxViewModel
        {
            get { return _messageBoxViewModel; }
            set
            {
                if (value == _messageBoxViewModel)
                    return;

                _messageBoxViewModel = value;

                RaisePropertyChanged("MessageBoxViewModel");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public AudioPlayerViewModel(ConnectionInfo conInfo, PlaylistViewModel vmPlayList)
        {
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.SqlServer)
            {
                _dataServiceCountries = new DataServiceCountries_SQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.MySql)
            {
                _dataServiceCountries = new DataServiceCountries_MYSQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }

            _vmPlayList = vmPlayList;
            
            _audioPlayback = new AmmAudioPlayback();
            _audioPlayback.MaximumCalculated += audioGraph_MaximumCalculated;
            _audioPlayback.FftCalculated += audioGraph_FftCalculated;

            _messageBoxViewModel = new MessageBoxViewModel();

            SelectedVisualization = new PolylineWaveFormVisualization();

            Localize();
        }
        #endregion  // Constructor

        #region Public
        public void Play()
        {
            Song = _vmPlayList.GetCurrentSong();
            PlaySong();
        }

        public void Unload()
        {
            StopPlayback();

            _audioPlayback.MaximumCalculated -= audioGraph_MaximumCalculated;
            _audioPlayback.FftCalculated -= audioGraph_FftCalculated;
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            _dataServiceCountries.ChangeDatabase(conInfo);
            _dataServiceSongs.ChangeDatabase(conInfo);
            _vmPlayList.ChangeDatabase(conInfo);
        }
        public void ChangeDatabaseService(ConnectionInfo conInfo)
        {
            _vmPlayList.ChangeDatabaseService(conInfo);

            if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Dispose();
            }

            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.SqlServer)
            {
                _dataServiceCountries = new DataServiceCountries_SQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
            }
            if (AppSettings.DatabaseSettings.DefaultDatabase.ServerType == ServerType.MySql)
            {
                _dataServiceCountries = new DataServiceCountries_MYSQL(conInfo);
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
            }
        }
        #endregion

        #region Private
        private void PlaySong()
        {
            RaisePropertyChanged("Randomize");

            StopPlayback();

            if (File.Exists(Song.SongFullPath) )
            {
                if (_timer == null)
                {
                    _timer = new System.Timers.Timer();
                    _timer.Interval = 100;
                    _timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer_Elapsed);
                    _timer.Enabled = true;
                }
                else
                {
                    _timer.Start();
                }

                StreamMp3();
                _stopped = false;
                
                PlaybackPaused = false;

                Rating = Song.Rating;
                Song.RatingChanged += new SongItem.RatingChangedEventHandler(Song_RatingChanged);

                RaisePropertyChanged("StreamLength");

                GetCountry();
            }
            else
            {
                MessageBoxViewModel = new MessageBoxViewModel(MessageBoxButtons.OK_only);
                _messageBoxViewModel.MessageText = "File does not exists: \n\n" + Song.SongFullPath;
                //_messageBoxViewModel.OkAction = new Action(ReplacePlaylist);
                _messageBoxViewModel.MessageBoxVisible = true;
            }
        }
        private void StopPlayback()
        {
            if (_audioPlayback != null)
            {
                _audioPlayback.Stop();

                while ((_audioPlayback.StreamLength > 0) && (_audioPlayback.PlaybackStopped == false))
                {
                    
                }
                PlaybackPaused = false;
            }
            if (_timer != null)
            {
                 _timer.Stop();
            }
            _stopped = true;
        }
        private async void GetCountry()
        {
            if (_song.CountryId > 0)
            {
                _country = await _dataServiceCountries.GetCountry(_song.CountryId);
            }
        }
        private void StreamMp3()
        {
            _fileName = _song.SongFullPath;

            // Load the file in a new thread to avoid stuttering when navigating the GUI
            _audioPlayback.Load_Thread(_fileName);

            //_audioPlayback.Load(_fileName);
            _audioPlayback.Volume = _volume;
            
            _audioPlayback.Play();
        }
        void audioGraph_FftCalculated(object sender, FftEventArgs e)
        {
            if (SelectedVisualization != null)
            {
                SelectedVisualization.OnFftCalculated(e.Result);
            }
        }
        void audioGraph_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {   
            if (SelectedVisualization != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => SelectedVisualization.OnMaxCalculated(e.MinSample, e.MaxSample)));
                //SelectedVisualization.OnMaxCalculated(e.MinSample, e.MaxSample);
            }
        }
        private void PlayNextSong()
        {
            Song = _vmPlayList.GetNextSong();
            PlaySong();
        }
        private void PlayPreviousSong()
        {
            Song = _vmPlayList.GetPreviousSong();
            PlaySong();
        }

        private void OnTimer_Elapsed(object sender, EventArgs e)
        {
            if (_audioPlayback != null)
            {
                RaisePropertyChanged("StreamPosition");
                CurrentTime = _audioPlayback.CurrentTime;

                if ((_stopped == false) && (_audioPlayback.PlaybackStopped == true))
                {
                    PlayNextSong();
                }
            }
        }
        private void Song_RatingChanged(object sender, IntegerEventArgs e)
        {
            Mp3Metaedit metaData = new Mp3Metaedit(Song.SongFullPath);
            metaData.UpdateMetadata(Song);

            Task.Run(() => _dataServiceSongs.AddSong(Song));
        }
        #endregion

        #region Localization

        private String _albumLabel = "Album";
        private String _bandLabel = "Band";
        private String _genreLabel = "Genre";
        private String _songTitleLabel = "Title";
        private String _ratingLabel = "Rating";
        private String _trackLabel = "Track";
        private String _yearLabel = "Year";

        public String AlbumLabel
        {
            get { return _albumLabel; }
            set
            {
                if (value == _albumLabel)
                    return;

                _albumLabel = value;

                RaisePropertyChanged("AlbumLabel");
            }
        }
        public String BandLabel
        {
            get { return _bandLabel; }
            set
            {
                if (value == _bandLabel)
                    return;

                _bandLabel = value;

                RaisePropertyChanged("Band");
            }
        }
        public String GenreLabel
        {
            get { return _genreLabel; }
            set
            {
                if (value == _genreLabel)
                    return;

                _genreLabel = value;

                RaisePropertyChanged("Genre");
            }
        }
        public String RatingLabel
        {
            get { return _ratingLabel; }
            set
            {
                if (value == _ratingLabel)
                    return;

                _ratingLabel = value;

                RaisePropertyChanged("RatingValue");
            }
        }
        public String SongTitleLabel
        {
            get { return _songTitleLabel; }
            set
            {
                if (value == _songTitleLabel)
                    return;

                _songTitleLabel = value;

                RaisePropertyChanged("SongTitle");
            }
        }
        public String TrackLabel
        {
            get { return _trackLabel; }
            set
            {
                if (value == _trackLabel)
                    return;

                _trackLabel = value;

                RaisePropertyChanged("Track");
            }
        }
        public String YearLabel
        {
            get { return _yearLabel; }
            set
            {
                if (value == _yearLabel)
                    return;

                _yearLabel = value;

                RaisePropertyChanged("Year");
            }
        }
        public void Localize()
        {
            AlbumLabel = AmmLocalization.GetLocalizedString("Col_Album");
            BandLabel = AmmLocalization.GetLocalizedString("Col_Band");
            GenreLabel = AmmLocalization.GetLocalizedString("Col_Genre");
            RatingLabel = AmmLocalization.GetLocalizedString("Col_RatingValue");
            SongTitleLabel = AmmLocalization.GetLocalizedString("Col_Title");
            TrackLabel = AmmLocalization.GetLocalizedString("Col_Track");
            YearLabel = AmmLocalization.GetLocalizedString("Col_Year");
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

        ~AudioPlayerViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_audioPlayback != null)
            {
                _audioPlayback.Dispose();
            }

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }

            if (_vmPlayList != null)
            {
                _vmPlayList.Dispose();
            }

            if (_dataServiceCountries != null)
            {
                _dataServiceCountries.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
