using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Market1
{
    static class Islemler
    {
        // metot oluşturuyoruz
        public static double DoubleYap(string deger)
        {
            double sonuc;
            double.TryParse(deger, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out sonuc);
            return Math.Round(sonuc, 2);
        }

        public static void StokAzalt(string barkod, double miktar)
        {                        
                //barkod numarasi diger urunden farkli ise stok azalt
                if (barkod != "1111111111116")
                {
                    //veritabanını modelledik
                    using (var db = new BarkodDbEntities())
                    {
                         //database de urunler içerisinden barkod satırını al
                         var urunbilgi = db.Urun.SingleOrDefault(x => x.Barkod == barkod);
                         //urun bilgi miktarını satılan kadar azalt
                         urunbilgi.Miktar -= miktar;
                         db.SaveChanges();
                    }
                }                                         
        }
        public static void StokArtir(string barkod, double miktar)
        {
            //barkod numarasi diger urunden farkli ise stok arttir
            if (barkod != "1111111111116")
            {
                //veritabanını modelledik
                using (var db = new BarkodDbEntities())
                {
                    //database de urunler içerisinden barkod satırını al
                    var urunbilgi = db.Urun.SingleOrDefault(x => x.Barkod == barkod);
                    //urun bilgi miktarını satılan kadar arttır
                    urunbilgi.Miktar += miktar;
                    db.SaveChanges();
                }
            }
                
        }
        public static void GridDuzenle(DataGridView dgv)
        {
            //gridin tüm ozellestirmelerini burada yapacagız
            //datagrid sutunları sıfırdan buyukse
            if (dgv.Columns.Count > 0)
            {
                //tek tek sutunlara bak
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    //header textleri incele
                    switch (dgv.Columns[i].HeaderText)
                    {
                        //id yi numara olarak yaz
                        case "Id":
                            dgv.Columns[i].HeaderText = "Numara";
                            break;
                        //islemno yu işlem no olarak yaz
                        case "IslemNo":
                            dgv.Columns[i].HeaderText = "İşlem No";
                            break;
                        //ürünid yi urun numarası olarak yaz
                        case "UrunId":
                            dgv.Columns[i].HeaderText = "Ürün Numarası";
                            break;
                        case "UrunAd":
                            dgv.Columns[i].HeaderText = "Ürün Adı";
                            break;
                        case "Aciklama":
                            dgv.Columns[i].HeaderText = "Açıklama";
                            break;
                        case "UrunGrup":
                            dgv.Columns[i].HeaderText = "Ürün Grubu";
                            break;
                        case "AlisFiyat":
                            dgv.Columns[i].HeaderText = "Alış Fiyatı";
                            //bu sutuna para yazılacagi icin saga yasla
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "AlisFiyatToplam":
                            dgv.Columns[i].HeaderText = "Alış Fiyatları Toplamı";
                            //bu sutuna para yazılacagi icin saga yasla
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "SatisFiyat":
                            dgv.Columns[i].HeaderText = "Satış Fiyatı";
                            //bu sutuna para yazılacagi icin saga yasla
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "KdvOrani":
                            dgv.Columns[i].HeaderText = "Kdv Oranı";
                            //sutunu ortala
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "Birim":
                            //sutunu ortala
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "Miktar":
                            //sutunu ortala
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "OdemeSekli":
                            dgv.Columns[i].HeaderText = "Ödeme Şekli";
                            //sutunu ortala
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "Kart":
                            //sutunu saga yasla
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Nakit":
                            //sutunu saga yasla
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Gelir":
                            //sutunu saga yasla
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Gider":
                            //sutunu saga yasla
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Kullanici":
                            dgv.Columns[i].HeaderText = "Kullanıcı";
                            //sutunu ortala
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "KdvTutari":
                            dgv.Columns[i].HeaderText = "Kdv Tutarı";
                            //sutunu ortala
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;
                        case "Toplam":
                            dgv.Columns[i].HeaderText = "Toplam";
                            //sutunu ortala
                            dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //tl sembolu ile para miktarını yaz
                            dgv.Columns[i].DefaultCellStyle.Format = "C2";
                            break;

                    }
                }
            }
        }

            //-----------------------------ENKAPSÜLASYON GRİD ÖZELLEŞTİRME-------------------
           public class DataGridViewColumnCustomizer
                {
                private readonly DataGridView _dgv;

                public DataGridViewColumnCustomizer(DataGridView dgv)
                {
                    _dgv = dgv;
                }

                // Header textlerini ve hücre stilini ayarlayan metot
                public void CustomizeColumns()
                {
                    if (_dgv.Columns.Count > 0)
                    {
                        for (int i = 0; i < _dgv.Columns.Count; i++)
                        {
                            CustomizeColumn(i);
                        }
                    }
                }

                // Tek bir sütunu özelleştiren metot
                private void CustomizeColumn(int columnIndex)
                {
                    var column = _dgv.Columns[columnIndex];

                    switch (column.HeaderText)
                    {
                        case "Id":
                            SetHeaderText(column, "Numara");
                            break;
                        case "IslemNo":
                            SetHeaderText(column, "İşlem No");
                            break;
                        case "UrunId":
                            SetHeaderText(column, "Ürün Numarası");
                            break;
                        case "UrunAd":
                            SetHeaderText(column, "Ürün Adı");
                            break;
                        case "Aciklama":
                            SetHeaderText(column, "Açıklama");
                            break;
                        case "UrunGrup":
                            SetHeaderText(column, "Ürün Grubu");
                            break;
                        case "AlisFiyat":
                            SetHeaderText(column, "Alış Fiyatı");
                            AlignRightAndFormatAsCurrency(column);
                            break;
                        case "AlisFiyatToplam":
                            SetHeaderText(column, "Alış Fiyatları Toplamı");
                            AlignRightAndFormatAsCurrency(column);
                            break;
                        case "SatisFiyat":
                            SetHeaderText(column, "Satış Fiyatı");
                            AlignRightAndFormatAsCurrency(column);
                            break;
                        case "KdvOrani":
                            SetHeaderText(column, "Kdv Oranı");
                            AlignCenter(column);
                            break;
                        case "Birim":
                            AlignCenter(column);
                            break;
                        case "Miktar":
                            AlignCenter(column);
                            break;
                        case "OdemeSekli":
                            SetHeaderText(column, "Ödeme Şekli");
                            AlignCenter(column);
                            break;
                        case "Kart":
                        case "Nakit":
                        case "Gelir":
                        case "Gider":
                            AlignRightAndFormatAsCurrency(column);
                            break;
                        case "Kullanici":
                            SetHeaderText(column, "Kullanıcı");
                            AlignCenter(column);
                            break;
                        case "KdvTutari":
                            SetHeaderText(column, "Kdv Tutarı");
                            AlignRightAndFormatAsCurrency(column);
                            break;
                        case "Toplam":
                            SetHeaderText(column, "Toplam");
                            AlignRightAndFormatAsCurrency(column);
                            break;
                    }
                }

                // Sütunun başlık metnini ayarlama
                private void SetHeaderText(DataGridViewColumn column, string headerText)
                {
                    column.HeaderText = headerText;
                }

                // Sütunu sağa yaslama ve para formatı uygulama
                private void AlignRightAndFormatAsCurrency(DataGridViewColumn column)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.DefaultCellStyle.Format = "C2"; // TL formatında para
                }

                // Sütunu merkeze yaslama
                private void AlignCenter(DataGridViewColumn column)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }


            public static void GridDuzenle1(DataGridView dgv)
            {
                // DataGridView için sütun özelleştirme işlemleri
                var customizer = new DataGridViewColumnCustomizer(dgv);
                customizer.CustomizeColumns();
            }

        }
            //-------------------GRİD DUZENLE ENKAPSÜLASYON BİTİS-------------------------

}
    

    
    
