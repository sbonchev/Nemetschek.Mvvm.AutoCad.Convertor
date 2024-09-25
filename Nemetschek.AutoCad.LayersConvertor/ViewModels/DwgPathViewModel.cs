using Nemetschek.AutoCad.LayersConvertor.Models;
using System.Collections.ObjectModel;

namespace Nemetschek.AutoCad.LayersConvertor.ViewModels
{
    public class DwgPathViewModel : BaseViewModel
    {
        public DwgPathViewModel()
        {
            DwgPaths = new ObservableCollection<DwgPathModel>();
        }
        public ObservableCollection<DwgPathModel>? DwgPaths { get; set; }

        private DwgPathModel? _path;
        public DwgPathModel? DwgSelectedPath
        {
            get => _path ?? (_path = new DwgPathModel());
            set
            {
                if (_path == value)
                    return;

                _path = value;
                OnPropertyChanged(nameof(DwgSelectedPath));
            }
        }
    }
}
