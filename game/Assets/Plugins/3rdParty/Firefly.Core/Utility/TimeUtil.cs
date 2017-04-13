using System;

namespace Firefly.Core.Utility
{
    public static class TimeUtil
    {
        private static DateTime _DegineDataTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static long DeginTicks = _DegineDataTime.Ticks;

        public static long Offset = 0;

        public static long NowMilliseconds { get { return (DateTime.Now.ToUniversalTime().Ticks - DeginTicks) / TimeSpan.TicksPerMillisecond + Offset; } }

        public static DateTime Now { get { return DateTime.Now.ToUniversalTime(); } }

        public static DateTime TimestampToDataTime(long ts)
        {
            return _DegineDataTime.AddMilliseconds(ts);
        }

        public static bool IsSameDay(DateTime dt1, DateTime dt2)
        {
            return dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day;
        }

        public static bool IsSameDay(long ms1, long ms2)
        {
            DateTime dt1 = TimestampToDataTime(ms1);
            DateTime dt2 = TimestampToDataTime(ms2);

            return IsSameDay(dt1, dt2);
        }

        public static void Collabrate(long ts)
        {
            Offset += ts - NowMilliseconds;
        }
    }
}
