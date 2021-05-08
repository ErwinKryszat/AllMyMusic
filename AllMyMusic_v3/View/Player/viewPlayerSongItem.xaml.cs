using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AllMyMusic.ViewModel;

namespace AllMyMusic.View
{
    /// <summary>
    /// Interaction logic for viewPlayerSongItem.xaml
    /// </summary>
    public partial class viewPlayerSongItem : UserControl
    {
        public viewPlayerSongItem()
        {
            InitializeComponent();
        }

        private void RatingSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext is AudioPlayerViewModel)
            {
                AudioPlayerViewModel avm = (AudioPlayerViewModel)this.DataContext;
                avm.Song.Rating = ratingSlider.TagValue;
            }
        }
    }
}
