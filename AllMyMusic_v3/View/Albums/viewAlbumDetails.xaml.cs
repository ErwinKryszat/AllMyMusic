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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;


namespace AllMyMusic.View
{
    public partial class viewAlbumDetails : UserControl
    {
        public viewAlbumDetails()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(e.Uri.AbsoluteUri) == false)
                {
                    Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                }
            }
            catch (Exception)
            {


            }


            e.Handled = true;
        }
    }
}
