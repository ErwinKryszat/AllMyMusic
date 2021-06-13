using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic
{
    public class StatisticsItem
    {
        private Int32 _countSong;
        private Int32 _countAlbums;
        private Int32 _countBands;
      

        public Int32 CountSong
        {
            get { return _countSong; }
            set { _countSong = value; }
        }

        public Int32 CountAlbums
        {
            get { return _countAlbums; }
            set { _countAlbums = value; }
        }

        public Int32 CountBands
        {
            get { return _countBands; }
            set { _countBands = value; }
        }

    }
}
