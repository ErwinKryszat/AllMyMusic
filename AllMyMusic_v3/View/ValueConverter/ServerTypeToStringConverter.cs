using System;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace AllMyMusic_v3.View
{
    public class ServerTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ServerType)
            {
                if ((ServerType)value == ServerType.SqlServer)
                {
                    return "Sql-Server (Microsoft)";
                }

                if ((ServerType)value == ServerType.MySql)
                {
                    return "MySql (Oracle)";
                }
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType,  object parameter, CultureInfo culture)
        {
            if (value is String)
            {
                if ((String)value == "Sql-Server (Microsoft)")
                {
                    return ServerType.SqlServer;
                }
                if ((String)value == "MySql (Oracle)")
                {
                    return ServerType.MySql;
                }
                return ServerType.Unknown;
            }

            return ServerType.Unknown; ;
        }
    }   
}
