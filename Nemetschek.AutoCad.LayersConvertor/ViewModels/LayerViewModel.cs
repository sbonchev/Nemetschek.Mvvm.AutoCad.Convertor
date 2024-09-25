﻿using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Input;
using Nemetschek.AutoCad.LayersConvertor.Models;
using Nemetschek.AutoCad.LayersConvertor.Services;
using System.Collections.ObjectModel;
using Nemetschek.AutoCad.LayersConvertor.Commands;

namespace Nemetschek.AutoCad.LayersConvertor.ViewModels
{
    public class LayerViewModel : BaseViewModel
    {
        public LayerViewModel()
        {
            FromLayerNames = new ObservableCollection<LayerModel>();
            ToLayerNames = new ObservableCollection<LayerModel>();

            _layerService = new LayerService();
            _dwgPath = new DwgPathViewModel();
            _info = new InfoViewModel();

            FileCommand = new RelayCommand(OpenFiles, fc => true);
            ProcessCommand = new RelayCommand(ProcessFile, pc => _dwgPath!.DwgPaths!.Count > 0 && FromLayerNames!.Count > 0);

            _isClear = false;
        }

        private readonly LayerService _layerService;

        private readonly DwgPathViewModel _dwgPath;

        private readonly InfoViewModel _info;

        private LayerModel? _layer;

        private bool _isClear;



        public ObservableCollection<LayerModel>? FromLayerNames { get; set; }

        public ObservableCollection<LayerModel>? ToLayerNames { get; set; }

        public InfoViewModel GetInfo => _info;

        public ObservableCollection<DwgPathModel>? GetDwgPath => _dwgPath.DwgPaths;

        public bool IsClear => _isClear;

        public LayerModel Layer
        {
            get => _layer ?? (_layer = new LayerModel());
            set
            {
                if (_layer == value)
                    return;

                _layer = value;
                OnPropertyChanged(nameof(Layer));
            }
        }

        public string? LayerName => _layer?.LayerName;

        public ICommand FileCommand { get; set; }

        public ICommand ProcessCommand { get; set; }

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
                {
                    GetInfo.InfoAll = $" {totalCount} files";
                    //_dwgPath.DwgPaths[0]!.IsSelected = true;
                }
                else
                    GetInfo.Info = "No selected files";
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void ProcessFile(object? parameter = null)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                string fromLayer = FromLayerNames?.LastOrDefault(f => f.IsSelected==true)?.LayerName!;
                string toLayer = ToLayerNames?.LastOrDefault(t => t.IsSelected==true)?.LayerName!;
                if (string.IsNullOrEmpty(fromLayer))
                    return;

                int i = 1, 
                    totalCount = _dwgPath.DwgPaths!.Count;
                GetInfo.ProcessColor = new SolidColorBrush(Colors.DarkBlue);
                var dwgItems = _dwgPath.DwgPaths.Where(f => f.IsSelected == true);
                foreach (var itm in dwgItems)
                {
                    GetInfo.Info = $"Processing ( {i} of {totalCount} files) - {itm.SelectedPath}";
                    var msg = _layerService.ProcessLayer(itm.SelectedPath!, fromLayer, toLayer);
                    GetInfo.Info = msg;
                    GetInfo.ProgressInfo = (i / totalCount) * 100;
                    i++;
                }
            }
            finally
            {
                GetInfo.ProgressInfo = 0;
                Mouse.OverrideCursor = null;
            }
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

                if (ds.Count > 0)
                {
                    FromLayerNames[0].IsSelected = true;
                    ToLayerNames[0].IsSelected = true;
                    GetInfo.Info = path?.SelectedPath??"";
                }
            }
        }

        internal void SelectionLayerChanged()
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
