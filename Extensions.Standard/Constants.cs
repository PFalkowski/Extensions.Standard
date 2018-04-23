using System;
using System.Collections.Generic;

namespace Extensions.Standard
{
    public static class Constants
    {
        public const byte BitsInByte = 8;

        /// <summary>
        ///     1000
        /// </summary>
        public const ulong Kilo = 1000;

        /// <summary>
        ///     1000000
        /// </summary>
        public const ulong Mega = Kilo * 1000;

        /// <summary>
        ///     1000000000
        /// </summary>
        public const ulong Giga = Mega * 1000;

        /// <summary>
        ///     1000000000000
        /// </summary>
        public const ulong Tera = Giga * 1000;

        /// <summary>
        ///     1000000000000000
        /// </summary>
        public const ulong Peta = Tera * 1000;

        /// <summary>
        ///     1000000000000000000
        /// </summary>
        public const ulong Exa = Peta * 1000;

        /// <summary>
        ///     1000000000000000000000
        /// </summary>
        public const decimal Zetta = (decimal)Exa * 1000;

        /// <summary>
        ///     1000000000000000000000000
        /// </summary>
        public const decimal Yotta = (decimal)Zetta * 1000;



        /// <summary>
        ///     1024
        /// </summary>
        public const ulong KiB = 1024;

        /// <summary>
        ///     1048576
        /// </summary>
        public const ulong MiB = KiB * 1024;

        /// <summary>
        ///     1073741824
        /// </summary>
        public const ulong GiB = MiB * 1024;

        /// <summary>
        ///     1099511627776
        /// </summary>
        public const ulong TiB = GiB * 1024;

        /// <summary>
        ///     1125899906842624
        /// </summary>
        public const ulong PiB = TiB * 1024;

        /// <summary>
        ///     1152921504606846976
        /// </summary>
        public const ulong EiB = PiB * 1024;

        /// <summary>
        ///     1180591620717411303424
        /// </summary>
        public const decimal ZiB = (decimal)EiB * 1024;

        /// <summary>
        ///     1180591620717411303424
        /// </summary>
        public const decimal YiB = ZiB * 1024;


        public const ulong BitsInKilobyte = BitsInByte * Kilo;

        public const ulong BitsInMegabyte = BitsInByte * Mega;

        public const ulong BitsInGigabyte = BitsInByte * Giga;

        public const ulong BitsInTerabyte = BitsInByte * Tera;

        public const ulong BitsInPetabyte = BitsInByte * Peta;

        public const ulong BitsInExabyte = BitsInByte * Exa;

        public const decimal BitsInZettabyte = BitsInByte * Zetta;

        public const decimal BitsInYottabyte = BitsInByte * Yotta;


        public const ulong BitsInKibibyte = BitsInByte * KiB;

        public const ulong BitsInMebibyte = BitsInByte * MiB;

        public const ulong BitsInGibibyte = BitsInByte * GiB;

        public const ulong BitsInTebibyte = BitsInByte * TiB;

        public const ulong BitsInPebibyte = BitsInByte * PiB;

        public const ulong BitsInExbibyte = BitsInByte * EiB;

        public const decimal BitsInZebibyte = BitsInByte * ZiB;

        public const decimal BitsInYobibyte = BitsInByte * YiB;


        public const int MillisecondsInSecond = 1000;
        public const int MillisecondsInMinute = MillisecondsInSecond * 60;
        public const int MillisecondsInHour = MillisecondsInMinute * 60;
        public const int MillisecondsInDay = MillisecondsInHour * 24;

        public const string KilobyteSuffix = "kB";
        public const string MegabyteSuffix = "MB";
        public const string GigabyteSuffix = "GB";
        public const string TerabyteSuffix = "TB";
        public const string PetabyteSuffix = "PB";
        public const string ExabyteSuffix = "EB";
        public const string ZettabyteSuffix = "ZB";
        public const string YottabyteSuffix = "YB";

        public const string KibibyteSuffix = "KiB";
        public const string MebibyteSuffix = "MiB";
        public const string GibibyteSuffix = "GiB";
        public const string TebibyteSuffix = "TiB";
        public const string PebibyteSuffix = "PiB";
        public const string ExbibyteSuffix = "EiB";
        public const string ZebibyteSuffix = "ZiB";
        public const string YobibyteSuffix = "YiB";

        public static readonly Tuple<string, decimal>[] BinaryOrders = new Tuple<string, decimal>[]
        {
            new Tuple<string, decimal>("byte", 1),
            new Tuple<string, decimal>(KibibyteSuffix, KiB),
            new Tuple<string, decimal>(MebibyteSuffix, MiB),
            new Tuple<string, decimal>(GibibyteSuffix, GiB),
            new Tuple<string, decimal>(TebibyteSuffix, TiB),
            new Tuple<string, decimal>(PebibyteSuffix, PiB),
            new Tuple<string, decimal>(ExbibyteSuffix, EiB),
            new Tuple<string, decimal>(ZebibyteSuffix, ZiB)
        };
        public static readonly Tuple<string, decimal>[] DecimalOrders = new Tuple<string, decimal>[]
        {
            new Tuple<string, decimal>("byte", 1),
            new Tuple<string, decimal>(KilobyteSuffix, Kilo),
            new Tuple<string, decimal>(MegabyteSuffix, Mega),
            new Tuple<string, decimal>(GigabyteSuffix, Giga),
            new Tuple<string, decimal>(TerabyteSuffix, Tera),
            new Tuple<string, decimal>(PetabyteSuffix, Peta),
            new Tuple<string, decimal>(ExabyteSuffix, Exa),
            new Tuple<string, decimal>(ZettabyteSuffix, Zetta),
            new Tuple<string, decimal>(YottabyteSuffix, Yotta)
        };

    }
}
