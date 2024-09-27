using System.Windows.Media;

namespace Nemetschek.AutoCad.LayersConvertor.ViewModels
{
    /// <summary>
    /// Presents files and processed files info.
    /// </summary>
    public class InfoViewModel : BaseViewModel
    {

        private string? _text;
        public string? Text
        {
            get => _text;
            set
            {
                if (_text == value)
                    return;

                _text = value??"";
                OnPropertyChanged(nameof(Text));
            }
        }

        private string? _textPath;
        public string? TextPath
        {
            get => _textPath;
            set
            {
                if (_textPath == value)
                    return;

                _textPath = value ?? "";
                OnPropertyChanged(nameof(TextPath));
            }
        }

        private Brush? _processColor;
        public Brush? ProcessColor
        {
            get => _processColor;
            set
            {
                if (_processColor == value)
                    return;

                _processColor = value;
                OnPropertyChanged(nameof(ProcessColor));
            }
        }

        private int _progressInfo;
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
