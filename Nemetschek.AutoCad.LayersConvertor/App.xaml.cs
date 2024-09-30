using Microsoft.Extensions.DependencyInjection;
using Nemetschek.AutoCad.LayersConvertor.ViewModels;
using System.Windows;

namespace Nemetschek.AutoCad.LayersConvertor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            //services.AddDbContext<ProductContext>(options =>
            //{});
            services.AddSingleton<LayerViewModel>(); // todo
        }
    }

}
