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
using System.Windows.Shapes;
using System.Windows.Forms;



using System.IO;
using AllMyMusic_v3.Settings;

namespace AllMyMusic_v3
{
    /// <summary>
    /// Interaction logic for FrmCoverImage.xaml
    /// </summary>
    public partial class frmCoverImage : Window
    {
        private double aspectRatio = 0.0;
        double clientWidth = -1;
        double clientHeight = -1;

        double windowFrameWidth;
        double windowFrameHeight;

        private AlbumItem album;
        private SongItem song;
        private ImageType imageType;
        private String imagePath;

        public frmCoverImage(AlbumItem album)
        {
            InitializeComponent();
            this.album = album;
        }

        public frmCoverImage(SongItem song)
        {
            InitializeComponent();
            this.song = song;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
            LoadCoverImage(ImageType.Frontal);
        }
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AppSettings.FormSettings.FrmManageCoverImages_Position = new Point(this.Left, this.Top);
            AppSettings.FormSettings.FrmManageCoverImages_Size = new Size(this.Width, this.Height);
        }

        private void LoadSettings()
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = AppSettings.FormSettings.FrmManageCoverImages_Position.X;
            this.Top = AppSettings.FormSettings.FrmManageCoverImages_Position.Y;

            if (AppSettings.FormSettings.FrmMain_Size != new Size(0, 0))
            {
                this.Width = AppSettings.FormSettings.FrmManageCoverImages_Size.Width;
                this.Height = AppSettings.FormSettings.FrmManageCoverImages_Size.Height;
            }
            else
            {
                this.Width = 500;
                this.Height = 700;
            }
        }

        private void LoadCoverImage(ImageType imageType)
        {
            try
            {
                this.imageType = imageType;
                this.imagePath = GetImagePath(this.imageType);
                if (String.IsNullOrEmpty(this.imagePath) == false)
                {
                    BitmapImage bitmap = GetImage(this.imagePath);
                    coverImage.Source = bitmap;

                    GetWindowFrameSize();

                    System.Drawing.Size MonitorSize = SystemInformation.PrimaryMonitorSize;
                    if (MonitorSize.Height < bitmap.PixelHeight)
                    {
                        this.Height = MonitorSize.Height - 60;
                        this.Width = (MonitorSize.Height - 60) * ((double)bitmap.PixelWidth / (double)bitmap.PixelHeight);

                    }
                    else
                    {
                        this.Height = windowFrameHeight + bitmap.PixelHeight;
                        this.Width = windowFrameWidth + bitmap.PixelWidth;
                    }
                    aspectRatio = ((double)bitmap.PixelWidth / (double)bitmap.PixelHeight);
                    this.Title = "Dimensions: " + bitmap.PixelWidth.ToString() + " x " + bitmap.PixelHeight.ToString() + " Path: " + this.imagePath;
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error Loading CoverImage ";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (imageType == ImageType.Frontal)
            {
                LoadCoverImage(ImageType.Back);
            }
            else
            {
                LoadCoverImage(ImageType.Frontal);
            }
        }
       
        private BitmapImage GetImage(String localPath)
        {
            BitmapImage bi = null;
            try
            {
                Uri uri;
                if (File.Exists(localPath) == false)
                {
                    // The default image
                    uri = new Uri(Global.Images + "cover.jpg", UriKind.Relative);
                }
                else
                {
                    uri = new Uri(localPath);
                }

                bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = uri;
                bi.EndInit();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error Loading CoverImage from: " + localPath;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            
            return bi;
        }
        private String GetImagePath(ImageType imageType)
        {
            if (album != null)
            {
                if (imageType == ImageType.Frontal)
                {
                    return album.FrontImageFullpath;
                }
                else
                {
                    return album.BackImageFullpath;
                }
            }

            if (song != null)
            {
                if (imageType == ImageType.Frontal)
                {
                    return song.FrontImageFullpath;
                }
                else
                {
                    return song.BackImageFullpath;
                }
            }

            return String.Empty;
        }
        private void GetWindowFrameSize()
        {
            FrameworkElement pnlClient = this.Content as FrameworkElement;
            if (pnlClient != null)
            {
                clientWidth = pnlClient.ActualWidth;
                clientHeight = pnlClient.ActualHeight;

                windowFrameWidth = this.Width - clientWidth;
                windowFrameHeight = this.Height - clientHeight;
            }
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (aspectRatio != 0)
            {
                if (sizeInfo.HeightChanged)
                {
                    this.Height = sizeInfo.NewSize.Height;
                }
                else
                {
                    this.Width = sizeInfo.NewSize.Width;
                }
            }  
        }

       
    }
}
