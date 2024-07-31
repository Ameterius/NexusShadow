using System;

//date utillity

namespace NexusShadow.Core.Subcore
{
    public static class DateUtillity
    {
        public static string GetCurrentDateTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static DateTime GetNextWorkday(DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            } while (IsWeekend(date));

            return date;
        }
    }
}