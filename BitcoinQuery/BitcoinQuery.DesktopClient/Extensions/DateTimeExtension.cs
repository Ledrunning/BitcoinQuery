using System;

namespace BitcoinQuery.DesktopClient.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            // Unix start date and time
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var utcDateTime = unixEpoch.AddSeconds(unixTimeStamp);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.Local);
        }
    }
}