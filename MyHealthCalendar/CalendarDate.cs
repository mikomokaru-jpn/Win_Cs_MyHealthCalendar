using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHealthCalendar
{
    public enum MonthType
    {
        PreMonth = 0,
        ThidMonth = 1,
        NextMonth = 2
    }
    //コンストラクタ
    class CalendarDate
    {
        public int year;
        public int month;
        public int day;
        public int yearMonthDay;
        public DayOfWeek weekday;
        public String weekdaySymbol;
        public bool isHoliday = false;      //祝祭日フラグ
        public String holidayName = "";     //祝祭日名
        public MonthType monthType = MonthType.ThidMonth; //月タイプ

        //血圧データ
        public int upperValue = 0;
        public int lowerValue = 0;
        public bool confirm = false;

        public CalendarDate(DateTime date)
        {
            year = date.Year;
            month = date.Month;
            day = date.Day;
            weekday = date.DayOfWeek;
            weekdaySymbol = DateUtil.weekdaySymbol(date);
            yearMonthDay = DateUtil.intDate(date);
        }

    }
}
