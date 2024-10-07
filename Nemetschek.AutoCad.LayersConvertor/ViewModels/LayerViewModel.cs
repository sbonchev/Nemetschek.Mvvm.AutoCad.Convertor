using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Input;
using Nemetschek.AutoCad.LayersConvertor.Models;
using Nemetschek.AutoCad.LayersConvertor.Services;
using System.Collections.ObjectModel;
using Nemetschek.AutoCad.LayersConvertor.Commands;
using System.ComponentModel;

namespace Nemetschek.AutoCad.LayersConvertor.ViewModels
{
    /// <summary>
    /// Select drawing files and layers' processing
    /// </summary>
    public class LayerViewModel : BaseViewModel
    {
        public LayerViewModel()
        {

            _worker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            _worker.DoWork += DoWork;
            _worker.ProgressChanged += ProgressChanged;
            _worker.RunWorkerCompleted += WorkerCompleted;

            FromLayerNames = new ObservableCollection<LayerModel>();
            ToLayerNames = new ObservableCollection<LayerModel>();

            _layerService = new LayerService();
            _dwgPath = new DwgPathViewModel();
            _info = new InfoViewModel();

            FileCommand = new RelayCommand(OpenFiles, fc => true);
            ProcessCommand = new RelayCommand(o => RunWorker(), pc => _dwgPath!.DwgPaths!.Count > 0
                                && FromLayerNames!.Count > 0
                                && SelectedLayerFrom != null
                                && SelectedLayerTo != null
                                && SelectedLayerFrom.LayerName != SelectedLayerTo.LayerName 
                                && !_worker.IsBusy);
            CancelCommand = new RelayCommand(Cancel, cc => true);
            GetInfo.ProcessColor = new SolidColorBrush(Colors.DarkGray);
            GetInfo.TextPath = "Select drawing file(s)!";
        }

        private readonly BackgroundWorker _worker;

        private readonly LayerService _layerService;

        private readonly DwgPathViewModel _dwgPath;

        private readonly InfoViewModel _info;

        private LayerModel? _layerFrom;

        private LayerModel? _layerTo;

        private DwgPathModel? _dwgPathSelected;

        private bool _isClear;

        public ObservableCollection<LayerModel>? FromLayerNames { get; set; }

        public ObservableCollection<LayerModel>? ToLayerNames { get; set; }

        public InfoViewModel GetInfo => _info;

        public ObservableCollection<DwgPathModel>? GetDwgPath => _dwgPath.DwgPaths;

        public bool IsClear => _isClear;

        public LayerModel? SelectedLayerFrom
        {
            get => _layerFrom;
            set
            {
                if (_layerFrom == value)
                    return;

                _layerFrom = value;
                OnPropertyChanged(nameof(SelectedLayerFrom));
            }
        }

        public LayerModel? SelectedLayerTo
        {
            get => _layerTo;
            set
            {
                if (_layerTo == value)
                    return;

                _layerTo = value;
                OnPropertyChanged(nameof(SelectedLayerTo));
            }
        }

        public DwgPathModel? SelectedItemDwg
        {
            get => _dwgPathSelected;
            set
            {
                if (_dwgPathSelected == value)
                    return;

                _dwgPathSelected = value;
                OnPropertyChanged(nameof(SelectedItemDwg));
                SelectionDwgChanged();
            }
        }

        public string? LayerName => _layerFrom?.LayerName;

        public ICommand FileCommand { get; set; }

        public ICommand ProcessCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        private void OpenFiles(object? parameter = null)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Drawing files (*.dwg)|*.dwg|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                var dwgPaths = _dwgPath.DwgPaths!;
                if (openFileDialog.ShowDialog() == true)
                {
                    _isClear = dwgPaths.Count() > 0;
                    if (_isClear)
                    {
                        _dwgPath.DwgPaths?.Clear();
                        _isClear = false;
                    }
                    foreach (string filename in openFileDialog.FileNames)
                                                dwgPaths.Add(new DwgPathModel
                                                {
                                                      SelectedPath = Path.GetFullPath(filename),
                                                      IsSelected = false
                                                });
                }
                var totalCount = dwgPaths.Count;
                GetInfo.ProcessColor = new SolidColorBrush(Colors.DarkGray);
                if (totalCount > 0)
                    GetInfo.Text = $" {totalCount} files";

                var sPath = dwgPaths.FirstOrDefault(s => s.IsSelected == true);
                GetInfo.TextPath = sPath == null ? "No selected files"
                                                 : GetInfo.TextPath = sPath?.SelectedPath;
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void ProcessFile(object? prm = null)
        {
            string fromLayer = FromLayerNames?.LastOrDefault(f => f.IsSelected == true)?.LayerName!;
            string toLayer = ToLayerNames?.LastOrDefault(t => t.IsSelected == true)?.LayerName!;
            if (string.IsNullOrEmpty(fromLayer))
                return;

            int i = 1,
                totalCount = _dwgPath.DwgPaths!.Count;
            GetInfo.ProcessColor = new SolidColorBrush(Colors.DarkBlue);
            var dwgItems = _dwgPath.DwgPaths.Where(f => f.IsSelected == true);
            foreach (var itm in dwgItems)
            {
                var prmEvn = prm as DoWorkEventArgs;
                if (prmEvn != null && _worker.CancellationPending)
                {
                    prmEvn.Cancel = true;
                    _worker.ReportProgress(0);
                    GetInfo.TextPath = "Process has been canceled!";
                    return;
                }
                GetInfo.TextPath = $"Processing ( {i} of {totalCount} files) - {itm.SelectedPath}";
                _worker.ReportProgress((i / totalCount) * 50);
                InfoModel? prgStatus = null; // --- it will be waiting untill the return-result! 
                ProcessDispatcher.Execute( () => prgStatus = _layerService.ProcessLayer(itm.SelectedPath!, fromLayer, toLayer)); // --- AutoCad Layer Processing
                ProcessDispatcher.Execute(_ = new Action(() =>
                {
                    GetInfo.TextPath = prgStatus?.Text;
                    GetInfo.ProcessColor = prgStatus?.Status == Enums.ProcessStatus.Succed ? new SolidColorBrush(Colors.DarkGreen)
                                                                                           : new SolidColorBrush(Colors.Red);
                    Mouse.OverrideCursor = null;
                }));
                i++;
            }
        }

        private void RunWorker()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
        }

        private void DoWork(object? sender, DoWorkEventArgs e)
        {
            ProcessFile(e);
        }

        private void Cancel(object? sender)
        {
                _worker.CancelAsync();
        }

        private void ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            GetInfo.ProgressInfo = e.ProgressPercentage;
        }

        private void WorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            _worker.ReportProgress(0);
        }

        private void LoadComboLayers()
        {
            var totalCount = _dwgPath.DwgPaths!.Count;
            if (totalCount > 0)
            {
                var path = _dwgPath.DwgPaths.FirstOrDefault(s => s.IsSelected == true);
                var ds = _layerService.GetlayerList(path?.SelectedPath!);
                if (FromLayerNames?.Count > 0)
                {
                    FromLayerNames.Clear();
                    ToLayerNames?.Clear();
                }
                foreach ( var itm in ds)
                    FromLayerNames?.Add(new LayerModel {LayerName = itm });

                var ds1 = new List<string>(ds); // --clone it   
                foreach (var itm1 in ds1)
                    ToLayerNames?.Add(new LayerModel { LayerName = itm1 });

                if (FromLayerNames != null && FromLayerNames.Count > 0)
                {
                    FromLayerNames!.First().IsSelected = true;
                    ToLayerNames!.First().IsSelected = true;
                    GetInfo.TextPath = path?.SelectedPath??"";
                }
            }
        }

        private void SelectionDwgChanged()
        {
            if (!_isClear)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                LoadComboLayers();
            }
            Mouse.OverrideCursor = null;
        }

    }

    

}
