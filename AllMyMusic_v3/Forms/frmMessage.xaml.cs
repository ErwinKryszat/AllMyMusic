using System;
using System.Collections;
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

using AllMyMusic_v3.Settings;
using AllMyMusic_v3.ViewModel;


namespace AllMyMusic_v3
{
    public partial class frmMessage : Window
    {
        #region Form
        public frmMessage()
        {
            InitializeComponent();
        }
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Settings();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AppSettings.FormSettings.FrmMessage_Position = new Point(this.Left, this.Top);
            AppSettings.FormSettings.FrmMessage_Size = new Size(this.Width, this.Height);
        }

        private void Settings()
        {
            this.Left = AppSettings.FormSettings.FrmMessage_Position.X;
            this.Top = AppSettings.FormSettings.FrmMessage_Position.Y;

            if (AppSettings.FormSettings.FrmMessage_Size != new Size(0, 0))
            {
                this.Width = AppSettings.FormSettings.FrmMessage_Size.Width;
                this.Height = AppSettings.FormSettings.FrmMessage_Size.Height;
            }
        }
        #endregion

        private void cmd_OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void cmd_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
