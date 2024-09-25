using Nemetschek.AutoCad.LayersConvertor.Models;
using System.Windows.Media;

namespace Nemetschek.AutoCad.LayersConvertor.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {

        private InfoModel? _info;

        private InfoModel? _infoAll;

        private int _progressInfo;

        public string Info
        {
            get => (_info ??= new InfoModel()).Text??"";
            set
            {
                _info ??= new InfoModel();
                if (_info.Text == value)
                    return;

                _info.Text = value;
                OnPropertyChanged(nameof(Info));
            }
        }

        public string InfoAll
        {
            get => (_infoAll ??= new InfoModel()).Text ?? "";
            set
            {
                if ((_infoAll ??= new InfoModel()).Text == value)
                    return;

                _infoAll!.Text = value;
                OnPropertyChanged(nameof(InfoAll));
            }
        }

        public Brush ProcessColor
        {
            get => (_infoAll ??= new InfoModel()).ProcessColor ?? new SolidColorBrush(Colors.DarkBlue);
            set
            {
                if ((_infoAll ??= new InfoModel()).ProcessColor == value)
                    return;

                _infoAll!.ProcessColor = value;
                OnPropertyChanged(nameof(ProcessColor));
            }
        }

        public int ProgressInfo
        {
            get => _progressInfo;
            set
            {
                if (_progressInfo == value)
                    return;

                _progressInfo = value;
                OnPropertyChanged(nameof(ProgressInfo));
            }
        }
    }
}
