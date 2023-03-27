using Microsoft.Data.SqlClient;

namespace AkbilYonetimiUI
{
    public partial class Form1 : Form
    {
        public string Email { get; set; }//kay�t ol formunda kay�t olan kullan�c�n�n emaili buraya gelsin
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Email != null)
            {
                txtEmail.Text = Email;
            }
            txtEmail.TabIndex = 1;
            txtSifre.TabIndex = 2;
            checkBoxHatirla.TabIndex = 3;
            btnGirisYap.TabIndex = 4;
            btnKayitOl.TabIndex = 5;

            if (Properties.Settings1.Default.BeniHatirla)
            {
                txtEmail.Text = Properties.Settings1.Default.KullaniciEmail;
                txtSifre.Text = Properties.Settings1.Default.KullaniciSifre;
                checkBoxHatirla.Checked = true;
                BeniHatirla();
                

            }
        }
        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            //Bu formu gizleyece�iz.
            //Kay�t ol formunu a�aca��z.
            this.Hide();
            FrmKayitOl frm = new FrmKayitOl();
            frm.Show();

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            GirisYap();
        }

        private void GirisYap()
        {
            try
            {
                //1)Email ve sifre textboxlar� dolu mu?
                if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSifre.Text))
                {
                    MessageBox.Show("Bilgilerinizi eksiksiz giriniz", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                //2)Girdi�i email ve �ifre veritaban�nda mevcut mu?
                //select*from Kullanicilar where Email='' ans Sifre=''
                string baglantiCumlesi = "Server=DESKTOP-E30TBPJ\\MSSQLSERVER01;Database=AKBILUYGULAMADB;Trusted_Connection=True; TrustServerCertificate=True;";
                SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
                string sorgu = $"select * from Kullanicilar where Email='{txtEmail.Text.Trim()}' and Parola='{txtSifre.Text.Trim()}'";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                baglanti.Open();
                SqlDataReader okuyucu = komut.ExecuteReader();
                if (!okuyucu.HasRows)  //De�ilse yanl�� girdiniz mesaj� verecek
                {
                    MessageBox.Show("Email ve �ifrenizi do�ru girdi�inize emin olunuz!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    baglanti.Close();
                    return;
                }
                else
                {
                    while (okuyucu.Read())
                    {
                        MessageBox.Show($"HO�GELD�N�Z {okuyucu["Ad"]} {okuyucu["Soyad"]}");
                        Properties.Settings1.Default.KullaniciId = (int)okuyucu["Id"];
                    }
                }
                //3)E�er email-�ifre do�ruysa
                //E�er beni hat�rla'ya t�klad�ysa ?? Bilgileri hat�rlanacak.
                //ho�geldiniz yazacak ve anasayfa formuna y�nlendirecek
                if (checkBoxHatirla.Checked)
                {
                    Properties.Settings1.Default.BeniHatirla = true;
                    Properties.Settings1.Default.KullaniciEmail = txtEmail.Text.Trim();
                    Properties.Settings1.Default.KullaniciSifre = txtSifre.Text.Trim();
                    Properties.Settings1.Default.Save();
                }
                this.Hide();
                FrmAnasayfa frma = new FrmAnasayfa();
                frma.Show();

                //bu form gizlenecek
                //Ana sayfa formu a��lacak
            }
            catch (Exception hata)
            {
                //Dipnot: Exceptionlar asla kullan�c�ya g�sterilemez.Exceptionlar loglan�r, yaz�l�mc�ya iletilir.Biz ��renmek i�in mbox �n i�ine yazd�k.

                MessageBox.Show("Beklenmedik bir sorun olu�tu!" + hata.Message);
            }
        }

        private void checkBoxHatirla_CheckedChanged(object sender, EventArgs e)
        {
            BeniHatirla();
        }

        private void BeniHatirla()
        {
            if (checkBoxHatirla.Checked)
            {
                Properties.Settings1.Default.BeniHatirla = true;
            }
            else
            {
                Properties.Settings1.Default.BeniHatirla = false;
            }
        }

        private void txtSifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==Convert.ToChar(Keys.Enter))//basilan tus enter ise giris yapilacak
            {
                GirisYap();
            }
        }
    }
}