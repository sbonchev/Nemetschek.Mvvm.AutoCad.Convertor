using Nemetschek.AutoCad.LayersConvertor.Models;
using System.Windows.Media;

namespace Nemetschek.AutoCad.LayersConvertor.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        public InfoViewModel() 
        {
            _info = new();
            _info.ProcessColor = new SolidColorBrush(Colors.DarkBlue);
        }

        private readonly InfoModel _info;

        public string Info
        {
            get => _info.Text??"";
            set
            {
                if (_info.Text == value)
                    return;

                _info.Text = value;
                OnPropertyChanged(nameof(Info));
            }
        }

        public string InfoAll
        {
            get => _info.TextAll ?? "";
            set
            {
                if (_info.TextAll == value)
                    return;

                _info.TextAll = value;
                OnPropertyChanged(nameof(InfoAll));
            }
        }

        public Brush ProcessColor
        {
            get => _info.ProcessColor ?? new SolidColorBrush(Colors.DarkBlue);
            set
            {
                if (_info.ProcessColor == value)
                    return;

                _info.ProcessColor = value;
                OnPropertyChanged(nameof(ProcessColor));
            }
        }

        public int ProgressInfo
        {
            get => _info.ProgressInfo;
            set
            {
                if (_info.ProgressInfo == value)
                    return;

                _info.ProgressInfo = value;
                OnPropertyChanged(nameof(ProgressInfo));
            }
        }
    }

}
