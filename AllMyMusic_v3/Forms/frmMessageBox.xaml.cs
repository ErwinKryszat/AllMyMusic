using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AllMyMusic_v2.AmmForms
{
    /// <summary>
    /// Interaction logic for frmMessageBox.xaml
    /// </summary>
    public partial class frmMessageBox : Window
    {
        public frmMessageBox(String caption, String message, Exception exception, MessageBoxImage imageType)
        {
            InitializeComponent();
            this.Title = caption;
            textBoxMessage.Text = message;
            textBoxException.Text = exception.ToString();

            
            if (imageType == MessageBoxImage.Error)
            {
                BitmapImage newImage = new BitmapImage(new Uri("/AllMyMusic_v2;component/images/error.png", UriKind.Relative));
                infoIcon.Source = newImage;
            }
            if (imageType == MessageBoxImage.Warning)
            {
                BitmapImage newImage = new BitmapImage(new Uri("/AllMyMusic_v2;component/images/warning.png", UriKind.Relative));
                infoIcon.Source = newImage;
            }

            if (imageType == MessageBoxImage.Question)
            {
                BitmapImage newImage = new BitmapImage(new Uri("/AllMyMusic_v2;component/images/help.png", UriKind.Relative));
                infoIcon.Source = newImage;
            }
        }

        public frmMessageBox(String message, Exception exception)
        {
            InitializeComponent();
            this.Title = "Error";
            textBoxMessage.Text = message;

            if (exception != null)
            {
                textBoxException.Text = exception.ToString();
            }
            

            BitmapImage newImage = new BitmapImage(new Uri("/AllMyMusic_v2;component/images/error.png", UriKind.Relative));
            infoIcon.Source = newImage;

        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
