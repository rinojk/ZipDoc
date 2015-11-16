﻿using System;
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
                var loginVM = new LoginView
                {
                    DataContext = new LoginViewModel()
                };
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
