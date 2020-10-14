using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic_v3.QueryBuilder
{
    public static class QueryBuilderBands
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


        private static String columns = "SELECT DISTINCT BandName, IDBand, SortName, AlbumCount, Bookmarked, VariousArtists";

        private static String source = " FROM viewBands ";

        public static String BandById(Int32 bandID)
        {
            String condition = "  WHERE IDBand = " + bandID.ToString();
            String strSQL = columns + source + condition;
            return strSQL;
        }

        public static String BandsByAlphabet()
        {
            String condition = String.Empty;


            if (serverType == ServerType.SqlServer)
            {
                condition = " WHERE BandName LIKE @FirstChar + '%'" + conditionVA;
            }
            if (serverType == ServerType.MySql)
            {
                condition = " WHERE BandName LIKE CONCAT(?var_FirstChar, '%') " + conditionVA;

            }
            String order = " ORDER BY BandName";

            String strSQL = columns + source + condition + order;
            return strSQL;
        }

        public static String SearchBands()
        {
            String condition = String.Empty;

            if (serverType == ServerType.SqlServer)
            {
                condition = " WHERE BandName like '%' + @Name + '%'" + conditionVA;
            }
            if (serverType == ServerType.MySql)
            {
                condition = " WHERE BandName LIKE CONCAT('%', ?var_Name, '%') " + conditionVA;

            }
            String order = " ORDER BY BandName";

            String strSQL = columns + source + condition + order;
            return strSQL;
        }

        public static String BandsByDigit()
        {
            String condition = " WHERE  substring(BandName ,1,1) >= '0' AND substring(BandName ,1,1) <= '9' " + conditionVA;
            String order = " ORDER BY BandName";

            String strSQL = columns + source + condition + order;
            return strSQL;
        }

        public static String BandsBySpecialCharacter()
        {
            String condition = " WHERE  (substring(BandName ,1,1) >= ' ' AND substring(BandName ,1,1) <= '0') "
                + " OR  (substring(BandName ,1,1) > '9' AND substring(BandName ,1,1) < 'A') "
                + " OR  (substring(BandName ,1,1) > 'Z' AND substring(BandName ,1,1) < 'a') ";
            String order = " ORDER BY BandName";

            String strSQL = columns + source + condition + order;
            return strSQL;
        }
    }
}
