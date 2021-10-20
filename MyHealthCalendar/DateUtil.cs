using System;
using System.Globalization;


namespace MyHealthCalendar
{
    public static class DateUtil
    {
        //指定の日付から日数を加算した日付を返す
        public static DateTime addDays(DateTime date, int days)
        {
            return date.AddDays(days);
        }
        //指定の日付から月数を加算した日付を返す
        public static DateTime addMonths(DateTime date, int months)
        {
            return date.AddMonths(months);
        }
        //指定の日付から年数を加算した日付を返す
        public static DateTime addYears(DateTime date, int years)
        {
            return date.AddYears(years);
        }
        //指定の日付の月初日
        public static DateTime firstDate(DateTime date)
        {
            DateTime retDate = new DateTime(date.Year, date.Month, 1);
            return retDate;
        }
        //指定の日付の月末日
        public static DateTime lastDate(DateTime date)
        {
            int days = DateTime.DaysInMonth(date.Year, date.Month);

            DateTime retDate = new DateTime(date.Year, date.Month, days);
            return retDate;
        }
        //指定の日付の月の日数
        public static int daysOfMonth(DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }
        //指定の日付の年
        public static int intYear(DateTime date)
        {
            return date.Year;
        }
        //指定の日付の月
        public static int intMonth(DateTime date)
        {
            return date.Month;
        }
        //指定の日付の日
        public static int intDay(DateTime date)
        {
            return date.Day;
        }
        //指定の日付の曜日（コード）
        public static DayOfWeek weekday(DateTime date)
        {
            return date.DayOfWeek;
        }
        //指定の日付の曜日（短縮形文字）
        public static String weekdaySymbol(DateTime date)
        {
            return date.ToString("ddd");
        }
        ////指定の日付の整数表現
        public static int intDate(DateTime date)
        {
            return date.Year * 10000 + date.Month * 100 + date.Day;
        }
        //日付の比較：同じか否か
        public static bool isEqualDateTime(DateTime date1, DateTime date2)
        {
            return date1 == date2;
        }
        //日付の比較：同じか否か
        public static bool isEqualDate(DateTime date1, DateTime date2)
        {
            DateTime date1w = new DateTime(date1.Year, date1.Month, date1.Day);
            DateTime date2w = new DateTime(date2.Year, date2.Month, date2.Day);
            return date1w == date2w;
        }
        //和暦年
        public static String warekiOfYear(DateTime date)
        {
            CultureInfo culture = new CultureInfo("ja-JP", true);
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();
            String[] array = new string[2];
            return date.ToString("ggy", culture);
        }
    }
}
