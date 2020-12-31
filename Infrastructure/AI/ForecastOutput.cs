namespace Venjix.Infrastructure.AI
{
    public class ForecastOutput
    {
        public double MAE { get; set; }
        public double RMSE { get; set; }
        public double[] ForecastedValues { get; set; }
        public double[] LowerBounds { get; set; }
        public double[] UpperBounds { get; set; }
    }
}
