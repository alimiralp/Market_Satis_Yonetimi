using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Market1
{
    internal class Nesnelerim
    {
    }

    //Sınıfımıza label ozelliKlerini lstandart classına kazandırdık
    //MİRAS ALMAYI KULLANDIK
    class lStandart : Label
    {
        //sınıfımızın kurucu metotdunu olusturuyoruz
        public lStandart()
        {
            //forecolor rengi belirledik
            this.ForeColor = System.Drawing.Color.DarkCyan;
            this.Text = "lStandart";
            // fontumuzu, puntomuzu, font stilimizi , yazı tipi boyutunun point cinsinden olacagini belirttik
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "lStandart";
        }
    }

    class bStandart:Button
    {
        //BUTTON ÖZELLİĞİ KAZANMAK İÇİN MİRAS ALMA KULLANIYORUZ
        //sınıfımızın kurucu metotdunu olusturuyoruz
        public bStandart() 
        {
                //fsatis formunda nakit butonunun form özelliklerini kopyalayıp özellestirdik
                this.BackColor = System.Drawing.Color.Tomato;
                this.FlatAppearance.BorderColor = System.Drawing.Color.Tomato;
                this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                this.ForeColor = System.Drawing.Color.White;
                this.Image = global::Market1.Properties.Resources.tl_48;
                this.Location = new System.Drawing.Point(1, 1);
                this.Margin = new System.Windows.Forms.Padding(1);
                this.Name = "bNakit";
                this.Size = new System.Drawing.Size(142, 117);
                this.TabIndex = 0;
                this.Text = "NAKİT\r\n(F1)";
                this.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
                this.UseVisualStyleBackColor = false;    
        }
    }

    //miras alma ile textbox özelliği kazandırıyoruz
    class tStandart: TextBox
    {
        //kurucu metot olusturuyoruz
        public tStandart()
        {
            //textbox özelliklerini belirledik
            this.Size = new System.Drawing.Size(250, 26);
            this.BackColor= System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);


        }
    }

    class tNumeric : TextBox
    {
        //kurucu metodu olusturuyoruz
        public tNumeric()
        {
            //textbox özelliklerini belirledik
            this.Size = new System.Drawing.Size(115, 26);
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Name = "tNumeric";
            //yazıyı saga yasla
            this.TextAlign=System.Windows.Forms.HorizontalAlignment.Right;
            //tıklandıgında tnumericlick metodunu cagir
            this.Click += TNumeric_Click;
            //sadece sayıları yazdırmak icin keypress metoduna bak
            this.KeyPress += TNumeric_KeyPress;
        }

        private void TNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece sayıların ve backsapace isaretinin ve virgulun yazılmasına izin ver
           if (char.IsDigit(e.KeyChar) == false && e.KeyChar!= (char)08 && e.KeyChar!=(char)44)
            {
                //tus basımını engelle
                e.Handled = true;
            }
        }

        private void TNumeric_Click(object sender, EventArgs e)
        {
            //üzerine tıklanıldıgı zaman içerisindeki tüm karakterleri seç
            this.SelectAll();
        }
    }

    //miras alma ile datagrid özelliği kazandırıyoruz
    class gridOzel : DataGridView
    {
        //kurucu metodunu olusturuyoruz
        public gridOzel()
        {
            //gridin özelliklerini fsatis windows formundan kopyaladigimiz özellikler ile belirliyoruz
            this.AllowUserToAddRows = false;
            this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnHeadersDefaultCellStyle = this.DefaultCellStyle;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnableHeadersVisualStyles = false;
            this.Location = new System.Drawing.Point(3, 103);
            this.Name = "gridSatisListesi";
            this.RowHeadersVisible = false;
            this.RowHeadersWidth = 51;
            this.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.RowsDefaultCellStyle = this.DefaultCellStyle;
            this.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Silver;
            this.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.RowTemplate.Height = 32;
            this.Size = new System.Drawing.Size(716, 468);
        }
    }

    
}
