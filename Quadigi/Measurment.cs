
namespace Quadigi
{
    public class Measurement
    {
        public DateTime Time { get; }
        public double Value { get; }
        public MeasurementType Type { get; }

        public Measurement(DateTime time, double value, MeasurementType type)
        {
            this.Time = time;
            this.Value = value;
            this.Type = type;
        }
    }
}
