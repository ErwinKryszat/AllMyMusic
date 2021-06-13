using System;
using System.Windows;
using System.Windows.Controls.Ribbon;
using AllMyMusic.ViewModel;
using AllMyMusic.Settings;

namespace AllMyMusic
{
   

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();

            EventLogging.Write.Information("Application closed", 0);
        }

        private void LoadSettings()
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = AppSettings.FormSettings.FrmMain_Position.X;
            this.Top = AppSettings.FormSettings.FrmMain_Position.Y;

            if (AppSettings.FormSettings.FrmMain_Size != new Size(0, 0))
            {
                this.Width = AppSettings.FormSettings.FrmMain_Size.Width;
                this.Height = AppSettings.FormSettings.FrmMain_Size.Height;
            }
            else
            {
                this.Width = 1280;
                this.Height = 900;
            }
        }
        private void SaveSettings()
        {
            MainWindowViewModel vm = (MainWindowViewModel)this.DataContext;
            vm.CloseViewModels();

            AppSettings.FormSettings.FrmMain_Position = new Point(this.Left, this.Top);
            AppSettings.FormSettings.FrmMain_Size = new Size(this.Width, this.Height);
            AppSettings.Save();
        }

        private void CloseWindowRequest(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}