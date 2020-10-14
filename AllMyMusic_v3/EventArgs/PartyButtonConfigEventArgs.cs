using System;

using AllMyMusic_v3.ViewModel;

namespace AllMyMusic_v3
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
