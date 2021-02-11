namespace Venjix.Infrastructure.AI
{
    public class ModelOutput
    {
        public float[] ForecastedValues { get; set; }

        public float[] LowerBounds { get; set; }

        public float[] UpperBounds { get; set; }
    }
}
