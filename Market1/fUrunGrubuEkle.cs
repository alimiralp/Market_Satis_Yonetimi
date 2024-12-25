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
    public partial class fUrunGrubuEkle : Form
    {
        public fUrunGrubuEkle()
        {
            InitializeComponent();
        }

        BarkodDbEntities db = new BarkodDbEntities();
        private void fUrunGrubuEkle_Load(object sender, EventArgs e)
        {
            GrupDoldur();
        }

        private void bEkle_Click(object sender, EventArgs e)
        {
            //urun grup kısmı bos degilse
            if(tUrunGrupAd.Text!= "")
            {
                UrunGrup ug = new UrunGrup();
                ug.UrunGrupAd = tUrunGrupAd.Text;
                db.UrunGrup.Add(ug);
                db.SaveChanges();
                GrupDoldur();
                tUrunGrupAd.Clear();
                MessageBox.Show("Ürün grubu eklenmiştir");
                //acik olan fgiris sayfasina ulas
                fUrunGiris f = (fUrunGiris)Application.OpenForms["fUrunGiris"];
                if (f != null)
                {
                    //urungiris formunun kodundaki grupdoldur metodunu cagirdik
                    f.GrupDoldur();
                }              
            }
            else
            {
                MessageBox.Show("Grup bilgisi ekleyiniz");
            }
        }
        private void GrupDoldur()
        {
            //kullanıcıya urun grup ad goster arka planda id tut
            listUrunGrup.DisplayMember = "UrunGrupAd";
            listUrunGrup.ValueMember = "Id";
            //a dan z ye dogru sırala
            listUrunGrup.DataSource = db.UrunGrup.OrderBy(a => a.UrunGrupAd).ToList();

        }

        private void bSil_Click(object sender, EventArgs e)
        {
            //urun grubu silme için id alındı
            int grupid = Convert.ToInt32(listUrunGrup.SelectedValue.ToString());
            string grupad = listUrunGrup.Text;
            //silmek istediğine emin misin?
            DialogResult onay = MessageBox.Show(grupad + "grubunu silmek isitiyor musunuz?", "Silme İşlemi", MessageBoxButtons.YesNo);
            //eger silmek icin onayladıysa
            if (onay == DialogResult.Yes)
            {
                //ürün grubu silme
                var grup = db.UrunGrup.FirstOrDefault(x => x.Id == grupid);
                db.UrunGrup.Remove(grup);
                db.SaveChanges();
                GrupDoldur();
                tUrunGrupAd.Focus();
                MessageBox.Show(grupad + "adlı ürün grubu silindi");
                //silindikten sonra urun giris formunu ac, ürün grubu kismini doldur
                fUrunGiris f = (fUrunGiris)Application.OpenForms["fUrunGiris"];
                f.GrupDoldur();
            }
        }
    }
}
