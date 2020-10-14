using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for frmAmmTemplate.xaml
    /// </summary>
    public partial class frmPartyButtonDesigner : Window
    {
        #region Fields
        private PartyButtonDesignerViewModel _partyButtonDesignerViewModel;

        #endregion

        #region Form
        public frmPartyButtonDesigner(PartyButtonDesignerViewModel partyButtonDesignerViewModel)
        {            
            _partyButtonDesignerViewModel = partyButtonDesignerViewModel;
            this.DataContext = _partyButtonDesignerViewModel;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Settings();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _partyButtonDesignerViewModel.Close();

            AppSettings.FormSettings.FrmPartyButtonDesigner_Position = new Point(this.Left, this.Top);
            AppSettings.FormSettings.FrmPartyButtonDesigner_Size = new Size(this.Width, this.Height);
        }

        private void Settings()
        {
            this.Left = AppSettings.FormSettings.FrmPartyButtonDesigner_Position.X;
            this.Top = AppSettings.FormSettings.FrmPartyButtonDesigner_Position.Y;

            if (AppSettings.FormSettings.FrmPartyButtonDesigner_Size != new Size(0, 0))
            {
                this.Width = AppSettings.FormSettings.FrmPartyButtonDesigner_Size.Width;
                this.Height = AppSettings.FormSettings.FrmPartyButtonDesigner_Size.Height;
            }
        }
        #endregion

        #region Commands
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion


    }
}
