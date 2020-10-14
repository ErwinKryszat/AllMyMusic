using System;
using System.Collections.Generic;
using System.Text;


namespace AllMyMusic_v3.QueryBuilder
{
    public static class QueryBuilderAlbums
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

        private static String columns = "SELECT DISTINCT AlbumName, IDAlbum, Count(IDSong) AS SongCount, " +
          " CASE WHEN VariousArtists = 0 THEN BandName ELSE 'VA' END AS BandName, " +
          " CASE WHEN VariousArtists = 0 THEN IDBand ELSE 0 END AS IDBand, " +
          " Path, Year, AlbumGenre, VariousArtists, Front, Back, Stamp, IDAlbumGenre, " +
          " CASE WHEN VariousArtists = 0 THEN SUM(LengthInteger) ELSE 0 END  AS TotalLength ";


        private static String source = " FROM viewSongs ";

        private static String group = " GROUP BY AlbumName, IDAlbum, BandName, IDBand, Path, Year, albumGenre, VariousArtists, Front, Back, Stamp, IDAlbumGenre ";

        public static String Album(Int32 albumID)
        {
            String condition = "  WHERE IDAlbum = " + albumID.ToString();
            String strSQL = columns + source + condition + group;
            return strSQL;
        }

        public static String Band(TreeviewCategory category, Int32 IDBand, Int32 itemID )
        {
            String order = " ORDER BY VariousArtists, Year DESC, AlbumName ";

            String condition = String.Empty;
            if (category == TreeviewCategory.Band)
            {
                condition = " WHERE IDBand = " + IDBand.ToString() + conditionVA;
            }

            if (category == TreeviewCategory.AlbumGenre)
            {
                condition = " WHERE IDBand = " + IDBand.ToString() +
                            " AND IDAlbumGenre = " + itemID.ToString() + conditionVA;
            }

            if (category == TreeviewCategory.Country)
            {
                condition = " WHERE IDBand = " + IDBand.ToString() +
                            " AND IDCountry = " + itemID.ToString() + conditionVA;
            }

            if (category == TreeviewCategory.Genre)
            {
                condition =  " WHERE IDBand = " + IDBand.ToString() +
                             " AND IDGenre = " + itemID.ToString() + conditionVA;
            }

            if (category == TreeviewCategory.Language)
            {
                condition =  " WHERE IDBand = " + IDBand.ToString() +
                             " AND IDLanguage = " + itemID.ToString() + conditionVA;
            }
            String strSQL = columns + source + condition + group + order;
            return strSQL;
        }

        public static String Albums(TreeviewCategory category, String name)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";

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
                case TreeviewCategory.Alphabet:
                    condition = " WHERE AlbumName LIKE " + condition2 + conditionVA;
                    order = " ORDER BY VariousArtists, AlbumName ";
                    break;

                case TreeviewCategory.Album:
                    condition = " WHERE AlbumName LIKE " + condition2 + conditionVA;
                    order = " ORDER BY VariousArtists, AlbumName ";
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

            String strSQL = columns + source + condition + group + order;
            return strSQL;
        }

        public static String AlbumsByAlbumGenre(AlbumGenreItem item)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";
            String condition = " WHERE IDAlbumGenre = " + item.AlbumGenreId.ToString() + conditionVA;
            String strSQL = columns + source + condition + group + order;
            return strSQL;
        }
        public static String AlbumsByBand(BandItem band)
        {
            if (band.ArtistType == ArtistType.SingleArtist)
            {
                String order = " ORDER BY VariousArtists, Year DESC, AlbumName ";
                String condition = " WHERE IDBand = " + band.BandId.ToString();
                String strSQL = columns + source + condition + conditionVA + group + order;
                return strSQL;  
            }
            else
            {
                // Return some albums even if artists is on VA
                String order = " ORDER BY VariousArtists, Year DESC, AlbumName ";
                String condition = " WHERE IDBand = " + band.BandId.ToString();
                String strSQL = columns + source + condition + group + order;
                return strSQL;
            }
        }
        public static String AlbumsByComposer(ComposerItem composer)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";
            String condition = " WHERE IDComposer = " + composer.ComposerId.ToString();
            String strSQL = columns + source + condition + conditionVA + group + order;
            return strSQL;
        }
        public static String AlbumsByConductor(ConductorItem conductor)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";
            String condition = " WHERE IDConductor = " + conductor.ConductorId.ToString();
            String strSQL = columns + source + condition + conditionVA + group + order;
            return strSQL;
        }
        public static String AlbumsByCountry(CountryItem country)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";
            String condition = " WHERE IDCountry = " + country.CountryId.ToString();
            String strSQL = columns + source + condition + conditionVA + group + order;
            return strSQL;
        }
        public static String AlbumsByGenre(GenreItem genre)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";
            String condition = " WHERE IDGenre = " + genre.GenreId.ToString() + conditionVA;
            String strSQL = columns + source + condition + conditionVA + group + order;
            return strSQL;
        }
        public static String AlbumsByLanguage(LanguageItem language)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";
            String condition = " WHERE IDLanguage = " + language.LanguageId.ToString() + conditionVA;
            String strSQL = columns + source + condition + conditionVA + group + order;
            return strSQL;
        }
        public static String AlbumsByLeadPerformer(LeadPerformerItem leadPerformer)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";
            String condition = " WHERE IDLeadPerformer = " + leadPerformer.LeadPerformerId.ToString();
            String strSQL = columns + source + condition + conditionVA + group + order;
            return strSQL;
        }
        public static String SearchAlbums()
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

            String strSQL = columns + source + condition + conditionVA + group + order;
            return strSQL;
        }
        
        public static String AlbumsByCondition(String condition)
        {
            String order = " ORDER BY BandName, VariousArtists, Year DESC, AlbumName ";

            String strSQL = columns + source + condition + conditionVA + group + order;
            return strSQL;
        }

        public static String Folder_SQL()
        {
            String order = " ORDER BY BandName, AlbumName ";

            String condition = String.Empty;

            String strParameter = String.Empty;

            condition = " WHERE Path LIKE @PathPart + '%'"; 
       
            String strSQL = columns + source + condition + group + order;
            return strSQL;
        }
        public static String Folder_MYSQL()
        {
            String order = " ORDER BY BandName, AlbumName ";

            String condition = String.Empty;

            String strParameter = String.Empty;

            condition = " WHERE Path LIKE CONCAT(?var_PathPart, '%')";

            String strSQL = columns + source + condition + group + order;
            return strSQL;
        }

        public static String DeleteAlbum(AlbumItem item)
        {
            String delete = "Delete From Albums ";
            String condition = " WHERE ID = " + item.AlbumId.ToString();
            String strSQL = delete + condition;
            return strSQL;
        }
        public static String DeleteAlbumsByAlbumGenre(AlbumGenreItem item)
        {
            String delete = "Delete From Albums ";
            String condition = " WHERE IDAlbumGenre = " + item.AlbumGenreId.ToString();
            String strSQL = delete + condition;
            return strSQL;
        }
        public static String DeleteAlbumsByBand(BandItem band)
        {
            String delete = "Delete From Albums ";
            String condition = " WHERE IDBand = " + band.BandId.ToString();
            String strSQL = delete + condition;
            return strSQL;
        }
    }  
}
