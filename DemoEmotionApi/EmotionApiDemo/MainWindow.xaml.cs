using EmotionApiDemo.Core;
using EmotionApiDemo.Core.Model;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EmotionApiDemo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ClaveSuscripcion = ""; // your Cognitive Services Emotion Api subscription key goes here

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new Microsoft.Win32.OpenFileDialog();

            openDlg.Filter = "JPEG Image(*.jpg)|*.jpg";
            bool? result = openDlg.ShowDialog(this);

            if (!(bool)result)
            {
                return;
            }

            string filePath = openDlg.FileName;

            Uri fileUri = new Uri(filePath);
            BitmapImage bitmapSource = new BitmapImage();

            bitmapSource.BeginInit();
            bitmapSource.CacheOption = BitmapCacheOption.None;
            bitmapSource.UriSource = fileUri;
            bitmapSource.EndInit();

            FacePhoto.Source = bitmapSource;

            Title = "Detectando emociones...";

            using (EmotionEngine emotionEngine = new EmotionEngine(this.ClaveSuscripcion))
            {
                FaceEmotion[] emotionFaces = await emotionEngine.CalculateEmotion(this.GetImageAsByteArray(filePath));

                if (emotionFaces.Length > 0)
                {
                    DrawingVisual visual = new DrawingVisual();
                    DrawingContext drawingContext = visual.RenderOpen();
                    drawingContext.DrawImage(bitmapSource,
                        new Rect(0, 0, bitmapSource.Width, bitmapSource.Height));
                    double dpi = bitmapSource.DpiX;
                    double resizeFactor = 96 / dpi;

                    // pintando caras
                    foreach (FaceEmotion face in emotionFaces)
                    {
                        drawingContext.DrawRectangle(
                              Brushes.Transparent,
                              new Pen(Brushes.Red, 2),
                              new Rect(
                                  face.faceRectangle.left * resizeFactor,
                                  face.faceRectangle.top * resizeFactor,
                                  face.faceRectangle.width * resizeFactor,
                                  face.faceRectangle.height * resizeFactor
                                  )
                          );

                        // reconociendo emociones
                        MessageBox.Show(emotionEngine.DetectEmocion(face.scores), "Emotion API", MessageBoxButton.OK, MessageBoxImage.Information);
                        Title = $"{emotionFaces.Length} rostro(s) detectados.";
                    }

                    drawingContext.Close();
                    RenderTargetBitmap faceWithRectBitmap = new RenderTargetBitmap(
                        (int)(bitmapSource.PixelWidth * resizeFactor),
                        (int)(bitmapSource.PixelHeight * resizeFactor),
                        96,
                        96,
                        PixelFormats.Pbgra32);

                    faceWithRectBitmap.Render(visual);
                    FacePhoto.Source = faceWithRectBitmap;
                }
                else
                {
                    MessageBox.Show("No existen rostros detectados.", "Emotion API", MessageBoxButton.OK, MessageBoxImage.Information);
                    Title = "Emotion API Demo";
                }
            }
        }

        private byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
