using EmotionApiDemo.Core;
using EmotionApiDemo.Core.Model;
using Isra.Movs.EmotionDemo.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Text;
using Xamarin.Forms;

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
                await CrossMedia.Current.Initialize();
                MediaFile file;

                // update  1
                // tomar la foto desde la camara o desde la biblioteca de imagenes
                string imageOption = await App.Current.MainPage.DisplayActionSheet("Emotion API Demo",
                    "Cancelar", null, "Tomar foto de cámara", "Tomar foto de librería");

                if (imageOption == "Tomar foto de cámara")
                {
                    model.EmotionResult = "Tomando foto...";

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await App.Current.MainPage.DisplayAlert("Emotion API Demo", "Error: Cámara no disponible.", "Ok");
                        model.EmotionResult = "";
                        return;
                    }

                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        DefaultCamera = CameraDevice.Front,
                        Directory = "Sample",
                        Name = "test.jpg"
                    });

                    if (file == null)
                    {
                        return;
                    }
                }
                else if (imageOption == "Tomar foto de librería")
                {
                    model.EmotionResult = "Seleccionando foto...";

                    file = await CrossMedia.Current.PickPhotoAsync();

                    if (file == null)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                model.PhotoSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                model.EmotionResult = "Detectando emoción(es)...";

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

                    builder.Append($"Número de rostros detectados: {emotionFaces.Length}\n");

                    foreach (var face in emotionFaces) 
                    {
                        builder.Append($"{emotionEngine.DetectEmocion(face.scores)}\n");
                    }

                    model.EmotionResult = builder.ToString();

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Emotion API Demo", "No existen rostros detectados.", "OK");
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
