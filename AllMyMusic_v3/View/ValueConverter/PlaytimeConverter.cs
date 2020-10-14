using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;

namespace AllMyMusic_v3.View
{
    public class PlaytimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan playTime = new TimeSpan();

            if (value is TimeSpan)
            {
                playTime = (TimeSpan)value;
            }

            if (value is Int32)
            {
                playTime = TimeSpan.FromSeconds((Int32)value);
            }

            if (playTime.TotalSeconds > 0)
            {
                Int32 totalDays = (Int32)playTime.TotalDays;
                Int32 totalHours = (Int32)playTime.TotalHours;

                if (totalDays > 0)
                {
                    return string.Format("{0:dd\\.hh\\:mm\\:ss}", playTime);
                }
                else
                {
                    if (totalHours > 0)
                    {
                        return string.Format("{0:hh\\:mm\\:ss}", playTime);
                    }
                    else
                    {
                        return string.Format("{0:mm\\:ss}", playTime);
                    }
                }
            }

            return String.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
