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
    public partial class fGelirGider : Form
    {
        public fGelirGider()
        {
            InitializeComponent();
        }

        public string gelirgider { get; set; }

        public string kullanici { get; set; }
        private void fGelirGider_Load(object sender, EventArgs e)
        {
            //gelir gider textinde rapor sayfasından giden gelir veya gider degeri olacak işlem buna göre gerçekleşecek
            lGelirGider.Text = gelirgider + " " + "İŞLEMİ YAPILIYOR";
        }

        private void cmbOdemeTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOdemeTuru.SelectedIndex==0) //nakit secili ise
            {
                //sadece nakit miktari girilebilir
                tNakit.Enabled = true;
                tKart.Enabled = false;
            }
            else if (cmbOdemeTuru.SelectedIndex == 1) //kart secili ise
            {
                //sadece kart para miktari girilebilir
                tNakit.Enabled = false;
                tKart.Enabled = true;
            }
            else if (cmbOdemeTuru.SelectedIndex == 2) //kart-nakit secili ise
            {
                //iki kutucuga da veri girilebilsin
                tNakit.Enabled = true;
                tKart.Enabled = true;
            }
            //islem bitince textleri 0 yap
            tNakit.Text = "0";
            tKart.Text = "0";

        }

        private void bEkle_Click(object sender, EventArgs e)
        {
            //odeme turu secili ise
            if (cmbOdemeTuru.Text!="")
            {
                if (tNakit.Text!="" && tKart.Text!="")
                {
                    //veritabani baglantisini sagladik
                    using (var db = new BarkodDbEntities())
                    {
                        //veritabanina gelir gider islemlerini isleyecegiz
                        IslemOzet io = new IslemOzet();
                        io.IslemNo = 0;
                        io.Iade = false;
                        io.OdemeSekli = cmbOdemeTuru.Text;
                        io.Nakit = Islemler.DoubleYap(tNakit.Text);
                        io.Kart = Islemler.DoubleYap(tKart.Text);
                        //eger gelir islemi ise
                        if (gelirgider=="GELİR")
                        {
                            //veritabanina gelir olarak isle
                            io.Gelir = true;
                            io.Gider = false;
                        }
                        //degilse veritabanina gider olarak yaz
                        else
                        {
                            io.Gelir = false;
                            io.Gider = true;
                        }
                        io.AlisFiyatToplam = 0;
                        //islemi ve aciklamasini yaz
                        io.Aciklama = gelirgider + " - İşlemi " + tAciklama.Text;
                        io.Tarih = dtTarih.Value;
                        io.Kullanici = kullanici;
                        //yukaridaki io modelini islemozet tablosuna ekle
                        db.IslemOzet.Add(io);
                        db.SaveChanges();
                        MessageBox.Show(gelirgider + " işlemi kaydedilmiştir");
                        tNakit.Text = "0";
                        tKart.Text = "0";
                        tAciklama.Clear();
                        cmbOdemeTuru.Text = "";
                        //bu islemi yaptıktan sonra hemen rapor sayfasini güncelleyelim
                        //frapor sayfasi da altta aciksa
                        fRapor f = (fRapor)Application.OpenForms["fRapor"];
                        //rapor sayfasindaki göster butonuna tikla ki grid yenilensin girilen gelir gider islemi gride hemen yazilsin
                        if (f != null)
                        {
                            f.bGoster_Click(null, null);
                        }
                        //islem yapildiktan sonra formu gizle
                        this.Hide();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen ödeme türünü seçiniz");
            }
        }
    }
}
