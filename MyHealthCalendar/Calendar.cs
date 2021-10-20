using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Reflection;
using System.Diagnostics;

namespace MyHealthCalendar
{
    class Calendar
    {
        //当年
        public int year
        {
            get { return DateUtil.intYear(firstDateOfMonth); }
        }
        //当年和暦
        public String woreki
        {
            get { return DateUtil.warekiOfYear(firstDateOfMonth); }
        }
        //当月
        public int month
        {
            get { return DateUtil.intMonth(firstDateOfMonth); }
        }
        //カレンダの日数 28 or 35 or 42
        public int daysOfCalendar
        {
            get { return dateList.Count; }
        }
        //カレンダー最初の日
        public int startOfCalendar
        {
            get { return dateList[0].yearMonthDay; }
        }
        //カレンダー最後の日
        public int endOfCalendar
        {
            get { return dateList[dateList.Count - 1].yearMonthDay; }
        }
        //休日ファイル
        Dictionary<int, String> holidayList;

        //コンストラクタ
        public Calendar()
        {
            firstDateOfMonth = DateUtil.firstDate(DateTime.Now);

            //var deskTop = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //var path = Path.Combine(deskTop, "holiday.json");
            //StreamReader reader = new StreamReader(path, Encoding.GetEncoding("utf-8"));

            //休日ファイルをリソースから読み込む
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MyHealthCalendar.Resources.holiday.json";
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resourceName));
            String jsonString = reader.ReadToEnd();
            holidayList = JsonSerializer.Deserialize<Dictionary<int, String>>(jsonString);
            //カレンダー日付の作成
            createDateList();
            Console.WriteLine("createDateList");
        }

        int currentDateIndex = 0;   //現在日のインデックス
        int firstDateIndex = 0;     //1日のインデックス
        int lastDateIndex = 0;     //末日のインデックス

        //指定のインデックスの年
        public int yearAtIndex(int index)
        {
            return dateList[index].year;
        }
        //指定のインデックスの月
        public int monthAtIndex(int index)
        {
            return dateList[index].month;
        }
        //指定のインデックスの日
        public int dayAtIndex(int index)
        {
            return dateList[index].day;
        }
        //指定のインデックスの年月日
        public int yearMonthDay(int index)
        {
            return dateList[index].yearMonthDay;
        }
        //指定のインデックスの曜日コード
        public DayOfWeek weekdayAtIndex(int index)
        {
            return dateList[index].weekday;
        }
        //指定のインデックスの曜日シンボル
        public String weekdaySymbolAtIndex(int index)
        {
            return dateList[index].weekdaySymbol;
        }
        //指定のインデックスは祝祭日か？
        public bool isHoliday(int index)
        {
            return dateList[index].isHoliday;
        }
        //指定のインデックスが月タイプ
        public MonthType monthTYpe(int index)
        {
            return dateList[index].monthType;
        }
        //指定のインデックスが当日か？
        public bool isCurrentDate(int index)
        {
             return index == currentDateIndex;
        }

        public bool haveBloodPresureData(int index)
        {
            return dateList[index].confirm;
        }


        //月を移動してカレンダーを作成する
        public void createCalendar(int months)
        {
            firstDateOfMonth = DateUtil.addMonths(firstDateOfMonth, months);
            this.createDateList();
        }

        private DateTime firstDateOfMonth;      //当月の1日
        private List<CalendarDate> dateList;    //日付の配列
        private int wkIndex = 0;

        //カレンダ日付の作成
        private void createDateList()
        {
            wkIndex = 0;
            dateList = new List<CalendarDate>();
            currentDateIndex = -1;
            var table = new int[] { 6, 0, 1, 2, 3, 4, 5,};
            //前月処理
            var weekOf1st = (int)DateUtil.weekday(firstDateOfMonth);
            var preDays = table[weekOf1st];
            var preDate = DateUtil.addDays(firstDateOfMonth, -preDays);
            for (int i = 0; i < preDays; i++)
            {
                makeDate(DateUtil.addDays(preDate, i), MonthType.PreMonth);
            }
            //当月処理
            int daysOfThisMonth = DateUtil.daysOfMonth(firstDateOfMonth);
            for (int i = 0; i < daysOfThisMonth; i++) 
            {
                if (i == 0){
                    firstDateIndex = wkIndex;   //1日のインデックス
                }
                if (i == daysOfThisMonth -1)
                {
                    lastDateIndex = wkIndex;    //末日のインデックス
                }
                makeDate(DateUtil.addDays(firstDateOfMonth, i), MonthType.ThidMonth);
            }
            //翌月処理
            var firstDateNext = DateUtil.addMonths(firstDateOfMonth, 1);
            var nextDays = (7 - (dateList.Count % 7)) % 7;
            for (int i = 0; i < nextDays; i++)
            {
                makeDate(DateUtil.addDays(firstDateNext, i), MonthType.NextMonth);
            }
            //血圧
            bloodPresureData();

        }
        //日付の作成
        private void makeDate(DateTime date, MonthType type)
        {
            CalendarDate calDate = new CalendarDate(date);
            calDate.monthType = type;
            dateList.Add(calDate);
            if (DateUtil.isEqualDate(date, DateTime.Now))
            {
                currentDateIndex = wkIndex; //現在日のインデックス
            }
            var key = DateUtil.intDate(date);
            if (holidayList.ContainsKey(key))
            {
                Console.WriteLine(holidayList[key]);
                calDate.isHoliday = true;
                calDate.holidayName = holidayList[key];
            }
            wkIndex++;
        }
        //血圧データの設定
        public void bloodPresureData()
        {
            //血圧データの取得
            var bpList = HTTPService.bpRecordList(startOfCalendar, endOfCalendar);
            foreach (CalendarDate dt in dateList)
            {
                foreach (Dictionary<string, int> dict in bpList)
                {
                    if (dict["date"] == dt.yearMonthDay)
                    {
                        dt.upperValue = dict["upper"];
                        dt.lowerValue = dict["lower"];
                        dt.confirm = Convert.ToBoolean(dict["confirm"]);
                        Debug.Print(dict["date"].ToString());
                        break;
                    }
                }
            }

        }
    }
}
