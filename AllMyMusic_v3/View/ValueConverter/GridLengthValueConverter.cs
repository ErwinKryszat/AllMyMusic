using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Text;


namespace AllMyMusic.View
{
    public class GridLengthValueConverter : IValueConverter
    {
        GridLengthConverter _converter = new GridLengthConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _converter.ConvertFrom(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(double))
            {
                return ((GridLength)value).Value;
            }
            else
            {
                return _converter.ConvertTo(value, targetType);
            }
        }
    }
}
