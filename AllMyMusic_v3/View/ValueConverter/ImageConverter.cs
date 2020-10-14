using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Resources;

namespace AllMyMusic_v3.View
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if ((String)value != "")
                {
                    if (File.Exists((String)value) == true)
                    {
                        value = new Uri((string)value);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.CreateOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;  
                        bi.UriSource = (Uri)value;
                        bi.EndInit();
                        bi.Freeze();
                        return bi;
                    }
                    else
                    {
                        // "/AllMyMusic_v3;component/Images/cover.jpg"
                        //value = new Uri(Global.Images + "cover.jpg", UriKind.Relative);
                        Uri imageReference = new Uri(Global.Images + "cover.jpg", UriKind.Relative);
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
            }

            // "/AllMyMusic_v3;component/Images/cover.jpg"
            value = new Uri(Global.Images + "cover.jpg", UriKind.Relative);
            Uri imageReference2 = new Uri(Global.Images + "cover.jpg", UriKind.Relative);
            StreamResourceInfo sri2 = Application.GetResourceStream(imageReference2);
            BitmapImage bmp2 = new BitmapImage();
            bmp2.BeginInit();
            bmp2.StreamSource = sri2.Stream;
            bmp2.CacheOption = BitmapCacheOption.OnLoad;
            bmp2.EndInit();
            bmp2.Freeze();
            return bmp2;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
