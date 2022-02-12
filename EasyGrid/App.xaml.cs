﻿using CommunityToolkit.Mvvm.DependencyInjection;
using EasyGrid.Core;
using EasyGrid.ViewModels;
using EasyGrid.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EasyGrid
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    //Page ViewModels
                    .AddTransient<CreateViewModel>()
                    .AddTransient<SettingsViewModel>()
                    //Windows
                    .AddSingleton<MainViewModel>()
                    .AddSingleton<IViewFactory, MappingViewFactory>()
                    .BuildServiceProvider());

            var window = new MainWindow { DataContext = Ioc.Default.GetRequiredService<MainViewModel>() };
            window.Show();
        }
    }
}
