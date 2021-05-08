using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel.Composition.Hosting;

using AllMyMusic.ViewModel;

namespace AllMyMusic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow mainWindow;

        [STAThread]
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var vm = new MainWindowViewModel();
            Boolean bResult = vm.Initialize();

            if (bResult == true)
            {
                try
                {
                    EventLogging.Write.Information("Application startup", 0);
                    vm.CloseWindowRequest += new MainWindowViewModel.CloseWindowRequestEventHandler(CloseApplication);
                    mainWindow = new MainWindow();
                    mainWindow.DataContext = vm;
                    mainWindow.Show();
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                    throw;
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void CloseApplication(object sender, EventArgs e)
        {
            if (mainWindow != null)
	        {
                mainWindow.Close();
	        }
            Application.Current.Shutdown();
        }
    }
}

