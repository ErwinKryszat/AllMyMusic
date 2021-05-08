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
    public class CustomDataGrid : DataGrid
    {

        public CustomDataGrid()
        {
            this.SelectionChanged += CustomDataGrid_SelectionChanged;
            this.Sorting += CustomDataGrid_Sorting;

            MyDefaults();
        }

        private void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
        }

        private void MyDefaults()
        { 
            this.IsReadOnly=true;
            this.AutoGenerateColumns= false;
            this.AlternatingRowBackground = new SolidColorBrush(Colors.WhiteSmoke);
            this.GridLinesVisibility = DataGridGridLinesVisibility.None;
            this.HeadersVisibility = DataGridHeadersVisibility.Column;
            this.CanUserAddRows = false;
        }

        #region SelectedItemsList
        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CustomDataGrid), new PropertyMetadata(null));
        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty ChangedSongProperty = DependencyProperty.Register("ChangedSong", typeof(SongItem), typeof(CustomDataGrid), new PropertyMetadata(null));
        public SongItem ChangedSong
        {
            get { return (SongItem)GetValue(ChangedSongProperty); }
            set { SetValue(ChangedSongProperty, value); }
        }
        #endregion
  
        protected override void OnColumnReordered(DataGridColumnEventArgs e)
        {
            base.OnColumnReordered(e);
            Int32 newIndex = e.Column.DisplayIndex;
        }

        private void CustomDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataGridColumn column = e.Column;

            // I'm only interested in a custom sort for the Track column
            if (column.SortMemberPath != "Track") return;

            // Prevent the built-in sort from sorting 
            e.Handled = true;


            ListSortDirection direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
            IComparer comparer = new TrackColumnsSorter(direction);

            ICollectionView listView = CollectionViewSource.GetDefaultView(this.ItemsSource);
            ListCollectionView listCollectionView = (ListCollectionView)listView;
            listCollectionView.CustomSort = new TrackColumnsSorter(direction);
        }

        #region Bindable Columns
        public static readonly DependencyProperty BindableColumnsProperty = DependencyProperty.RegisterAttached("BindableColumns",
                            typeof(ObservableCollection<DataGridColumn>),
                            typeof(CustomDataGrid),
                            new UIPropertyMetadata(null, BindableColumnsPropertyChanged));

        public ObservableCollection<DataGridColumn> BindableColumns
        {
            get { return (ObservableCollection<DataGridColumn>)GetValue(BindableColumnsProperty); }
            set { SetValue(BindableColumnsProperty, value); }
        }

        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = source as DataGrid;
            ObservableCollection<DataGridColumn> columns = e.NewValue as ObservableCollection<DataGridColumn>;
            dataGrid.Columns.Clear();
            if ((columns == null) || (columns.Count == 0))
            {
                return;
            }
            foreach (DataGridColumn column in columns)
            {
                dataGrid.Columns.Add(column);
            }
            columns.CollectionChanged += (sender, e2) =>
            {
                NotifyCollectionChangedEventArgs ne = e2 as NotifyCollectionChangedEventArgs;
                if (ne.Action == NotifyCollectionChangedAction.Reset)
                {
                    dataGrid.Columns.Clear();
                }
                else if (ne.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (DataGridColumn column in ne.NewItems)
                    {
                        dataGrid.Columns.Add(column);
                    }
                }
                else if (ne.Action == NotifyCollectionChangedAction.Move)
                {
                    dataGrid.Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);
                }
                else if (ne.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (DataGridColumn column in ne.OldItems)
                    {
                        dataGrid.Columns.Remove(column);
                    }
                }
                else if (ne.Action == NotifyCollectionChangedAction.Replace)
                {
                    dataGrid.Columns[ne.NewStartingIndex] = ne.NewItems[0] as DataGridColumn;
                }
            };
        }
       
        #endregion


        

    }
}
