using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllMyMusic.QueryBuilder
{
    public static class QueryBuilderCountries
    {
        private static String columns = "SELECT Country, Abbreviation, ID, FlagPath ";

        private static String source = " FROM Countries ";

        public static String AllCountries()
        {
            String condition = " WHERE Country NOT LIKE ''";
            String order = " ORDER BY Country";

            String strSQL = columns + source + condition + order;
            return strSQL;
        }

        public static String Country(Int32 countryId)
        {
            String condition = "  WHERE ID = " + countryId.ToString();
            String strSQL = columns + source + condition;
            return strSQL;
        }

        public static String Country(String countryName)
        {
            String condition = "  WHERE Country = '" + countryName + "'";
            String strSQL = columns + source + condition;
            return strSQL;
        }
    }
}
