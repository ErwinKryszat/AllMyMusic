using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllMyMusic_v3.QueryBuilder
{
    public static class QueryBuilderItems
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
                    conditionVA = " WHERE VariousArtists >= 0 ";
                }
                else
                {
                    conditionVA = " WHERE VariousArtists = 0 ";
                }
            }
        }

        public static String Counties()
        {
            String strSQL = "SELECT Country FROM Countries ORDER BY Country";
            return strSQL;
        }

        public static String Languages()
        {
            String strSQL = "SELECT Language FROM Languages ORDER BY Language";
            return strSQL;
        }

        public static String Genres()
        {
            String strSQL = "SELECT Genre FROM Genres ORDER BY Genre";
            return strSQL;
        }

        public static String GetAlphabet(TreeviewCategory tvCategory)
        {
            String strAlphaQuerry = String.Empty;

            switch (tvCategory)
            {
                case TreeviewCategory.Band:
                    if (showVariousArtists)
                    {
                        strAlphaQuerry = "SELECT DISTINCT UPPER(SUBSTRING(Bands.Name,1,1)) AS alpha FROM Albums RIGHT Join Bands ON Albums.IDBand = Bands.ID ORDER BY alpha ";
                    }
                    else
                    {
                        strAlphaQuerry = "SELECT DISTINCT UPPER(SUBSTRING(Bands.Name,1,1)) AS alpha FROM Albums RIGHT Join Bands ON Albums.IDBand = Bands.ID WHERE Albums.VariousArtists = 0 ORDER BY alpha ";
                    }
                    
                    break;
                case TreeviewCategory.Album:
                    strAlphaQuerry = " SELECT DISTINCT UPPER(SUBSTRING(Name,1,1)) AS alpha FROM Albums WHERE Name <> '' ORDER BY alpha";
                    break;
                case TreeviewCategory.LeadPerformer:
                    strAlphaQuerry = " SELECT DISTINCT UPPER(SUBSTRING(name,1,1)) AS alpha FROM LeadPerformer WHERE name <> '' ORDER BY alpha";
                    break;
                case TreeviewCategory.Composer:
                    strAlphaQuerry = " SELECT DISTINCT UPPER(SUBSTRING(name,1,1)) AS alpha FROM Composer WHERE name <> '' ORDER BY alpha";
                    break;
                case TreeviewCategory.Conductor:
                    strAlphaQuerry = " SELECT DISTINCT UPPER(SUBSTRING(name,1,1)) AS alpha FROM Conductor WHERE name <> '' ORDER BY alpha";
                    break;
                case TreeviewCategory.SongName:
                    strAlphaQuerry = " SELECT DISTINCT UPPER(SUBSTRING(SongName,1,1)) AS alpha FROM viewSongs WHERE SongName <> '' ORDER BY alpha";
                    break;
                default:
                    strAlphaQuerry = " SELECT DISTINCT UPPER(SUBSTRING(name,1,1)) AS alpha FROM Bands WHERE name <> '' ORDER BY alpha";
                    break;
            }

            return strAlphaQuerry;
        }

        public static String GetIntItemsByColumn(String columName)
        {
            if (columName == "BandName")
            {
                String strSQL = "SELECT DISTINCT " + columName + " FROM viewSongs " + conditionVA + " ORDER BY " + columName;
                return strSQL;
            }
            else
            {
                String strSQL = "SELECT DISTINCT " + columName + " FROM viewSongs ORDER BY " + columName;
                return strSQL; 
            }
        }

        public static String GetStringItemsByColumn(String columName)
        {
            String strSQL = "SELECT DISTINCT " + columName + " FROM viewSongs ORDER BY " + columName;
            return strSQL;
        }
        
        public static String GetStringItemsByAlphabet(String columName, String firstCharacter)
        {
            if (columName == "BandName")
            {
                String strSQL = "SELECT DISTINCT " + columName + " FROM viewSongs " + conditionVA +
                            " AND " + columName + " LIKE '" + firstCharacter + "%'" +
                            " ORDER BY " + columName;
                return strSQL;

            }
            else
            {
                String strSQL = "SELECT DISTINCT " + columName + " FROM viewSongs " +
                             " WHERE " + columName + " LIKE '" + firstCharacter + "%'" +
                             " ORDER BY " + columName;
                return strSQL;
            }
        }

        public static String DateAdded()
        {
            String strSQL = "SELECT  DISTINCT TOP 20 CONVERT(VARCHAR(10),DateAdded, 102) AS [YYYY.MM.DD]" +
                            " FROM viewSongs " +
                            " ORDER BY CONVERT(VARCHAR(10),DateAdded, 102) DESC ";

            return strSQL;
        }

        public static String DatePlayed()
        {
            String strSQL = "SELECT  DISTINCT TOP 20 CONVERT(VARCHAR(10),DatePlayed, 102) AS [YYYY.MM.DD]" +
                            " FROM viewSongs " +
                            " WHERE DatePlayed > '1900'" +
                            " ORDER BY CONVERT(VARCHAR(10),DatePlayed, 102) DESC ";

            return strSQL;
        }

        public static String Decade()
        {
            String strSQL = "SELECT  DISTINCT CAST(ROUND(YEAR/10,0) * 10 AS CHAR) AS YEAR , -1 AS ID" +
                            " FROM viewSongs WHERE YEAR > 0 ORDER BY YEAR ";

            return strSQL;
        }

        public static String YearByDecade(String decade)
        {
            String strSQL = "	SELECT DISTINCT CAST( YEAR AS NVARCHAR) AS YEAR, -1 AS ID FROM viewSongs " +
                         " WHERE YEAR >= '" + decade + "' AND YEAR < CAST(" + decade + " + 10 AS NVARCHAR) " +
                         " ORDER BY YEAR";

            return strSQL;
        }

        public static String Drives(ServerType serverType)
        {
            String strSQL = null;
            if (serverType == ServerType.SqlServer)
            {
                strSQL = @"SELECT DISTINCT SUBSTRING(Path,  1, Charindex('\', Path, 1) ) AS name, 0 AS ID FROM Songs";
            }
            if (serverType == ServerType.MySql)
            {
                strSQL = @"SELECT DISTINCT SUBSTRING(Path,  1, LOCATE('\\', Path) ) AS name, 0 AS ID  FROM Songs";
            }
            return strSQL;
        }

        public static String FolderByParentFolder(ServerType serverType, String parentFolder)
        {
            String strSQL = null;
            int len = 0;
            if (parentFolder.Substring(parentFolder.Length - 1, 1) == "\\")
            {
                len = parentFolder.Length + 1;

            }
            else
            {
                len = parentFolder.Length + 2;
            }

            if (serverType == ServerType.SqlServer)
            {
                strSQL = "SELECT DISTINCT SUBSTRING(Path, " + len + @", Charindex('\', Path, " + len + ") - " + len + ") AS name, 0 AS ID " +
                    "  FROM Songs" +
                    "  WHERE Path LIKE @PathPart + '%'" +
                    "    AND LEN(Path) > " + len +
                    "    ORDER BY name ";
            }
            if (serverType == ServerType.MySql)
            {
                strSQL = "SELECT DISTINCT SUBSTRING(Path, " + len + @", LOCATE('\\', Path," + len + ") - " + len + ") AS name, 0 AS ID " +
                    "  FROM Songs" +
                    "  WHERE Path LIKE CONCAT(?var_PathPart, '%')" +
                    "    AND LENGTH(Path) > " + len +
                    "    ORDER BY name ";
            }
            return strSQL;
        }

    }

}
