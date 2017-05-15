#Emotion Detector !!

Detector de emociones construido en Xamarin.Forms que hace lo siguiente:

- Toma una fotografía utilizando la(s) cámara(s) del dispositivo.
- Se conecta a "Azure Cognitive Services - Emotion API", para obtener los Scores de Emotion.
- En base a los scores, interpreta la emoción mostrándola en la parte inferior.

Para logar estos objetivos se usaron las siguiente tecnicas y herramientas:

- Patrón de diseño MVVM (se anexa proyecto base para INotifyPropertyChanged).
- Proyecto portable EmotionEngine desarrollado por mí (https://github.com/iperezmx87/iHouseWork/tree/master/DemoEmotionApi).
- Plantilla de Xamarin.Forms en Visual Studio 2017.
- Aplicación nativa de iOS, Android y UWP.
- Uso de la herramienta "Media Plugin" de James Montemagno (https://github.com/jamesmontemagno/MediaPlugin)
- Uso de Microsoft Web Api Client para conectarse a Azure Emotion API

Posibles fallas detectadas
- No interpreta cuando Emotion API devuelva un error.
- Fallas en la cámara y/o permisos
- No se ha probado en iPhone ya que no tengo uno :(
- Traducción a otros idiomas.

Cualquier aportación a mejorar la app se agradece.
