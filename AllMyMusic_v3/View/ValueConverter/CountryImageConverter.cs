using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Resources;

namespace AllMyMusic_v3.View
{
    public class CountryImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if ((String)value != "")
                {
                    value = new Uri((string)value);
                }
            }

            if (value is Uri)
            {
                String localPath = ((Uri)value).LocalPath;
                if (File.Exists(localPath) == true)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = (Uri)value;
                    bi.EndInit();
                    return bi;
                }
                else
                {
                    // "/AllMyMusic_v3;component/Images/cover.jpg"
                    //value = new Uri(Global.Images + "world_64.png", UriKind.Relative);
                    Uri imageReference = new Uri(Global.Images + "world_64.png", UriKind.Relative);
                    StreamResourceInfo sri = Application.GetResourceStream(imageReference);
                    BitmapImage bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.StreamSource = sri.Stream;
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.EndInit();
                    bmp.Freeze();
                    return bmp;
                }
            }

            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
