namespace Venjix.Infrastructure.AI
{
    public class ModelOutput
    {
        public double[] ForecastedValues { get; set; }

        public double[] LowerBounds { get; set; }

        public double[] UpperBounds { get; set; }
    }
}
