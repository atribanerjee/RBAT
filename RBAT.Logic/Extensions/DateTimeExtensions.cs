using RBAT.Core.Clasess;
using System;
using System.Globalization;

namespace RBAT.Logic.Extensions
{
    public static class DateTimeExtensions
    {
        public static (int timeComponentValue, int year) GetTimeComponentValueCalculatedFromBeginingOfTheYear(this DateTime date, TimeComponent timeComponent)
        {
            switch (timeComponent)
            {
                case TimeComponent.Month:
                    return (date.Month, date.Year);
                case TimeComponent.Week:
                    CultureInfo CI = new CultureInfo("en-US");
                    Calendar cal = CI.Calendar;
                    return (cal.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday), date.Year);
                case TimeComponent.Day:
                    return (date.DayOfYear, date.Year);
                default:
                    return (date.DayOfYear, date.Year);
            }
        }
    }
}
