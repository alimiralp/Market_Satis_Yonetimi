using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Market1
{
    public partial class fRapor : Form
    {
        public fRapor()
        {
            InitializeComponent();
        }

        public void bGoster_Click(object sender, EventArgs e)
        {
            //gostere tıklandıgında fare imleci yuvarlak olacak
            Cursor.Current = Cursors.WaitCursor;
            // baslangic tarihinin kısa halini al
            DateTime baslangic = DateTime.Parse(dtBaslangic.Value.ToShortDateString());
            //bitis tarihini kısa tarih olarak al
            DateTime bitis = DateTime.Parse(dtBitis.Value.ToShortDateString());
            //bitis tarihini alırken bir gun ekledik
            bitis = bitis.AddDays(1);
            using (var db = new BarkodDbEntities())
            {
                //eger tumunu filtrele secildiyse
                if (listFiltrelemeTuru.SelectedIndex == 0) //tümünü getir
                {
                    //bu tarih aralıgındaki islemleri tersine gore sırala
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis).OrderByDescending(x => x.Tarih).Load();
                    //veritabanini gereksiz mesgul etmemek icin verileri islemozet degiskenii icine aldık
                    var islemozet = db.IslemOzet.Local.ToBindingList();
                    //verileri islemozet icerisinden al
                    gridListe.DataSource = islemozet;

                    // iade olmayanları, gelir olamayanları, gider olmayanları getir ve topla...
                    tSatisNakit.Text = Convert.ToDouble(islemozet.Where(x => x.Iade == false && x.Gelir == false && x.Gider == false).Sum(x => x.Nakit)).ToString("C2");
                    // iade olmayanları, gelir olamayanları, gider olmayanları getir ve topla...
                    tSatisKart.Text = Convert.ToDouble(islemozet.Where(x => x.Iade == false && x.Gelir == false && x.Gider == false).Sum(x => x.Kart)).ToString("C2");

                    //nakit iadeleri topla iade nakit textine
                    tIadeNakit.Text = Convert.ToDouble(islemozet.Where(x => x.Iade == true).Sum(x => x.Nakit)).ToString("C2");
                    //kart iadeleri topla iadekart textine
                    tIadeKart.Text = Convert.ToDouble(islemozet.Where(x => x.Iade == true).Sum(x => x.Kart)).ToString("C2");

                    //gelirnakit textine nakit gelirleri topla
                    tGelirNakit.Text = Convert.ToDouble(islemozet.Where(x => x.Gelir == true).Sum(s => s.Nakit)).ToString("C2");
                    //gelirkart textine kart gelirleri topla
                    tGelirKart.Text = Convert.ToDouble(islemozet.Where(x => x.Gelir == true).Sum(s => s.Kart)).ToString("C2");

                    //gidernakit textine nakit giderlerini topla
                    tGiderNakit.Text = Convert.ToDouble(islemozet.Where(x => x.Gider == true).Sum(s => s.Nakit)).ToString("C2");
                    //giderkart textine kart giderlerini topla
                    tGiderKart.Text = Convert.ToDouble(islemozet.Where(x => x.Gider == true).Sum(s => s.Kart)).ToString("C2");

                    db.Satis.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis).Load();
                    var satistablosu = db.Satis.Local.ToBindingList();
                    //iade haric kdvleri topla
                    double kdvtutarisatis = Islemler.DoubleYap(satistablosu.Where(x => x.Iade == false).Sum(x => x.KdvTutari).ToString());
                    //satis tablomdaki iadelerin kdv tutarlarını topla
                    double kdvtutariiade = Islemler.DoubleYap(satistablosu.Where(x => x.Iade == true).Sum(x => x.KdvTutari).ToString());
                    tKdvToplam.Text = (kdvtutarisatis - kdvtutariiade).ToString("C2");
                }
                else if (listFiltrelemeTuru.SelectedIndex==1) //satislar secili ise
                {
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis && x.Iade == false && x.Gelir == false && x.Gider == false).Load();
                    //islemozet icerisine databaseden gelen verileri listele
                    var islemozet = db.IslemOzet.Local.ToBindingList();
                    //islemozeti datasource olarak ekle
                    gridListe.DataSource = islemozet;
                }
                else if (listFiltrelemeTuru.SelectedIndex == 2) //iade secili ise
                {
                    //iade olanları listele
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis && x.Iade == true).Load();
                    //gridliste icerisine databaseden gelen verileri listele
                    gridListe.DataSource = db.IslemOzet.Local.ToBindingList();
                }
                else if (listFiltrelemeTuru.SelectedIndex == 3) //gelir secili ise
                {
                    //gelir olanları listele
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis && x.Gelir == true).Load();
                    //gridliste icerisine databaseden gelen verileri listele
                    gridListe.DataSource = db.IslemOzet.Local.ToBindingList();
                }
                else if (listFiltrelemeTuru.SelectedIndex == 4) // gider secili ise
                {
                    //gider olanları listele
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis && x.Gider == true).Load();
                    //gridliste icerisine databaseden gelen verileri listele
                    gridListe.DataSource = db.IslemOzet.Local.ToBindingList();
                }
            }
            //metot ile gridimizi ve headeri duzenledik
            Islemler.GridDuzenle(gridListe);
            //fare imleci islem bitince normal haline donecek
            Cursor.Current = Cursors.Default;
        }

        private void fRapor_Load(object sender, EventArgs e)
        {
            //tumu secenegi secili gelsin   
            listFiltrelemeTuru.SelectedIndex = 0;
            //form yuklendiginde o güne ait tüm islemleri geride getir
            bGoster_Click(null, null);
        }

        private void gridListe_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //grid listedeki ıade, gelir ve gider sutununnlarını bulduk
            if (e.ColumnIndex==2 || e.ColumnIndex ==6 || e.ColumnIndex ==7)
            {
                //unboxing işlemini yapıyoruz
                if (e.Value is bool)
                {
                    //iade sutunu true ise evet yaz, false ise hayır yaz
                    bool value = (bool)e.Value;
                    e.Value = (value) ? "Evet" : "Hayır";
                    e.FormattingApplied = true; 
                }

            }
        }

        private void bGelirEkle_Click(object sender, EventArgs e)
        {
            //form basina rapor sayfasındaki gelir butonuna tıklandigi icin gelir olarak yaz
            fGelirGider f = new fGelirGider();
            f.gelirgider = "GELİR";
            f.kullanici = lKullanici.Text;
            f.ShowDialog();
        }

        private void bGiderEkle_Click(object sender, EventArgs e)
        {
            //form basina rapor sayfasındaki gider butonuna tıklandigi icin gelir olarak yaz
            fGelirGider f = new fGelirGider();
            f.gelirgider = "GİDER";
            f.kullanici = lKullanici.Text;
            f.ShowDialog();
        }

        private void detayGösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //data grid icerisinde islem varsa
            if (gridListe.Rows.Count>0)
            {
                //islemno yu aldik
                int islemno = Convert.ToInt32(gridListe.CurrentRow.Cells["IslemNo"].Value.ToString());
                //islem no sıfırdan farkli ise cunku 0 islem no gelir ya da giderdir
                if (islemno!=0)
                {
                    //islemno yu fdetaygoster sayfasina gonderdik
                    fDetayGoster f = new fDetayGoster();
                    f.islemno = islemno;
                    f.ShowDialog();
                }
            }
        }
    }
}
