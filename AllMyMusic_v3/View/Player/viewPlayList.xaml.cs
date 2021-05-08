using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;




namespace AllMyMusic.View
{
    public partial class viewPlayList : UserControl
    {



        public viewPlayList()
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

        private void albumList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
