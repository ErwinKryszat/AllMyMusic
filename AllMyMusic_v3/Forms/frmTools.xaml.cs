using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Threading.Tasks;

//using AllMyMusic.AllSql;
using AllMyMusic.DataService;
using AllMyMusic.Settings;
using AllMyMusic.ViewModel;
using AllMyMusic.View;


namespace AllMyMusic
{
    public delegate void ToolsCallbackDelegate(ToolType toolType, ChangedPropertiesList changedProperties, ObservableCollection<SongItem> songs);

    public partial class frmTools : Window, IDisposable
    {
        #region Fields
        private ToolsViewModel _toolsViewModel;
       

        ToolsCallbackDelegate _callback;
        ToolType _previousToolType;
        #endregion

        public ToolType ToolType
        {
            get { return _toolsViewModel.ToolType; }
        }
        public ChangedPropertiesList ChangedProperties
        {
            get { return _toolsViewModel.ChangedProperties; }
        }
        public ToolsViewModel ToolsVM
        {
            get { return _toolsViewModel; }
        }
        public ObservableCollection<SongItem> Songs
        {
            get { return _toolsViewModel.SongsVM.Songs; }
        }

        #region Form
        public frmTools(ConnectionInfo conInfo, IList SelectedSongs, ToolsCallbackDelegate callback)
        {
            _callback = callback;

            ObservableCollection<SongItem> songs = new ObservableCollection<SongItem>();
            for (int i = 0; i < SelectedSongs.Count; i++)
            {
                songs.Add((SongItem)SelectedSongs[i]);
            }

            _toolsViewModel = new ToolsViewModel(conInfo, songs);
            _toolsViewModel.ToolTypeChanged += new ToolsViewModel.ToolTypeChangedEventHandler(_toolsViewModel_ToolTypeChanged);
            this.DataContext = ToolsVM;

            InitializeComponent();
        }
        public frmTools(ConnectionInfo conInfo, ObservableCollection<SongItem> songs, ToolsCallbackDelegate callback)
        {
            _callback = callback;

            _toolsViewModel = new ToolsViewModel(conInfo, songs);
            _toolsViewModel.ToolTypeChanged += new ToolsViewModel.ToolTypeChangedEventHandler(_toolsViewModel_ToolTypeChanged);
            this.DataContext = ToolsVM;
            
            InitializeComponent();
        }
        public frmTools(ConnectionInfo conInfo, SongItem song, ToolsCallbackDelegate callback)
        {
            _callback = callback;

            _toolsViewModel = new ToolsViewModel(conInfo, song);
            _toolsViewModel.ToolTypeChanged += new ToolsViewModel.ToolTypeChangedEventHandler(_toolsViewModel_ToolTypeChanged);
            this.DataContext = ToolsVM;

            InitializeComponent();
        }
        public frmTools(ConnectionInfo conInfo, String strSongsQuery, ToolsCallbackDelegate callback)
        {
            _callback = callback;

            _toolsViewModel = new ToolsViewModel(conInfo, strSongsQuery);
            _toolsViewModel.ToolTypeChanged += new ToolsViewModel.ToolTypeChangedEventHandler(_toolsViewModel_ToolTypeChanged);
            this.DataContext = ToolsVM;

            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Settings();
            Localize();
            SetupColumnsPropertiesTool();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();

            AppSettings.FormSettings.FrmTools_Position = new Point(this.Left, this.Top);
            AppSettings.FormSettings.FrmTools_Size = new Size(this.Width, this.Height);

            AppSettings.FormSettings.FrmTools_PropertiesViewWidth = _toolsViewModel.PropertiesViewWidth;
            AppSettings.FormSettings.FrmTools_ColumnSelectorVisible = _toolsViewModel.ViewColumnSelector;
            if (this.DataContext is ToolsViewModel)
            {
                ToolsViewModel vm = (ToolsViewModel)this.DataContext;
                vm.Close();
            }
            else
            {
                return;
            }
        }
        #endregion

        #region private
        private void _toolsViewModel_ToolTypeChanged(object sender, ToolTypeEventArgs e)
        {
            SaveSettings();

            switch (e.ToolType)
            {
                case ToolType.PropertiesTool:
                    SetupColumnsPropertiesTool();
                    break;

                case ToolType.AutoTagTool:
                    SetupColumnsAutoTagTool();
                    break;

                case ToolType.RenameTool:
                    SetupColumnsRenameTool();
                    break;

                default:
                    SetupColumnsPropertiesTool();
                    break;
            }
            _previousToolType = e.ToolType;
        }
        private void Settings()
        {
            this.Left = AppSettings.FormSettings.FrmTools_Position.X;
            this.Top = AppSettings.FormSettings.FrmTools_Position.Y;

            if (AppSettings.FormSettings.FrmFolderSelect_Size != new Size(0, 0))
            {
                this.Width = AppSettings.FormSettings.FrmTools_Size.Width;
                this.Height = AppSettings.FormSettings.FrmTools_Size.Height;
            }
        }
        private void Localize()
        {
            cmdOK.Content = AmmLocalization.GetLocalizedString("Common_OK");
            cmdCancel.Content = AmmLocalization.GetLocalizedString("Common_Cancel");
        }
        #endregion

        #region Commands
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            // _callback(_toolsViewModel.ToolType, _toolsViewModel.ChangedProperties, _toolsViewModel.SongsVM.Songs);

            this.DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void cmdApply_Click(object sender, RoutedEventArgs e)
        {
            _callback(_toolsViewModel.ToolType, _toolsViewModel.ChangedProperties, _toolsViewModel.SongsVM.Songs);
        }
        
        #endregion

        #region Setup Columns
        private void SetupColumnsPropertiesTool()
        {
            dataGridTracks.Columns.Clear();

            DataGridTextColumn track = new DataGridTextColumn();
            track.Header = AmmLocalization.GetLocalizedString("Col_Track");
            track.Binding = new Binding("Track");
            DataGridNameProperty.SetName(track, "Track");
            Style style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter
            {
                Property = Control.HorizontalAlignmentProperty,
                Value = HorizontalAlignment.Right
            });
            track.CellStyle = style;
            dataGridTracks.Columns.Add(track);

            DataGridTextColumn albumName = new DataGridTextColumn();
            albumName.Header = AmmLocalization.GetLocalizedString("Col_Album");
            albumName.Binding = new Binding("AlbumName");
            DataGridNameProperty.SetName(albumName, "AlbumName");
            dataGridTracks.Columns.Add(albumName);

            DataGridTextColumn albumGenre = new DataGridTextColumn();
            albumGenre.Header = AmmLocalization.GetLocalizedString("Col_AlbumGenre");
            albumGenre.Binding = new Binding("AlbumGenre");
            DataGridNameProperty.SetName(albumGenre, "AlbumGenre");
            dataGridTracks.Columns.Add(albumGenre);

            DataGridTextColumn artistType = new DataGridTextColumn();
            artistType.Header = AmmLocalization.GetLocalizedString("Col_ArtistType");
            Binding bindArtistType = new Binding("ArtistType");
            bindArtistType.Converter = new ArtistTypeEnumToStringConverter();
            bindArtistType.Mode = BindingMode.TwoWay;
            artistType.Binding = bindArtistType;
            DataGridNameProperty.SetName(artistType, "ArtistType");
            dataGridTracks.Columns.Add(artistType);

            DataGridTextColumn bandName = new DataGridTextColumn();
            bandName.Header = AmmLocalization.GetLocalizedString("Col_Band");
            bandName.Binding = new Binding("BandName");
            DataGridNameProperty.SetName(bandName, "BandName");
            dataGridTracks.Columns.Add(bandName);

            DataGridTextColumn bitrate = new DataGridTextColumn();
            bitrate.Header = AmmLocalization.GetLocalizedString("Col_Bitrate");
            bitrate.Binding = new Binding("Bitrate");
            DataGridNameProperty.SetName(bitrate, "Bitrate");
            dataGridTracks.Columns.Add(bitrate);

            DataGridTextColumn bitrateType = new DataGridTextColumn();
            bitrateType.Header = AmmLocalization.GetLocalizedString("Col_BitrateType");
            bitrateType.Binding = new Binding("BitrateType");
            DataGridNameProperty.SetName(bitrateType, "BitrateType");
            dataGridTracks.Columns.Add(bitrateType);

            DataGridTextColumn comment = new DataGridTextColumn();
            comment.Header = AmmLocalization.GetLocalizedString("Col_Comment");
            comment.Binding = new Binding("Comment");
            DataGridNameProperty.SetName(comment, "Comment");
            dataGridTracks.Columns.Add(comment);

            DataGridTextColumn composerName = new DataGridTextColumn();
            composerName.Header = AmmLocalization.GetLocalizedString("Col_Composer");
            composerName.Binding = new Binding("ComposerName");
            DataGridNameProperty.SetName(composerName, "ComposerName");
            dataGridTracks.Columns.Add(composerName);

            DataGridTextColumn conductorName = new DataGridTextColumn();
            conductorName.Header = AmmLocalization.GetLocalizedString("Col_Conductor");
            conductorName.Binding = new Binding("ConductorName");
            DataGridNameProperty.SetName(conductorName, "ConductorName");
            dataGridTracks.Columns.Add(conductorName);

            DataGridTextColumn country = new DataGridTextColumn();
            country.Header = AmmLocalization.GetLocalizedString("Col_Country");
            country.Binding = new Binding("Country");
            DataGridNameProperty.SetName(country, "Country");
            dataGridTracks.Columns.Add(country);

            DataGridTextColumn filename = new DataGridTextColumn();
            filename.Header = AmmLocalization.GetLocalizedString("Col_Filename");
            filename.Binding = new Binding("SongFilename");
            DataGridNameProperty.SetName(filename, "SongFilename");
            dataGridTracks.Columns.Add(filename);

            DataGridTextColumn genre = new DataGridTextColumn();
            genre.Header = AmmLocalization.GetLocalizedString("Col_Genre");
            genre.Binding = new Binding("Genre");
            DataGridNameProperty.SetName(genre, "Genre");
            dataGridTracks.Columns.Add(genre);

            DataGridTextColumn language = new DataGridTextColumn();
            language.Header = AmmLocalization.GetLocalizedString("Col_Language");
            language.Binding = new Binding("Language");
            DataGridNameProperty.SetName(language, "Language");
            dataGridTracks.Columns.Add(language);

            DataGridTextColumn leadPerformerName = new DataGridTextColumn();
            leadPerformerName.Header = AmmLocalization.GetLocalizedString("Col_LeadPerformer");
            leadPerformerName.Binding = new Binding("LeadPerformer");
            DataGridNameProperty.SetName(leadPerformerName, "LeadPerformer");
            dataGridTracks.Columns.Add(leadPerformerName);

            DataGridTextColumn lengthString = new DataGridTextColumn();
            lengthString.Header = AmmLocalization.GetLocalizedString("Col_Length");
            lengthString.Binding = new Binding("Duration");
            DataGridNameProperty.SetName(lengthString, "Duration");
            dataGridTracks.Columns.Add(lengthString);

            DataGridTextColumn path = new DataGridTextColumn();
            path.Header = AmmLocalization.GetLocalizedString("Col_Path");
            path.Binding = new Binding("SongPath");
            DataGridNameProperty.SetName(path, "SongPath");
            dataGridTracks.Columns.Add(path);

            DataGridTextColumn sampleRate = new DataGridTextColumn();
            sampleRate.Header = AmmLocalization.GetLocalizedString("Col_SampleRate");
            sampleRate.Binding = new Binding("SampleRate");
            DataGridNameProperty.SetName(sampleRate, "SampleRate");
            dataGridTracks.Columns.Add(sampleRate);

            DataGridTextColumn title = new DataGridTextColumn();
            title.Header = AmmLocalization.GetLocalizedString("Col_Title");
            title.Binding = new Binding("SongTitle");
            DataGridNameProperty.SetName(title, "SongTitle");
            dataGridTracks.Columns.Add(title);

            DataGridTextColumn websiteArtist = new DataGridTextColumn();
            websiteArtist.Header = AmmLocalization.GetLocalizedString("Col_Website_Artist");
            websiteArtist.Binding = new Binding("WebsiteArtist");
            DataGridNameProperty.SetName(websiteArtist, "WebsiteArtist");
            dataGridTracks.Columns.Add(websiteArtist);

            DataGridTextColumn websiteUser = new DataGridTextColumn();
            websiteUser.Header = AmmLocalization.GetLocalizedString("Col_Website_User");
            websiteUser.Binding = new Binding("WebsiteUser");
            DataGridNameProperty.SetName(websiteUser, "WebsiteUser");
            dataGridTracks.Columns.Add(websiteUser);

            DataGridTextColumn year = new DataGridTextColumn();
            year.Header = AmmLocalization.GetLocalizedString("Col_Year");
            year.Binding = new Binding("Year");
            DataGridNameProperty.SetName(year, "Year");
            dataGridTracks.Columns.Add(year);


            DataGridTemplateColumn rating = GetRatingColumn();
            DataGridNameProperty.SetName(rating, "Rating");
            dataGridTracks.Columns.Add(rating);

            LoadSettingsPropertiesTool();
        }
        private void SetupColumnsAutoTagTool()
        {
            dataGridTracks.Columns.Clear();

            DataGridTextColumn track = new DataGridTextColumn();
            track.Header = AmmLocalization.GetLocalizedString("Col_Track");
            track.Binding = new Binding("Track");
            DataGridNameProperty.SetName(track, "Track");
            Style style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter
            {
                Property = Control.HorizontalAlignmentProperty,
                Value = HorizontalAlignment.Right
            });
            track.CellStyle = style;
            dataGridTracks.Columns.Add(track);

            DataGridTextColumn title = new DataGridTextColumn();
            title.Header = AmmLocalization.GetLocalizedString("Col_Title");
            title.Binding = new Binding("SongTitle");
            DataGridNameProperty.SetName(title, "SongTitle");
            dataGridTracks.Columns.Add(title);

            DataGridTextColumn bandName = new DataGridTextColumn();
            bandName.Header = AmmLocalization.GetLocalizedString("Col_Band");
            bandName.Binding = new Binding("BandName");
            DataGridNameProperty.SetName(bandName, "BandName");
            dataGridTracks.Columns.Add(bandName);

            DataGridTextColumn albumName = new DataGridTextColumn();
            albumName.Header = AmmLocalization.GetLocalizedString("Col_Album");
            albumName.Binding = new Binding("AlbumName");
            DataGridNameProperty.SetName(albumName, "AlbumName");
            dataGridTracks.Columns.Add(albumName);

            DataGridTextColumn path = new DataGridTextColumn();
            path.Header = AmmLocalization.GetLocalizedString("Col_Path");
            path.Binding = new Binding("SongPath");
            DataGridNameProperty.SetName(path, "SongPath");
            dataGridTracks.Columns.Add(path);

            DataGridTextColumn filename = new DataGridTextColumn();
            filename.Header = AmmLocalization.GetLocalizedString("Col_Filename");
            filename.Binding = new Binding("SongFilename");
            DataGridNameProperty.SetName(filename, "SongFilename");
            dataGridTracks.Columns.Add(filename);

            DataGridTextColumn composerName = new DataGridTextColumn();
            composerName.Header = AmmLocalization.GetLocalizedString("Col_Composer");
            composerName.Binding = new Binding("ComposerName");
            DataGridNameProperty.SetName(composerName, "ComposerName");
            composerName.Visibility = Visibility.Collapsed;
            dataGridTracks.Columns.Add(composerName);

            DataGridTextColumn conductorName = new DataGridTextColumn();
            conductorName.Header = AmmLocalization.GetLocalizedString("Col_Conductor");
            conductorName.Binding = new Binding("ConductorName");
            DataGridNameProperty.SetName(conductorName, "ConductorName");
            conductorName.Visibility = Visibility.Collapsed;
            dataGridTracks.Columns.Add(conductorName);

            DataGridTextColumn country = new DataGridTextColumn();
            country.Header = AmmLocalization.GetLocalizedString("Col_Country");
            country.Binding = new Binding("Country");
            DataGridNameProperty.SetName(country, "Country");
            country.Visibility = Visibility.Collapsed;
            country.Visibility = Visibility.Collapsed;
            dataGridTracks.Columns.Add(country);

            DataGridTextColumn genre = new DataGridTextColumn();
            genre.Header = AmmLocalization.GetLocalizedString("Col_Genre");
            genre.Binding = new Binding("Genre");
            DataGridNameProperty.SetName(genre, "Genre");
            genre.Visibility = Visibility.Collapsed;
            dataGridTracks.Columns.Add(genre);

            DataGridTextColumn lengthString = new DataGridTextColumn();
            lengthString.Header = AmmLocalization.GetLocalizedString("Col_Length");
            lengthString.Binding = new Binding("Duration");
            DataGridNameProperty.SetName(lengthString, "Duration");
            dataGridTracks.Columns.Add(lengthString);

            DataGridTextColumn year = new DataGridTextColumn();
            year.Header = AmmLocalization.GetLocalizedString("Col_Year");
            year.Binding = new Binding("Year");
            DataGridNameProperty.SetName(year, "Year");
            year.Visibility = Visibility.Collapsed;
            dataGridTracks.Columns.Add(year);

            LoadSettingsAutotagTool();
        }
        private void SetupColumnsRenameTool()
        {
            dataGridTracks.Columns.Clear();

            DataGridTextColumn track = new DataGridTextColumn();
            track.Header = AmmLocalization.GetLocalizedString("Col_Track");
            track.Binding = new Binding("Track");
            DataGridNameProperty.SetName(track, "Track");
            Style style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter
            {
                Property = Control.HorizontalAlignmentProperty,
                Value = HorizontalAlignment.Right
            });
            track.CellStyle = style;
            dataGridTracks.Columns.Add(track);

            DataGridTextColumn title = new DataGridTextColumn();
            title.Header = AmmLocalization.GetLocalizedString("Col_Title");
            title.Binding = new Binding("SongTitle");
            DataGridNameProperty.SetName(title, "SongTitle");
            dataGridTracks.Columns.Add(title);

            DataGridTextColumn bandName = new DataGridTextColumn();
            bandName.Header = AmmLocalization.GetLocalizedString("Col_Band");
            bandName.Binding = new Binding("BandName");
            DataGridNameProperty.SetName(bandName, "BandName");
            dataGridTracks.Columns.Add(bandName);

            DataGridTextColumn albumName = new DataGridTextColumn();
            albumName.Header = AmmLocalization.GetLocalizedString("Col_Album");
            albumName.Binding = new Binding("AlbumName");
            DataGridNameProperty.SetName(albumName, "AlbumName");
            dataGridTracks.Columns.Add(albumName);

            DataGridTextColumn path = new DataGridTextColumn();
            path.Header = AmmLocalization.GetLocalizedString("Col_Path");
            path.Binding = new Binding("SongPath");
            DataGridNameProperty.SetName(path, "SongPath");
            dataGridTracks.Columns.Add(path);

            DataGridTextColumn filename = new DataGridTextColumn();
            filename.Header = AmmLocalization.GetLocalizedString("Col_Filename");
            filename.Binding = new Binding("SongFilename");
            DataGridNameProperty.SetName(filename, "SongFilename");
            dataGridTracks.Columns.Add(filename);

            DataGridTextColumn newFullPath = new DataGridTextColumn();
            newFullPath.Header = AmmLocalization.GetLocalizedString("Col_NewFilename");
            newFullPath.Binding = new Binding("NewFullPath");
            DataGridNameProperty.SetName(newFullPath, "NewFullPath");
            dataGridTracks.Columns.Add(newFullPath);

            LoadSettingsRenameTool();
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

        private void SortColumnsByDisplayIndex()
        {
            for (int i = dataGridTracks.Columns.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    DataGridColumn o1 = dataGridTracks.Columns[j - 1];
                    DataGridColumn o2 = dataGridTracks.Columns[j];
                    if (o1.DisplayIndex > o2.DisplayIndex)
                    {
                        dataGridTracks.Columns.Remove(o1);
                        dataGridTracks.Columns.Insert(j, o1);
                    }
                }
            }
        }
        public void SaveSettings()
        {
            switch (_previousToolType)
            {
                case ToolType.PropertiesTool:
                    SaveColumnsPropertiesTool();
                    break;

                case ToolType.AutoTagTool:
                    SaveColumnsAutotagTool();
                    break;

                case ToolType.RenameTool:
                    SaveColumnsRenameTool();
                    break;
            }
        }
        private void SaveColumnsPropertiesTool()
        {
            AppSettings.SongTableConfigPropertiesTool.Clear();

            for (int i = 0; i < dataGridTracks.Columns.Count; i++)
            {
                DataGridColumn column = this.dataGridTracks.Columns[i];
                dgColumnConfig columnCfg = new dgColumnConfig();

                columnCfg.ColumnName = DataGridNameProperty.GetName(column);
                columnCfg.Width = (int)((DataGridLength)column.ActualWidth).Value;

                if (column.Visibility == Visibility.Visible)
                {
                    columnCfg.Visible = true;
                }
                else
                {
                    columnCfg.Visible = false;
                }

                columnCfg.DisplayIndex = column.DisplayIndex;

                AppSettings.SongTableConfigPropertiesTool.Add(columnCfg);
            }
        }
        private void SaveColumnsRenameTool()
        {
            AppSettings.SongTableConfigRenameTool.Clear();

            for (int i = 0; i < dataGridTracks.Columns.Count; i++)
            {
                DataGridColumn column = this.dataGridTracks.Columns[i];
                dgColumnConfig columnCfg = new dgColumnConfig();

                columnCfg.ColumnName = DataGridNameProperty.GetName(column);
                columnCfg.Width = (int)((DataGridLength)column.ActualWidth).Value;

                if (column.Visibility == Visibility.Visible)
                {
                    columnCfg.Visible = true;
                }
                else
                {
                    columnCfg.Visible = false;
                }

                columnCfg.DisplayIndex = column.DisplayIndex;

                AppSettings.SongTableConfigRenameTool.Add(columnCfg);
            }
        }
        private void SaveColumnsAutotagTool()
        {
            AppSettings.SongTableConfigRenameTool.Clear();

            for (int i = 0; i < dataGridTracks.Columns.Count; i++)
            {
                DataGridColumn column = this.dataGridTracks.Columns[i];
                dgColumnConfig columnCfg = new dgColumnConfig();

                columnCfg.ColumnName = DataGridNameProperty.GetName(column);
                columnCfg.Width = (int)((DataGridLength)column.ActualWidth).Value;

                if (column.Visibility == Visibility.Visible)
                {
                    columnCfg.Visible = true;
                }
                else
                {
                    columnCfg.Visible = false;
                }

                columnCfg.DisplayIndex = column.DisplayIndex;

                AppSettings.SongTableConfigRenameTool.Add(columnCfg);
            }
        }

        private void LoadSettingsPropertiesTool()
        {
            foreach (dgColumnConfig columnCfg in AppSettings.SongTableConfigPropertiesTool)
            {
                DataGridColumn column = GetColumnByName(columnCfg.ColumnName);
                if (column != null)
                {
                    column.Width = new DataGridLength((double)columnCfg.Width);

                    if (columnCfg.Visible == true)
                    {
                        column.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        column.Visibility = Visibility.Hidden;
                    }

                    column.DisplayIndex = columnCfg.DisplayIndex;
                }
            }
        }
        private void LoadSettingsRenameTool()
        {
            foreach (dgColumnConfig columnCfg in AppSettings.SongTableConfigRenameTool)
            {
                DataGridColumn column = GetColumnByName(columnCfg.ColumnName);
                if (column != null)
                {
                    column.Width = new DataGridLength((double)columnCfg.Width);

                    if (columnCfg.Visible == true)
                    {
                        column.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        column.Visibility = Visibility.Hidden;
                    }

                    column.DisplayIndex = columnCfg.DisplayIndex;
                }
            }
        }
        private void LoadSettingsAutotagTool()
        {
            foreach (dgColumnConfig columnCfg in AppSettings.SongTableConfigAutotagTool)
            {
                DataGridColumn column = GetColumnByName(columnCfg.ColumnName);
                if (column != null)
                {
                    column.Width = new DataGridLength((double)columnCfg.Width);

                    if (columnCfg.Visible == true)
                    {
                        column.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        column.Visibility = Visibility.Hidden;
                    }

                    column.DisplayIndex = columnCfg.DisplayIndex;
                }
            }
        }
        private DataGridColumn GetColumnByName(String columnName)
        {
            foreach (DataGridColumn column in dataGridTracks.Columns)
            {
                if (DataGridNameProperty.GetName(column) == columnName)
                {
                    return column;
                }
            }
            return null;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();
        }

        ~frmTools()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_toolsViewModel != null)
            {
                _toolsViewModel.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion

    }
}
