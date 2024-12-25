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
    public partial class fBaslangic : Form
    {
        public fBaslangic()
        {
            InitializeComponent();
        }

        private void bSatisİslemi_Click(object sender, EventArgs e)
        {
            //fare isaretini dönen yuvarlak yap
            Cursor.Current = Cursors.WaitCursor;
            fSatis f = new fSatis();
            //baslangic sayfamdaki kullaniciyi satis sayfasina aktar
            f.lKullanici.Text = lKullanici.Text;
            f.ShowDialog();
            //fare imlecini eski haline getir
            Cursor.Current = Cursors.Default;
        }

        private void bGenelRapor_Click(object sender, EventArgs e)
        {
            //fare isaretini dönen yuvarlak yap
            Cursor.Current = Cursors.WaitCursor;
            fRapor f = new fRapor();
            //baslangic sayfamdaki kullaniciyi rapor sayfasina aktar
            f.lKullanici.Text = lKullanici.Text;
            f.ShowDialog();
            //fare imlecini eski haline getir
            Cursor.Current = Cursors.Default;


        }

        private void bUrunGiris_Click(object sender, EventArgs e)
        {
            //fare isaretini dönen yuvarlak yap
            Cursor.Current = Cursors.WaitCursor;
            fUrunGiris f = new fUrunGiris();
            //baslangic sayfamdaki kullaniciyi urungiris sayfasina aktar
            f.lKullanici.Text = lKullanici.Text;
            f.ShowDialog();
            //fare imlecini eski haline getir
            Cursor.Current = Cursors.Default;
        }

        private void bAyarlar_Click(object sender, EventArgs e)
        {
            //fare isaretini dönen yuvarlak yap
            Cursor.Current = Cursors.WaitCursor;
            fAyarlar f = new fAyarlar();
            //baslangic sayfamdaki kullaniciyi ayarlar sayfasina aktar
            f.lKullanici.Text = lKullanici.Text;
            f.ShowDialog();
            //fare imlecini eski haline getir
            Cursor.Current = Cursors.Default;
        }

        private void bCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bFiyatGuncelle_Click(object sender, EventArgs e)
        {
            //fare isaretini dönen yuvarlak yap
            Cursor.Current = Cursors.WaitCursor;
            fFiyatGuncelle f = new fFiyatGuncelle();
            //baslangic sayfamdaki kullaniciyi fiyatguncelle sayfasina aktar
            f.lKullanici.Text = lKullanici.Text;
            f.ShowDialog();
            //fare imlecini eski haline getir
            Cursor.Current = Cursors.Default;
        }

        private void bKullaniciDegistir_Click(object sender, EventArgs e)
        {
            //kullanici giris ekranini ac
            fLogin login = new fLogin();
            login.Show();
            this.Hide();
        }
    }
}
