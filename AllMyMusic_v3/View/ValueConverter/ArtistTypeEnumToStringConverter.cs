using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;


namespace AllMyMusic.View
{
    public class ArtistTypeEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ArtistType)value).ToString();
        }

        public object ConvertBack(object value, Type targetType,  object parameter, CultureInfo culture)
        {
            return null;
        }
    }   
}
