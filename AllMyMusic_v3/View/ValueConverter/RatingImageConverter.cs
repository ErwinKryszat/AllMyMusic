using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;

namespace AllMyMusic.View
{
    class RatingImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Int32)
            {
                Int32 rating = (Int32)value;
                
                if ((rating >= 0) && (rating <= 19)) { value = new Uri(Global.Images + "Rating_00.png", UriKind.Relative); }
                if ((rating >= 20) && (rating <= 43)) { value = new Uri(Global.Images + "Rating_01.png", UriKind.Relative); }
                if ((rating >= 44) && (rating <= 67)) { value = new Uri(Global.Images + "Rating_02.png", UriKind.Relative); }
                if ((rating >= 68) && (rating <= 91)) { value = new Uri(Global.Images + "Rating_03.png", UriKind.Relative); }
                if ((rating >= 92) && (rating <= 115)) { value = new Uri(Global.Images + "Rating_04.png", UriKind.Relative); }
                if ((rating >= 116) && (rating <= 139)) { value = new Uri(Global.Images + "Rating_05.png", UriKind.Relative); }
                if ((rating >= 140) && (rating <= 163)) { value = new Uri(Global.Images + "Rating_06.png", UriKind.Relative); }
                if ((rating >= 164) && (rating <= 187)) { value = new Uri(Global.Images + "Rating_07.png", UriKind.Relative); }
                if ((rating >= 188) && (rating <= 211)) { value = new Uri(Global.Images + "Rating_08.png", UriKind.Relative); }
                if ((rating >= 212) && (rating <= 235)) { value = new Uri(Global.Images + "Rating_09.png", UriKind.Relative); }
                if ((rating >= 236) && (rating <= 255)) { value = new Uri(Global.Images + "Rating_10.png", UriKind.Relative); }

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = (Uri)value;
                bi.EndInit();
                return bi;
            }

            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
