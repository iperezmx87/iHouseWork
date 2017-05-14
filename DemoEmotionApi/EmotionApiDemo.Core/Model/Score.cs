namespace EmotionApiDemo.Core.Model
{
    public class Score
    {
        public string anger
        {

            set
            {
                _anger = decimal.Parse(value, System.Globalization.NumberStyles.Float);
            }
        }
        public decimal _anger { get; set; }

        public string contempt
        {
            set
            {
                _contempt = decimal.Parse(value, System.Globalization.NumberStyles.Float);
            }
        }
        public decimal _contempt { get; set; }

        public string disgust
        {
            set
            {
                _disgust = decimal.Parse(value, System.Globalization.NumberStyles.Float);
            }
        }
        public decimal _disgust { get; set; }

        public string fear
        {
            set
            {
                _fear = decimal.Parse(value, System.Globalization.NumberStyles.Float);
            }
        }
        public decimal _fear { get; set; }

        public string happiness
        {
            set
            {
                _happiness = decimal.Parse(value, System.Globalization.NumberStyles.Float);
            }
        }
        public decimal _happiness { get; set; }

        public string neutral
        {
            set
            {
                _neutral = decimal.Parse(value, System.Globalization.NumberStyles.Float);
            }
        }
        public decimal _neutral { get; set; }

        public string sadness
        {
            set
            {
                _sadness = decimal.Parse(value, System.Globalization.NumberStyles.Float);
            }
        }
        public decimal _sadness { get; set; }

        public string surprise
        {
            set
            {
                _surprise = decimal.Parse(value, System.Globalization.NumberStyles.Float);
            }
        }
        public decimal _surprise { get; set; }
    }
}
