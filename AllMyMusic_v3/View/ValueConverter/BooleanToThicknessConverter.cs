using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace AllMyMusic_v3.View
{
    public class BooleanToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Boolean)
            {
                if ((Boolean)value == false)
                {
                    value = new Thickness(0.0D);
                }
                else
                {
                    value = new Thickness(3.0D);
                }
            }
            else
            {
                value = new Thickness(0.0D);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
