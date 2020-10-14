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

using AllMyMusic_v3.ViewModel;

namespace AllMyMusic_v3.View
{
    /// <summary>
    /// Interaction logic for viewPager.xaml
    /// </summary>
    public partial class viewPropertiesEditor : UserControl
    {
        public viewPropertiesEditor()
        {
            InitializeComponent();
        }

        private void bandSortName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!((PropertiesToolViewModel)this.DataContext).BandSortNameClickedCommand.CanExecute(null)) return;

            //Execute the command
            ((PropertiesToolViewModel)this.DataContext).BandSortNameClickedCommand.Execute(null);
        }
    }
}
