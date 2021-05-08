using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace AllMyMusic.View
{
    public class CustomListBox : ListBox
    {
        private Boolean _selectionChangedInProgress;

        public static readonly DependencyProperty SelectedAlbumsProperty = DependencyProperty.Register(
            "SelectedAlbums",
            typeof(ObservableCollection<AlbumItem>),
            typeof(CustomListBox),
            new PropertyMetadata(new ObservableCollection<AlbumItem>(), PropertyChangedCallback));

        public ObservableCollection<AlbumItem> SelectedAlbums
        {
            get { return (ObservableCollection<AlbumItem>)GetValue(SelectedAlbumsProperty); }
            set { SetValue(SelectedAlbumsProperty, value); }
        }

        public CustomListBox()
        {
            this.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectionChangedInProgress) return;
            _selectionChangedInProgress = true;
            
            foreach (var item in e.RemovedItems)
            {
                if (SelectedAlbums.Contains((AlbumItem)item))
                {
                    SelectedAlbums.Remove((AlbumItem)item);
                }
            }

            if ((e.AddedItems.Count > 0) && (SelectedAlbums == null))
            {
                this.SelectedAlbums = new ObservableCollection<AlbumItem>();
            }

            foreach (var item in e.AddedItems)
            {
                if (!SelectedAlbums.Contains((AlbumItem)item))
                {
                    SelectedAlbums.Add((AlbumItem)item);
                }
            }
            _selectionChangedInProgress = false;
        }


        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NotifyCollectionChangedEventHandler handler = (s, e) => SelectedAlbumsChanged(sender, e);
            if (args.OldValue is ObservableCollection<AlbumItem>)
            {
                (args.OldValue as ObservableCollection<AlbumItem>).CollectionChanged -= handler;
            }

            if (args.NewValue is ObservableCollection<AlbumItem>)
            {
                (args.NewValue as ObservableCollection<AlbumItem>).CollectionChanged += handler;
            }
        }

        private static void SelectedAlbumsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is CustomListBox)
            {
                CustomListBox listBoxBase = (CustomListBox)sender;

                var listSelectedAlbums = listBoxBase.SelectedAlbums;
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (listSelectedAlbums.Contains((AlbumItem)item))
                        {
                            listSelectedAlbums.Remove((AlbumItem)item);
                        }
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (!listSelectedAlbums.Contains((AlbumItem)item))
                        {
                            listSelectedAlbums.Add((AlbumItem)item);
                        }
                    }
                }
            }
        }

       
    }
}
