namespace AkbilYonetimiUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            //Bu formu gizleyece�iz.
            //Kay�t ol formunu a�aca��z.
            this.Hide();
            FrmKayitOl frm = new FrmKayitOl();
            frm.Show();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}