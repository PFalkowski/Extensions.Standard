using Xunit;

namespace Extensions.Standard.Test
{
    public class Extensions
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
        }

        [Fact]
        public void KiloMegaGigaPeExa()
        {
            Assert.Equal(1000u, Constants.Kilo);
            Assert.Equal(1000000u, Constants.Mega);
            Assert.Equal(1000000000u, Constants.Giga);
            Assert.Equal(1000000000000u, Constants.Tera);
            Assert.Equal(1000000000000000u, Constants.Penta);
            Assert.Equal(1000000000000000000u, Constants.Exa);
        }

        [Fact]
        public void MillisecondsInSecondMinuteHourDay()
        {
            Assert.Equal(1000, Constants.MillisecondsInSecond);
            Assert.Equal(60000, Constants.MillisecondsInMinute);
            Assert.Equal(3600000, Constants.MillisecondsInHour);
            Assert.Equal(86400000, Constants.MillisecondsInDay);
        }
    }
}
