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
    public partial class fFiyatGuncelle : Form
    {
        public fFiyatGuncelle()
        {
            InitializeComponent();
        }

        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //eger barkod girilip entere basilirsa
            if (e.KeyCode==Keys.Enter)
            {
                //database baglantisi sagladik
                using (var db = new BarkodDbEntities())
                {
                    //barkod urun tablosunda var ise
                    if (db.Urun.Any(x=> x.Barkod==tBarkod.Text))
                    {
                        //barkodu databaseden getir
                        var getir = db.Urun.Where(x => x.Barkod == tBarkod.Text).SingleOrDefault();
                        lBarkod.Text = getir.Barkod;
                        lUrunAdi.Text = getir.UrunAd;
                        //double türünden mevcut fiyati aldik
                        double mevcutfiyat = Convert.ToDouble(getir.SatisFiyat);
                        lMevcutFiyat.Text = mevcutfiyat.ToString("C2");
                    }
                    else
                    {
                        MessageBox.Show("Ürün kayıtlı değildir.");
                    }
                }
            }
        }

        private void bKaydet_Click(object sender, EventArgs e)
        {
            if (tYeniFiyat.Text!= "" && lBarkod.Text!= "")
            {
                //veritabani baglantisi
                using (var db = new BarkodDbEntities())
                {
                    //barkodu aldik ve urun fiyatini guncelledik
                    var guncellenecek = db.Urun.Where(x=> x.Barkod==lBarkod.Text).SingleOrDefault();
                    guncellenecek.SatisFiyat = Islemler.DoubleYap(tYeniFiyat.Text);
                    //yeni kdv tutari hesaplama islemi
                    int kdvorani = Convert.ToInt16(guncellenecek.KdvOrani);
                    Math.Round(Islemler.DoubleYap(tYeniFiyat.Text) * kdvorani / 100, 2);
                    db.SaveChanges();
                    MessageBox.Show("Fiyat kaydedilmiştir");
                    //textleri temizle
                    lBarkod.Text = "";
                    lUrunAdi.Text ="";
                    lMevcutFiyat.Text = "";
                    tYeniFiyat.Clear();
                    tBarkod.Clear();
                    tBarkod.Focus();
                }
            }
            else
            {
                MessageBox.Show("Ürün okutunuz");
                tBarkod.Focus();
            }
        }

        private void fFiyatGuncelle_Load(object sender, EventArgs e)
        {
            lBarkod.Text = "";
            lUrunAdi.Text = "";
            lMevcutFiyat.Text = "";
        }
    }
}
