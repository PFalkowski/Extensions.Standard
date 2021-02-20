using Xunit;

namespace Extensions.Standard.Test
{
    public class ConstantsTest
    {
        [Fact]
        public void KiloByteMegaByteGigaBytePeByteExaByte()
        {
            Assert.Equal(1024u, Constants.KiB);
            Assert.Equal(1048576u, Constants.MiB);
            Assert.Equal(1073741824u, Constants.GiB);
            Assert.Equal(1099511627776u, Constants.TiB);
            Assert.Equal(1125899906842624u, Constants.PiB);
            Assert.Equal(1152921504606846976u, Constants.EiB);
            Assert.Equal(1180591620717411303424m, Constants.ZiB);
            Assert.Equal(1208925819614629174706176m, Constants.YiB);
        }
        [Fact]
        public void BitsInKiloByteMegaByteGigaBytePeByteExaByte()
        {
            Assert.Equal(8 * 1024u, Constants.BitsInKibibyte);
            Assert.Equal(8 * 1048576u, Constants.BitsInMebibyte);
            Assert.Equal(8 * 1073741824ul, Constants.BitsInGibibyte);
            Assert.Equal(8 * 1099511627776ul, Constants.BitsInTebibyte);
            Assert.Equal(8 * 1125899906842624ul, Constants.BitsInPebibyte);
            Assert.Equal(8 * 1152921504606846976ul, Constants.BitsInExbibyte);
            Assert.Equal(8 * 1180591620717411303424m, Constants.BitsInZebibyte);
            Assert.Equal(8 * 1208925819614629174706176m, Constants.BitsInYobibyte);
        }

        [Fact]
        public void KiloMegaGigaPeExa()
        {
            Assert.Equal(1000u, Constants.Kilo);
            Assert.Equal(1000000u, Constants.Mega);
            Assert.Equal(1000000000u, Constants.Giga);
            Assert.Equal(1000000000000u, Constants.Tera);
            Assert.Equal(1000000000000000u, Constants.Peta);
            Assert.Equal(1000000000000000000u, Constants.Exa);
            Assert.Equal(1000000000000000000000m, Constants.Zetta);
            Assert.Equal(1000000000000000000000000m, Constants.Yotta);
        }

        [Fact]
        public void BitsInKiloMegaGigaPeExa()
        {
            Assert.Equal(8 * 1000u, Constants.BitsInKilobyte);
            Assert.Equal(8 * 1000000u, Constants.BitsInMegabyte);
            Assert.Equal(8 * 1000000000ul, Constants.BitsInGigabyte);
            Assert.Equal(8 * 1000000000000ul, Constants.BitsInTerabyte);
            Assert.Equal(8 * 1000000000000000ul, Constants.BitsInPetabyte);
            Assert.Equal(8 * 1000000000000000000ul, Constants.BitsInExabyte);
            Assert.Equal(8 * 1000000000000000000000m, Constants.BitsInZettabyte);
            Assert.Equal(8 * 1000000000000000000000000m, Constants.BitsInYottabyte);
        }

        [Fact]
        public void MillisecondsInSecondMinuteHourDay()
        {
            Assert.Equal(1000, Constants.MillisecondsInSecond);
            Assert.Equal(60000, Constants.MillisecondsInMinute);
            Assert.Equal(3600000, Constants.MillisecondsInHour);
            Assert.Equal(86400000, Constants.MillisecondsInDay);
        }

        [Fact]
        public void BytesDecimalOrders()
        {
            decimal value = 1;
            decimal muliplier = 1000;
            foreach (var binaryOrder in Constants.DecimalOrders)
            {
                Assert.Equal(binaryOrder.Item2, value);
                value *= muliplier;
            }
        }
        [Fact]
        public void BytesBinaryOrders()
        {
            decimal value = 1;
            decimal muliplier = 1024;
            foreach (var binaryOrder in Constants.BinaryOrders)
            {
                Assert.Equal(binaryOrder.Item2, value);
                value *= muliplier;
            }
        }
    }
}
