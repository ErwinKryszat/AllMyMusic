using System;

using AllMyMusic.ViewModel;

namespace AllMyMusic
{
    public class PartyButtonConfigEventArgs : EventArgs
    {
        private PartyButtonConfigViewModel _partyButtonConfig;
        public PartyButtonConfigViewModel PartyButtonConfig
        {
            get { return _partyButtonConfig; }
        }

        public PartyButtonConfigEventArgs(PartyButtonConfigViewModel playlistConfig)
        {
            this._partyButtonConfig = playlistConfig;
        }
    }
}
