using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Market1
{
    public partial class fNakitKart : Form
    {
        public fNakitKart()
        {
            InitializeComponent();
        }

        private void tNakit_KeyDown(object sender, KeyEventArgs e)
        {
            //eger tnakit bos degilse 
            if (tNakit.Text != "")
            {
                //entere basılınca
                if (e.KeyCode == Keys.Enter)
                {
                    //hesapla metodunu cagiriyoruz
                    Hesapla();
                }
            } 
        }

        private void Hesapla()
        {
            //text kutusundaki degeri al satıs formundaki lnakite gonder
            fSatis f = (fSatis)Application.OpenForms["fSatis"];
            double nakit = Islemler.DoubleYap(tNakit.Text);
            double geneltoplam = Islemler.DoubleYap(f.tGenelToplam.Text);
            //kart tutarını hesaplamak icin genel toplamdan nakiti cıkartıyoruz
            double kart = geneltoplam - nakit;
            //tl degeri olarak yazdır
            f.lNakit.Text = nakit.ToString("C2");
            f.lKart.Text = kart.ToString("C2");
            f.SatisYap("Kart-Nakit");
            //ogeyi gizledik
            this.Hide();
        }


        private void bNx_Click(Object sender, EventArgs e)
        {
            //gelecek nesnenin de buton oldugunu belirttik
            Button b = (Button)sender;
            if (b.Text == ",")
            {
                //virgulleri saydırıyoruz
                int virgul = tNakit.Text.Count(x => x == ',');
                if (virgul < 1)
                {
                    tNakit.Text += b.Text;
                }
            }
            //backsapace tusu
            else if (b.Text == "<")
            {
                //nakit sıfırdan buyukse
                if (tNakit.Text.Length > 0)
                {
                    //son karakteri siler
                    tNakit.Text = tNakit.Text.Substring(0, tNakit.Text.Length - 1);
                }
            }
            else
            {
                tNakit.Text += b.Text;
            }
        }

        private void bEnter_Click(object sender, EventArgs e)
        {
            if (tNakit.Text != "")
            {
                Hesapla();
            }
        }

        private void tNakit_KeyPress(object sender, KeyPressEventArgs e)
        {
            //text kutusu sadece rakam karakterlerini alacak, silme tusu haric
            if (char.IsDigit(e.KeyChar)==false && e.KeyChar !=(char)08)
            {
                //yazmasını engelle
                e.Handled = true;
            }
        }
    }
}
