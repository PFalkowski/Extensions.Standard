using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Extensions.Standard.Test
{
    /// <summary>
    /// Black-box tests for public surface that had no direct coverage. Expectations are derived from
    /// each method's name and XML documentation / well-known definitions (e.g. HSV colour theory),
    /// not from the current implementation.
    /// </summary>
    public class UncoveredPathsTests
    {
        private const int Precision = 5;

        #region HsVtoArgb (advertised in the package description, previously untested)

        // Reference values from the standard HSV->RGB conversion. Result layout is [A, R, G, B].
        [Theory]
        [InlineData(0.0, 0.0, 0.0, 0, 0, 0)]       // black
        [InlineData(0.0, 0.0, 1.0, 255, 255, 255)] // white
        [InlineData(0.0, 1.0, 1.0, 255, 0, 0)]     // red
        [InlineData(120.0, 1.0, 1.0, 0, 255, 0)]   // green
        [InlineData(240.0, 1.0, 1.0, 0, 0, 255)]   // blue
        [InlineData(60.0, 1.0, 1.0, 255, 255, 0)]  // yellow
        [InlineData(180.0, 1.0, 1.0, 0, 255, 255)] // cyan
        [InlineData(300.0, 1.0, 1.0, 255, 0, 255)] // magenta
        public void HsVtoArgbConvertsKnownColors(double hue, double saturation, double value,
            int expectedRed, int expectedGreen, int expectedBlue)
        {
            var argb = Utilities.HsVtoArgb(hue, saturation, value);

            Assert.Equal(255, (int)argb[0]); // fully opaque alpha
            Assert.Equal(expectedRed, (int)argb[1]);
            Assert.Equal(expectedGreen, (int)argb[2]);
            Assert.Equal(expectedBlue, (int)argb[3]);
        }

        [Fact]
        public void HsVtoArgbHue0AndHue360ProduceSameColor()
        {
            var atZero = Utilities.HsVtoArgb(0.0, 1.0, 1.0);
            var atFull = Utilities.HsVtoArgb(360.0, 1.0, 1.0);
            Assert.Equal(atZero, atFull);
        }

        #endregion

        #region Head / Tail / HeadAndTail

        [Fact]
        public void HeadThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((IList<int>)null).Head(1));
        }

        [Fact]
        public void TailThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((IList<int>)null).Tail(1));
        }

        [Fact]
        public void HeadAndTailThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((IList<int>)null).HeadAndTail(1));
        }

        [Fact]
        public void HeadReturnsEmptyStringForEmptyInput()
        {
            Assert.Equal(string.Empty, new List<int>().Head(3));
            Assert.Equal(string.Empty, new List<int>().Tail(3));
            Assert.Equal(string.Empty, new List<int>().HeadAndTail(3));
        }

        [Fact]
        public void HeadThrowsWhenNExceedsCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new List<int> { 1, 2 }.Head(3));
        }

        [Fact]
        public void HeadContainsFirstNItemsInOrderFollowedByEllipsis()
        {
            var list = new List<string> { "alpha", "beta", "gamma", "delta" };

            var head = list.Head(2);

            Assert.Contains("alpha", head);
            Assert.Contains("beta", head);
            Assert.Contains("...", head);
            Assert.DoesNotContain("gamma", head);
            Assert.DoesNotContain("delta", head);
            Assert.True(head.IndexOf("alpha", StringComparison.Ordinal) < head.IndexOf("beta", StringComparison.Ordinal));
        }

        [Fact]
        public void TailContainsLastNItemsFollowedByEllipsis()
        {
            var list = new List<string> { "alpha", "beta", "gamma", "delta" };

            var tail = list.Tail(2);

            Assert.Contains("gamma", tail);
            Assert.Contains("delta", tail);
            Assert.Contains("...", tail);
            Assert.DoesNotContain("alpha", tail);
            Assert.DoesNotContain("beta", tail);
        }

        [Fact]
        public void HeadAndTailContainsBothEnds()
        {
            var list = new List<string> { "alpha", "beta", "gamma", "delta" };

            var headAndTail = list.HeadAndTail(1);

            Assert.Contains("alpha", headAndTail);
            Assert.Contains("delta", headAndTail);
            Assert.Contains("...", headAndTail);
            Assert.DoesNotContain("beta", headAndTail);
            Assert.DoesNotContain("gamma", headAndTail);
        }

        #endregion

        #region PluralizeWhenNeeded

        [Theory]
        [InlineData(1, "day")]
        [InlineData(2, "days")]
        [InlineData(0, "days")]
        [InlineData(5, "days")]
        public void PluralizeWhenNeededPluralizesEverythingButOne(int number, string expected)
        {
            Assert.Equal(expected, "day".PluralizeWhenNeeded(number));
        }

        #endregion

        #region AsTime(TimeSpan)

        [Fact]
        public void AsTimeSpanFormatsAllComponents()
        {
            var ts = new TimeSpan(2, 3, 4, 5, 6); // 2 days, 3 h, 4 min, 5 s, 6 ms
            Assert.Equal("2 days, 3 h, 4 min, 5 s, 6 ms", ts.AsTime());
        }

        [Fact]
        public void AsTimeSpanUsesSingularDay()
        {
            var ts = new TimeSpan(1, 1, 0, 0, 0); // 1 day, 1 hour
            Assert.Equal("1 day, 1 h", ts.AsTime());
        }

        [Fact]
        public void AsTimeSpanOmitsZeroComponentsAndHasNoTrailingSeparator()
        {
            Assert.Equal("5 min", TimeSpan.FromMinutes(5).AsTime());
        }

        [Fact]
        public void AsTimeSpanZeroReturnsZeroMilliseconds()
        {
            Assert.Equal("0 ms", TimeSpan.Zero.AsTime());
        }

        #endregion

        #region AsTime<T> singular-day branch (previously only the plural branch was tested)

        [Fact]
        public void AsTimeGenericUsesSingularDay()
        {
            long ms = Constants.MillisecondsInDay + 1500; // 1 day + 1.5 s
            Assert.Equal("1 day, 1.5 s", ms.AsTime());
        }

        #endregion

        #region Scale / FindMinMaxInOn / MaxIndex contracts

        [Fact]
        public void FindMinMaxInOnReturnsExtremes()
        {
            var (min, max) = new[] { 3.0, -1.0, 7.5, 2.0 }.FindMinMaxInOn();
            Assert.Equal(-1.0, min);
            Assert.Equal(7.5, max);
        }

        [Fact]
        public void ScaleNormalizesToUnitRangeByDefault()
        {
            var result = new[] { 0.0, 5.0, 10.0 }.Scale().ToList();
            Assert.Equal(0.0, result[0], Precision);
            Assert.Equal(0.5, result[1], Precision);
            Assert.Equal(1.0, result[2], Precision);
        }

        [Fact]
        public void MaxIndexReturnsFirstOccurrenceOnTies()
        {
            // Documented contract: "If the sequence contains more than one, first occurrence's index will be returned."
            Assert.Equal(1, new[] { 1.0, 9.0, 3.0, 9.0 }.MaxIndex());
        }

        #endregion

        #region Breaking-change fixes (v12.0.0)

        [Fact]
        public void AreaReturnsNonNegativeForClockwiseWinding()
        {
            // Same unit square as the existing CCW test, listed clockwise: the magnitude is identical
            // and a method named Area must never return a negative number.
            var clockwise = new List<double[]>
            {
                new[] { 0.0, 0.0 },
                new[] { 10.0, 0.0 },
                new[] { 10.0, 10.0 },
                new[] { 0.0, 10.0 },
            };
            Assert.Equal(100.0, clockwise.Area());
        }

        [Fact]
        public void MaxIndexThrowsInvalidOperationOnEmptySequence()
        {
            // Matches LINQ Max()/Min() behaviour for empty sequences.
            Assert.Throws<InvalidOperationException>(() => new List<double>().MaxIndex());
        }

        [Fact]
        public void FindMinMaxInOnThrowsInvalidOperationOnEmptySequence()
        {
            Assert.Throws<InvalidOperationException>(() => new double[0].FindMinMaxInOn());
        }

        [Fact]
        public void ScaleEmptySequenceReturnsEmpty()
        {
            // Scaling an empty sequence is a valid no-op and must keep returning empty, even though the
            // underlying FindMinMaxInOn now rejects empty input.
            Assert.Empty(new double[0].Scale((0.0, 1.0)));
            Assert.Empty(new double[0].Scale());
            Assert.Empty(new int[0].Scale((0.0, 1.0)));
        }

        #endregion
    }
}
