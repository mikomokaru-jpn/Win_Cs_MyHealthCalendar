using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace MyHealthCalendar
{
    public delegate void PushNumberDelegate(int num); //Delegate宣言

    class EntryForm : Form
    {
        //NumberButton[] numbers = new NumberButton[10]; //数字ボタン
        //年月日
        public int ymd = 0;
        public Label header = new Label
        {
            Size = new Size(180, 40),
            Location = new Point(0, 0),
            Text = "xxxx年xx月yy日(x)",
            TextAlign = ContentAlignment.MiddleCenter,
        };
        //テキストフィールド
        public ValueField upperValue = new ValueField()
        {
            Location = new Point(10, 65),
        };
        public ValueField lowerValue = new ValueField()
        {
            Location = new Point(10, 115),
        };
        //確定チェックボックス
        public CheckBox confirm = new CheckBox()
        {
            Location = new Point(10, 155),
            Width = 50,
            Text = "確定",
        };

        //コンストラクタ
        public EntryForm()
        {
            this.ClientSize = new Size(180, 240);
            this.Text = "血圧を入力してください";     // タイトルを設定
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.DialogResult = DialogResult.Cancel;
            
            //血圧表示フィールド
            this.Controls.Add(upperValue);
            this.Controls.Add(lowerValue);

            var upperMidashi = new Label
            {
                Location = new Point(10, 45),
                AutoSize = true,
                Text = "最高血圧",
            };
            upperMidashi.Font = new Font(upperMidashi.Font.FontFamily, 9);
            this.Controls.Add(upperMidashi);

            var lowerMidashi = new Label
            {
                Location = new Point(10, 95),
                AutoSize = true,
                Text = "最低血圧",
            };
            lowerMidashi.Font = new Font(lowerMidashi.Font.FontFamily, 9);
            this.Controls.Add(lowerMidashi);

            //確定チェックボックス
            this.Controls.Add(confirm);
            confirm.KeyDown += new KeyEventHandler(confirm_KeyDown);

            //年月日
            header.Font = new Font(header.Font.FontFamily, 12);
            this.Controls.Add(header);

            //数値ボタン
            var num7 = new NumberButton(7, pressNumber)
            {
                Location = new Point(80, 50),
            };
            this.Controls.Add(num7);
            var num8 = new NumberButton(8, pressNumber)
            {
                Location = new Point(110, 50),
            };
            this.Controls.Add(num8);
            var num9 = new NumberButton(9, pressNumber)
            {
                Location = new Point(140, 50),
            };
            this.Controls.Add(num9);
            var num4 = new NumberButton(4, pressNumber)
            {
                Location = new Point(80, 80),
            };
            this.Controls.Add(num4);
            var num5 = new NumberButton(5, pressNumber)
            {
                Location = new Point(110, 80),
            };
            this.Controls.Add(num5);
            var num6 = new NumberButton(6, pressNumber)
            {
                Location = new Point(140, 80),
            };
            this.Controls.Add(num6);
            var num1 = new NumberButton(1, pressNumber)
            {
                Location = new Point(80, 110),
            };
            this.Controls.Add(num1);
            var num2 = new NumberButton(2, pressNumber)
            {
                Location = new Point(110, 110),
            };
            this.Controls.Add(num2);
            var num3 = new NumberButton(3, pressNumber)
            {
                Location = new Point(140, 110),
            };
            this.Controls.Add(num3);
            var num0 = new NumberButton(0, pressNumber)
            {
                Location = new Point(80, 140),
            };
            this.Controls.Add(num0);
            var clear = new NumberButton(-1, pressNumber)
            {
                Size = new Size(28+28+2, 28),
                Location = new Point(110, 140),
            };
            this.Controls.Add(clear);

            //登録ボタン
            var update = new Button()
            {
                Size = new Size(80, 25),
                FlatStyle = FlatStyle.Standard,
                Text = "登録",
            };
            update.Location = new Point(this.ClientSize.Width / 2 - update.Width / 2, 200);
            this.Controls.Add(update);
            update.Click += new EventHandler(update_Click);

            this.Load += new EventHandler(form_Load);
        }
        void form_Load(object sender, EventArgs e)
        {
            //血圧の取得
            var bpList = HTTPService.bpRecord(ymd);
            if (bpList.Count > 0)
            {
                upperValue.Text = bpList[0]["upper"].ToString();
                lowerValue.Text = bpList[0]["lower"].ToString();
                confirm.Checked = Convert.ToBoolean(bpList[0]["confirm"]);
            }
            else
            {
                upperValue.Text = 0.ToString();
                lowerValue.Text = 0.ToString();
                confirm.Checked = false;
            }
            this.ActiveControl = upperValue;
            //upperValue.Select(upperValue.Text.Length, 0);

        }

        void update_Click(object sender, EventArgs e)
        {
            //血圧の更新
            HTTPService.bpRecordUpdate(int.Parse(upperValue.Text), int.Parse(lowerValue.Text),
                                       ymd, Convert.ToInt32(confirm.Checked));
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        //コントロールのインデックス
        private int whichControl()
        {
            int ind = -1;
            foreach (Control ctr in this.Controls)
            {
                if (ctr.Focused)
                {
                    ind = (int)this.Controls.IndexOf(ctr);
                }
            }
            return ind;
        }
        
        //delegate method
        public void pressNumber(int num)
        {
            int thisIndex = whichControl();

            ValueField thisValue; 
            if (thisIndex == 0) //upper value
            {
                thisValue = upperValue;
            }
            else if (thisIndex == 1) //lower value
            {
                thisValue = lowerValue;
            }
            else
            {
                return;
            }
            if (num < 0)
            {
                thisValue.Text = "";    //クリア
            }
            else　if (thisValue.Text.Length < 3)
            {
                thisValue.Text += num.ToString(); //値の入力
            }
        }
        //確定チェックボックス
        void confirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                confirm.Checked = !confirm.Checked;
            }
        }
    }
}
