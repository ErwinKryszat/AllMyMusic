using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using AllMyMusic.Settings;
using AllMyMusic.ViewModel;


namespace AllMyMusic.View
{
    public partial class viewAlbum : UserControl
    {
        public viewAlbum()
        {
            InitializeComponent();
        }

        private void viewAlbum1_Loaded(object sender, RoutedEventArgs e)
        {
            SetupColumnsAlbumView();
        }

        private void SetupColumnsAlbumView()
        {
            DataGridTextColumn track = (DataGridTextColumn)dataGridTracks.Columns[0];
            track.Header = AmmLocalization.GetLocalizedString("Col_Track");
            DataGridNameProperty.SetName(track, "Track");
            Style style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter
            {
                Property = Control.HorizontalAlignmentProperty,
                Value = HorizontalAlignment.Right
            });
            track.CellStyle = style;
            

            DataGridTextColumn bandName = (DataGridTextColumn)dataGridTracks.Columns[1];
            bandName.Header = AmmLocalization.GetLocalizedString("Col_Band");
            DataGridNameProperty.SetName(bandName, "BandName");

            DataGridTextColumn title = (DataGridTextColumn)dataGridTracks.Columns[2];
            title.Header = AmmLocalization.GetLocalizedString("Col_Title");
            DataGridNameProperty.SetName(title, "SongTitle");

            DataGridTextColumn lengthString = (DataGridTextColumn)dataGridTracks.Columns[3];
            lengthString.Header = AmmLocalization.GetLocalizedString("Col_Length");
            DataGridNameProperty.SetName(lengthString, "Duration");

            DataGridTemplateColumn rating = (DataGridTemplateColumn)dataGridTracks.Columns[4];
            rating.Header = AmmLocalization.GetLocalizedString("Col_RatingValue");
            rating.SortMemberPath = "Rating";
            DataGridNameProperty.SetName(rating, "Rating");

            //if (dataGridTracks.Columns.Count == 0)
            //{
            //    DataGridTextColumn track = new DataGridTextColumn();
            //    track.Header = AmmLocalization.GetLocalizedString("Col_Track");
            //    track.Binding = new Binding("Track");
            //    Style style = new Style(typeof(DataGridCell));
            //    style.Setters.Add(new Setter
            //    {
            //        Property = Control.HorizontalAlignmentProperty,
            //        Value = HorizontalAlignment.Right
            //    });
            //    track.CellStyle = style;
            //    DataGridNameProperty.SetName(track, "Track");
            //    dataGridTracks.Columns.Add(track);

            //    DataGridTextColumn bandName = new DataGridTextColumn();
            //    bandName.Header = AmmLocalization.GetLocalizedString("Col_Band");
            //    bandName.Binding = new Binding("BandName");
            //    DataGridNameProperty.SetName(bandName, "BandName");
            //    dataGridTracks.Columns.Add(bandName);

            //    DataGridTextColumn title = new DataGridTextColumn();
            //    title.Header = AmmLocalization.GetLocalizedString("Col_Title");
            //    title.Binding = new Binding("SongTitle");
            //    DataGridNameProperty.SetName(title, "SongTitle");
            //    dataGridTracks.Columns.Add(title);

            //    DataGridTextColumn lengthString = new DataGridTextColumn();
            //    lengthString.Header = AmmLocalization.GetLocalizedString("Col_Length");
            //    lengthString.Binding = new Binding("Duration");
            //    lengthString.IsReadOnly = true;
            //    DataGridNameProperty.SetName(lengthString, "Duration");
            //    dataGridTracks.Columns.Add(lengthString);

            //    DataGridTemplateColumn rating = GetRatingColumn();
            //    DataGridNameProperty.SetName(rating, "Rating");
            //    dataGridTracks.Columns.Add(rating);
            //}
        }

        private DataGridTemplateColumn GetRatingColumn()
        {
            // Create The Column
            DataGridTemplateColumn ratingColumn = new DataGridTemplateColumn();
            ratingColumn.Header = AmmLocalization.GetLocalizedString("Col_RatingValue");
            ratingColumn.CanUserSort = true;
            ratingColumn.SortDirection = ListSortDirection.Ascending;
            ratingColumn.SortMemberPath = "Rating";

            Binding bind = new Binding("Rating");
            bind.Converter = new RatingImageConverter();
            bind.Mode = BindingMode.TwoWay;

            // Create the Image
            FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));
            imageFactory.SetBinding(Image.SourceProperty, bind);
            DataTemplate imageTemplate = new DataTemplate();
            imageTemplate.VisualTree = imageFactory;


            // Set the Templates to the Column
            ratingColumn.CellTemplate = imageTemplate;

            return ratingColumn;
        }

        private void dataGridTracks_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            String columnName = e.Column.SortMemberPath;

            String cellValue = String.Empty;
            Int32 rating = 0;

            SongItem song = (SongItem)e.Row.DataContext;
            Boolean songChanged = false;

            switch (columnName)
            {
                case "BandName":
                    cellValue = (e.EditingElement as TextBox).Text;
                    if (song.BandName != cellValue)
                    {
                        songChanged = true;
                        song.BandName = cellValue;
                    }
                    
                    break;

                case "SongTitle":
                    cellValue = (e.EditingElement as TextBox).Text;
                    if (song.SongTitle != cellValue)
                    {
                        songChanged = true;
                        song.SongTitle = cellValue;
                    }
                    
                    break;

                case "Track":
                    cellValue = (e.EditingElement as TextBox).Text;
                    if (song.Track != cellValue)
                    {
                        songChanged = true;
                        song.Track = cellValue;
                    }
                    
                    break;

                case "Rating":
                    DataGridTemplateColumn ratingColumn = (DataGridTemplateColumn)e.Column;
                    ContentPresenter presenter = (ContentPresenter)e.Column.GetCellContent(e.Row);
                    RatingSlider rt = (RatingSlider)ratingColumn.CellEditingTemplate.FindName("ratingSlider1", presenter);
                    rating = rt.TagValue;
                    if (song.Rating != rating)
                    {
                        songChanged = true;
                        song.Rating = rating;
                    }
                    
                    break;
            }

            if (songChanged == true)
            {
                AlbumViewModel av = (AlbumViewModel)this.DataContext;
                av.EditSong = song;
            }
        }
    }
}
