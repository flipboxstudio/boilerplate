namespace System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Convert to unix epoch date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixEpochDate(this DateTime date)
        {
            return (long) Math.Round((date.ToUniversalTime() -
                                      new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }
    }
}