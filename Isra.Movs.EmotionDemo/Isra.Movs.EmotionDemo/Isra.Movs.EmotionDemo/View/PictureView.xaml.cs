using Isra.Movs.EmotionDemo.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Isra.Movs.EmotionDemo.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureView : ContentPage
    {
        public PictureView()
        {
            InitializeComponent();

            this.BindingContext = new TakePictureViewModel(new Model.TakePictureModel()
            {
                EmotionColor = Color.Black,
                EmotionResult = "",
                TakePictureColorText = "Take photo and detect emotion"
            });
        }
    }
}
