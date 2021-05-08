using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;

namespace AllMyMusic.View
{
    class DecadeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is String))
            {
                throw new NotImplementedException("DecadeToImageConverter can only convert from String");
            }

            String decadeValue = ((String)value).Substring(2,2);


            String path = null;
            switch (decadeValue)
            {
                case "00":
                    path = "00ts.png";
                    break;
                case "10":
                    path = "10ts.png";
                    break;
                case "20":
                    path = "20ts.png";
                    break;
                case "30":
                    path = "30ts.png";
                    break;
                case "40":
                    path = "40ts.png";
                    break;
                case "50":
                    path = "50ts.png";
                    break;
                case "60":
                    path = "60ts.png";
                    break;
                case "70":
                    path = "70ts.png";
                    break;
                case "80":
                    path = "80ts.png";
                    break;
                case "90":
                    path = "90ts.png";
                    break;
                default:

                    throw new NotSupportedException();

            }

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("/AllMyMusic;component/images/" + path, UriKind.Relative);
            bi.EndInit();
            return bi;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
