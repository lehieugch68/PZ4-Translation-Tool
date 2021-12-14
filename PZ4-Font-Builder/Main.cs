using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace PZ4_Font_Builder
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            textBoxGDLG.Text = @"D:\CSharp\PZ4-RSL-Unpacker\PZ4-RSL-Unpacker\bin\Debug\EF926\0\0.GDLG";
            textBoxTranslation.Text = @"D:\VietHoaGame\Fatal Frame 4\Translation\Vietnamese Script\EF926.RSL.txt";
            textBoxImage.Text = @"D:\VietHoaGame\Fatal Frame 4\Texture\FontSize-0-Vietnamese";
            textBoxKerning.Text = "4";
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                
                GDLG gdlg = new GDLG(textBoxGDLG.Text);
                gdlg.GetStrings();
                CustomEncoding customEncoding = new CustomEncoding();
                customEncoding.Build(textBoxTranslation.Text, checkBoxNumber.Checked);
                string[] temp = new string[gdlg.Messages.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = customEncoding.Message[i].StartsWith("{Copy}") ? gdlg.Messages[i] : customEncoding.Message[i];
                }
                gdlg.Messages = temp;
                
                FontBuilder fontBuilder = new FontBuilder();
                fontBuilder.ImagePath = textBoxImage.Text;
                fontBuilder.KerningMod = int.Parse(textBoxKerning.Text);
                foreach (KeyValuePair<char, int> entry in customEncoding.CharCode)
                {
                    fontBuilder.AddCharImage(entry.Key, entry.Value);
                }
                Bitmap bitmap = fontBuilder.FontImage;
                bitmap.Save(@"D:\VietHoaGame\Fatal Frame 4\ISO\test.png", ImageFormat.Png);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            
        }
        private void textBoxKerning_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
