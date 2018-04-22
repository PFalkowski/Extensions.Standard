namespace Extensions.Standard
{
    public static class Constants
    {
        public const byte BitsInByte = sizeof(byte);

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
        public const ulong Penta = Tera * 1000;

        /// <summary>
        ///     1000000000000000000
        /// </summary>
        public const ulong Exa = Penta * 1000;



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
        

        public const ulong BitsInKilobyte = BitsInByte * Kilo;

        public const ulong BitsInMegabyte = BitsInByte * Mega;

        public const ulong BitsInGigabyte = BitsInByte * Giga;

        public const ulong BitsInTerabyte = BitsInByte * Tera;

        public const ulong BitsInPentabyte = BitsInByte * Penta;

        public const ulong BitsInExabyte = BitsInByte * Exa;


        public const ulong BitsInKibibyte = BitsInByte * KiB;

        public const ulong BitsInMebibyte = BitsInByte * MiB;

        public const ulong BitsInGibibyte = BitsInByte * GiB;

        public const ulong BitsInTebibyte = BitsInByte * TiB;

        public const ulong BitsInPebibyte = BitsInByte * PiB;

        public const ulong BitsInExbibyte = BitsInByte * EiB;


        public const int MillisecondsInSecond = 1000;
        public const int MillisecondsInMinute = MillisecondsInSecond * 60;
        public const int MillisecondsInHour = MillisecondsInMinute * 60;
        public const int MillisecondsInDay = MillisecondsInHour * 24;

    }
}
