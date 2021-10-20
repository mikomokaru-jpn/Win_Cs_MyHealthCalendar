using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace MyHealthCalendar
{
    class ValueField : TextBox
    {
        //コンストラクタ
        public ValueField() {
            Size = new Size(60, 0);
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 14);
            TextAlign = HorizontalAlignment.Right;
            //ReadOnly = true;
            BackColor = Color.White;
        }
        //一文字入力後
        protected override void OnTextChanged(EventArgs e)
        {
            //base.OnTextChanged(e);
            if (this.Text.Length > 0)
            {
                this.Text = int.Parse(this.Text).ToString();
            }
            else
            {
                this.Text = 0.ToString();
            }
            this.Select(this.Text.Length, 0);
        }
        //キー入力
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
                return;
            }
            if (this.Text.Length >= 3 && e.KeyChar != '\b')
            {
                e.Handled = true;
                return;
            }

        }


    }
}
