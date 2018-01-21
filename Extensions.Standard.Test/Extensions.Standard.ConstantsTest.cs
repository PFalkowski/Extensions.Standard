using Xunit;

namespace Extensions.Standard.Test
{
    public class Extensions
    {
        [Fact]
        public void KiloByteMegaByteGigaBytePeByteExaByte()
        {
            const ulong kiB = 1024;
            const ulong miB = kiB * kiB;
            const ulong giB = miB * kiB;
            const ulong tiB = giB * kiB;
            const ulong piB = tiB * kiB;
            const ulong eiB = piB * kiB;
            const ulong ulongMaxValueComp = 15ul * eiB + (eiB - 1);
            Assert.Equal(kiB, Constants.KiB);
            Assert.Equal(miB, Constants.MiB);
            Assert.Equal(giB, Constants.GiB);
            Assert.Equal(tiB, Constants.TiB);
            Assert.Equal(piB, Constants.PiB);
            Assert.Equal(eiB, Constants.EiB);
            Assert.Equal(4 * giB - 1, uint.MaxValue);
            Assert.Equal(ulongMaxValueComp, ulong.MaxValue);
            Assert.True(1025 > kiB);
        }

        [Fact]
        public void KiloMegaGigaPeExa()
        {
            const ulong kilo = 1000;
            const ulong mega = kilo * kilo;
            const ulong giga = mega * kilo;
            const ulong tera = giga * kilo;
            const ulong p = tera * kilo;
            const ulong e = p * kilo;

            Assert.Equal(kilo, Constants.Kilo);
            Assert.Equal(mega, Constants.Mega);
            Assert.Equal(giga, Constants.Giga);
            Assert.Equal(tera, Constants.Tera);
            Assert.Equal(p, Constants.Penta);
            Assert.Equal(e, Constants.Exa);
        }

    }
}
