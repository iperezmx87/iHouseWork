using Isra.Movs.EmotionDemo.mvvm;
using Xamarin.Forms;

namespace Isra.Movs.EmotionDemo.Model
{
    public class TakePictureModel : BaseNotify
    {
        private Color _EmotionColor;
        public Color EmotionColor
        {
            get
            {
                return _EmotionColor;
            }
            set
            {
                SetPropertyChanged(ref _EmotionColor, value, "EmotionColor");
            }
        }

        private string _TakePictureColorText;
        public string TakePictureColorText
        {
            get
            {
                return _TakePictureColorText;
            }
            set
            {
                SetPropertyChanged(ref _TakePictureColorText, value, "TakePictureColorText");
            }
        }

        private string _EmotionResult;
        public string EmotionResult
        {
            get
            {
                return _EmotionResult;
            }
            set
            {
                SetPropertyChanged(ref _EmotionResult, value, "EmotionResult");
            }
        }

        private ImageSource _PhotoSource;
        public ImageSource PhotoSource
        {
            get
            {
                return _PhotoSource;
            }
            set
            {
                SetPropertyChanged(ref _PhotoSource, value, "PhotoSource");
            }
        }
    }
}
