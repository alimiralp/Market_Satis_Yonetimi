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
    public partial class fUrunGiris : Form
    {
        public fUrunGiris()
        {
            InitializeComponent();
        }

        private void fUrunGiris_Load(object sender, EventArgs e)
        {
            //kayıtlı urunleri say ve urun sayısı kutuuguna yaz
            tUrunSayisi.Text = db.Urun.Count().ToString();
            //son kaydedilen 20 urunu listele
            gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
            //grid ozellestirmelerini getir
            Islemler.GridDuzenle1(gridUrunler);
            //urungrubu comboboxı veriler dolduruyoruz
            GrupDoldur();
        }
        BarkodDbEntities db = new BarkodDbEntities();
        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //eger entere basılılıyorsa
            if (e.KeyCode == Keys.Enter)
            {
                //boslukları al
                string barkod = tBarkod.Text.Trim();
                //database icerisinde urunler tablosunda var mı
                if (db.Urun.Any(a=> a.Barkod == barkod))
                {
                    //barkod varsa bilgileri urun giris ekranına getir
                    var urun = db.Urun.Where(a => a.Barkod == barkod).SingleOrDefault();
                    tUrunAdi.Text = urun.UrunAd;
                    tAciklama.Text = urun.Aciklama;
                    cmbUrunGrubu.Text = urun.UrunGrup;
                    tAlisFiyati.Text = urun.AlisFiyat.ToString();
                    tSatisFiyati.Text = urun.SatisFiyat.ToString();
                    tMiktar.Text = urun.Miktar.ToString();
                    tKdvOrani.Text = urun.KdvOrani.ToString();
                    //eger urun birimi kg ise 
                    if (urun.Birim=="Kg")
                    {
                        //isaretli ise kg tipi ver
                        chUrunTipi.Checked = true;
                    }
                    else
                    {
                        //isaretli degilse adet tipi ver
                        chUrunTipi.Checked = false;
                    }

                }
                else
                {
                    MessageBox.Show("Ürün kayıtlı değil, kaydedebilirsiniz.");
                }


            }
        }

        private void bKaydet_Click(object sender, EventArgs e)
        {
            //urungiris ekranındaki belirli kutucuklar bos degilse urunuekleyebilirsin
            if (tBarkod.Text != "" && tUrunAdi.Text != "" && cmbUrunGrubu.Text != "" && tAlisFiyati.Text != "" && tSatisFiyati.Text != "" && tKdvOrani.Text != "" && tMiktar.Text != "")
            {
                //eger database icinde barkod varsa miktar guncellemesi yap
                if(db.Urun.Any(a=> a.Barkod == tBarkod.Text))
                {
                    var guncelle = db.Urun.Where(a=> a.Barkod ==tBarkod.Text).SingleOrDefault();
                    guncelle.UrunAd = tUrunAdi.Text;
                    guncelle.Aciklama = tAciklama.Text;
                    guncelle.UrunGrup = cmbUrunGrubu.Text;
                    guncelle.AlisFiyat = Convert.ToDouble(tAlisFiyati.Text);
                    guncelle.SatisFiyat = Convert.ToDouble(tSatisFiyati.Text);
                    guncelle.KdvOrani = Convert.ToInt32(tKdvOrani.Text);
                    guncelle.KdvTutari = Math.Round(Islemler.DoubleYap(tSatisFiyati.Text) * Convert.ToInt32(tKdvOrani.Text) / 100, 2);
                    guncelle.Miktar += Convert.ToDouble(tMiktar.Text);
                    //eger ürün tipi kilogram isaretli ise kg isaretli degilse adet olsun
                    if (chUrunTipi.Checked)
                    {
                        guncelle.Birim = "Kg";
                    }
                    else
                    {
                        guncelle.Birim = "Adet";
                    }                    
                    guncelle.Tarih = DateTime.Now;
                    guncelle.Kullanici = lKullanici.Text;
                    db.SaveChanges();
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(10).ToList();
                }
                else
                {
                    //yoksa kutucuklardan girilen verileri al ve urunu kaydet, ardından gerekli satırları temizle
                    Urun urun = new Urun();
                    urun.Barkod = tBarkod.Text;
                    urun.UrunAd = tUrunAdi.Text;
                    urun.Aciklama = tAciklama.Text;
                    urun.UrunGrup = cmbUrunGrubu.Text;
                    urun.AlisFiyat = Convert.ToDouble(tAlisFiyati.Text);
                    urun.SatisFiyat = Convert.ToDouble(tSatisFiyati.Text);
                    urun.KdvOrani = Convert.ToInt32(tKdvOrani.Text);
                    urun.KdvTutari = Math.Round(Islemler.DoubleYap(tSatisFiyati.Text) * Convert.ToInt32(tKdvOrani.Text) / 100 ,2);
                    urun.Miktar = Convert.ToDouble(tMiktar.Text);
                    //eger ürün tipi kilogram isaretli ise kg isaretli degilse adet olsun
                    if (chUrunTipi.Checked)
                    {
                        urun.Birim = "Kg";
                    }
                    else
                    {
                        urun.Birim = "Adet";
                    }                   
                    urun.Tarih = DateTime.Now;
                    urun.Kullanici = lKullanici.Text;
                    db.Urun.Add(urun);
                    db.SaveChanges();
                    //kaydettikten sonra barkodno yu 1 arttır
                    if (tBarkod.Text.Length == 8)
                    {
                        var ozelbarkod = db.Barkod.First();
                        ozelbarkod.BarkodNo += 1;
                        db.SaveChanges();
                    }
                    Temizle();
                    //urun giris ekranında eklenmiş son 10 urunu gridde göster
                    gridUrunler.DataSource=db.Urun.OrderByDescending(a=> a.UrunId).Take(20).ToList();
                    Islemler.GridDuzenle1(gridUrunler);
                }
            }
            else
            {
                MessageBox.Show("Bilgi girişlerini kontrol ediniz.");
                tBarkod.Focus();
            }
        }
        
        private void tUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (tUrunAra.Text.Length >= 2)
            {
                //aranan urun varsa getir ve listele
                string urunad = tUrunAra.Text;
                gridUrunler.DataSource= db.Urun.Where(a=> a.UrunAd.Contains(tUrunAra.Text)).ToList();
                Islemler.GridDuzenle1(gridUrunler);
            }
        }

        private void blptal_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            tBarkod.Clear();
            tUrunAdi.Clear();
            tAciklama.Clear();
            tAlisFiyati.Text = "0";
            tSatisFiyati.Text = "0";
            tMiktar.Text = "0";
            tKdvOrani.Text = "8";
            tBarkod.Focus();
            //urun tipi otomatik adet olarak gelsin
            chUrunTipi.Checked = false;
        }

        private void bUrunGrubuEkle_Click(object sender, EventArgs e)
        {
            //furungrubu ekle formunu aç
            fUrunGrubuEkle f = new fUrunGrubuEkle();
            f.ShowDialog();
        }
        public void GrupDoldur()
        {
            //kullanıcıya urun grup ad goster arka planda id tut
            cmbUrunGrubu.DisplayMember = "UrunGrupAd";
            cmbUrunGrubu.ValueMember = "Id";
            //a dan z ye dogru sırala
            cmbUrunGrubu.DataSource = db.UrunGrup.OrderBy(a => a.UrunGrupAd).ToList();

        }

        private void bBarkodOlustur_Click(object sender, EventArgs e)
        {
            //tbarkod textine yeni bir barkod olusturacagiz

            var barkodno = db.Barkod.First();
            int karakter = barkodno.BarkodNo.ToString().Length;
            string sifirlar = string.Empty;
            // ornek barkod no 1 ise 00000001 , barkod no 122 ise 00000122 olacak
            for(int i = 0; i < 8 - karakter; i++)
            {
                sifirlar = sifirlar + "0";
            }
            string olusanbarkod = sifirlar + barkodno.BarkodNo.ToString();
            //tbarkod textine olusan barkodu yaz
            tBarkod.Text = olusanbarkod;
            tUrunAdi.Focus();
        }

        private void tSatisFiyati_KeyPress(object sender, KeyPressEventArgs e)
        {
            //girilen fiyata virgül ekleme, urun giris ekranında alıs ve satıs fiyatı bu metoda bagli
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridUrunler.Rows.Count > 0)
            {
                // ürün ekle sayfasında sag tik ile ürün silme islemi
                //urunid yi al
                int urunid = Convert.ToInt32(gridUrunler.CurrentRow.Cells["UrunId"].Value.ToString());
                //urun adını al
                string urunad = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();
                //barkodu al
                string barkod = gridUrunler.CurrentRow.Cells["Barkod"].Value.ToString();
                //... urununu silme islem onayı
                DialogResult onay = MessageBox.Show(urunad + " " +" ürününü silmek istiyor musunuz?", "Ürün Silme İşlemi",MessageBoxButtons.YesNo);
                //onaylıyorsa
                if (onay == DialogResult.Yes)
                {
                var urun = db.Urun.Find(urunid);
                //urunu kaldır
                db.Urun.Remove(urun);
                db.SaveChanges();
                //hizli urunlerde aldigimiz barkod degeri varsa al
                var hizliurun = db.HizliUrun.Where(x => x.Barkod == barkod).SingleOrDefault();
                 // hizli urun kutucuguna - isareti koy asagıdaki degiskenler icin
                 //eger hizliurun ici bos degilse
                    if (hizliurun!= null)
                    {
                        hizliurun.Barkod = "-";
                        hizliurun.UrunAd = "-";
                        //hizli urun kutucuk fiyatini 0 yap
                        hizliurun.Fiyat = 0;
                    }              
                db.SaveChanges();
                MessageBox.Show("Ürün silinmiştir");
                //son kaydedilen 20 urunu listele
                gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
                //grid ozellestirmelerini getir
                Islemler.GridDuzenle1(gridUrunler);
                //tbarkoda odaklan
                tBarkod.Focus();
                }
            }
            
        }

        private void chUrunTipi_CheckedChanged(object sender, EventArgs e)
        {
            if (chUrunTipi.Checked)
            {
                chUrunTipi.Text = "Gramajlı Ürün İşlemi";
                //gramajlı urun kaydederken barkod olusturmaya tıklanmasın
                bBarkodOlustur.Enabled = false;
            }
            else
            {
                chUrunTipi.Text = "Barkodlu Ürün İşlemi";
                //barkod olustur butonuna tiklanabilsin
                bBarkodOlustur.Enabled = true;
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //gridurunler listesinde urun varsa
            if (gridUrunler.Rows.Count>0)
            {
                //gridden asagidaki verileri urun giris ekranına aliyoruz
                tBarkod.Text = gridUrunler.CurrentRow.Cells["Barkod"].Value.ToString();
                tUrunAdi.Text = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();
                tAciklama.Text = gridUrunler.CurrentRow.Cells["Aciklama"].Value.ToString();
                cmbUrunGrubu.Text = gridUrunler.CurrentRow.Cells["UrunGrup"].Value.ToString();
                tAlisFiyati.Text = gridUrunler.CurrentRow.Cells["AlisFiyat"].Value.ToString();
                tSatisFiyati.Text = gridUrunler.CurrentRow.Cells["SatisFiyat"].Value.ToString();
                tKdvOrani.Text = gridUrunler.CurrentRow.Cells["KdvOrani"].Value.ToString();
                tMiktar.Text = gridUrunler.CurrentRow.Cells["Miktar"].Value.ToString();
                //birimi al ve ata
                string birim = gridUrunler.CurrentRow.Cells["Birim"].Value.ToString();
                //eger birim kg ise gramajlı satıs yapılıyor yaz
                if (birim=="Kg")
                {
                    chUrunTipi.Checked = true;
                }
                else
                {
                    chUrunTipi.Checked = false;
                }
            }
        }
    }
}
