using System.Windows;
using Nemetschek.AutoCad.LayersConvertor.ViewModels;


namespace Nemetschek.AutoCad.LayersConvertor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LayerConvertorWindow : Window
    {
        public LayerConvertorWindow()
        {
            InitializeComponent();
            var layerViewModel = new LayerViewModel();
            DataContext = layerViewModel;

            AppWindow = this;
        }

        private static LayerConvertorWindow? _lcw;
        public static LayerConvertorWindow? AppWindow
        {
            get
            {
                if (_lcw == null)
                {
                   _lcw = new LayerConvertorWindow();
                }

                return _lcw;
            }
            set
            {
                _lcw = value;
            }
        }

    }
}