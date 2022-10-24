using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Steganografi
{

    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        private Bitmap bmp = null;
        private string extractText = string.Empty;
        private void btnAc_Click(object sender, EventArgs e)//btnOpen_Click
        {
           OpenFileDialog dialog = new OpenFileDialog();

            
            dialog.Filter = "Image Files (*.png,*.bmp,*.jpg)|*.png;*.bmp;*.jpg";


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                picIsleme.Image = Image.FromFile(dialog.FileName);
                btnGizle.Enabled = true;
            }
              int lsb1,lsb2;
              for (int i = 0; i < picIsleme.Height; i++)
              {
                  for (int j = 0; j < picIsleme.Width; j++)
                  {
                      lsb1 = picIsleme.Height * picIsleme.Width * 3;
                      lsb2 = lsb1 / 8;
                      toolStripSayi.Text = lsb2.ToString();
                      //toolStripStatusLabel1.Text = txtMesaj.TextLength.ToString();
                      //toolStripStatusLabel3.Text = (lsb2 - txtMesaj.TextLength).ToString();





                  }
              }






        }



        private void btnKaydet_Click(object sender, EventArgs e)//btnSave_Click
        {
            SaveFileDialog save_dialog = new SaveFileDialog();
            //save_dialog.Filter = "Png Image|*.png|Bitmap Image|*.bmp";
         save_dialog.Filter = "Png Image|*.png";


            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                switch (save_dialog.FilterIndex)
                {
                    case 0:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Png);
                        }
                        break;
                   /* case 1:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Bmp);
                        }
                        break;*/
                }


            }
        }
        private void btnGizle_Click(object sender, EventArgs e)//btnEncrypt_Click
        {
            
            bmp = (Bitmap)picIsleme.Image;
            string yazi = txtMesaj.Text;
            // bmp = StegoIslem.hide_text(yazi, bmp);
            //MessageBox.Show("İşlendi. Resmi Kaydetmeyi unutmayın!");

            if (picIsleme.Image == null)
            {
                MessageBox.Show("Please Select Input Image!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (this.txtMesaj.Text.Equals(string.Empty))
            {
                MessageBox.Show("Cannot be able to encrypt\n" +
                    "Reason: Text not provided", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                bmp = StegoIslem.hide_text(yazi, bmp);
                MessageBox.Show("Processed! Don't forget to Check and Save the Picture!");
            }


        }

        private void btnCoz_Click(object sender, EventArgs e)//btnDecrypt_Click
        {
            

            if (picIsleme.Image == null)
            {
                MessageBox.Show("Please Select Input Image!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                bmp = (Bitmap)picIsleme.Image;
                string coz = StegoIslem.decrypt (bmp);
                txtMesaj.Text = "";
                txtMesaj.Text = coz;
            }
            
            
        }

        private void menuCikis_Click(object sender, EventArgs e)//menuExit_Click
        {
            this.Close();
        }

        private void menuIletisim_Click(object sender, EventArgs e)//menuDeveloper_Click
        {
            MessageBox.Show("Dilay Kanber");
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//btnCheck_Click
        {
          
            int lsb1, lsb2;
            
            for (int i = 0; i < picIsleme.Height; i++)
            {
                for (int j = 0; j < picIsleme.Width; j++)
                {
                    lsb1 = picIsleme.Height * picIsleme.Width * 3;
                    lsb2 = lsb1 / 8;
                    toolStripSayi.Text = lsb2.ToString();
                    toolStripStatusLabel1.Text = txtMesaj.TextLength.ToString();
                    toolStripStatusLabel3.Text = (lsb2 - txtMesaj.TextLength).ToString();





                }
            }
        }
    }
}
