using System;

/**
 *  Author: Nick Guy, http://stackoverflow.com/a/29908680
 *  Date: 01/12/2016
 *  Contains: Conversion Utils
 */
namespace Mod003263.utils {
    public class ConversionUtils {

        public static long ToEpochTime(DateTime dateTime) {
            DateTime date = dateTime.ToUniversalTime();
            long ticks = date.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
            long ts = ticks / TimeSpan.TicksPerSecond;
            return ts;
        }

        public static long ToEpochTime(DateTimeOffset dateTime) {
            DateTimeOffset date = dateTime.ToUniversalTime();
            long ticks = date.Ticks - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).Ticks;
            long ts = ticks / TimeSpan.TicksPerSecond;
            return ts;
        }

        public static DateTime ToDateTimeFromEpoch(long intDate) {
            long timeInTicks = intDate * TimeSpan.TicksPerSecond;
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks);
        }

        public static DateTimeOffset ToDateTimeOffsetFromEpoch(long intDate) {
            long timeInTicks = intDate * TimeSpan.TicksPerSecond;
            return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(timeInTicks);
        }

    }
}