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
    public partial class fSatis : Form
    {
        public fSatis()
        {
            InitializeComponent();
        }

        private void fSatis_Load(object sender, EventArgs e)
        {
            //metodu çağırdık
            HizliButonDoldur();
            //para butonlarını form yüklenirken stringe cevirdik ve tl isareti getirdik
            b5.Text = 5.ToString("C2");
            b10.Text = 10.ToString("C2");
            b20.Text = 20.ToString("C2");
            b50.Text = 50.ToString("C2");
            b100.Text = 100.ToString("C2");
            b200.Text = 200.ToString("C2");
        }

        //metot oluşturuyoruz hızlı butonlar için
        private void HizliButonDoldur()
        {
            //hızlı urun tablosundan listele
            var hizliurun = db.HizliUrun.ToList();
            //foreach ile tüm listeyi gez
            foreach ( var item in hizliurun)
                {
                    //form içerisindeki kontrollerden arama yap
                    Button bH = this.Controls.Find("bH" + item.Id, true).FirstOrDefault() as Button;
                    // bh hizli butonların textine ürünad, fiyat yazdır
                    if (bH!=null)
                     {
                    // ıslemler classındaki doubleyap işlemini uygula
                        double fiyat = Islemler.DoubleYap(item.Fiyat.ToString());
                    //TL simgemiz c2 ile gözükecek
                        bH.Text = item.UrunAd + "\n" + fiyat.ToString("C2");    
                     }
                }
        }

        //hızlı buton click metodu oluşturuyoruz
        private void HizliButonClick(object sender, EventArgs e)
        {
            //unboxing ile b değiskeni tanımladık
            Button b = (Button)sender;
            //bh hizli butonların isimlerindeki numaralarını alıyoruz orn bH16 ise 16
            int butonid = Convert.ToInt16(b.Name.ToString().Substring(2, b.Name.Length - 2));

            //buton - ile baslıyorsa
            if (b.Text.ToString().StartsWith("-"))
            {
                // urun tanımlı olmayan hızlı butonlarda hızlı urun ekleme ekranını açar
                fHizliButonUrunEkle f = new fHizliButonUrunEkle();
                f.lButonId.Text = butonid.ToString();
                f.ShowDialog();
            }
            else
            {            
            //hızlı işlem için butona tıklandığunda hızlı urun tablosunda barkodunu getir
            var urunbarkod = db.HizliUrun.Where(a=> a.Id == butonid).Select(a=> a.Barkod).FirstOrDefault();
            //urun barkodunu urun degıskenine ata
            var urun = db.Urun.Where(a => a.Barkod == urunbarkod).FirstOrDefault();
            //her tıklama ile 1 ürün getirecek
            UrunGetirListeye(urun, urunbarkod, Convert.ToDouble(tMiktar.Text));
            //Genel toplam metodunu cagır
            GenelToplam();
            }
            
        }

        //veritabanımızı tanımladık
        BarkodDbEntities db = new BarkodDbEntities();
        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            //basılan tuş eğer enter ise
            if (e.KeyCode==Keys.Enter)
            {
                //burada boşlukları aldık
                string barkod = tBarkod.Text.Trim();
                //iki veya daha az karakterlik bir sayı girilirse miktar olarak algılayacak
                if (barkod.Length<=2)
                {
                    tMiktar.Text = barkod;
                    //tBarkod kutucuğunu temizle
                    tBarkod.Clear();
                    // tBarkod kontrolüne odaklan
                    tBarkod.Focus();
                }
                //ikiden büyük bir karakter girilirse
                else
                {
                    //Tabloların içerisindeki barkod alanında girilen barkod mevcut mu?
                    if (db.Urun.Any(a => a.Barkod==barkod))
                    {
                        //ürün tablomun barkod alanında girilen barkod varsa bu ürünü al ve değişkenin içerisine ata
                        var urun = db.Urun.Where(a => a.Barkod == barkod).FirstOrDefault();
                        //metot yardımıyla ürünü listeye getirecek
                        UrunGetirListeye(urun, barkod, Convert.ToDouble(tMiktar.Text));                      
                    }
                    //ürün listede yoksa terazi tablomdan on ek var mı?
                    else
                    {
                        //teraziyi kontrol et
                        //barkodun ilk iiki karakterini al
                        int onek = Convert.ToInt32(barkod.Substring(0, 2));
                        //onek varsa
                        if (db.Terazi.Any(a=> a.TeraziOnEk==onek))
                        {
                            //barkodun 2. değerden sonraki 5 değeri al
                            string teraziurunno = barkod.Substring(2, 5);
                            //eger ürünü tabloda buluyorsan
                            if (db.Urun.Any(a=> a.Barkod==teraziurunno))
                            {
                                //teraziurunno urun tablosunda varsa geri döndür
                                var urunterazi = db.Urun.Where(a => a.Barkod == teraziurunno).FirstOrDefault();
                                //gramaj bilgisini alıyoruz ve 1000 e bolerek kg değerini buluyoruz
                                double miktarkg = Convert.ToDouble(barkod.Substring(7, 5)) / 1000;
                                //metot yardımıyla tanımları çağıralım
                                UrunGetirListeye(urunterazi, teraziurunno, miktarkg);                              
                            }
                            else
                            {
                                //Yoksa uyarı ver ve ürün ekleme sayfasını aç
                                Console.Beep(900, 2000);
                                MessageBox.Show("Kg Ürün Ekleme Sayfası");
                            }                                                                                    
                        }
                        //teraziden gelen ürün de değilse
                        else
                        {
                            //normal ürün ekleme sayfasını aç
                            Console.Beep(900, 2000);
                            //urungiris formunu ac
                            fUrunGiris f = new fUrunGiris();  
                            f.tBarkod.Text = barkod;
                            f.ShowDialog();
                        }
                    }
                }
                //data grid üzerinde fare ile gezinirken satırlar renkli gözükmeyecek
                gridSatisListesi.ClearSelection();
                //genel toplam metodunu çalıştır
                GenelToplam();
                
            }
        }

        //Listeye ürünleri getirecek metodu oluşturuyoruz

        private void UrunGetirListeye(Urun urun,string barkod, double miktar)
        {
            //satır sayısımı aldık
            int satirsayisi = gridSatisListesi.Rows.Count;
            //miktar değikenini double veri türünde  aldık
            //double miktar = Convert.ToDouble(tMiktar.Text);  > yukarıdan miktarı çağıracağız
            //ürün öncesinde data gridimize eklenmiş mi?
            bool eklenmismi = false;
            //eğer barkod birden fazla girilmişse...
            if (satirsayisi > 0)
            {
                //sıfırdan başla ve satır sayısı kadar data grid satırlarında gez
                for (int i = 0; i < satirsayisi; i++)
                {
                    //grid satış listesinin satırlarından i satırındaki hücrelerde varsa içerisindeki barkod alanından değeri stringe çevir
                    if (gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString() == barkod)
                    {
                        //okutulan miktarın üzerine mevcut satırdaki miktar ile topla
                        gridSatisListesi.Rows[i].Cells["Miktar"].Value = miktar + Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Miktar"].Value);
                        //hesaplanan toplamı buluyoruz(miktarxfiyat)(virgülden 2 karakter sonrası yuvarlanacak şekilde yazıldı)
                        gridSatisListesi.Rows[i].Cells["Toplam"].Value = Math.Round(Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Miktar"].Value) * Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Fiyat"].Value), 2);
                        eklenmismi = true;
                    }
                }
            }
            //eğer eklenmemişse
            if (!eklenmismi)
            {
                //grid satış listesine satır ekle
                gridSatisListesi.Rows.Add();
                //data gridde barkod alanının değerini girilen barkod yap
                gridSatisListesi.Rows[satirsayisi].Cells["Barkod"].Value = barkod;
                //sıralı bilgileri getir
                gridSatisListesi.Rows[satirsayisi].Cells["UrunAdi"].Value = urun.UrunAd;
                gridSatisListesi.Rows[satirsayisi].Cells["UrunGrup"].Value = urun.UrunGrup;
                gridSatisListesi.Rows[satirsayisi].Cells["Birim"].Value = urun.Birim;
                gridSatisListesi.Rows[satirsayisi].Cells["Fiyat"].Value = urun.SatisFiyat;
                //okutulan tmikarı baz alıyoruz
                gridSatisListesi.Rows[satirsayisi].Cells["Miktar"].Value = miktar;
                //satış fiyatını double olarak aç, miktar ile çarp ve yuvarlayarak toplamı bul
                gridSatisListesi.Rows[satirsayisi].Cells["Toplam"].Value = Math.Round(miktar * (double)urun.SatisFiyat, 2);
                gridSatisListesi.Rows[satirsayisi].Cells["AlisFiyati"].Value = urun.AlisFiyat;
                gridSatisListesi.Rows[satirsayisi].Cells["KdvTutari"].Value = urun.KdvTutari;


            }
            GenelToplam();
        }

        //metot oluşturuyoruz her yerde aynı kodları yazmamak için
        private void GenelToplam()
        {        
                double toplam = 0;
                //grid satış listesini sayıp satır sayısı kadar atsın 
                for (int i = 0; i < gridSatisListesi.Rows.Count; i++)
                {
                    //grid satış listesindeki satırları topla
                    toplam += Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Toplam"].Value);
                }
                //tGenelToplam textine toplamı tl olarak aktar
                tGenelToplam.Text = toplam.ToString("C2");
                tMiktar.Text = "1";
                tBarkod.Clear();
                //barkoda focusla
                tBarkod.Focus();           
        }

        private void gridSatisListesi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //eğer 9. kolon yani sil butonu aktifse
            if (e.ColumnIndex==9)
            {
                //satırı sil
                gridSatisListesi.Rows.Remove(gridSatisListesi.CurrentRow);
                gridSatisListesi.ClearSelection();
                //genel toplamı değiştir
                GenelToplam();
                tBarkod.Focus();
            }
        }

        //sag tıklama metodu olusturduk
        private void bh_MouseDown(object sender, MouseEventArgs e)
        {
            //eger buton sag tıklandıysa
            if (e.Button== MouseButtons.Right) 
            {
                //yeni gelecek olan nesnemiz de buton olacak
                Button b = (Button)sender;
                // eger texti - degilse yani buton doluysa
                if(!b.Text.StartsWith("-"))
                {
                    //butonidyi aldık
                    int butonid = Convert.ToInt16(b.Name.ToString().Substring(2,b.Name.Length - 2));
                    //sag tıklayarak buton temizleme islemi
                    ContextMenuStrip s = new ContextMenuStrip();
                    ToolStripMenuItem sil = new ToolStripMenuItem();
                    sil.Text = "Temizle - Buton No:" + butonid.ToString();
                    sil.Click += Sil_Click;
                    s.Items.Add(sil);
                    this.ContextMenuStrip = s;

                }

            }
        }

        private void Sil_Click(object sender, EventArgs e)
        {
            //butonid aldık 19 karakterden sonrasını hesaplayarak temizle - buton no kısmındanki
            int butonid =Convert.ToInt16((sender.ToString().Substring(19, sender.ToString().Length - 19)));
            //butonadaki verileri guncellıyoruz
            var guncelle = db.HizliUrun.Find(butonid);
            guncelle.Barkod = "-";
            guncelle.UrunAd = "-";
            guncelle.Fiyat = 0;
            //veritabanına kaydet
            db.SaveChanges();
            double fiyat = 0;
            //dinamik hızlı butonları bul ve b degiskenine ata
            Button b = this.Controls.Find("bH" +butonid, true).FirstOrDefault() as Button;
            //sildikten sonra - koy ve 0 tl yazdır
            b.Text = "-" + "\n" + fiyat.ToString("C2");
        }

        //Numaratör için bir metot oluşturuyoruz
        private void bNx_Click(Object sender, EventArgs e)
        {
            //gelecek nesnenin de buton oldugunu belirttik
            Button b = (Button)sender;
            if (b.Text == ",")
            {
                //virgulleri saydırıyoruz
                int virgul = tNumarator.Text.Count(x => x == ',');
                if (virgul < 1)
                {
                    tNumarator.Text += b.Text;
                }
            }
            //backsapace tusu
            else if (b.Text == "<")
            {
                //numrator sıfırdan buyukse
                if (tNumarator.Text.Length > 0)
                {
                    //son karakteri siler
                    tNumarator.Text = tNumarator.Text.Substring(0, tNumarator.Text.Length - 1);
                }
            }
            else
            {
                tNumarator.Text += b.Text;
            }
        }

        private void bAdet_Click(object sender, EventArgs e)
        {
            //numarator kutucuğu dolu ise
           if(tNumarator.Text!="")
            {
                tMiktar.Text = tNumarator.Text;
                tNumarator.Clear();
                //barkod varsa temizle
                tBarkod.Clear();
                tBarkod.Focus();
            }
        }

        private void bOdenen_Click(object sender, EventArgs e)
        {

            //numarator bos degilse
            if (tNumarator.Text!= "")
            {
                //islemler classından double yap metodunu cagırıyoruz
                //numaratordeki miktardan genel toplamı cıkarıp para ustunu hesaplıyoruz
                double sonuc = Islemler.DoubleYap(tNumarator.Text) - Islemler.DoubleYap(tGenelToplam.Text);
                tParaUstu.Text = sonuc.ToString("C2");
                //odenen textine odenen miktarı yaz
                tOdenen.Text = Islemler.DoubleYap(tNumarator.Text).ToString("C2");
                tNumarator.Clear();
                tBarkod.Focus();
            }
        }

        private void bBarkod_Click(object sender, EventArgs e)
        {
            if (tNumarator.Text!= "")
            {
                //girilen barkod database içerisinde var mı
                if (db.Urun.Any(a => a.Barkod==tNumarator.Text))
                {
                    //urunun barkodunu alıyoruz
                    var urun= db.Urun.Where(a=> a.Barkod==tNumarator.Text).FirstOrDefault();
                    //urunleri listeye getiriyoruz
                    UrunGetirListeye(urun, tNumarator.Text, Convert.ToDouble(tMiktar.Text));
                    tNumarator.Clear();
                    tBarkod.Focus();
                }
                else
                {
                    MessageBox.Show("Ürün ekleme sayfasını aç");
                }
            }
        }

        //para ustu hesaplama metodu
        private void ParaUstuHesapla_Click(object sender, EventArgs e) 
        {
            //tıklanan para butonunu genel toplamdan dus
            Button b = (Button)sender;
            double sonuc = Islemler.DoubleYap(b.Text) - Islemler.DoubleYap(tGenelToplam.Text);
            //odenen textine yaz
            tOdenen.Text = Islemler.DoubleYap(b.Text).ToString("C2");
            //para ustunu yaz
            tParaUstu.Text = sonuc.ToString("C2");


        }

        private void bDigerUrun_Click(object sender, EventArgs e)
        {
            //eger numarator kutucugu bos degilse
            if (tNumarator.Text!="")
            {
                //satıs listesinin satırlarını say
                int satirsayisi = gridSatisListesi.RowCount;
                //grid satıs listesine satır ekle
                gridSatisListesi.Rows.Add();
                //barkodsuz urun verilerini ekledik
                gridSatisListesi.Rows[satirsayisi].Cells["Barkod"].Value="1111111111116";
                gridSatisListesi.Rows[satirsayisi].Cells["UrunAdi"].Value = "Barkodsuz Ürün";
                gridSatisListesi.Rows[satirsayisi].Cells["UrunGrup"].Value = "Barkodsuz Ürün";
                gridSatisListesi.Rows[satirsayisi].Cells["Birim"].Value = "Adet";
                gridSatisListesi.Rows[satirsayisi].Cells["Miktar"].Value = "1";
                gridSatisListesi.Rows[satirsayisi].Cells["AlisFiyati"].Value = "0";
                //fiyatı numaratore girdigimiz sayı olacak
                gridSatisListesi.Rows[satirsayisi].Cells["Fiyat"].Value = Convert.ToDouble(tNumarator.Text);
                gridSatisListesi.Rows[satirsayisi].Cells["KdvTutari"].Value = "0";
                gridSatisListesi.Rows[satirsayisi].Cells["Toplam"].Value = Convert.ToDouble(tNumarator.Text);
                tNumarator.Text = "";
                GenelToplam();
                tBarkod.Focus();

            }
        }

        private void bIade_Click(object sender, EventArgs e)
        {
            //satıs iade secili ise
            if (chSatisIadeIslemi.Checked)
            {
                chSatisIadeIslemi.Checked = false;
                chSatisIadeIslemi.Text = "Satış Yapılıyor";
            }
            else
            {
                chSatisIadeIslemi.Checked = true;
                chSatisIadeIslemi.Text = "İade İşlemi";
            }
        }

        private void bTemizle_Click(object sender, EventArgs e)
        {
            Temizle(); 
        }

        private void Temizle()
        {
            tMiktar.Text = "1";
            tBarkod.Clear();
            tOdenen.Clear();
            tParaUstu.Clear();
            tGenelToplam.Text = 0.ToString("C2");
            chSatisIadeIslemi.Checked = false;
            tNumarator.Clear();
            gridSatisListesi.Rows.Clear();
            tBarkod.Clear();
            tBarkod.Focus();
        }

        public void SatisYap(string odemesekli)
        {
            int satirsayisi = gridSatisListesi.Rows.Count;
            //satıs mı iade mi
            bool satisiade = chSatisIadeIslemi.Checked;
            double alisfiyattoplam = 0;
            if (satirsayisi > 0)
            {
                //0 olabilir sekilde islem no aldık
                int? islemno = db.Islem.First().IslemNo;
                //satis modelini olusturuyoruz
                Satis satis = new Satis();
                for (int i = 0; i < satirsayisi; i++)
                {
                    //databaseden aldıgın islem no yu modele ekle
                    satis.IslemNo = islemno;
                    //satis modeli degiskenine asagidaki verileri alıyoruz
                    satis.UrunAd = gridSatisListesi.Rows[i].Cells["UrunAdi"].Value.ToString();
                    satis.UrunGrup = gridSatisListesi.Rows[i].Cells["UrunGrup"].Value.ToString();
                    satis.Barkod = gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString();
                    satis.Birim = gridSatisListesi.Rows[i].Cells["Birim"].Value.ToString();
                    satis.AlisFiyat = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["AlisFiyati"].Value.ToString());
                    satis.SatisFiyat = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Fiyat"].Value.ToString());
                    satis.Miktar = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());
                    satis.Toplam = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Toplam"].Value.ToString());
                    satis.KdvTutari = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["KdvTutari"].Value.ToString()) * Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());
                    satis.OdemeSekli = odemesekli;
                    satis.Iade = satisiade;
                    satis.Tarih = DateTime.Now;
                    satis.Kullanici = lKullanici.Text;                
                    db.Satis.Add(satis);
                    db.SaveChanges();                  
                    //Eger iade işlemi yoksa stok azalt varsa arttır
                    if (!satisiade)
                    {
                        Islemler.StokAzalt(gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString(), Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString()));
                    }
                    else
                    {
                        Islemler.StokArtir(gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString(), Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString()));
                    }
                    //alisfiyatlarını topla
                    alisfiyattoplam += Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["AlisFiyati"].Value.ToString()) * Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());


                }
                //io degiskenini tanımladık
                IslemOzet io = new IslemOzet();
                //io içerisine yukarıda yapılan islemnoyu aldık
                io.IslemNo = islemno;
                //iade mi?
                io.Iade = satisiade;
                //alisfiyatlarının toplamını aldık
                io.AlisFiyatToplam = alisfiyattoplam;
                io.Gelir = false;
                io.Gider = false;
                if (!satisiade)
                {
                    //kart, nakit, kart nakit satis
                    io.Aciklama = odemesekli + " " + "Satış";
                }
                else
                {
                    //iade işlemi aciklamasi
                    io.Aciklama = "İade İşlemi(" + odemesekli + ")";
                }
                //odeme seklini aldık
                io.OdemeSekli= odemesekli;
                //kullanıcı adını aldık
                io.Kullanici = lKullanici.Text;
                //islem tarihini aldık
                io.Tarih = DateTime.Now;
                switch(odemesekli)
                {
                    //odeme nakit ise tablodaki nakit sutununa genel toplamı al
                    case "Nakit":
                        io.Nakit = Islemler.DoubleYap(tGenelToplam.Text);
                        io.Kart = 0;break;
                    //odeme sekli kart ise tablodaki kart sutununa genel toplamı
                    case "Kart":
                        io.Nakit = 0;
                        io.Kart = Islemler.DoubleYap(tGenelToplam.Text); break;
                    case "Kart-Nakit":
                        //odememnın nakit kısmını lnakit labelindeki degerden al
                        io.Nakit = Islemler.DoubleYap(lNakit.Text);
                        //odememnın kart kısmını lkart labelindeki degerden al
                        io.Kart = Islemler.DoubleYap(lKart.Text); break;
                }

                //databasedeki ıslem ozet bolumune bu islemleri ekle
                db.IslemOzet.Add(io);
                db.SaveChanges();

                //databasedeki ilk islemi al
                var islemnoartir = db.Islem.First();
                //islemnoyu 1 arttir
                islemnoartir.IslemNo += 1;
                db.SaveChanges();
                MessageBox.Show("İşlem başarıyla gerçekleşmiştir.");
                Temizle();
            }
        }

        private void bNakit_Click(object sender, EventArgs e)
        {
            SatisYap("Nakit");
        }
        private void bKart_Click(object sender, EventArgs e)
        {
            SatisYap("Kart");
        }

        private void bKartNakit_Click(object sender, EventArgs e)
        {
            //fnakitkart formunu cagirdik
            fNakitKart f = new fNakitKart();
            f.ShowDialog();
        }

        private void tBarkod_KeyPress(object sender, KeyPressEventArgs e)
        {
            //text kutusu sadece rakam karakterlerini alacak, silme tusu haric
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                //yazmasını engelle
                e.Handled = true;
            }
        }

        private void tMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            //text kutusu sadece rakam karakterlerini alacak, silme tusu haric
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                //yazmasını engelle
                e.Handled = true;
            }
        }

        private void fSatis_KeyDown(object sender, KeyEventArgs e)
        {
            //kısayollar kullanarak satis yapma
            if (e.KeyCode == Keys.F1)
                SatisYap("Nakit");
            if (e.KeyCode == Keys.F2)
                SatisYap("Kart");
            //nakit kart islemini yap kısayol ile
            if (e.KeyCode == Keys.F3)
            {
                //fnakitkart formunu cagirdik
                fNakitKart f = new fNakitKart();
                f.ShowDialog();
            }          
        }
        private void bIslemBeklet_Click(object sender, EventArgs e)
        {
            if (bIslemBeklet.Text == "İşlem Beklet")
            {
                Bekle();
                //beklerken butonun rengini orangered yap
                bIslemBeklet.BackColor = System.Drawing.Color.OrangeRed;
                //texti degistir
                bIslemBeklet.Text = "İşlem Bekliyor";
                //gridsatis listesi satirlarini temizle
                gridSatisListesi.Rows.Clear();
            }
            else
            {
                BeklemedenCik();
                //butonun rengini eski haline getir
                bIslemBeklet.BackColor = System.Drawing.Color.DimGray;
                //texti eski haline getir
                bIslemBeklet.Text = "İşlem Beklet";
                //grid bekle temizle
                gridBekle.Rows.Clear();
            }
            

        }

        private void Bekle ()
        {
            //grid satis satırlarını say
            int satir = gridSatisListesi.RowCount;
            //grid satis sutun say
            int sutun = gridSatisListesi.ColumnCount;
            if (satir > 0 )
            {
                //satir ve sutunlarda ilerle
                for( int i = 0; i < satir ; i++ )
                {
                    //her satır icin bir satir ekle
                    gridBekle.Rows.Add();
                    //sil sutunu haric ilerle
                    for ( int j = 0;j < sutun-1; j++)
                    {
                        //degerleri gride aktar
                        gridBekle.Rows[i].Cells[j].Value= gridSatisListesi.Rows[i].Cells[j].Value;
                    }
                }
            }

        }

        private void BeklemedenCik()
        {
            //grid bekle satırlarını say
            int satir = gridBekle.RowCount;
            //grid bekle sutun say
            int sutun = gridBekle.ColumnCount;
            if (satir > 0)
            {
                //satir ve sutunlarda ilerle
                for (int i = 0; i < satir; i++)
                {
                    //her satır icin bir satir ekle
                    gridSatisListesi.Rows.Add();
                    //sil sutunu haric ilerle
                    for (int j = 0; j < sutun - 1; j++)
                    {
                        //degerleri gride aktar
                        gridSatisListesi.Rows[i].Cells[j].Value =gridBekle.Rows[i].Cells[j].Value;
                    }
                }
            }

        }

        private void chSatisIadeIslemi_CheckedChanged(object sender, EventArgs e)
        {
            if (chSatisIadeIslemi.Checked)
            {
                chSatisIadeIslemi.Text = "İade Yapılıyor";
            }
            else
            {
                chSatisIadeIslemi.Text = "Satış Yapılıyor";
            }
        }

        private void bYeniUrun_Click(object sender, EventArgs e)
        {
            fUrunGiris f = new fUrunGiris();
            //baslangic sayfamdaki kullaniciyi urungiris sayfasina aktar
            f.lKullanici.Text = lKullanici.Text;
            f.ShowDialog();
        }

       
    }
}
