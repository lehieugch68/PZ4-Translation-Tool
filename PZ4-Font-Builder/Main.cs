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
            textBoxGDLG.Text = @"D:\VietHoaGame\Fatal Frame 4\PZ4-RSL-Unpacker\PZ4-RSL-Unpacker\bin\Debug\D0001\7\0.GDLG";
            textBoxSTRIMAG2File.Text = @"D:\VietHoaGame\Fatal Frame 4\PZ4-RSL-Unpacker\PZ4-RSL-Unpacker\bin\Debug\D0001\7\1";
            textBoxTranslation.Text = @"D:\VietHoaGame\Fatal Frame 4\Translation\Vietnamese Script\D0001.RSL.txt";
            textBoxImage.Text = @"D:\VietHoaGame\Fatal Frame 4\Texture\FontSize-0-Vietnamese";
            textBoxGhostListFont.Text = @"D:\VietHoaGame\Fatal Frame 4\Texture\FontSize-0-Vietnamese";
            textBoxKerning.Text = "4";
            textBoxKerningModTitle.Text = "4";
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                
                GDLG gdlg = new GDLG(textBoxGDLG.Text);
                gdlg.GetStrings(textBoxTranslation.Text);
                CustomEncoding customEncoding = new CustomEncoding();
                customEncoding.Build(gdlg.Messages, checkBoxGhostList.Checked);
                gdlg.Messages = customEncoding._Messages;

                FontBuilder fontBuilder = new FontBuilder();
                fontBuilder.ImagePath = textBoxImage.Text;
                fontBuilder.KerningMod = int.Parse(textBoxKerning.Text);
                foreach (KeyValuePair<char, int> entry in customEncoding.CharCodes)
                {
                    fontBuilder.AddCharImage(entry.Key, entry.Value);
                }
                if (checkBoxGhostList.Checked)
                {
                    fontBuilder.ImagePath = textBoxGhostListFont.Text;
                    fontBuilder.KerningMod = int.Parse(textBoxKerningModTitle.Text);
                    foreach (KeyValuePair<char, int> entry in customEncoding.GhostListCharCodes)
                    {
                        fontBuilder.AddCharImage(entry.Key, entry.Value);
                    }
                }
                Bitmap bitmap = fontBuilder.FontImage;
                List<SymbolMap> glyphs = fontBuilder.FontMap;
                STRIMAG2 strimag2 = new STRIMAG2(textBoxSTRIMAG2File.Text);
                byte[] strimag2Data = strimag2.Build(glyphs, bitmap);
                File.WriteAllBytes(textBoxSTRIMAG2File.Text, strimag2Data);

                byte[] gdlgData = gdlg.Build();
                File.WriteAllBytes(textBoxGDLG.Text, gdlgData);

                //preview
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
