using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace AllMyMusic_v3.View
{
    public class FrequencyFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object
            parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double frequency = System.Convert.ToDouble(value) / 1000;
                return frequency.ToString(parameter as string);
            }
            catch (Exception)
            {
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
