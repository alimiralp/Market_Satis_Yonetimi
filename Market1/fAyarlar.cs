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
    public partial class fAyarlar : Form
    {
        public fAyarlar()
        {
            InitializeComponent();
        }

        private void Temizle()
        {
            //temizleme metodu olusturduk
            tAdSoyad.Clear();
            tTelefon.Clear();
            tEposta.Clear();
            tKullanici.Clear();
            tSifre.Clear();
            tSifreTekrar.Clear();
            chSatisEkrani.Checked = false;
            chRapor.Checked = false;
            chUrunGiris.Checked = false;
            chAyarlar.Checked = false;
            chFiyatGuncelle.Checked = false;
        }
        private void bKaydet_Click(object sender, EventArgs e)
        {
            //kaydet yapiliyor ise
            if (bKaydet.Text == "Kaydet")
            {
                //kutucukların hepsi dolu ise
                if (tAdSoyad.Text != "" && tTelefon.Text != "" && tKullanici.Text != "" && tSifre.Text != "" && tSifreTekrar.Text != "")
                {
                    //girilen sifre sifre tekrari ile aynı ise
                    if (tSifre.Text == tSifreTekrar.Text)
                    {
                        //try blok hata olusursa program kirilmadan programın devam etmesini saglar
                        try
                        {
                            //database baglantisi
                            using (var db = new BarkodDbEntities())
                            {
                                //kullanici sistemde yok ise
                                if (!db.Kullanici.Any(x => x.KullaniciAd == tKullanici.Text))
                                {
                                    //kullanici bilgi ve yetkilerini kaydet
                                    Kullanici k = new Kullanici();
                                    k.AdSoyad = tAdSoyad.Text;
                                    k.Telefon = tTelefon.Text;
                                    k.EPosta = tEposta.Text;
                                    k.KullaniciAd = tKullanici.Text.Trim();
                                    k.Sifre = tSifre.Text;
                                    k.Satis = chSatisEkrani.Checked;
                                    k.Rapor = chRapor.Checked;
                                    k.UrunGiris = chUrunGiris.Checked;
                                    k.Ayarlar = chAyarlar.Checked;
                                    k.FiyatGuncelle = chFiyatGuncelle.Checked;
                                    //database'e ekle
                                    db.Kullanici.Add(k);
                                    //degisiklikleri kaydet
                                    db.SaveChanges();
                                    //gride adsoyad, kullaniciad, telefon yaz
                                    Doldur();
                                    Temizle();
                                }
                                //kullanici sistemde kayıtlı ise
                                else
                                {
                                    MessageBox.Show("Bu kullanıcı zaten kayıtlı durumdadır.");
                                }
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Hata oluştu");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Şifreniz tekrarı ile uyuşmamaktadır.");
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen kullanıcı işlemleri alanlarının hepsini eksiksiz şekilde doldurunuz.");
                }
            }
            else if (bKaydet.Text == "Düzenle/Kaydet")
            {
                //kutucukların hepsi dolu ise
                if (tAdSoyad.Text != "" && tTelefon.Text != "" && tKullanici.Text != "" && tSifre.Text != "" && tSifreTekrar.Text != "")
                {
                    //girilen sifre sifre tekrari ile aynı ise
                    if (tSifre.Text == tSifreTekrar.Text)
                    {
                        //id al 
                        int id = Convert.ToInt32(lKullaniciId.Text);
                        using (var db = new BarkodDbEntities())
                        {
                            //kullanici bilgilerini guncelliyoruz
                            var guncelle = db.Kullanici.Where(x => x.Id == id).FirstOrDefault();
                            guncelle.AdSoyad = tAdSoyad.Text;
                            guncelle.Telefon = tTelefon.Text;
                            guncelle.EPosta = tEposta.Text;
                            guncelle.KullaniciAd = tKullanici.Text.Trim();
                            guncelle.Sifre = tSifre.Text;
                            guncelle.Satis = chSatisEkrani.Checked;
                            guncelle.Rapor = chRapor.Checked;
                            guncelle.UrunGiris = chUrunGiris.Checked;
                            guncelle.Ayarlar = chAyarlar.Checked;
                            guncelle.FiyatGuncelle = chFiyatGuncelle.Checked;
                            db.SaveChanges();
                            MessageBox.Show("Güncelleme yapılmıştır.");
                            bKaydet.Text = "Kaydet";
                            Temizle();
                            Doldur();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Şifreniz ile şifre tekrarınız birbiri ile uyuşmamaktadır.");
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen kullanıcı işlemleri alanlarının hepsini eksiksiz şekilde doldurunuz.");
                }
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //gridde veri varsa
            if (gridListeKullanici.Rows.Count > 0)
            {
                //id al
                int id = Convert.ToInt32(gridListeKullanici.CurrentRow.Cells["Id"].Value.ToString());
                //ayarlar ekranında gozukmeyen texte aktar
                lKullaniciId.Text = id.ToString();
                using (var db = new BarkodDbEntities())
                {
                    //secili bilgileri düzenle kaydet
                    var getir = db.Kullanici.Where(x => x.Id == id).FirstOrDefault();
                    tAdSoyad.Text = getir.AdSoyad;
                    tTelefon.Text = getir.Telefon;
                    tEposta.Text = getir.EPosta;
                    tKullanici.Text = getir.KullaniciAd;
                    tSifre.Text = getir.Sifre;
                    tSifreTekrar.Text = getir.Sifre;
                    chSatisEkrani.Checked = (bool)getir.Satis;
                    chRapor.Checked = (bool)getir.Rapor;
                    chUrunGiris.Checked = (bool)getir.UrunGiris;
                    chAyarlar.Checked = (bool)getir.Ayarlar;
                    chFiyatGuncelle.Checked = (bool)getir.FiyatGuncelle;
                    bKaydet.Text = "Düzenle/Kaydet";
                    Doldur();
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı seçiniz");
            }
        }

        private void fAyarlar_Load(object sender, EventArgs e)
        {
            Doldur();
        }
        private void Doldur()
        {
            using (var db = new BarkodDbEntities())
            {
                gridListeKullanici.DataSource = db.Kullanici.Select(x => new { x.Id, x.AdSoyad, x.KullaniciAd, x.Telefon }).ToList();
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridListeKullanici.Rows.Count > 0)
            {
                using (var db = new BarkodDbEntities())
                {
                    // ayarlar kullanici sayfasında sag tik ile kullanici silme islemi
                    //kullanıcı id yi al
                    int Id = Convert.ToInt32(gridListeKullanici.CurrentRow.Cells["Id"].Value.ToString());
                    //ad soyad adını al
                    string adsoyad = gridListeKullanici.CurrentRow.Cells["AdSoyad"].Value.ToString();
                    //kullaniciadını al
                    string kullaniciad = gridListeKullanici.CurrentRow.Cells["KullaniciAd"].Value.ToString();
                    //... kullanıcı silme islem onayı
                    DialogResult onay = MessageBox.Show(adsoyad + " " + " kullanıcısını silmek istiyor musunuz?", "Kullanıcı Silme İşlemi", MessageBoxButtons.YesNo);
                    //onaylıyorsa
                    if (onay == DialogResult.Yes)
                    {
                        var kullanici = db.Kullanici.Find(Id);
                        //urunu kaldır
                        db.Kullanici.Remove(kullanici);
                        db.SaveChanges();
                        MessageBox.Show("Kullanıcı silinmiştir.");
                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi iptal edilmiştir.");
                    }                  
                    Doldur();

                }
            }
        }
    }   
}
