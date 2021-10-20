using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace MyHealthCalendar
{
    partial class CalendarForm
    {
        //日付をダブルクリックする
        public void item_DoubleClick(object sender, EventArgs e)
        {
            //年月日
            entryForm.ymd = calendar.yearMonthDay(selectedIndex);
            entryForm.header.Text = String.Format("{0}年{1}月{2}日({3})",
                calendar.year,
                calendar.month,
                calendar.dayAtIndex(selectedIndex),
                calendar.weekdaySymbolAtIndex(selectedIndex));
            //データ入力フォームを開く
            Point myPoint = this.Location;
            entryForm.Location = new Point(this.Location.X + this.Width,
                                           this.Location.Y );
            DialogResult result = entryForm.ShowDialog();    
            //カレンダー再表示
            if (result == DialogResult.OK)
            {
                Debug.Print("entryForm OK");
                calendar.bloodPresureData();
                this.calendarDateSetting();
            }
        }
    }
}
