using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace MyHealthCalendar
{
    class DayItem : Control
    {
        public int day = 0;                 //日付 DD
        public DayOfWeek weekDay = 0;       //曜日
        public bool today = false;          //当日
        public bool holiday = false;        //祝祭日
        public bool focused = false;        //選択中 
        //血圧
        public bool confirm = false;        //確定フラグ
        public int upperValue = 0;
        public int lowerValue = 0;

        Pen penNormal = new Pen(Color.Gray, 2);
        Pen penFocused = new Pen(Color.Blue, 4);
        Point[] mark = { new Point(0, 0), new Point(10, 0), new Point(0, 10) };
        
        //コンストラクタ
        public DayItem()
        {
            BackColor = Color.White;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            String text = day.ToString();
            Size textSize = TextRenderer.MeasureText(text, Font);
            Point point = new Point((Width / 2) - (textSize.Width / 2) + 1, 
                                    (Height / 2) - (textSize.Height / 2) - 1);


            //曜日による文字色の変更
            var brush = Brushes.Black;
            if (weekDay == DayOfWeek.Sunday || holiday)
            {
                brush = Brushes.Red;
            }else if (weekDay == DayOfWeek.Saturday)
            {
                brush = Brushes.Blue;
            }
            e.Graphics.DrawString(text,Font, brush, point.X, point.Y);
            //現在日
            
            if (today)
            {
                this.BackColor = Color.Yellow;
            }
            else
            {
                this.BackColor = Color.White;
            }
            
            //血圧
            
            if (confirm)
            {

                e.Graphics.FillPolygon(Brushes.Red, mark);
            }
           
            //枠線
            if (focused){
                e.Graphics.DrawRectangle(penFocused, 0, 0, Width , Height);
            }else{
                e.Graphics.DrawRectangle(penNormal, 0, 0, Width, Height);
            }

        }



        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.focused = true;
            this.Invalidate();

        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.focused = false;
            this.Invalidate();

        }
    }
}
