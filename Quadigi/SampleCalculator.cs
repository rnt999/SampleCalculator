
namespace Quadigi
{
    public class SampleCalculator
    {
        private readonly DateTime _startOfSampling;
        private readonly TimeSpan _interval;

        public SampleCalculator(DateTime startOfSampling, int interval = 300)
        {
            _interval = new TimeSpan(0, 0, interval);
            _startOfSampling = startOfSampling;
        }

        public Dictionary<MeasurementType, List<Measurement>> Sample(DateTime startOfSampling, List<Measurement> unsampledMeasurements)
        {
            var result = unsampledMeasurements
                            .Where(x => x.Time > _startOfSampling)
                            .GroupBy(x => x.Type)
                            .ToDictionary(a => a.Key, x => x
                                .OrderBy(x => x.Time)
                                .GroupBy(x => x.Time.RoundUp(_interval))
                                .Select(x => getMeasurement(x.Last()))
                                .ToList()
                            );

            return result;
        }

        private Measurement getMeasurement(Measurement measurement)
        {
            return new Measurement(measurement.Time.RoundUp(_interval), measurement.Value, measurement.Type);
        }
    }
}
