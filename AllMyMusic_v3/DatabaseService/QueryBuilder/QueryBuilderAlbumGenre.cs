using System;
using System.Collections.Generic;
using System.Text;


namespace AllMyMusic.QueryBuilder
{
    public static class QueryBuilderAlbumGenre
    {
        private static ServerType serverType = ServerType.Unknown;
        public static ServerType ServerType
        {
            get { return serverType; }
            set { serverType = value; }
        }

        public static String VariousArtistsGenres()
        {
            String strSQL = "SELECT  AlbumGenre, IDAlbumGenre, COUNT(AlbumGenre) as AlbumCount  FROM Albums" +
                 "  INNER JOIN AlbumGenres ON Albums.IDAlbumGenre = AlbumGenres.ID" +
                 "  WHERE VariousArtists = 1" +
                 "  GROUP BY  AlbumGenre, IDAlbumGenre" +
                 "  ORDER BY  AlbumGenre, IDAlbumGenre";
            return strSQL;
        }
    }
}
