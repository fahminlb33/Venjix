using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace Venjix.Infrastructure.Services.Forecasting
{
    public interface IForecastingService
    {
        ForecastSummary RunPredictions(List<ForecastModelInput> data, ForecastingOptions options);
    }

    public class ForecastingService : IForecastingService
    {
        public ForecastSummary RunPredictions(List<ForecastModelInput> data, ForecastingOptions options)
        {
            var mlContext = new MLContext();

            // split data
            var (trainSet, trainCount, testSet, _) = SplitData(data, options.TestSize, mlContext);

            // create pipeline
            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(ForecastModelOutput.ForecastedValues),
                inputColumnName: nameof(ForecastModelInput.Value),
                windowSize: options.WindowSize,
                seriesLength: options.SeriesLength,
                trainSize: trainCount,
                horizon: options.Horizon,
                confidenceLevel: options.ConfidenceLevel,
                confidenceLowerBoundColumn: nameof(ForecastModelOutput.LowerBounds),
                confidenceUpperBoundColumn: nameof(ForecastModelOutput.UpperBounds));

            // fit model
            var forecaster = forecastingPipeline.Fit(trainSet);

            // evaluate
            var (mae, rmse) = EvaluateForecast(testSet, forecaster, mlContext);

            // run predictions
            var forecastEngine = forecaster.CreateTimeSeriesEngine<ForecastModelInput, ForecastModelOutput>(mlContext);
            var forecasted = forecastEngine.Predict();

            return new ForecastSummary
            {
                MAE = mae,
                RMSE = rmse,
                ForecastedValues = forecasted.ForecastedValues,
                LowerBounds = forecasted.LowerBounds,
                UpperBounds = forecasted.UpperBounds
            };
        }

        private (IDataView train, int trainCount, IDataView test, int testCount) SplitData<T>(IList<T> data, float testSize, MLContext mlContext) where T : class
        {
            var testCount = (int)(testSize * data.Count);
            var trainSet = mlContext.Data.LoadFromEnumerable<T>(data.Take(data.Count - testCount));
            var testSet = mlContext.Data.LoadFromEnumerable<T>(data.Skip(data.Count - testCount));

            return (trainSet, data.Count - testCount, testSet, testCount);
        }

        private (double mae, double rmse) EvaluateForecast(IDataView testData, ITransformer model, MLContext mlContext)
        {
            IDataView predictions = model.Transform(testData);
            var actual = mlContext.Data.CreateEnumerable<ForecastModelInput>(testData, true)
                .Select(observed => observed.Value);
            var forecast = mlContext.Data.CreateEnumerable<ForecastModelOutput>(predictions, true)
                .Select(prediction => prediction.ForecastedValues[0]);

            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);
            var mae = metrics.Average(error => Math.Abs(error));
            var rmse = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2)));

            return (mae, rmse);
        }
    }
}
