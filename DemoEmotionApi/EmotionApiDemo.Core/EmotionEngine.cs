using System;
using EmotionApiDemo.Core.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EmotionApiDemo.Core
{
    public class EmotionEngine : IDisposable
    {
        public string SubscriptionKey { get; set; }

        HttpResponseMessage response;
        HttpClient client = new HttpClient();

        public EmotionEngine(string SubscriptionKey)
        {
            this.SubscriptionKey = SubscriptionKey;
        }

        public async Task<FaceEmotion[]> CalculateEmotion(byte[] byteData)
        {
            try
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);

                string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
                string responseContent;
                FaceEmotion[] emotionsFaces;
                
                using (var content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json" and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                    responseContent = response.Content.ReadAsStringAsync().Result;

                    emotionsFaces = JsonConvert.DeserializeObject<FaceEmotion[]>(responseContent);
                }

                return emotionsFaces;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DetectEmocion(Score emotion)
        {
            string EmotionName = "";
            decimal emotionMax = 0.00m;

            emotionMax = emotion._anger;
            if (emotion._anger > emotionMax)
            {
                EmotionName = "Un rostro parece estar MOLESTO";
                emotionMax = emotion._anger;
            }

            if (emotion._contempt > emotionMax)
            {
                EmotionName = "Un rostro puede tener DESPRECIO";
                emotionMax = emotion._contempt;
            }
            if (emotion._disgust > emotionMax)
            {
                EmotionName = "Un rostro parece estar DISGUSTADO";
                emotionMax = emotion._disgust;
            }

            if (emotion._fear > emotionMax)
            {
                EmotionName = "Un rostro parece tener MIEDO";
                emotionMax = emotion._fear;
            }

            if (emotion._happiness > emotionMax)
            {
                EmotionName = "Un rostro parece estar FELIZ";
                emotionMax = emotion._happiness;
            }

            if (emotion._neutral > emotionMax)
            {
                EmotionName = "Un rostro parece no mostrar emociones (Neutral)";
                emotionMax = emotion._neutral;
            }

            if (emotion._sadness > emotionMax)
            {
                EmotionName = "Un rostro parece estar TRISTE";
                emotionMax = emotion._sadness;
            }

            if (emotion._surprise > emotionMax)
            {
                EmotionName = "Un rostro parece estar SORPRENDIDO";
                emotionMax = emotion._surprise;
            }
            return EmotionName;
        }

        public void Dispose()
        {
            client.Dispose();
            response.Dispose();

        }
    }
}
