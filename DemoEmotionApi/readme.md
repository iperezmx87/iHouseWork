#Azure Cognitive Services
#Emotion API Engine

Biblioteca portable que permite conectarse a Azure Cognitive Services Emotion API, enviar una foto e interpretar los resultados
- La biblioteca necesita la clave de la suscripcion al servicio. Debes anexarla en el constructor.
- La bilbioteca se conecta y entrega el objeto EmotionFaces que en su interior contiene los Scores de cada emoción.
- Además la biblioteca interpreta la emoción mas prominente en la fotografia y lo hace por cada rostro detectado.

Se anexa un proyecto WPF para mostrar el consumo de la biblioteca.

Posibles Fallas:
- No interpeta el mensaje de error cuando Emotion API lo entregue.
- Puede no interpetar correctamente los rostros.
- Optimizar los tiempos de respuesta de Emotion API.
- Traducción de los textos a otro idiomas.

Cualquier aportacion a mejorar la biblioteca se agradece.
