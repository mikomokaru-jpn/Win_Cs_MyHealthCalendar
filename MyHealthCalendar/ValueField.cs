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
            if (this.Text.Length > 0)
            {   //値の設定
                this.Text = int.Parse(this.Text).ToString();
            }
            else
            {   //値なしはゼロとする
                this.Text = 0.ToString();
            }
            this.Select(this.Text.Length, 0);
        }
        //キー入力
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {   //ニューメリックチェック
                e.Handled = true;
                return;
            }
            if (this.Text.Length >= 3 && e.KeyChar != '\b')
            {   //3文字あるときは次の入力不可。ただしバックスペースは除く。　
                e.Handled = true;
                return;
            }
        }
        //フォーカス
        protected override void OnGotFocus(EventArgs e)
        {
            this.Select(this.Text.Length, 0);
        }
    }
}
