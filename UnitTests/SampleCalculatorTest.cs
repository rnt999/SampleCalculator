using FluentAssertions;
using Quadigi;
using System.Linq;

namespace UnitTests
{

    public class SampleCalculatorTest
    {
        private List<Measurement> _unsampledMeasurements = new ();
        private SampleCalculator _sampleCalculator;
        private DateTime _startOfSampling = new DateTime(2017, 01, 03, 10, 00, 00);

        public SampleCalculatorTest()
        {
            _sampleCalculator = new SampleCalculator(_startOfSampling);

            _unsampledMeasurements.Add(new Measurement( new DateTime(2017, 01, 03, 10, 04, 45), 35.79, MeasurementType.TEMP ));
            _unsampledMeasurements.Add(new Measurement( new DateTime(2017, 01, 03, 10, 01, 18), 98.78, MeasurementType.SPO2 ));
            _unsampledMeasurements.Add(new Measurement( new DateTime(2017, 01, 03, 10, 09, 07), 35.01, MeasurementType.TEMP ));
            _unsampledMeasurements.Add(new Measurement( new DateTime(2017, 01, 03, 10, 03, 34), 96.49, MeasurementType.SPO2 ));
            _unsampledMeasurements.Add(new Measurement( new DateTime(2017, 01, 03, 10, 02, 01), 35.82, MeasurementType.TEMP ));
            _unsampledMeasurements.Add(new Measurement( new DateTime(2017, 01, 03, 10, 05, 00), 97.17, MeasurementType.SPO2 ));
            _unsampledMeasurements.Add(new Measurement( new DateTime(2017, 01, 03, 10, 05, 01), 95.08, MeasurementType.SPO2 ));
        }

        [Fact]
        public void Sample_EmptyList_ShouldReturnEmptyList()
        {
            var result = _sampleCalculator.Sample(_startOfSampling, new List<Measurement>());

            result.Should().BeEmpty();
        }

        [Fact]
        public void Sample_UnsampledMeasurements_ShouldReturnSeparatedTypes()
        {
            var expectedResult = 2;
                        
            var result = _sampleCalculator.Sample(_startOfSampling, _unsampledMeasurements);

            result.Count.Should().Be(expectedResult);
        }

        [Fact]
        public void Sample_5MinutesInterval_ShouldReturnLastMeasurement()
        {
            var expectedResult = 2;

            var result = _sampleCalculator.Sample(_startOfSampling, _unsampledMeasurements);

            result[MeasurementType.TEMP].Count.Should().Be(expectedResult);
            result[MeasurementType.SPO2].Count.Should().Be(expectedResult);
        }

        [Fact]
        public void Sample_MeasurementUnsorter_ShouldReturnSortedByAsc()
        {
            var result = _sampleCalculator.Sample(_startOfSampling, _unsampledMeasurements);

            var firstTempTime = result[MeasurementType.TEMP][0].Time;
            var secondTempTime = result[MeasurementType.TEMP][1].Time;

            firstTempTime.Ticks.Should().BeLessThan(secondTempTime.Ticks);

            var firstSpo2Time = result[MeasurementType.SPO2][0].Time;
            var secondSpo2Time = result[MeasurementType.SPO2][1].Time;

            firstSpo2Time.Ticks.Should().BeLessThan(secondSpo2Time.Ticks);
        }

        [Fact]
        public void Sample_MeasurementMatchInterval_ShouldReturnInternalBorder()
        {
            var result = _sampleCalculator.Sample(_startOfSampling, _unsampledMeasurements);

            var firstTemp = result[MeasurementType.TEMP][0];
            var secondTemp = result[MeasurementType.TEMP][1];

            var firstSpo2 = result[MeasurementType.SPO2][0];
            var secondSpo2 = result[MeasurementType.SPO2][1];

            firstTemp.Time.Should().Be(new DateTime(2017, 01, 03, 10, 05, 00));
            firstTemp.Value.Should().Be(35.79);
            secondTemp.Time.Should().Be(new DateTime(2017, 01, 03, 10, 10, 00));
            secondTemp.Value.Should().Be(35.01);

            firstSpo2.Time.Should().Be(new DateTime(2017, 01, 03, 10, 05, 00));
            firstSpo2.Value.Should().Be(97.17);
            secondSpo2.Time.Should().Be(new DateTime(2017, 01, 03, 10, 10, 00));
            secondSpo2.Value.Should().Be(95.08);
        }
    }

    


}