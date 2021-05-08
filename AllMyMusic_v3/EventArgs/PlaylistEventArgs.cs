using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;



namespace AllMyMusic
{
    public class PlaylistEventArgs : EventArgs
    {
        private ObservableCollection<SongItem> _songs;
        public ObservableCollection<SongItem> Songs
        {
            get { return _songs; }
        }

        private QueueMode _queueMode;
        public QueueMode QueueMode
        {
            get { return _queueMode; }
        }

        private String _headline;
        public String Headline
        {
            get { return _headline; }
        }

        private Boolean _unlockPlaylist;
        public Boolean UnlockPlaylist
        {
            get { return _unlockPlaylist; }
        }

        private Boolean _playRandom;
        public Boolean PlayRandom
        {
            get { return _playRandom; }
        }

        public PlaylistEventArgs(ObservableCollection<SongItem> songs, QueueMode queueMode, String headline, Boolean unlockPlaylist)
        {
            this._songs = songs;
            this._queueMode = queueMode;
            this._headline = headline;
            this._unlockPlaylist = unlockPlaylist;
        }

        public PlaylistEventArgs(ObservableCollection<SongItem> songs, QueueMode queueMode, String headline, Boolean unlockPlaylist, Boolean playRandom)
        {
            this._songs = songs;
            this._queueMode = queueMode;
            this._headline = headline;
            this._unlockPlaylist = unlockPlaylist;
            this._playRandom = playRandom;
        }
    }
}
