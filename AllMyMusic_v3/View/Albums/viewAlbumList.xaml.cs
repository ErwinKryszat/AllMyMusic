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



namespace AllMyMusic_v3.View
{
    public partial class viewAlbumList : UserControl
    {
        public viewAlbumList()
        {
            InitializeComponent();
        }

        private void albumList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                AlbumItem album = (AlbumItem)e.AddedItems[0];
                AlbumItemEventArgs args = new AlbumItemEventArgs(album);
                OnAlbumItemSelected(this, args);
            }
        }

        #region Events
        public delegate void AlbumItemSelectedEventHandler(object sender, AlbumItemEventArgs e);

        public event AlbumItemSelectedEventHandler AlbumItemSelected;
        protected virtual void OnAlbumItemSelected(object sender, AlbumItemEventArgs e)
        {
            if (this.AlbumItemSelected != null)
            {
                this.AlbumItemSelected(this, e);
            }
        }
        #endregion // Events


    }
}
