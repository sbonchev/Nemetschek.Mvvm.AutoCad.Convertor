using Microsoft.Extensions.DependencyInjection;
using Nemetschek.AutoCad.LayersConvertor.Services;
using Nemetschek.AutoCad.LayersConvertor.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Nemetschek.AutoCad.LayersConvertor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
          
        }
        //private void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddTransient<ILayerService, LayerService>();
        //    services.AddTransient<LayerViewModel>();
        //    services.AddTransient<ICommand, RelayRibbonCommand>();
        //    //services.AddSingleton<LayerConvertorWindow>();
        //    var serviceProvider = services.BuildServiceProvider();

        //    //var mainWindow = serviceProvider.GetRequiredService<LayerConvertorWindow>();
        //    //mainWindow?.Show();
        //    //new LayerConvertorWindow().ShowDialog();
        //}

        //private void OnStartupEvent(StartupEventArgs e)
        //{
        //    //var mainWindow = serviceProvider.GetService<LayerConvertorWindow>();
        //    //mainWindow?.Show();
        //}

    }

}
