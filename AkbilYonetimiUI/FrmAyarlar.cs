using Microsoft.Data.SqlClient;

namespace AkbilYonetimiUI
{
    public partial class FrmAyarlar : Form
    {
        public FrmAyarlar()
        {
            InitializeComponent();
        }

        private void FrmAyarlar_Load(object sender, EventArgs e)
        {

            txtSifre.PasswordChar = '*';
            dtpDogumTarihi.MaxDate = new DateTime(2016, 1, 1);
            dtpDogumTarihi.Value = new DateTime(2016, 1, 1);
            dtpDogumTarihi.Format = DateTimePickerFormat.Short;
            KullanicininBilgileriniGetir();
        }

        private void KullanicininBilgileriniGetir()
        {
            try
            {
                //NOT: Giriş yapmis kullanicinin bilgileriyle select sorgusu yazacagiz.
                //kullanici bilgisi alabilmek icin burada 2 yontem kullanabiliriz
                //static bir class acip icinde GirisYapmisKullaniciEmail propertysi kullanilabilir.
                //2. yöntem olarak Properties settings icine kayitli email bilgisinden yararlanilabilir.
                if (string.IsNullOrEmpty(Properties.Settings1.Default.KullaniciEmail))
                {
                    MessageBox.Show("Giris yapmadan bu sayfaya ulasamazsınız!");
                    return;
                    //Giris formuna yönlendirilebilir..showdialog();
                }
                else
                {
                    string baglantiCumlesi = "Server=DESKTOP-E30TBPJ\\MSSQLSERVER01;Database=AKBILUYGULAMADB;Trusted_Connection=True; TrustServerCertificate=True;";
                    SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
                    SqlCommand komut = new SqlCommand($"select* from Kullanicilar where Email='{Properties.Settings1.Default.KullaniciEmail}' and Parola='{Properties.Settings1.Default.KullaniciSifre}'", baglanti);
                    baglanti.Open();
                    SqlDataReader okuyucu = komut.ExecuteReader();
                    if (okuyucu.HasRows)
                    {
                        while (okuyucu.Read())
                        {
                            txtEmail.Text = okuyucu["Email"].ToString();
                            txtEmail.Enabled = false;
                            txtAd.Text = okuyucu["Ad"].ToString();
                            txtSoyad.Text = okuyucu["Soyad"].ToString();
                            dtpDogumTarihi.Value = Convert.ToDateTime(okuyucu["DogumTarihi"]);
                        }
                    }
                    baglanti.Close();
                }
            }
            catch (Exception hata)
            {

                MessageBox.Show("Beklenmedik bir hata oluştu!" + hata.Message);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                string baglantiCumlesi = "Server=DESKTOP-E30TBPJ\\MSSQLSERVER01;Database=AKBILUYGULAMADB;Trusted_Connection=True; TrustServerCertificate=True;";
                SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
                string sorgu = $"update Kullanicilar set Ad='{txtAd.Text.Trim()}', Soyad='{txtSoyad.Text.Trim()}', DogumTarihi='{dtpDogumTarihi.Value.ToString("yyyyMMdd")}'";
                if (!string.IsNullOrEmpty(txtSifre.Text))
                {
                    sorgu += $" ,Parola='{txtSifre.Text.Trim()}'";
                }

                sorgu += $"where Email='{txtEmail.Text.Trim()}'";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                baglanti.Open();
                if (komut.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Bilgiler Güncellendi !");
                    KullanicininBilgileriniGetir();
                }
                else
                {
                    MessageBox.Show("Bilgiler Güncellenemedi !");
                }
            }
            catch (Exception hata)
            {

                MessageBox.Show("Güncelleme BAŞARISIZDIR" + hata.Message); ;
            }
        }
    }
}
