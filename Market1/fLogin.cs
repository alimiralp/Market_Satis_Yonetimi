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
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void bGiris_Click(object sender, EventArgs e)
        {
            GirisYap();
        }

        private void GirisYap()
        {
            if (tKullaniciAdi.Text != "" && tSifre.Text != "")
            {
                try
                {
                    using (var db = new BarkodDbEntities())
                    {
                        //kayıtlı kullanici var ise
                        if (db.Kullanici.Any())
                        {
                            //kullanici adi ve sifre eslesen 
                            var bak = db.Kullanici.Where(x => x.KullaniciAd == tKullaniciAdi.Text && x.Sifre == tSifre.Text).FirstOrDefault();
                            //eger eslesen varsa
                            if (bak != null)
                            {
                                //fare imlecini yuvarla
                                Cursor.Current = Cursors.WaitCursor;
                                //baslangic formunu ac
                                fBaslangic f = new fBaslangic();
                                //asagidaki sayfalara erisim izinlerini kontrol et
                                f.bSatisİslemi.Enabled = (bool)bak.Satis;
                                f.bGenelRapor.Enabled = (bool)bak.Rapor;
                                f.bUrunGiris.Enabled = (bool)bak.UrunGiris;
                                f.bAyarlar.Enabled = (bool)bak.Ayarlar;
                                f.bFiyatGuncelle.Enabled = (bool)bak.FiyatGuncelle;
                                //kullanıcı adını sayfaya yaz
                                f.lKullanici.Text = bak.AdSoyad;
                                f.Show();
                                //sonrasinda login sayfasini gizle
                                this.Hide();
                                Cursor.Current = Cursors.Default;
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı adı veya şifre hatalı");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void fLogin_KeyDown(object sender, KeyEventArgs e)
        {
            //entere basinca giris yapma
            if (e.KeyCode==Keys.Enter)
            {
                GirisYap();
            }
        }
    }
}
