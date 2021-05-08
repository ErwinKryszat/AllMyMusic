using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AllMyMusic
{
    public class BandItemEventArgs : EventArgs
    {
        private BandItem _band;
        public BandItem Band
        {
            get { return _band; }
        }


        public BandItemEventArgs(BandItem band)
        {
            this._band = band;
        }
    }
}
