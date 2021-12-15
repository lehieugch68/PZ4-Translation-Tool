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
            textBoxGDLG.Text = @"D:\VietHoaGame\Fatal Frame 4\PZ4-RSL-Unpacker\PZ4-RSL-Unpacker\bin\Debug\EF926\0\0.GDLG";
            textBoxSTRIMAG2File.Text = @"D:\VietHoaGame\Fatal Frame 4\PZ4-RSL-Unpacker\PZ4-RSL-Unpacker\bin\Debug\EF926\0\1";
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
                List<SymbolMap> glyphs = fontBuilder.FontMap;
                STRIMAG2 strimag2 = new STRIMAG2(textBoxSTRIMAG2File.Text);
                byte[] strimag2Data = strimag2.Build(glyphs, bitmap);
                File.Move(textBoxSTRIMAG2File.Text, textBoxSTRIMAG2File.Text + ".backup");
                File.WriteAllBytes(textBoxSTRIMAG2File.Text, strimag2Data);

                byte[] gdlgData = gdlg.Build();
                File.Move(textBoxGDLG.Text, textBoxGDLG.Text + ".backup");
                File.WriteAllBytes(textBoxGDLG.Text, gdlgData);

                File.WriteAllLines(textBoxGDLG.Text + ".txt", gdlg.Messages);
                bitmap.Save(textBoxSTRIMAG2File.Text + ".bmp", ImageFormat.Bmp);

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

        private void textBoxGDLG_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBoxGDLG_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                textBoxGDLG.Text = fileNames.FirstOrDefault();
            }
        }

        private void textBoxTranslation_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBoxTranslation_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                textBoxTranslation.Text = fileNames.FirstOrDefault();
            }
        }

        private void textBoxSTRIMAG2File_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                textBoxSTRIMAG2File.Text = fileNames.FirstOrDefault();
            }
        }

        private void textBoxSTRIMAG2File_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBoxImage_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                textBoxImage.Text = fileNames.FirstOrDefault();
            }
        }

        private void textBoxImage_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
