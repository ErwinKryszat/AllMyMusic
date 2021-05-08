using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllMyMusic.QueryBuilder
{
    public static class QueryBuilderAlbumsVA
    {
        private static String columns = "SELECT AlbumName, IDAlbum, Count(IDSong) AS SongCount, 'Various Artists' as BandName, 0 as IDBand, Path, " +
                "Year, AlbumGenre, VariousArtists, Front, Back, Stamp, IDAlbumGenre, SUM(LengthInteger) AS TotalLength ";

        private static String source = " FROM viewSongs ";

        private static String group = " GROUP BY AlbumName, IDAlbum, Path, Year, albumGenre, VariousArtists, Front, Back, Stamp, IDAlbumGenre ";

        private static String order = " ORDER BY Year DESC, AlbumName ";

        public static String Albums(AlbumGenreItem albumGenre)
        {
            String condition = " WHERE IDAlbumGenre = " + albumGenre.AlbumGenreId.ToString() +
                " AND VariousArtists = 1 ";

            String strSQL = columns + source + condition + group + order;
            return strSQL;
        }
    }
}
