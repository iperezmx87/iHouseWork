using Isra.Movs.EmotionDemo.Model;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using EmotionApiDemo.Core.Model;
using EmotionApiDemo.Core;
using System.IO;

namespace Isra.Movs.EmotionDemo.ViewModel
{
    public class TakePictureViewModel
    {

        #region General

        public TakePictureViewModel(TakePictureModel model)
        {
            this.model = model;
        }

        public TakePictureModel model { get; set; }

        private string EmotionApiSubsctiptionKey = "";  // your subscription goes here

        #endregion

        #region Commands

        private Command _cmdTakePhoto;
        public Command cmdTakePhoto
        {
            get
            {
                if (_cmdTakePhoto == null)
                {
                    _cmdTakePhoto = new Command(() =>
                    {
                        this.TakePhoto();

                    });
                }
                return _cmdTakePhoto;
            }
        }

        #endregion

        #region Methods

        private async void TakePhoto()
        {
            try
            {
                this.model.EmotionResult = "Taking picture...";

                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("Emotion API Demo", "Error: No camera :(", "Ok");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                {
                    return;
                }

                // await App.Current.MainPage.DisplayAlert("Emotion API Demo", file.Path, "OK");

                model.PhotoSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                model.EmotionResult = "Detecting emotion...";

                byte[] imageBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    file.GetStream().CopyTo(ms);
                    imageBytes = ms.ToArray();
                }

                EmotionEngine emotionEngine = new EmotionEngine(EmotionApiSubsctiptionKey);

                FaceEmotion[] emotionFaces = await emotionEngine.CalculateEmotion(imageBytes);

                if (emotionFaces.Length > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var face in emotionFaces)
                    {
                        builder.Append($"{emotionEngine.DetectEmocion(face.scores)}\n");
                    }

                    model.EmotionResult = builder.ToString();

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Emotion API Demo", "No Faces Detected.", "OK");
                    model.EmotionResult = "";
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Emotion API Demo", $"Error: {ex.Message}.", "Ok");
                model.EmotionResult = "";
            }
        }

        #endregion

    }
}
