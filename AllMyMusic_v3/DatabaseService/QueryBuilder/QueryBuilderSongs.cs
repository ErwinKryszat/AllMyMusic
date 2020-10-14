using System;
using System.Collections.Generic;
using System.Text;


namespace AllMyMusic_v3.QueryBuilder
{
    public static class QueryBuilderSongs
    {
        private static ServerType serverType = ServerType.Unknown;
        public static ServerType ServerType
        {
            get { return serverType; }
            set { serverType = value; }
        }

        private static String conditionVA = " AND VariousArtists >= 0 ";
        private static Boolean showVariousArtists = true;
        public static Boolean ShowVariousArtists
        {
            get { return showVariousArtists; }
            set
            {
                showVariousArtists = value;
                if (showVariousArtists == true)
                {
                    conditionVA = " AND VariousArtists >= 0 ";
                }
                else
                {
                    conditionVA = " AND VariousArtists = 0 ";
                }
            }
        }

        private static String columns = "SELECT * from viewSongs";

        public static String SongById(Int32 songID)
        {
            String condition = "  WHERE IDSong = " + songID.ToString();
            String strSQL = columns + condition;
            return strSQL;
        }
        public static String SongsByAlbum(Int32 album)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDAlbum = " + album.ToString();
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String SongsByAlbumGenre(Int32 albumGenreID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDAlbumGenre = " + albumGenreID.ToString();
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String SongsByAlbumGenreVA(Int32 albumGenreID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDAlbumGenre = " + albumGenreID.ToString();
            String strSQL = columns + condition + " AND VariousArtists = 1 " + order;
            return strSQL;
        }

        public static String SongsByBand(Int32 bandID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDBand = " + bandID.ToString();
            String strSQL = columns + condition + conditionVA + order;
            return strSQL;
        }

        public static String SongsByComposer(Int32 composerID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDComposer = " + composerID.ToString();
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String SongsByConductor(Int32 conductorID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDConductor = " + conductorID.ToString();
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String SongsByCountry(Int32 countryID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDCountry = " + countryID.ToString();
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String SongsByGenre(Int32 genreID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDCountry = " + genreID.ToString();
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String SongsByLanguange(Int32 languageID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDLanguage = " + languageID.ToString();
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String SongsByLeadPerformer(Int32 leadPerformerID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";
            String condition = " WHERE IDLeadPerformer = " + leadPerformerID.ToString();
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String SearchSongs()
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";

            String condition = String.Empty;

            if (serverType == ServerType.SqlServer)
            {
                condition = " WHERE SongName like '%' + @Name + '%'";
            }
            if (serverType == ServerType.MySql)
            {
                condition = " WHERE SongName like CONCAT('%', ?var_Name, '%')";
            }

            String strSQL = columns +  condition + conditionVA + order;
            return strSQL;
        }

        public static String SongsByCondition(String condition)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";

            String strSQL = columns + condition + conditionVA + order;
            return strSQL;
        }

        public static String Band(TreeviewCategory category, Int32 IDBand, Int32 itemID)
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName, Track ";


            String condition = String.Empty;
            if (category == TreeviewCategory.Band)
            {
                condition = " WHERE IDBand = " + IDBand.ToString();
            }

            if (category == TreeviewCategory.AlbumGenre)
            {
                condition = " WHERE IDBand = " + IDBand.ToString() +
                            " AND IDAlbumGenre = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Country)
            {
                condition = " WHERE IDBand = " + IDBand.ToString() +
                            " AND IDCountry = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Genre)
            {
                condition = " WHERE IDBand = " + IDBand.ToString() +
                             " AND IDGenre = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Language)
            {
                condition = " WHERE IDBand = " + IDBand.ToString() +
                             " AND IDLanguage = " + itemID.ToString();
            }
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String ByName(TreeviewCategory category, String name)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName, Track ";

            String condition = String.Empty;
            String condition2 = String.Empty;

            if (serverType == ServerType.SqlServer)
            {
                condition2 = " @Name + '%' ";
            }
            if (serverType == ServerType.MySql)
            {
                condition2 = " CONCAT(?var_Name, '%') ";
            }

            switch (category)
            {
                case TreeviewCategory.Album:
                    condition = " WHERE AlbumName LIKE " + condition2 + conditionVA;
                    order = " ORDER BY AlbumName ";
                    break;
                case TreeviewCategory.AlbumGenre:
                    break;
                case TreeviewCategory.Band:
                    condition = " WHERE BandName LIKE " + condition2 + conditionVA;
                    break;
                case TreeviewCategory.Composer:
                    condition = " WHERE ComposerName LIKE " + condition2 + conditionVA;
                    break;
                case TreeviewCategory.Conductor:
                    condition = " WHERE ConductorName LIKE " + condition2 + conditionVA;
                    break;
                case TreeviewCategory.Country:
                    condition = " WHERE Country LIKE " + condition2 + conditionVA;
                    break;
                case TreeviewCategory.DateAdded:
                    condition = " WHERE CONVERT(VARCHAR(10), DateAdded, 102)  LIKE " + condition2 + conditionVA;
                    break;
                case TreeviewCategory.DatePlayed:
                    condition = " WHERE CONVERT(VARCHAR(10), DatePlayed, 102)  LIKE " + condition2 + conditionVA;
                    break;
                case TreeviewCategory.Decade:
                    condition = " WHERE YEAR >= '" + name + "' AND YEAR < CAST(" + name + " + 10 AS NVARCHAR) " + conditionVA;
                    break;
                case TreeviewCategory.Genre:
                    condition = " WHERE Genre LIKE " + condition2 + conditionVA;
                    break;
                case TreeviewCategory.Language:
                    break;
                case TreeviewCategory.LeadPerformer:
                    condition = " WHERE LeadPerformerName LIKE " + condition2 + conditionVA;
                    break;
                case TreeviewCategory.Library:
                    break;
                case TreeviewCategory.Location:
                    break;
                case TreeviewCategory.MyComputer:
                    break;
                case TreeviewCategory.Rating:
                    break;
                case TreeviewCategory.Year:
                    condition = " WHERE Year = '" + name + "'" + conditionVA;
                    break;
                case TreeviewCategory.Unknown:
                    break;
                default:
                    break;
            }

            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String ById(TreeviewCategory category, Int32 itemID)
        {
            String order = "  ORDER BY BandName, VariousArtists, Year DESC, AlbumName, Track ";


            String condition = String.Empty;

            if (category == TreeviewCategory.AlbumGenre)
            {
                condition = " WHERE IDAlbumGenre = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Album)
            {
                condition = " WHERE IDAlbum = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Band)
            {
                condition = " WHERE IDBand = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Composer)
            {
                condition = " WHERE IDComposer = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Conductor)
            {
                condition = " WHERE IDConductor = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Country)
            {
                condition = " WHERE IDCountry = " + itemID.ToString();
            }

            if (category == TreeviewCategory.Genre)
            {
                condition = " WHERE IDGenre = " + itemID.ToString();

            }
            if (category == TreeviewCategory.Language)
            {
                condition = " WHERE IDLanguage = " + itemID.ToString();
            }

            if (category == TreeviewCategory.LeadPerformer)
            {
                condition = " WHERE IDLeadPerformer = " + itemID.ToString();
            }

            String strSQL = columns + condition + order;
            return strSQL;
        }
        public static String AlbumGenreVA(Int32 albumGenreID)
        {
            String order = " ORDER BY AlbumName, Track ";
            String condition = condition = " WHERE IDAlbumGenre = " + albumGenreID.ToString() +
                " AND VariousArtists = 1 ";

            String strSQL = columns + condition + order;
            return strSQL;
        }
        public static String SongsByRating(Int32 rating)
        {
            String columns = String.Empty;
            String order = " ORDER BY BandName, Year, AlbumName, Track";

            if ((rating >= 0) && (rating <= 19)) { columns = "SELECT * FROM viewSongs WHERE Rating >= 0 AND Rating <= 19"; }
            if ((rating >= 20) && (rating <= 43)) { columns = "SELECT * FROM viewSongs WHERE Rating > 20 AND Rating <= 43"; }
            if ((rating >= 44) && (rating <= 67)) { columns = "SELECT * FROM viewSongs WHERE Rating > 44 AND Rating <= 67"; }
            if ((rating >= 68) && (rating <= 91)) { columns = "SELECT * FROM viewSongs WHERE Rating > 68 AND Rating <= 91"; }
            if ((rating >= 92) && (rating <= 115)) { columns = "SELECT * FROM viewSongs WHERE Rating > 92 AND Rating <= 115"; }
            if ((rating >= 116) && (rating <= 139)) { columns = "SELECT * FROM viewSongs WHERE Rating > 116 AND Rating <= 139"; }
            if ((rating >= 140) && (rating <= 163)) { columns = "SELECT * FROM viewSongs WHERE Rating > 140 AND Rating <= 163"; }
            if ((rating >= 164) && (rating <= 187)) { columns = "SELECT * FROM viewSongs WHERE Rating > 164 AND Rating <= 187"; }
            if ((rating >= 188) && (rating <= 211)) { columns = "SELECT * FROM viewSongs WHERE Rating > 188 AND Rating <= 211"; }
            if ((rating >= 212) && (rating <= 235)) { columns = "SELECT * FROM viewSongs WHERE Rating > 212 AND Rating <= 235"; }
            if ((rating >= 236) && (rating <= 255)) { columns = "SELECT * FROM viewSongs WHERE Rating > 236 AND Rating <= 255"; }

            String strSQL = columns + order;
            return strSQL;
        }

        public static String Album(Int32 IDAlbum)
        {
            String condition = " WHERE IDAlbum = " + IDAlbum.ToString();
            String order = " ORDER BY Track ";
            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String Album(Int32 IDAlbum, String condition2)
        {
            condition2 = condition2.Replace("WHERE", "AND");
            String condition = " WHERE IDAlbum = " + IDAlbum.ToString();
            String order = " ORDER BY Track ";
            String strSQL = columns + condition + condition2 + order;
            return strSQL;
        }

        public static String Songs(String condition)
        {
            String order = " ORDER BY BandName, Year, AlbumName, Track ";

            String strSQL = columns +  condition + conditionVA + order;
            return strSQL;
        }

        public static String Folder()
        {
            String condition = String.Empty;
            String order = String.Empty;

            if (serverType == ServerType.SqlServer)
            {
                condition = " WHERE Path LIKE @PathPart + '%'";
            }
            if (serverType == ServerType.MySql)
            {
                condition = " WHERE Path LIKE CONCAT(?var_PathPart, '%')";
            }

            String strSQL = columns + condition + order;
            return strSQL;
        }

        public static String FullPath()
        {
            String condition = String.Empty;
            String order = String.Empty;

            if (serverType == ServerType.SqlServer)
            {
                condition = " WHERE Path+Filename = @FullPath";
                order = " ORDER BY Path+Filename";
            }
            if (serverType == ServerType.MySql)
            {
                condition = " WHERE CONCAT(Path, Filename) = ?var_FullPath";
                order = " ORDER BY CONCAT(Path, Filename)";
            }

            String strSQL = columns + condition + order;
            return strSQL;
        }
    }
}
