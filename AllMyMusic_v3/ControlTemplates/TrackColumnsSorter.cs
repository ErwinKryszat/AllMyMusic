using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;


namespace AllMyMusic.View
{
    public class TrackColumnsSorter : IComparer
    {
        ListSortDirection _direction;

        public TrackColumnsSorter(ListSortDirection direction)
        {
            _direction = direction;
        }

        public int Compare(object x, object y)
        {
            SongItem songX = (SongItem)x;
            SongItem songY = (SongItem)y;
            Int32 trackX = Convert.ToInt32(songX.Track);
            Int32 trackY = Convert.ToInt32(songY.Track);

            if (_direction == ListSortDirection.Ascending)
            {
                if (trackX == trackY) return 0;
                else
                {
                    return (trackX < trackY) ? -1 : 1;
                }
            }
            else
            {
                if (trackX == trackY) return 0;
                else
                {
                    return (trackX > trackY) ? -1 : 1;
                }
            }
        }
    }
}
