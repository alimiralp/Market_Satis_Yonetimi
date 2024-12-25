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
    public partial class fHizliButonUrunEkle : Form
    {
        public fHizliButonUrunEkle()
        {
            InitializeComponent();
        }

        BarkodDbEntities db = new BarkodDbEntities();
        private void tUrunAra_TextChanged(object sender, EventArgs e)
        {
            //eğer türünara boştan farklı ise
            if(tUrunAra.Text!="")
            {
                //databasedeki ürünler içerisinden ara,
                string urunad = tUrunAra.Text;
                var urunler = db.Urun.Where(a=> a.UrunAd.Contains(urunad)).ToList();
                //gride ürünleri getir
                gridUrunler.DataSource = urunler;
                Islemler.GridDuzenle(gridUrunler);

            }
        }

        private void gridUrunler_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //satılar boş değilse
            if (gridUrunler.Rows.Count>0)
            {
                //seçili, aktif satırdaki hücrelerde barkod, urunad,fiyat sütunundaki kesisen degeri al
                string barkod = gridUrunler.CurrentRow.Cells["Barkod"].Value.ToString();
                string urunad = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();
                double fiyat = Convert.ToDouble(gridUrunler.CurrentRow.Cells["SatisFiyat"].Value.ToString());
                
                //urun tablosunda id turunden arama yap
                int id = Convert.ToInt16(lButonId.Text);
                //butona atamayı yapıyoruz
                var guncellenecek = db.HizliUrun.Find(id);
                guncellenecek.Barkod = barkod;
                guncellenecek.UrunAd = urunad;
                guncellenecek.Fiyat = fiyat;
                //database e güncellemeleri kaydet
                db.SaveChanges();
                MessageBox.Show("Buton Tanımlanmıştır");
                //açılmış bir formdan başka bir forma gecis islemi
                //fsatis formu açık mı
                fSatis f = (fSatis)Application.OpenForms["fSatis"];
                if (f!=null)
                {
                    //dinamik hızlı butonları bul ve b degiskenine ata
                    Button b = f.Controls.Find("bH" + id, true).FirstOrDefault() as Button;
                    b.Text=urunad + "\n" + fiyat.ToString("C2");
                }


            }
        }

        private void chTumu_CheckedChanged(object sender, EventArgs e)
        {
            //eger işaretliyse
            if (chTumu.Checked)
            {
                //urunler veritabınındaki urunleri yeni urun ekledeki tümünü goster ile listele
                gridUrunler.DataSource = db.Urun.ToList();
                //tumunu goster isaretlenmesi ile griddeki asagidaki sutunları gizle ve digerlerini göster
                gridUrunler.Columns["AlisFiyat"].Visible = false;
                gridUrunler.Columns["SatisFiyat"].Visible = false;
                gridUrunler.Columns["KdvOrani"].Visible = false;
                gridUrunler.Columns["KdvTutari"].Visible = false;
                gridUrunler.Columns["Miktar"].Visible = false;
                Islemler.GridDuzenle(gridUrunler);
            }
            else
            {
                gridUrunler.DataSource = null;
            }
            
        }
    }
}
