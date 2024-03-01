using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Extensions.Standard
{
    public static class Utilities
    {
        #region Primes

        /// <summary>
        ///     Find whether a number is the prime number.
        ///     Not so intuitive at first glance, but fast.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsPrime(this int n)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;
            var maxDivisor = (int)Math.Sqrt(n);
            var divisor = 5;
            while (divisor <= maxDivisor)
            {
                if (n % divisor == 0 || n % (divisor + 2) == 0)
                    return false;
                divisor += 6;
            }
            return true;
        }

        #endregion

        #region Geometry

        public static double ToDegrees(this double radians)
        {
            return radians * (180.0 / Math.PI);
        }

        public static double ToRadians(this double degrees)
        {
            return degrees / (180.0 / Math.PI);
        }

        public static double[] ConstructLine(double[] startingPointXy, double length, double angleDegrees)
        {
            return new[]
            {
                startingPointXy[0],
                startingPointXy[1],
                startingPointXy[0] + length * Math.Cos(angleDegrees),
                startingPointXy[1] + length * Math.Sin(angleDegrees)
            };
        }

        public static double Interpolate(double[] p1, double[] p2, double x)
        {
            return p1[1] + ((x - p1[0]) * p2[1] - (x - p1[0]) * p1[1]) / (p2[0] - p1[0]);
        }

        /// <summary>
        ///     Calculate area of a polygon defined by points in <paramref name="polygon"/>.
        /// </summary>
        /// <param name="polygon">Should contain consecutive points, index 0 for x and 1 for y. Further values will be omited.</param>
        /// <returns>Area</returns>
        public static double Area(this IList<double[]> polygon)
        {
            var num = 0.0;
            var index = polygon.Count - 1;
            for (var i = 0; i < polygon.Count; ++i)
            {
                num += (polygon[index][0] + polygon[i][0]) * (polygon[index][1] - polygon[i][1]);
                index = i;
            }
            return num / 2;
        }

        #endregion

        #region Distance Measures

        public static bool InClosedRange<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return value.CompareTo(@from) > 0 && value.CompareTo(to) < 0;
        }

        public static bool InOpenRange<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return value.CompareTo(@from) >= 0 && value.CompareTo(to) <= 0;
        }

        /// <summary>
        ///     The distance between two points measured along axes at right angles. Sum of absolute differences of Cartesian
        ///     coordinates.
        ///     Points coordinates may be either: for one axis only (than invariant X axis is assumed) or X and Y values at same
        ///     indexes,
        ///     example: { source[0] = x1, other[0] = x2, source[1] = y1, other[1] = y2, ... etc. }
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double ManhattanDistance(this IEnumerable<double> source, IEnumerable<double> other)
        {
            return source.InnerProduct(other, 0.0, (result, x, y) => result + Math.Abs(x - y));
        }

        /// <summary>
        ///     Euclidean Distance.
        ///     Points coordinates may be either: for one axis only (than invariant X axis is assumed) or X and Y values at same
        ///     indexes,
        ///     example: { source[0] = x1, other[0] = x2, source[1] = y1, other[1] = y2, ... etc. }
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double EuclideanDistance(this IEnumerable<double> source, IEnumerable<double> other)
        {
            return Math.Sqrt(source.InnerProduct(other, 0.0, (result, x, y) => result + Math.Pow(x - y, 2)));
        }

        /// <summary>
        ///     The Mean Squared Error (MSE) is a measure of how close a fitted line is to data points.
        ///     The difference between values implied by an estimator and the true values.
        ///     Average of the squares of the differences.
        ///     MSE multiplied by N(number of samples ) is equal sample Variance. Points coordinates may be
        ///     either: for one axis only (than invariant X axis is assumed) or X and Y values at same indexes.
        ///     ref: http://en.wikipedia.org/wiki/Mean_squared_error
        /// </summary>
        /// <param name="predicted"></param>
        /// <param name="trueValues"></param>
        /// <returns></returns>
        public static double MeanSquareError(this IEnumerable<double> predicted, IEnumerable<double> trueValues)
        {
            var distance = 0.0D;
            var c = 0;
            using (var predIter = predicted.GetEnumerator())
            using (var expectedIter = trueValues.GetEnumerator())
            {
                while (predIter.MoveNext() && expectedIter.MoveNext())
                {
                    distance += Math.Pow(predIter.Current - expectedIter.Current, 2);
                    ++c;
                }
            }
            return distance / c;
        }

        /// <summary>
        ///     RMSE is the average distance of a data point from the fitted line, measured along a vertical line.
        ///     Square root of MSE is the Euclidean distance of average data point to average true value. Points coordinates may be
        ///     either: for one axis only (than invariant X axis is assumed) or X and Y values at same indexes.
        ///     ref: http://www.vernier.com/til/1014/
        /// </summary>
        /// <param name="predicted"></param>
        /// <param name="trueValues"></param>
        /// <returns></returns>
        public static double RootMeanSquareError(this IEnumerable<double> predicted, IEnumerable<double> trueValues)
        {
            return Math.Sqrt(predicted.MeanSquareError(trueValues));
        }

        #endregion

        #region Sequence algorithms

        /// <summary>
        ///     Softmax that doesn't choke on Lists larger than 200. See: http://stackoverflow.com/q/9906136
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IList<double> Softmax(this IEnumerable<double> input)
        {
            var inputAsArray = input as double[] ?? input.ToArray();
            var max = inputAsArray.Max();

            var result = new double[inputAsArray.Length];
            var sum = 0.0;
            for (var i = 0; i < inputAsArray.Length; ++i)
            {
                result[i] = Math.Exp(inputAsArray[i] - max);
                sum += result[i];
            }

            for (var i = 0; i < inputAsArray.Length; ++i)
                result[i] /= sum;

            return result;
        }

        public static TAccumulate InnerProduct<TSource, TAccumulate>(this IEnumerable<TSource> source,
            IEnumerable<TSource> other, TAccumulate seed, Func<TAccumulate, TSource, TSource, TAccumulate> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (func == null) throw new ArgumentNullException(nameof(func));

            using (var xIter = source.GetEnumerator())
            using (var yIter = other.GetEnumerator())
            {
                var result = seed;
                while (xIter.MoveNext() && yIter.MoveNext())
                {
                    result = func(result, xIter.Current, yIter.Current);
                }
                return result;
            }
        }

        /// <summary>
        ///     Knuth Shuffle - reorder items randomly in-place, with Fisher-Yates algorithm.
        ///     O(n) complexity. see: http://rosettacode.org/wiki/Knuth_shuffle#C.23
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static void Shuffle<T>(this IList<T> input, Random rand)
        {
            for (var i = 0; i < input.Count; ++i)
            {
                var j = rand.Next(i, input.Count);
                var temp = input[i];
                input[i] = input[j];
                input[j] = temp;
            }
        }

        public static List<List<T>> Partition<T>(this IEnumerable<T> input, decimal firstPartitionRatio)
        {
            if (firstPartitionRatio <= 0 || firstPartitionRatio > 1.0m)
                throw new ArgumentOutOfRangeException(nameof(firstPartitionRatio));

            var partitionDefinition = new PartitioningDefinition(new List<decimal> { firstPartitionRatio, 1m - firstPartitionRatio });

            return input.ToList().Partition(partitionDefinition);
        }

        public static List<List<T>> Partition<T>(this IEnumerable<T> input, PartitioningDefinition partitioningDefinition)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var inputAsList = input.ToList();
            if (inputAsList.Count == 0) throw new ArgumentException(nameof(input));
            if (partitioningDefinition == null) throw new ArgumentNullException(nameof(partitioningDefinition));
            if (!partitioningDefinition.IsValid()) throw new ArgumentException(nameof(partitioningDefinition));

            var result = new List<List<T>>(partitioningDefinition.PartitionsNumber);
            var i = 0;

            foreach (var proportion in partitioningDefinition.PartitionsDefinitions)
            {
                var endingElementIndex = (int)(i + proportion * inputAsList.Count);
                var tempList = new List<T>(endingElementIndex - i);
                for (; i < endingElementIndex; ++i)
                {
                    tempList.Add(inputAsList[i]);
                }
                result.Add(tempList);
            }
            for (; i < inputAsList.Count; ++i)
            {
                result.Last().Add(inputAsList[i]);
            }
            return result;
        }

        public static bool EqualsWithTolerance(this double lhs, double rhs, double delta = 0.0000000001)
        {
            return Math.Abs(lhs - rhs) <= delta;
        }

        /// <summary>
        /// Verifies whether the collections have same elements within the <paramref name="tolerance"/> in same order.
        /// </summary>
        /// <param name="seqenceA"></param>
        /// <param name="seqenceB"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool SequenceEquals(this IEnumerable<double> seqenceA, IEnumerable<double> seqenceB, double tolerance)
        {
            if (seqenceA == null && seqenceB == null) return true;
            if (seqenceA == null || seqenceB == null) return false;
            var listA = seqenceA as IList<double> ?? seqenceA.ToList();
            var listB = seqenceB as IList<double> ?? seqenceB.ToList();
            if (listA.Count != listB.Count) return false;
            using (var e1 = listA.GetEnumerator())
            using (var e2 = listB.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    if (!e1.Current.EqualsWithTolerance(e2.Current, tolerance)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Verifies if the collections have exactly same elements in any order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionLhs"></param>
        /// <param name="collectionRhs"></param>
        /// <returns></returns>
        public static bool IsEquivalent<T>(this IEnumerable<T> collectionLhs, IEnumerable<T> collectionRhs)
        {
            var result = true;
            if (collectionLhs == null && collectionRhs == null)
            {
                return true;
            }

            if (collectionLhs == null || collectionRhs == null)
            {
                return false;
            }
            var listA = collectionLhs as IList<T> ?? collectionLhs.ToList();
            var listB = collectionRhs as HashSet<T> ?? collectionRhs.ToHashSet();

            if (listA.Count != listB.Count)
            {
                return false;
            }

            foreach (var listAItem in listA)
            {
                result = listB.Contains(listAItem);
            }

            return result;
        }

        /// <summary>
        /// Largest value index. If the sequence contains more than one, first occurence's index will be returned.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int MaxIndex(this IEnumerable<double> input)
        {
            var inputAsList = input.ToList();
            var index = 0;
            var value = inputAsList[0];
            for (var i = 0; i < inputAsList.Count; ++i)
            {
                if (inputAsList[i] > value)
                {
                    value = inputAsList[i];
                    index = i;
                }
            }
            return index;
        }
        
        public static string Head<T>(this IList<T> input, int n)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (!input.Any())
                return string.Empty;
            if (n > input.Count)
                throw new ArgumentOutOfRangeException(nameof(n));

            var stb = new StringBuilder();
            stb.AppendLine();
            stb.AppendJoin(Environment.NewLine, input.Take(n));
            stb.AppendLine();
            stb.AppendLine("...");
            stb.AppendLine();

            return stb.ToString();
        }

        public static string Tail<T>(this IList<T> input, int n)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (!input.Any())
                return string.Empty;
            if (n > input.Count)
                throw new ArgumentOutOfRangeException(nameof(n));

            var stb = new StringBuilder();
            stb.AppendLine();
            stb.AppendLine("...");
            stb.AppendJoin(Environment.NewLine, input.Skip(input.Count -n));
            stb.AppendLine();

            return stb.ToString();
        }

        
        public static string HeadAndTail<T>(this IList<T> input, int n)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (!input.Any())
                return string.Empty;
            if (n > input.Count)
                throw new ArgumentOutOfRangeException(nameof(n));

            var stb = new StringBuilder();
            stb.AppendLine();
            stb.AppendJoin(Environment.NewLine, input.Take(n));
            stb.AppendLine();
            stb.AppendLine("...");
            stb.AppendJoin(Environment.NewLine, input.Skip(input.Count -n));
            stb.AppendLine();

            return stb.ToString();
        }

        public static string CenterText(this string text, int totalLengthOfLine, char fill = ' ')
        {
            var fillsCount = totalLengthOfLine - text.Length > 0 ? (totalLengthOfLine - text.Length) / 2 : 0;
            var fillsHalf = string.Join("", Enumerable.Repeat(fill, fillsCount));

            return $"{fillsHalf}{text}{fillsHalf}";
        }
        #endregion

        #region Colors

        /// <summary>
        ///     (A)RGB values packed to one int.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static int AsColor(byte red, byte green, byte blue, byte alpha = 255)
        {
            return alpha + (red << 8) + (green << 16) + (blue << 24);
        }

        /// <summary>
        ///     Change int in AARRGGBB convention to byte array representing Red Green and Blue.
        /// </summary>
        /// <param name="argb"></param>
        /// <returns></returns>
        public static byte[] AsColor(this int argb)
        {
            return new[]
            {
                (byte) ((argb >> 24) & 0xff), (byte) ((argb >> 16) & 0xff), (byte) ((argb >> 8) & 0xff),
                (byte) ((argb >> 0) & 0xff)
            };
        }

        /// <summary>
        ///     HSV (Hue Saturation Value) scale (H 0..360, S 0.0..1.0, V 0.0..1.0) to ARGB scale.
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="value"></param>
        /// <returns>4 bytes for Alpha, Red, Green..</returns>
        public static byte[] HsVtoArgb(double hue, double saturation, double value)
        {
            var hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            var f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            var v = Convert.ToByte(value);
            var p = Convert.ToByte(value * (1 - saturation));
            var q = Convert.ToByte(value * (1 - f * saturation));
            var t = Convert.ToByte(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0:
                    return new byte[] { 255, v, t, p };
                case 1:
                    return new byte[] { 255, q, v, p };
                case 2:
                    return new byte[] { 255, p, v, t };
                case 3:
                    return new byte[] { 255, p, q, v };
                case 4:
                    return new byte[] { 255, t, p, v };
                default:
                    return new byte[] { 255, v, p, q };
            }
        }

        #endregion

        #region Suffixes

        /// <summary>
        ///     Provide number of bytes and receive user friendly string. This method uses binary orders of magnitude of data (KiB = kibibyte = 1024 bytes), for decimal use AsMemoryDecimal.
        ///     Note, that for kibibyte, mebibyte and gibibyte JEDEC convention is used, i.e. KB, MB and GB respectively. This is to hold on to convention used by Windows OS, which reports memory usage in JEDEC standard. 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="decimals">The number of decimal places in return value</param>
        /// <param name="numberSeparator"></param>
        /// <param name="culture"></param>
        /// <returns>User friendly string.</returns>
        public static string AsMemory<T>(this T bytes, byte decimals = 2, string numberSeparator = " ",
            CultureInfo culture = null)
            where T : IConvertible
        {
            var bytesConverted = Convert.ToDecimal(bytes);
            var numberFormat = (NumberFormatInfo)(culture?.NumberFormat ?? CultureInfo.InvariantCulture.NumberFormat).Clone();
            if (numberSeparator != null) numberFormat.NumberGroupSeparator = numberSeparator;

            if (bytesConverted < Constants.KiB)
            {
                return $"{bytesConverted.ToString(numberFormat)} {"byte".PluralizeWhenNeeded(bytesConverted)}";
            }
            if (bytesConverted < Constants.MiB)
            {
                return $"{Math.Round(bytesConverted / Constants.KiB, decimals).ToString(numberFormat)} {Constants.KibibyteSuffix}";
            }
            if (bytesConverted < Constants.GiB)
            {
                return $"{Math.Round(bytesConverted / Constants.MiB, decimals).ToString(numberFormat)} {Constants.MebibyteSuffix}";
            }
            if (bytesConverted < Constants.TiB)
            {
                return $"{Math.Round(bytesConverted / Constants.GiB, decimals).ToString(numberFormat)} {Constants.GibibyteSuffix}";
            }
            if (bytesConverted < Constants.PiB)
            {
                return $"{Math.Round(bytesConverted / Constants.TiB, decimals).ToString(numberFormat)} {Constants.TebibyteSuffix}";
            }
            if (bytesConverted < Constants.EiB)
            {
                return $"{Math.Round(bytesConverted / Constants.PiB, decimals).ToString(numberFormat)} {Constants.PebibyteSuffix}";
            }
            if (bytesConverted < Constants.ZiB)
            {
                return $"{Math.Round(bytesConverted / Constants.YiB, decimals).ToString(numberFormat)} {Constants.ExbibyteSuffix}";
            }
            return $"{Math.Round(bytesConverted / Constants.ZiB, decimals).ToString(numberFormat)} {Constants.YobibyteSuffix}";
        }

        /// <summary>
        ///     Provide number of bytes and receive user friendly string. This method uses decimal orders of magnitude of data (KB = kilobyte = 1000 bytes), for binary use AsMemory. 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="decimals">The number of decimal places in return value</param>
        /// <param name="numberSeparator"></param>
        /// <param name="culture"></param>
        /// <returns>User friendly string.</returns>
        public static string AsMemoryDecimal<T>(this T bytes, byte decimals = 2, string numberSeparator = " ",
            CultureInfo culture = null)
            where T : IConvertible
        {
            var bytesConverted = Convert.ToDecimal(bytes);
            var numberFormat = (NumberFormatInfo)(culture?.NumberFormat ?? CultureInfo.InvariantCulture.NumberFormat).Clone();
            if (numberSeparator != null) numberFormat.NumberGroupSeparator = numberSeparator;

            if (bytesConverted < Constants.Kilo)
            {
                return $"{bytesConverted.ToString(numberFormat)} {"byte".PluralizeWhenNeeded(bytesConverted)}";
            }
            if (bytesConverted < Constants.Mega)
            {
                return $"{Math.Round(bytesConverted / Constants.Kilo, decimals).ToString(numberFormat)} {Constants.KilobyteSuffix}";
            }
            if (bytesConverted < Constants.Giga)
            {
                return $"{Math.Round(bytesConverted / Constants.Mega, decimals).ToString(numberFormat)} {Constants.MegabyteSuffix}";
            }
            if (bytesConverted < Constants.Tera)
            {
                return $"{Math.Round(bytesConverted / Constants.Giga, decimals).ToString(numberFormat)} {Constants.GigabyteSuffix}";
            }
            if (bytesConverted < Constants.Peta)
            {
                return $"{Math.Round(bytesConverted / Constants.Tera, decimals).ToString(numberFormat)} {Constants.TerabyteSuffix}";
            }
            if (bytesConverted < Constants.Exa)
            {
                return $"{Math.Round(bytesConverted / Constants.Peta, decimals).ToString(numberFormat)} {Constants.PetabyteSuffix}";
            }
            if (bytesConverted < Constants.Zetta)
            {
                return $"{Math.Round(bytesConverted / Constants.Exa, decimals).ToString(numberFormat)} {Constants.ZettabyteSuffix}";
            }
            return $"{Math.Round(bytesConverted / Constants.Zetta, decimals).ToString(numberFormat)} {Constants.YobibyteSuffix}";
        }

        /// <summary>
        ///     Present milliseconds as h + min + sec and ms.
        ///     Works like TimeSpan.FromSeconds but faster. Appends information about what is what (ex. days, hours etc.).
        /// </summary>
        /// <param name="timeElapsed"></param>
        /// <returns>User friendly time string.</returns>
        public static string AsTime(this TimeSpan timeElapsed)
        {
            var stb = new StringBuilder();

            if (timeElapsed.Days >= 1)
            {
                stb.Append($"{timeElapsed.Days} {"day".PluralizeWhenNeeded(timeElapsed.Days)}, ");
            }
            if (timeElapsed.Hours != 0)
            {
                stb.AppendFormat($"{timeElapsed.Hours} h, ");
            }
            if (timeElapsed.Minutes != 0)
            {
                stb.AppendFormat("{0} min, ", timeElapsed.Minutes);
            }
            if (timeElapsed.Seconds != 0)
            {
                stb.AppendFormat("{0} s, ", timeElapsed.Seconds);
            }
            if (timeElapsed.Milliseconds != 0)
            {
                stb.AppendFormat("{0} ms", timeElapsed.Milliseconds);
            }
            return stb.ToString();
        }

        /// <summary>
        ///     Present milliseconds as h + min + sec and ms.
        ///     Works like TimeSpan.FromSeconds but faster. Appends information about what is what (ex. days, hours etc.).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milliseconds"></param>
        /// <returns>User friendly time string.</returns>
        public static string AsTime<T>(this T milliseconds)
            where T : IConvertible
        {
            var converted = Convert.ToUInt64(milliseconds);
            var stb = new StringBuilder();

            if (converted >= Constants.MillisecondsInDay)
            {
                stb.Append(converted / Constants.MillisecondsInDay);
                stb.Append(converted < 2 * Constants.MillisecondsInDay ? " day, " : " days, ");
                converted %= Constants.MillisecondsInDay;
            }
            if (converted >= Constants.MillisecondsInHour)
            {
                stb.Append(converted / Constants.MillisecondsInHour);
                stb.Append(" h, ");
                converted %= Constants.MillisecondsInHour;
            }
            if (converted >= Constants.MillisecondsInMinute)
            {
                stb.Append(converted / Constants.MillisecondsInMinute);
                stb.Append(" min, ");
                converted %= Constants.MillisecondsInMinute;
            }
            if (converted >= Constants.MillisecondsInSecond)
            {
                stb.Append(Math.Round(converted / 1000.0, 1).ToString(CultureInfo.InvariantCulture));
                stb.Append(" s");
            }
            else
            {
                stb.Append(converted);
                stb.Append(" ms");
            }
            return stb.ToString();
        }

        /// <summary>
        ///     Present milliseconds as hours + minutes + seconds and milliseconds.
        /// </summary>
        /// <param name="milliseconds">milliseconds</param>
        /// <returns></returns>
        internal static string AsTimeVerbose(this long milliseconds)
        {
            var stb = new StringBuilder();

            if (milliseconds >= Constants.MillisecondsInDay)
            {
                stb.Append(milliseconds / Constants.MillisecondsInDay);
                if (milliseconds < 2 * Constants.MillisecondsInDay)
                    stb.Append(" day, ");
                else
                    stb.Append(" days, ");
                milliseconds %= Constants.MillisecondsInDay;
            }
            if (milliseconds >= Constants.MillisecondsInHour)
            {
                stb.Append(milliseconds / Constants.MillisecondsInHour);
                if (milliseconds < 2 * Constants.MillisecondsInHour)
                    stb.Append(" hour, ");
                else
                    stb.Append(" hours, ");
                milliseconds %= Constants.MillisecondsInHour;
            }
            if (milliseconds >= Constants.MillisecondsInMinute)
            {
                stb.Append(milliseconds / Constants.MillisecondsInMinute);
                if (milliseconds < 2 * Constants.MillisecondsInMinute)
                    stb.Append(" minute, ");
                else
                    stb.Append(" minutes, ");
                milliseconds %= Constants.MillisecondsInMinute;
            }
            if (milliseconds >= 1000)
            {
                stb.Append(Math.Round(milliseconds / 1000.0, 1).ToString(CultureInfo.InvariantCulture));
                if (milliseconds < 2 * 1000)
                    stb.Append(" second");
                else
                    stb.Append(" seconds");
            }
            else
            {
                stb.Append(milliseconds);
                stb.Append(" millisecond".PluralizeWhenNeeded(milliseconds));
            }
            return stb.ToString();
        }

        public static string PluralizeWhenNeeded<T>(this string noun, T number)
            where T : IConvertible
        {
            return $"{noun}{(Convert.ToDecimal(number) != 1 ? "s" : string.Empty)}";
        }

        #endregion

        #region Scaling

        private static double ScaleSafe(this double value, double scaleMin, double scaleMax)
        {
            return scaleMin + value * scaleMax - value * scaleMin;
        }

        public static double Scale(this double value, (double Min, double Max) scale)
        {
            if (scale.Min > scale.Max) throw new ArgumentOutOfRangeException(nameof(scale.Min));
            var tmp = value * (scale.Max - scale.Min);
            if (double.IsInfinity(tmp) && value.InClosedRange(0.0, 1.0))
            {
                return value.ScaleSafe(scale.Min, scale.Max);
            }
            return scale.Min + tmp;
        }

        public static double Scale(this double value, (double Min, double Max) data, (double Min, double Max) scale)
        {
            var m = (scale.Max - scale.Min) / (data.Max - data.Min);
            var c = scale.Min - data.Min * m;
            return m * value + c;
        }

        public static (double Min, double Max) FindMinMaxInOn(this IEnumerable<double> data)
        {
            var dataMin = double.MaxValue;
            var dataMax = double.MinValue;
            foreach (var item in data)
            {
                if (item < dataMin) dataMin = item;
                if (item > dataMax) dataMax = item;
            }
            return (Min: dataMin, Max: dataMax);
        }

        public static IEnumerable<double> Scale(this IEnumerable<double> data, (double Min, double Max) scale)
        {
            var enumerated = data as double[] ?? data.ToArray();
            var (min, max) = data.FindMinMaxInOn();
            var m = (scale.Max - scale.Min) / (max - min);
            var c = scale.Min - min * m;
            var result = new double[enumerated.Length];

            for (int i = 0; i < enumerated.Length; ++i)
            {
                result[i] = m * enumerated[i] + c;
            }
            return result;
        }

        public static IEnumerable<double> Scale(this IEnumerable<double> data)
        {
            return Scale(data, (Min: 0, Max: 1.0));
        }

        public static IEnumerable<double> Scale<T>(this IEnumerable<T> data, (double Min, double Max) scale) where T : IConvertible
        {
            var enumerated = data as T[] ?? data.ToArray();
            var (min, max) = enumerated.Cast<double>().FindMinMaxInOn();
            var m = (scale.Max - scale.Min) / (max - min);
            var c = scale.Min - min * m;
            var result = new double[enumerated.Length];

            for (int i = 0; i < enumerated.Length; ++i)
            {
                result[i] = m * Convert.ToDouble(enumerated[i]) + c;
            }
            return result;
        }

        public static IEnumerable<double> Scale<T>(this IEnumerable<double> data) where T : IConvertible
        {
            return Scale(data, (Min: 0, Max: 1.0));
        }

        public static T Fit<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentException($"min ({min}) cannot be greater than max ({max})");
            }
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            return value.CompareTo(max) > 0 ? max : value;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        #endregion

        #region Reflection

        public static Dictionary<string, dynamic> GetAllPublicPropertiesValues<T>(this T input)
        {
            var properties = new Dictionary<string, dynamic>();
            foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Public |
                                                                               BindingFlags.Instance))
            {
                var value = propertyInfo.GetValue(input);
                properties.Add(propertyInfo.Name, value);
            }

            return properties;
        }

        #endregion
    }
}