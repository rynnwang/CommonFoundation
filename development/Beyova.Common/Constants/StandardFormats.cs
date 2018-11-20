namespace Beyova
{
    /// <summary>
    /// Class StandardFormats
    /// </summary>
    public static class StandardFormats
    {
        #region Format Constants

        /// <summary>
        /// The date time format for commonly use. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01 12:01:02.
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// The local date time format: yyyy-MM-dd HH:mm:ss zzzz
        /// </summary>
        public const string LocalDateTimeFormat = "yyyy-MM-dd HH:mm:ss zzzz";

        /// <summary>
        /// The full date time format. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01 12:01:02.027.
        /// </summary>
        public const string FullDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// The date time format for commonly use. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01.
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// The full date time tz format: yyyy-MM-ddTHH:mm:ss.fffZ
        /// </summary>
        public const string FullDateTimeTZFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        #endregion Format Constants
    }
}