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

using System.IO;

using AllMyMusic.ViewModel;

namespace AllMyMusic.View
{
    /// <summary>
    /// Interaction logic for SqlQueryPanel.xaml
    /// </summary>
    public partial class viewSqlPlaylistPanel : UserControl
    {
        public viewSqlPlaylistPanel()
        {
            InitializeComponent();
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SqlPlaylistViewModel vm = (SqlPlaylistViewModel)this.DataContext;
            if (vm != null)
            {
                vm.TextBoxSelectionStart = sqlQueryTexBox1.SelectionStart;
            }
        }

        private void sqlQueryTexBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.Count > 0)
            {
                TextChange c = e.Changes.FirstOrDefault<TextChange>();
                if (c.AddedLength > 1)
                {
                    sqlQueryTexBox1.SelectionStart = sqlQueryTexBox1.Text.Length;
                }
            }
        }        
    }
}
