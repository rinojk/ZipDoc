using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZipDoc.View;
using ZipDoc.ViewModel;

namespace ZipDoc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            if (string.IsNullOrEmpty(ZipDoc.Properties.Settings.Default.Token))
            {
                LoginViewModel logVM = new LoginViewModel();
                var loginVM = new LoginView
                {
                    DataContext = logVM
                };
                logVM.thisview = loginVM;
                loginVM.Show();
            }
            else
            {
                var mainVM = new MainWindowView
                {
                    DataContext = new MainWindowViewModel()
                };
                mainVM.Show();
            }
        }
    }
}
