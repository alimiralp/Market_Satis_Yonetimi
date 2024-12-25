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
    public partial class fDetayGoster : Form
    {
        public fDetayGoster()
        {
            InitializeComponent();
        }

        public int islemno { get; set; }
        private void fDetayGoster_Load(object sender, EventArgs e)
        {
            //form acilirken ıslemnoyu yaz
            lIslemNo.Text = "İşlem No : " + islemno.ToString();
            //veritabani baglantisi
            using (var db = new BarkodDbEntities())
            {
                //islemno ya göre detay goster ekran gridine bilgileri getir
                //gride islemno urunad urungrup miktar toplam sutunlarını getir
                gridListe.DataSource = db.Satis.Select(s=> new {s.IslemNo,s.UrunAd,s.UrunGrup,s.Miktar,s.Toplam}).Where(x=> x.IslemNo == islemno).ToList();
                //gridduzenle metodu ile gridi duzenledik
                Islemler.GridDuzenle(gridListe);
            }
        }
    }
}
