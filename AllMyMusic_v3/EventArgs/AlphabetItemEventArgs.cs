using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AllMyMusic.ViewModel;

namespace AllMyMusic
{
    public class AlphabetItemEventArgs : EventArgs
    {
        private AlphabetItem _item;
        public AlphabetItem Item
        {
            get { return _item; }
        }

        public AlphabetItemEventArgs(AlphabetItem item)
        {
            this._item = item;
        }
    }
}
