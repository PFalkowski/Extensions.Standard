using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using NSubstitute;

namespace Extensions.Standard.Test
{
    public class ExtensionsTest
    {
        private const int Precision = 5;
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(123)]
        [InlineData(99999)]
        [InlineData(0)]
        [InlineData(-0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-123)]
        [InlineData(-99999)]
        public void FitTest(double input)
        {
            const double min = 0.0;
            const double max = 1000.0;

            var expected = Math.Max(min, Math.Min(max, input));
            var received = input.Fit(min, max);
            Assert.Equal(expected, received);
            Assert.True(expected >= min);
            Assert.True(expected <= max);
            Assert.True(received >= min);
            Assert.True(received <= max);
        }

        [Fact]
        public void FitThrowsWhenMinIsGreaterThanMax()
        {
            const double min = 1000.1;
            const double max = 1000.0;

            Assert.Throws<ArgumentException>(() => 3.0.Fit(min, max));
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("aab")]
        [InlineData("cdz")]
        [InlineData("wa")]
        [InlineData("wxa")]
        [InlineData(".NETCore")]
        public void FitTestForLertters(string input)
        {
            const string min = "ddd";
            const string max = "www";
            var isLessThanMin = string.CompareOrdinal(input, min) < 0;
            var isGreaterThanMax = string.CompareOrdinal(input, max) > 0;
            var expected = isLessThanMin ? min : (isGreaterThanMax ? max : input);
            var received = input.Fit(min, max);
            Assert.Equal(expected, received);
        }

        [Fact]
        public void EqualsWithToleranceTest()
        {
            const double a = 9.99;
            const double b = 8.99;
            const double delta = 0.00001;
            Assert.True(a.EqualsWithTolerance(b, 1));
            Assert.True(a.EqualsWithTolerance(b, 1 + delta));
            Assert.False(a.EqualsWithTolerance(b, 1 - delta));


            const double c = -0.11111111111111;
            const double d = -0.11111111111110;
            Assert.True(c.EqualsWithTolerance(d, delta));
            Assert.True(a.EqualsWithTolerance(b, 1 + delta));
            Assert.False(a.EqualsWithTolerance(b, 1 - delta));
        }

        [Fact]
        public void SequenceEqualsEqualSeqencesTest()
        {
            var tested1 = new[] { 1.0, 2.0, 0.1, -0.2, -0.999, -5.0, 99999 };

            var tested2 = new[] { 1.0, 2.0, 0.1, -0.2, -0.999, -5.0, 99999 };

            var result = tested1.SequenceEquals(tested2, 0);
            Assert.True(result);

            result = tested1.SequenceEquals(tested2, double.Epsilon);
            Assert.True(result);

            result = tested1.SequenceEquals(tested2, 1.0);
            Assert.True(result);

            result = tested1.SequenceEquals(tested2, 1000000);
            Assert.True(result);

            tested2 = new[] { 1.000001, 1.999999, 0.10000001, -0.2, -0.999, -5.0, 99999 };

            result = tested1.SequenceEquals(tested2, 0.000001);
            Assert.True(result);
        }

        [Fact]
        public void SequenceEqualsTreatsTwoNullSeqAsEqual()
        {
            double[] tested1 = null;

            double[] tested2 = null;

            Assert.True(tested1.SequenceEquals((double[])null, 0.1));
            Assert.True(tested2.SequenceEquals((double[])null, 0.1));
        }

        [Fact]
        public void SequenceEqualsWithToleranceReturnsFalseForNullAndNotNullSeq()
        {
            double[] tested1 = null;

            var tested2 = new[] { 1.0, 1.0, 1.0 };

            Assert.False(tested1.SequenceEquals(tested2, 0.1));
            Assert.False(tested2.SequenceEquals((double[])null, 0.1));
        }

        [Fact]
        public void SequenceEqualsNonEqualSequencesTest()
        {
            const double delta = 0.00001;
            var tested1 = new[] { 1.0, 2.0, 0.1, -0.2, -0.999, -5.0, 99999 };
            var tested2 = new[] { 1 + delta, 2.0, 0.1, -0.2, -0.999, -5.0, 99999 };
            var result = tested1.SequenceEquals(tested2, delta);
            Assert.False(result);
        }

        [Fact]
        public void ScaleTest()
        {
            const double max = 129.43;
            const double min = 124.123;
            const double value = 1.0;
            var fromMin = 12131229.1;
            var fromMax = 124444.0;
            var expected = (max - min) * (value - fromMin) / (fromMax - fromMin) + min;
            var received = value.Scale(fromMin, fromMax, min, max);
            Assert.Equal(Math.Round(expected, Precision), Math.Round(received, Precision));
        }

        [Theory]
        [InlineData(0.5)]
        [InlineData(1.0)]
        [InlineData(5.0)]
        [InlineData(15.0)]
        [InlineData(22.2)]
        [InlineData(121.1)]
        public void ScaleReturnsScaledValue(double value)
        {
            const double max = 129.43;
            const double min = 124.123;
            var expected = min + value * (max - min);
            var received = value.Scale(min, max);
            Assert.Equal(Math.Round(expected, Precision), Math.Round(received, Precision));
        }

        //public double GetScalingMaxMin(double value)
        //{
        //    if (!value.InClosedRange(-1, 1))
        //    {
        //        return double.MaxValue / 2 / value;
        //    }
        //    else
        //    {
        //        return (double.MaxValue / 2) * value;
        //    }
        //}

        //[Fact]
        //public void saveRanegsForScaling()
        //{
        //    const int rangeMax = 1000;
        //    var stb = new StringBuilder();
        //    NumberFormatInfo nfi = new NumberFormatInfo { NumberDecimalSeparator = "." };
        //    stb.Append("value,minimal min,maximal max,");
        //    for (int i = -rangeMax; i < rangeMax; ++i)
        //    {
        //        var res = GetScalingMaxMin(i);
        //        stb.AppendLine($"{i.ToString(nfi)},{(-res).ToString(nfi)},{res.ToString(nfi)}");
        //        Assert.False(double.IsInfinity(res - -res));
        //    }
        //    var toSave = stb.ToString();
        //    //File.AppendAllText("res.csv", toSave);
        //}

        [Theory]
        [InlineData(2)]
        [InlineData(0.3)]
        public void ScaleTestBorderCase10(double factor)
        {
            double max = 10;
            double min = -10;
            var value = factor;
            var received = value.Scale(min, max);
            Assert.False(double.IsInfinity(received));
        }

        [Theory]
        [InlineData(1e+200)]
        public void ScaleTestBorderCase110(double factor)
        {
            var max = 1e+77;
            var min = -1e+77;
            var value = factor;
            var received = value.Scale(min, max);
            Assert.False(double.IsInfinity(received));
        }

        [Theory]
        [InlineData(1E+2)]
        [InlineData(1E+10)]
        [InlineData(1E+100)]
        [InlineData(1E+150)]
        public void ScaleTestBorderCase112(double factor)
        {
            var max = factor;
            var min = -factor;
            var value = factor;
            var received = value.Scale(min, max);
            Assert.False(double.IsInfinity(received));
        }

        [Fact]
        public void ScaleTestBorderCase11()
        {
            var half = 0.5.Scale(double.MinValue - 1E+10, double.MaxValue);
            var two = 2.0.Scale(double.MinValue / 2, double.MaxValue / 2);
        }
        [Fact]
        public void ScaleTestBorderCase0()
        {
            const double max = double.MaxValue / 1.5;
            const double min = double.MinValue / 1.5;
            const double value = 0.99;
            var received = value.Scale(min, max);
            var expected = min + value * max - value * min;
            Assert.Equal(Math.Round(expected, Precision), Math.Round(received, Precision));
        }

        [Fact]
        public void ScaleTestBorderCase1()
        {
            const double max = 0;
            const double min = double.MinValue;
            const double value = 1.0;
            var received = value.Scale(min, max);
            Assert.Equal(Math.Round(max, Precision), Math.Round(received, Precision));
        }

        [Fact]
        public void ScaleTestBorderCase2()
        {
            const double max = double.MaxValue;
            const double min = 0;
            const double value = 1.0;
            var received = value.Scale(min, max);
            Assert.Equal(Math.Round(max, Precision), Math.Round(received, Precision));
        }

        [Fact]
        public void InOpenRangeTest()
        {
            var val = -777;
            const int @from = -39127993;
            const int to = 312456789;
            var expected = val >= @from && val <= to;
            var actual = val.InOpenRange(@from, to);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InOpenRangeTest2()
        {
            var rangeMin = 0;
            var rangeMax = 10;
            Assert.True(10.InOpenRange(rangeMin, rangeMax));
            Assert.True(9.InOpenRange(rangeMin, rangeMax));
            Assert.True(0.InOpenRange(rangeMin, rangeMax));
            Assert.False(1237.InOpenRange(rangeMin, rangeMax));
            Assert.False((-1729).InOpenRange(rangeMin, rangeMax));
        }

        [Fact]
        public void InClosedRangeTest()
        {
            var val = 1234;
            var from = -1;
            var to = 1;
            var expected = val > from && val < to;
            var actual = val.InClosedRange(from, to);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InClosedRangeTest2()
        {
            var rangeMin = 0;
            var rangeMax = 10;
            Assert.False(10.InClosedRange(rangeMin, rangeMax));
            Assert.True(9.InClosedRange(rangeMin, rangeMax));
            Assert.False(0.InClosedRange(rangeMin, rangeMax));
            Assert.False(123123.InClosedRange(rangeMin, rangeMax));
            Assert.False((-123).InClosedRange(rangeMin, rangeMax));
        }

        [Fact]
        public void MaxIndexFindsIndexOfBiggestElement()
        {
            var testData = new double[] { 1, 2, -3, 0, 5, 6, 7, -1, 2, 3, 5 };
            var received = testData.MaxIndex();

            Assert.Equal(6, received);
        }

        [Fact]
        public void Scale2Test()
        {
            const double scaleMax = 15124.0;
            const double scaleMin = -124110 - double.Epsilon;
            var testSequece = new[] { 1.0, 2.3, 5.7, 13.09, -987.9, 0.123 };
            var dataMax = testSequece.Max();
            var dataMin = testSequece.Min();

            var expected = new List<double>();
            foreach (var value in testSequece)
            {
                expected.Add((scaleMax - scaleMin) * (value - dataMin) / (dataMax - dataMin) + scaleMin);
            }

            var received = testSequece.Scale(scaleMin, scaleMax);

            using (var receivedIter = received.GetEnumerator())
            using (var expectedIter = expected.GetEnumerator())
            {
                while (receivedIter.MoveNext() && expectedIter.MoveNext())
                {
                    Assert.Equal(Math.Round(expectedIter.Current, 5), Math.Round(receivedIter.Current, 5));
                }
            }
        }

        [Fact]
        public void InnerProductTest()
        {
            var sequence1 = new[] { 1.0, -1.0, 123.9, 123, 99999.7 };
            var sequence2 = new[] { -1.0, 1.0, -123.9, 123, -99999.7 };

            var received = sequence1.InnerProduct(sequence2, 0.0, (result, x, y) => result + x * y);
            var expeced = 0.0;
            for (var i = 0; i < sequence1.Length; ++i)
                expeced += sequence1[i] * sequence2[i];
            Assert.Equal(expeced, received);
        }

        [Fact]
        public void InnerProductTest1()
        {
            var sequence1 = new[] { 1.0, -1.0, 123.9, 123, 99999.7 };
            var sequence2 = new[] { -1.0, 1.0, -123.9, 123, -99999.7 };

            var received = sequence1.InnerProduct(sequence2, 0.0, (result, x, y) => result + x * y);
            var expeced = sequence1.Zip(sequence2, (x, y) => x * y).Sum();

            Assert.Equal(expeced, received);
        }

        [Fact]
        public void InnerProductTest2()
        {
            List<double> sequence1 = null;
            var sequence2 = new[] { -1.0, 1.0, -123.9, 123, -99999.7 };

            Assert.Throws<ArgumentNullException>(() => sequence1.InnerProduct(sequence2, 0.0, (result, x, y) => result + x * y));
        }

        [Fact]
        public void InnerProductTest3()
        {
            var sequence1 = new[] { -1.0, 1.0, -123.9, 123, -99999.7 };

            Assert.Throws<ArgumentNullException>(() => sequence1.InnerProduct((List<double>)null, 0.0, (result, x, y) => result + x * y));
        }

        [Fact]
        public void InnerProductTest4()
        {
            var sequence1 = new[] { -1.0, 1.0, -123.9, 123, -99999.7 };
            var sequence2 = new[] { -1.0, 1.0, -123.9, 123, -99999.7 };

            Assert.Throws<ArgumentNullException>(() => sequence1.InnerProduct(sequence2, 0.0, null));
        }

        [Fact]
        public void EuclideanDistanceTest()
        {
            var sequence1 = new List<double> { 1, 2, 3 };
            var sequence2 = new List<double> { -1, -2, -4 };

            var euclideanDistance = EuclideanDistanceDebug(sequence1, sequence2);

            Assert.Equal(euclideanDistance, sequence1.EuclideanDistance(sequence2));
        }

        [Fact]
        public void EuclideanDistanceBiiggerValuesTest()
        {
            var sequence1 = new[] { -11111.0, 198989889.0, -123.9, 123, -99999.7 };
            var sequence2 = new[] { -11111.0, 198989889.0, -123.9, 123, -99999.7 };

            var euclideanDistance = EuclideanDistanceDebug(sequence1, sequence2);

            Assert.Equal(euclideanDistance, sequence1.EuclideanDistance(sequence2));
        }

        [Fact]
        public void ManhattanDistanceTest()
        {
            var sequence1 = new[] { -11111.0, 198989889.0, -123.9, 123, -99999.7 };
            var sequence2 = new[] { -11111.0, 198989889.0, -123.9, 123, -99999.7 };

            var manhattanDistance = ManhattanDistanceDebug(sequence1, sequence2);

            Assert.Equal(manhattanDistance, sequence1.ManhattanDistance(sequence2));
        }
        /// <summary>
        /// compare with http://compoasso.free.fr/primelistweb/page/prime/liste_online_en.php
        /// </summary>
        /// <param name="num">a prime</param>
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(37)]
        [InlineData(29)]
        [InlineData(109)]
        [InlineData(521)]
        [InlineData(997)]
        [InlineData(101489)]
        [InlineData(104729)]
        [InlineData(1686049)]
        [InlineData(157491121)]
        [InlineData(177488767)]
        [InlineData(111111293)]
        [InlineData(223253)]
        public void IsPrimeReturnsTrueForPrimeIntegers(int num)
        {
            Assert.True(num.IsPrime());
        }
        [Theory]
        [InlineData(10000000)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        [InlineData(-5)]
        [InlineData(6)]
        [InlineData(1)]
        [InlineData(-10)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(99)]
        [InlineData(999997)]
        [InlineData(10003290)]
        [InlineData(22222222)]
        [InlineData(33333333)]
        public void IsPrimeReturnsFalseForNotPrimeIntegers(int num)
        {
            Assert.False(num.IsPrime());
        }

        [Fact]
        public void AsMemoryTestbytes()
        {
            var testedAmount = 1;
            var tested = testedAmount.AsMemory();
            Assert.StartsWith("1 ", tested);
            Assert.Equal("1 byte", tested);
        }

        [Fact]
        public void AsMemoryTestbytes2()
        {
            var testedAmount = 1023;
            Assert.Equal(string.Concat(testedAmount, " bytes"), testedAmount.AsMemory());
        }

        [Fact]
        public void AsMemoryTestbytes3()
        {
            var testedAmount = 1023;
            Assert.Equal(string.Concat(testedAmount, " bytes"), testedAmount.AsMemory());
        }

        [Fact]
        public void AsMemoryTestkB()
        {
            var testedAmount = 1024;
            Assert.Equal(string.Concat(1.ToString(CultureInfo.InvariantCulture), $" {Constants.KibibyteSuffix}"), testedAmount.AsMemory());
            testedAmount = 1024 + 512;
            Assert.Equal(string.Concat(1.5.ToString(CultureInfo.InvariantCulture), $" {Constants.KibibyteSuffix}"), testedAmount.AsMemory());
            testedAmount = 1024 * 1024;
            Assert.Equal(string.Concat(1.ToString(CultureInfo.InvariantCulture), $" {Constants.MebibyteSuffix}"), testedAmount.AsMemory());
            testedAmount = 1024 * (1024 + 512);
            Assert.Equal(string.Concat(1.5.ToString(CultureInfo.InvariantCulture), $" {Constants.MebibyteSuffix}"), testedAmount.AsMemory());
            testedAmount = 1024 * 1024 * 1024;
            Assert.Equal(string.Concat(1.ToString(CultureInfo.InvariantCulture), $" {Constants.GibibyteSuffix}"), testedAmount.AsMemory());
            testedAmount = 1024 * 1024 * (1024 + 512);
            Assert.Equal(string.Concat(1.5.ToString(CultureInfo.InvariantCulture), $" {Constants.GibibyteSuffix}"), testedAmount.AsMemory());
        }

        [Fact]
        public void AsTime2()
        {
            var testedAmount = 83469231;
            Assert.Equal("23 h, 11 min, 9.2 s", testedAmount.AsTime());
            testedAmount = 1000000;
            Assert.Equal("16 min, 40 s", testedAmount.AsTime());
            testedAmount = 183469231;
            Assert.Equal("2 days, 2 h, 57 min, 49.2 s", testedAmount.AsTime());
            testedAmount = 183469231;
            Assert.Equal("2 days, 2 h, 57 min, 49.2 s", testedAmount.AsTime());
        }

        [Fact]
        public void DegreesToRadians()
        {
            Assert.True(29.0.ToRadians() < .5062 && 29.0.ToRadians() > .5061,
                $"wrong; 29 deg is not {29.0.ToRadians()} in radians, it's ~ .5064");
            Assert.True(100.0.ToRadians() < 1.7454 && 100.0.ToRadians() > 1.7453,
                $"wrong; 100 deg is not {100.0.ToRadians()} in radians, it's ~ 1.74532");

        }

        [Fact]
        public void RadiansToDegrees()
        {
            Assert.True(0.01745329251.ToDegrees() < 1.1 && 0.01745329251.ToDegrees() > .9);

            Assert.True(0.01745329251.ToDegrees() < 1.0001 && 0.01745329251.ToDegrees() > .9999);
            Assert.True(0.01745329251.ToDegrees() < 1.0000001 && 0.01745329251.ToDegrees() > .9999999);

            Assert.True(277.0.ToDegrees() < 15870.94 && 277.0.ToDegrees() > 15870.92);
            Assert.True(2777.0.ToDegrees() < 159110.38 && 2777.0.ToDegrees() > 159110.36);

        }

        [Fact]
        public void LineConstruction()
        {
            var start = new double[] { 0, 0 };
            var testedLine = Utilities.ConstructLine(start, 10, 0);

            Assert.True(testedLine[0] == 0 && testedLine[1] == 0 && testedLine[2] == 10 && testedLine[3] == 0,
                $"wrong points: ({testedLine[0]},{testedLine[1]} {testedLine[2]},{testedLine[3]}), should be (0,0 10,0)");

            testedLine = Utilities.ConstructLine(start, 10, 45);

            Assert.True(testedLine[0] == 0 && testedLine[1] == 0 && testedLine[2] > 5.253 && testedLine[2] < 5.254 && testedLine[3] > 8.509 && testedLine[3] < 8.51,
                $"wrong points: ({testedLine[0]},{testedLine[1]} {testedLine[2]},{testedLine[3]}), should be (0,0 5.253,8.509)");
        }

        [Fact]
        public void Interpolation()
        {
            var anyValue = 123.45;
            var first = new double[] { 0, 0 };
            var second = new double[] { 10, 10 };
            Assert.Equal(Utilities.Interpolate(first, second, anyValue), anyValue);


            first = new double[] { 0, 0 };
            second = new double[] { 20, 10 };
            Assert.Equal(Utilities.Interpolate(first, second, anyValue), anyValue / 2);


            first = new double[] { 0, 0 };
            second = new double[] { 10, 20 };
            Assert.Equal(Utilities.Interpolate(first, second, anyValue), anyValue * 2);
        }
        
        [Fact]
        public void Interpolation2()
        {
            var anyValue = 123.45;
            var first = new double[] { 0, 0 };
            var second = new double[] { 10, 10 };
            Assert.Equal(Utilities.Interpolate(first, second, anyValue), anyValue);


            first = new double[] { 0, 0 };
            second = new double[] { 20, 10 };
            Assert.Equal(Utilities.Interpolate(first, second, anyValue), anyValue / 2);


            first = new double[] { 0, 0 };
            second = new double[] { 10, 20 };
            Assert.Equal(Utilities.Interpolate(first, second, anyValue), anyValue * 2);
        }

        [Fact]
        public void Area1()
        {
            var first = new double[] { 0, 0 };
            var second = new double[] { 0, 10 };
            var third = new double[] { 10, 10 };
            var fourth = new double[] { 10, 0 };

            var all = new List<double[]> { first, second, third, fourth };

            Assert.Equal(100, all.Area());

            first = new double[] { -10, -10 };
            second = new double[] { -10, 10 };
            third = new double[] { 10, 10 };
            fourth = new double[] { 10, -10 };

            all = new List<double[]> { first, second, third };

            Assert.Equal(200, all.Area());

            all = new List<double[]> { first, second, third, fourth };

            Assert.Equal(400, all.Area());
        }

        [Theory]
        [InlineData("123123123123123123")]
        public void TruncateTruncates(string testData)
        {
            var received = testData.Truncate(2);
            Assert.Equal(2, received.Length);
            Assert.Equal("12", received);
        }

        [Theory]
        [InlineData("1234567")]
        public void TruncateDoesNotTruncateIfNotNeede(string testData)
        {
            var received = testData.Truncate(12);
            Assert.Equal(7, received.Length);
            Assert.Equal(testData, received);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TruncateDoesNothingOnEmptyStringOrNull(string testData)
        {
            var received = Utilities.Truncate(testData, 2);
            Assert.Equal(testData, received);
        }

        [Fact]
        public void MeanSquareErrorCalculatesValidResult()
        {
            var testDataA = new double[] { 1, 2, 3, 4, 5 };
            var testDataB = new double[] { 11, 12, 13, 14, 15 };

            var received = testDataA.MeanSquareError(testDataB);
            Assert.Equal(100, received);
        }

        [Fact]
        public void RootMeanSquareErrorCalculatesValidResult()
        {
            var testDataA = new double[] { 1, 2, 3, 4, 5 };
            var testDataB = new double[] { 11, 12, 13, 14, 15 };

            var received = testDataA.RootMeanSquareError(testDataB);
            Assert.Equal(10, received);
        }

        [Fact]
        public void ShuffleTest1()
        {
            var randomSubstitute = Substitute.For<Random>();
            randomSubstitute.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(1);
            var original = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var tested = original;

            tested.Shuffle(randomSubstitute);
            var expected = new List<int> { 2, 10, 1, 3, 4, 5, 6, 7, 8, 9 };

            using (var e1 = tested.ToList().GetEnumerator())
            using (var e2 = expected.ToList().GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    Assert.Equal(e1.Current, e2.Current);
                }
            }
        }

        [Fact]
        public void SoftmaxTest()
        {
            var testedVec = new[] { 1.0, 2.1, 2.3, 5.3, 7.2, 1.0, -9.1, 0.01, 0 };
            var expected = SoftmaxNaive(testedVec.ToList());
            var received = testedVec.ToList().Softmax();

            using (var recEnum = received.ToList().GetEnumerator())
            using (var expEnum = expected.ToList().GetEnumerator())
            {
                while (recEnum.MoveNext() && expEnum.MoveNext())
                {
                    Assert.Equal(Math.Round(recEnum.Current, Precision), Math.Round(expEnum.Current, Precision));
                }
            }
        }

        [Fact]
        public void PartitionTestException1()
        {
            Assert.Throws<ArgumentNullException>(() => ((List<double>)null).Partition(.5m));
        }

        [Fact]
        public void PartitionTestException2()
        {
            var blahblah = new List<double>();
            Assert.Throws<ArgumentException>(() => blahblah.Partition(.5m));
        }

        [Fact]
        public void PartitionTestException3()
        {
            const decimal inccorectRatio = 1.1m;
            var blahblah = new List<double> { .5, .6 };
            Assert.Throws<ArgumentOutOfRangeException>(() => blahblah.Partition(inccorectRatio));
        }

        [Fact]
        public void PartitionTestException4()
        {
            const decimal inccorectRatio = 0.0m;
            var blahblah = new List<double> { .5, .6 };

            Assert.Throws<ArgumentOutOfRangeException>(() => blahblah.Partition(inccorectRatio));
        }

        [Fact]
        public void PartitionReturnsCorrectPartitions1()
        {
            const double item1 = .5;
            const double item2 = .6;
            const decimal ratio = 0.5m;
            var blahblah = new List<double> { item1, item2 };
            var result = blahblah.Partition(ratio);
            Assert.Single(result[0]);
            Assert.Single(result[1]);
            Assert.Equal(result[0].First(), item1);
            Assert.Equal(result[1].First(), item2);
        }

        [Fact]
        public void PartitionReturnsCorrectPartitions2()
        {
            const double item1 = .5;
            const double item2 = .6;
            const double item3 = .7;
            const decimal ratio = 2.0m / 3.0m;

            var blahblah = new List<double> { item1, item2, item3 };
            var result = blahblah.Partition(ratio);
            Assert.Equal(2, result[0].Count);
            Assert.Single(result[1]);
            Assert.Equal(result[0][0], item1);
            Assert.Equal(result[0][1], item2);
            Assert.Equal(result[1][0], item3);
        }

        [Fact]
        public void PartitioningDefinitionTestCtor()
        {
            var tested = new PartitioningDefinition(new List<decimal> { 0.5m, 0.5m });
            Assert.NotNull(tested);
            Assert.True(tested.IsValid());
            Assert.NotNull(tested.PartitionsDefinitions);
        }

        [Fact]
        public void PartitioningDefinitionTestCtor2()
        {
            Assert.Throws<ArgumentException>(() => new PartitioningDefinition(new List<decimal>()));
        }

        [Fact]
        public void PartitioningDefinitionTestCtor3()
        {
            Assert.Throws<ArgumentNullException>(() => new PartitioningDefinition(null));
        }

        [Fact]
        public void PartitioningDefinitionTestIsValid()
        {
            var tested = new PartitioningDefinition(new List<decimal> { 0.5m, 0.5m });
            Assert.NotNull(tested);
            Assert.True(tested.IsValid());

            tested = new PartitioningDefinition(new List<decimal> { 0.55m, 0.5m });
            Assert.NotNull(tested);
            Assert.True(tested.IsValid());

            tested = new PartitioningDefinition(new List<decimal> { 123m, 3.5m });
            Assert.NotNull(tested);

            Assert.True(tested.IsValid());

            tested = new PartitioningDefinition(new List<decimal> { 123m, 3.5m, 3.315m });
            Assert.NotNull(tested);

            Assert.True(tested.IsValid());

            tested = new PartitioningDefinition(new List<decimal> { 123m, 3.5m, 3.315m, 31113.5m });
            Assert.NotNull(tested);

            Assert.True(tested.IsValid());

            tested = new PartitioningDefinition(new List<decimal> { 123m, 3.5m, 3.315m, 3.5m, 31113.5m, 3.5m, 3.5m, 399999.5m });
            Assert.NotNull(tested);

            Assert.True(tested.IsValid());
        }

        [Fact]
        public void PartitioningDefinitionTestPartitionsDefinitions()
        {
            var tested = new PartitioningDefinition(new List<decimal> { 0.5m, 0.5m });
            Assert.True(tested.PartitionsDefinitions.SequenceEqual(new List<decimal> { 0.5m, 0.5m }));

            tested = new PartitioningDefinition(new List<decimal> { 5000, 5000 });
            Assert.True(tested.PartitionsDefinitions.SequenceEqual(new List<decimal> { 0.5m, 0.5m }));

            tested = new PartitioningDefinition(new List<decimal> { 0.00005m, 0.00005m });
            Assert.True(tested.PartitionsDefinitions.SequenceEqual(new List<decimal> { 0.5m, 0.5m }));

            tested = new PartitioningDefinition(new List<decimal> { 74, 74, 74, 74 });
            Assert.True(tested.PartitionsDefinitions.SequenceEqual(new List<decimal> { 0.25m, 0.25m, 0.25m, 0.25m }));

            tested = new PartitioningDefinition(new List<decimal> { 25, 25, 25, 25 });
            Assert.True(tested.PartitionsDefinitions.SequenceEqual(new List<decimal> { 0.25m, 0.25m, 0.25m, 0.25m }));

            tested = new PartitioningDefinition(new List<decimal> { 123909876, 123909876, 123909876, 123909876 });
            Assert.True(tested.PartitionsDefinitions.SequenceEqual(new List<decimal> { 0.25m, 0.25m, 0.25m, 0.25m }));
        }

        [Fact]
        public void TestPartitionT()
        {
            var definition = new PartitioningDefinition(new List<decimal> { 0.5m, 0.5m });

            var input = new List<int> { 1, 2, 3, 4 };
            var result = input.Partition(definition);
            Assert.Equal(input[0], result[0][0]);
            Assert.Equal(input[1], result[0][1]);
            Assert.Equal(input[2], result[1][0]);
            Assert.Equal(input[3], result[1][1]);


            definition = new PartitioningDefinition(new List<decimal> { 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m });

            input = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            result = input.Partition(definition);
            for (var i = 0; i < input.Count; ++i)
            {
                Assert.Equal(result[i][0], input[i]);
            }
        }

        #region Unit test related

        private static IList<double> SoftmaxNaive(IList<double> input)
        {
            var sum = input.Sum(t => Math.Exp(t));

            var result = new double[input.Count];
            for (var i = 0; i < input.Count; ++i)
                result[i] = Math.Exp(input[i]) / sum;

            return result;
        }

        private static double ManhattanDistanceDebug(IEnumerable<double> source, IEnumerable<double> other)
        {
            var distance = 0.0D;
            using (var xIter = source.GetEnumerator())
            using (var yIter = other.GetEnumerator())
            {
                while (xIter.MoveNext() && yIter.MoveNext())
                {
                    distance += Math.Abs(xIter.Current - yIter.Current);
                }
            }
            return distance;
        }

        private static double EuclideanDistanceDebug(IEnumerable<double> source, IEnumerable<double> other)
        {
            var distance = 0.0D;
            using (var xIter = source.GetEnumerator())
            using (var yIter = other.GetEnumerator())
            {
                while (xIter.MoveNext() && yIter.MoveNext())
                {
                    distance += Math.Pow(xIter.Current - yIter.Current, 2);
                }
            }
            return Math.Sqrt(distance);
        }
        #endregion
    }
}