using System;
using System.Windows.Data;
using System.Globalization;

namespace AllMyMusic_v3.View
{
    public class LongToFileSizeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a Visibility");

            return string.Format(new FileSizeFormatProvider(), "{0:fs}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion


    }
}
