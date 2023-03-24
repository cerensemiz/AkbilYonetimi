using Microsoft.Data.SqlClient;

namespace AkbilYonetimiUI
{
    public partial class FrmKayitOl : Form
    {
        public FrmKayitOl()
        {
            InitializeComponent();//Formu İnşa etmek
        }

        private void FrmKayitOl_Load(object sender, EventArgs e)
        {
            #region Ayarlar
            textBoxSifre.PasswordChar = '*';
            dateTimePicker1.MaxDate = new DateTime(2016, 1, 1);//Girilecek tarihi ayarladık
            dateTimePicker1.Value = new DateTime(2016, 1, 1);
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            #endregion
        }

        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            try
            {
                //1) Emailden kayitli kisi var mi?
                string baglantiCumlesi = @"Server=DESKTOP-E30TBPJ\MSSQLSERVER01;Database=AKBILUYGULAMADB;Trusted_Connection=True; TrustServerCertificate=True;";

                SqlConnection baglanti = new SqlConnection();//baglanti nesnesi 
                baglanti.ConnectionString = baglantiCumlesi;//nereye baglanacak?
                SqlCommand komut = new SqlCommand();//komut nesnesi turettik
                komut.Connection = baglanti;//komutu hangi baglantida calisacagini atadik.
                komut.CommandText = $"select * from Kullanicilar (nolock) where Email='{textBoxEmail.Text.Trim()}'"; //sql komutu
                baglanti.Open();

                SqlDataReader okuyucu = komut.ExecuteReader();//calistir
                if (okuyucu.HasRows)//satir var mi
                {
                    MessageBox.Show("Bu mail zaten sisteme kayitlidir.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                baglanti.Close();

                //2)Emaili daha once kayitli degilse KAYIT OLACAK
                if (string.IsNullOrEmpty(textBoxIsim.Text) || string.IsNullOrEmpty(textBoxSoyisim.Text) || string.IsNullOrEmpty(textBoxEmail.Text) || string.IsNullOrEmpty(textBoxSifre.Text))
                {
                    MessageBox.Show("Bilgileri eksiksiz giriniz !", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                string insertSQL = $"insert into Kullanicilar (EklenmeTarihi,Email,Parola,Ad,Soyad,DogumTarihi) " +
                    $"values ('{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{textBoxEmail.Text.Trim()}','{textBoxSifre.Text.Trim()}','{textBoxIsim.Text.Trim()}','{textBoxSoyisim.Text.Trim()}','{dateTimePicker1.Value.ToString("yyyyMMdd")}')";

                baglanti.ConnectionString = baglantiCumlesi;
                SqlCommand eklemeKomutu = new SqlCommand(insertSQL, baglanti);
                baglanti.Open();
                int rowsEffected = eklemeKomutu.ExecuteNonQuery();// insert update delete icin kullanilir.
                if (rowsEffected > 0)
                {
                    MessageBox.Show("Kayit Eklendi!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kayit Eklenemedi!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                }
                baglanti.Close();
                //Temizlik gerekli
            }
            catch (Exception ex)
            {
                //ex log.txt'ye yazılacak (loglama) 
                MessageBox.Show("Beklenmedik bir hata oluştu! Lütfen tekrar deneyiniz !"); ;
            }
        }
    }
}






