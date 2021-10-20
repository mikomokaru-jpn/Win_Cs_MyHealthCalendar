using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace MyHealthCalendar
{
    public partial class CalendarForm : Form
    {
        private Calendar calendar;
        private List<DayItem> itemList = new List<DayItem>(); //日付コントロールリスト
        private Label label1;                   //年月見出し1
        private Label label2;                   //年月見出し2
        private int days = 0;                   //当月カレンダ日数（28 or 35 or 42）
        private int selectedIndex = 0;          //フォーカスのある日付
        EntryForm entryForm = new EntryForm();  //データ入力フォーム

        //コンストラクタ
        public CalendarForm()
        {
            this.ClientSize = new Size(324, 280);
            this.Text = "Simple Calendar";      // タイトルを設定
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.createControls();
            this.calendarDateSetting();
        }
        //コントロールの作成と配置
        private void createControls()
        {
            //ヘッダパネル
            Panel header = new Panel()
            {
                Size = new Size(304, 60),
                Location = new Point(10, 10),
                //BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(header);

            //見出し
            label1 = new Label()
            {
                Width = 140,
                Height = 30,
                Location = new Point(40, 0),
                TextAlign = ContentAlignment.MiddleRight

            };
            header.Controls.Add(label1);
            label2 = new Label()
            {
                Width = 80,
                Height = 30,
                Location = new Point(180, 0),
                TextAlign = ContentAlignment.MiddleLeft

            };
            header.Controls.Add(label2);
            //前月へボタン
            Button preButton = new Button()
            {
                Location = new Point(0, 0),
                Size = new Size(40, 30),
                Text = "←",
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 12),

            };
            header.Controls.Add(preButton);
            preButton.MouseUp += new MouseEventHandler(preButton_MouseUp);

            //翌月へボタン
            Button nextButton = new Button()
            {
                Location = new Point(304 - 40, 0),
                Size = new Size(40, 30),
                Text = "→",
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 12),
            };
            header.Controls.Add(nextButton);
            nextButton.MouseUp += new MouseEventHandler(nextButton_MouseUp);
            //曜日見出し
            var youbis = new string[] { "月", "火", "水", "木", "金", "土", "日" };
            for (int i = 0; i < 7; i++)
            {
                Label youbi = new Label()
                {
                    Location = new Point(i * 44, 37),
                    Size = new Size(40, 20),
                    TextAlign = ContentAlignment.TopCenter,
                    Text = youbis[i],
                };
                youbi.Font = new Font(youbi.Font.FontFamily, 10);
                header.Controls.Add(youbi);
            }
            //日付パネル
            Panel panel = new Panel()
            {
                Size = new Size(304, 200),
                Location = new Point(10, 70),
            };
            //panel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panel);

            //日付コントロールの作成
            for (int j = 0; j < 6; j++) //週
            {
                for (int i = 0; i < 7; i++) //曜日
                {
                    DayItem dayItem = new DayItem()
                    {
                        Location = new Point(i * 44, j * 34),
                        Size = new Size(40, 30),
                        BackColor = Color.White,
                    };
                    panel.Controls.Add(dayItem);
                    itemList.Add(dayItem);
                    dayItem.PreviewKeyDown += new PreviewKeyDownEventHandler(item_PreviewKeyDown);

                    dayItem.DoubleClick += new EventHandler(item_DoubleClick);
                }
            }
            //カレンダー日付の作成
            calendar = new Calendar();
        }

        //カレンダー日付の設定
        private void calendarDateSetting()
        {
            days = 0;
            //年月見出し
            label1.Text = String.Format("{0}年{1}月", calendar.year, calendar.month);
            label1.Font = new Font(label1.Font.FontFamily, 18);
            label2.Text = String.Format("{0}年", calendar.woreki);
            label2.Font = new Font(label2.Font.FontFamily, 12);
            //日付
            for (int i = 0; i < itemList.Count; i++)
            {
                DayItem dayItem = itemList[i];
                if (i < calendar.daysOfCalendar)
                {
                    dayItem.Visible = true;
                    dayItem.day = calendar.dayAtIndex(i);           //日付
                    dayItem.weekDay = calendar.weekdayAtIndex(i);   //曜日
                    //文字のフォント
                    if (calendar.monthTYpe(i) == MonthType.ThidMonth)
                    {
                        dayItem.Font = new Font(dayItem.Font.FontFamily, 16);
                    }
                    else
                    {
                        dayItem.Font = new Font(dayItem.Font.FontFamily, 9);
                    }
                    //祝祭日
                    if (calendar.isHoliday(i))
                    {
                        dayItem.holiday = true;
                    }
                    else
                    {
                        dayItem.holiday = false;
                    }
                    //当日
                    if (calendar.isCurrentDate(i))
                    {
                        dayItem.today = true;

                        this.ActiveControl = itemList[i];
                        selectedIndex = i;
                    }
                    else
                    {
                        dayItem.today = false;
                    }
                    //血圧
                    if (calendar.haveBloodPresureData(i))
                    {
                        dayItem.confirm = true;
                    }
                    else
                    {
                        dayItem.confirm = false;
                    }
                    dayItem.Click += new EventHandler(item_Click);
                    dayItem.Invalidate();
                    days++;
                }
                else
                {
                    dayItem.Visible = false;
                }
            }
        }
        //日付のクリック
        void item_Click(object sender, EventArgs e)
        {
            DayItem selectedItem = (DayItem)sender;

            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] == selectedItem)
                {
                    selectedIndex = i;
                    selectedItem.Focus();
                }
            }
        }
        //前月へボタン
        void preButton_MouseUp(object sender, MouseEventArgs e)
        {
            calendar.createCalendar(-1);
            this.calendarDateSetting();
            selectedIndex = days - 1;
            itemList[selectedIndex].Focus();
        }
        //翌月へボタン
        void nextButton_MouseUp(object sender, MouseEventArgs e)
        {
            calendar.createCalendar(1);
            this.calendarDateSetting();
            selectedIndex = 0;
            itemList[selectedIndex].Focus();
        }

        //矢印キー
        void item_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Debug.Print(e.KeyCode.ToString());
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Left && e.Shift)
            {   //前月
                calendar.createCalendar(-1);
                this.calendarDateSetting();
                selectedIndex = days - 1;
                itemList[selectedIndex].Focus();
            }
            else if (e.KeyCode == Keys.Right && e.Shift)
            {   //翌月
                calendar.createCalendar(1);
                this.calendarDateSetting();
                selectedIndex = 0;
                itemList[selectedIndex].Focus();
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (selectedIndex < days - 1)
                {
                    selectedIndex++;
                    itemList[selectedIndex].Focus();
                }
                else
                {   //翌月
                    calendar.createCalendar(1);
                    this.calendarDateSetting();
                    selectedIndex = 0;
                    itemList[selectedIndex].Focus();
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (selectedIndex > 0)
                {
                    selectedIndex--;
                    itemList[selectedIndex].Focus();
                }
                else
                {   //前月
                    calendar.createCalendar(-1);
                    this.calendarDateSetting();
                    selectedIndex = days - 1;
                    itemList[selectedIndex].Focus();
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (selectedIndex > 6)
                {
                    selectedIndex -= 7;
                    itemList[selectedIndex].Focus();
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (selectedIndex < days - 7)
                {
                    selectedIndex += 7;
                    itemList[selectedIndex].Focus();
                }
            }

        }

    }
}
