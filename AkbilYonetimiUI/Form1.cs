using Microsoft.Data.SqlClient;

namespace AkbilYonetimiUI
{
    public partial class Form1 : Form
    {
        public string Email { get; set; }//kayýt ol formunda kayýt olan kullanýcýnýn emaili buraya gelsin
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
            //Bu formu gizleyeceðiz.
            //Kayýt ol formunu açacaðýz.
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
                //1)Email ve sifre textboxlarý dolu mu?
                if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSifre.Text))
                {
                    MessageBox.Show("Bilgilerinizi eksiksiz giriniz", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                //2)Girdiði email ve þifre veritabanýnda mevcut mu?
                //select*from Kullanicilar where Email='' ans Sifre=''
                string baglantiCumlesi = "Server=DESKTOP-E30TBPJ\\MSSQLSERVER01;Database=AKBILUYGULAMADB;Trusted_Connection=True; TrustServerCertificate=True;";
                SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
                string sorgu = $"select * from Kullanicilar where Email='{txtEmail.Text.Trim()}' and Parola='{txtSifre.Text.Trim()}'";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                baglanti.Open();
                SqlDataReader okuyucu = komut.ExecuteReader();
                if (!okuyucu.HasRows)  //Deðilse yanlýþ girdiniz mesajý verecek
                {
                    MessageBox.Show("Email ve þifrenizi doðru girdiðinize emin olunuz!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    baglanti.Close();
                    return;
                }
                else
                {
                    while (okuyucu.Read())
                    {
                        MessageBox.Show($"HOÞGELDÝNÝZ {okuyucu["Ad"]} {okuyucu["Soyad"]}");
                        Properties.Settings1.Default.KullaniciId = (int)okuyucu["Id"];
                    }
                }
                //3)Eðer email-þifre doðruysa
                //Eðer beni hatýrla'ya týkladýysa ?? Bilgileri hatýrlanacak.
                //hoþgeldiniz yazacak ve anasayfa formuna yönlendirecek
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
                //Ana sayfa formu açýlacak
            }
            catch (Exception hata)
            {
                //Dipnot: Exceptionlar asla kullanýcýya gösterilemez.Exceptionlar loglanýr, yazýlýmcýya iletilir.Biz öðrenmek için mbox ýn içine yazdýk.

                MessageBox.Show("Beklenmedik bir sorun oluþtu!" + hata.Message);
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