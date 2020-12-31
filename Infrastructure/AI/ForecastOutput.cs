namespace Venjix.Infrastructure.AI
{
    public class ForecastOutput
    {
        public double MAE { get; set; }
        public double RMSE { get; set; }
        public float[] ForecastedValues { get; set; }
        public float[] LowerBounds { get; set; }
        public float[] UpperBounds { get; set; }
    }
}
