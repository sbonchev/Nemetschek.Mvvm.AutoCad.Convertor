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
            lbFiles.SelectionChanged += (o,e) => layerViewModel.SelectionLayerChanged();
        }

    }
}