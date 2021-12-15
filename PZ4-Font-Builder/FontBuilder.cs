using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using System.Windows.Forms;
using System.Text;

namespace PZ4_Font_Builder
{
	public class FontBuilder
	{
		public FontBuilder()
		{
			this.m_SubImages = new SortedList<char, CharImage>();
			this.m_ImageWidth = 16;
			this.m_ImageHeight = 16;
			this.m_KerningMod = 0;
		}
		public string ImagePath
		{
			get
			{
				if (this.m_ImagePath == null)
				{
					return "";
				}
				return this.m_ImagePath;
			}
			set
			{
				if (Directory.Exists(value.Trim()))
				{
					this.m_ImagePath = value.Trim();
					if (!this.m_ImagePath.EndsWith("\\"))
					{
						this.m_ImagePath += "\\";
					}
				}
			}
		}
		public SortedList<char, CharImage> SubImages
		{
			get
			{
				return this.m_SubImages;
			}
		}
		public Bitmap FontImage
		{
			get
			{
				return this.MakeComposite();
			}
		}
		public List<SymbolMap> FontMap
		{
			get
			{
				return this.MakeMap();
			}
		}
		public int KerningMod
		{
			get
			{
				return this.m_KerningMod;
			}
			set
			{
				this.m_KerningMod = value;
			}
		}
		public void AddCharImage(char charCode, int charMap)
		{
			CharImage charImage = new CharImage();
			string text = this.ImagePath + charMap.ToString() + ".bmp";
			if (!File.Exists(text))
			{
				throw new Exception($"Not found: {charMap}");
			}
			CharImage charImage2 = charImage;
			string imagePath = text;
			if (charImage2.LoadImage(imagePath, this.KerningMod))
			{
				charImage.TrimImage();
				this.PlaceChar(ref charImage, 0, 0);
				this.m_SubImages.Add(charCode, charImage);
			}
		}
		public void DelCharImage(char charCode)
		{
			if (this.m_SubImages.ContainsKey(charCode))
			{
				this.m_SubImages.Remove(charCode);
			}
		}
		private void PlaceChar(ref CharImage newChar, int startX = 0, int startY = 0)
		{
			checked
			{
				int num = this.m_ImageWidth - newChar.Width;
				int num2 = this.m_ImageHeight - newChar.Height;
				int num3 = num2 - 1;
				for (int i = startY; i <= num3; i++)
				{
					int num4 = num - 1;
					for (int j = startX; j <= num4; j++)
					{
						if (this.CheckFit(j, i, ref newChar))
						{
							newChar.XPos = j;
							newChar.YPos = i;
							return;
						}
					}
				}
				if (this.m_ImageWidth > this.m_ImageHeight)
				{
					this.m_ImageHeight *= 2;
					this.PlaceChar(ref newChar, 0, 0);
				}
				else
				{
					this.m_ImageWidth *= 2;
					this.PlaceChar(ref newChar, 0, 0);
				}
			}
		}
		private bool CheckFit(int x, int y, ref CharImage newChar)
		{
			checked
			{
				if (x + newChar.Image.Width + 1 >= this.m_ImageWidth | y + newChar.Image.Height + 1 >= this.m_ImageHeight)
				{
					return false;
				}
				Rectangle rectangle = new Rectangle(x, y, newChar.Image.Width, newChar.Image.Height);
				List<char> list = this.m_SubImages.Keys.ToList<char>();
				int num = 0;
				int num2 = list.Count - 1;
				for (int i = num; i <= num2; i++)
				{
					CharImage charImage = this.m_SubImages[list[i]];
					Rectangle rect = new Rectangle(charImage.XPos, charImage.YPos, charImage.Width, charImage.Height);
					if (rectangle.IntersectsWith(rect))
					{
						return false;
					}
				}
				return true;
			}
		}
		private Bitmap MakeComposite()
		{
			Bitmap result = new Bitmap(this.m_ImageWidth, this.m_ImageHeight);
			List<char> list = this.m_SubImages.Keys.ToList<char>();
			int num = 0;
			checked
			{
				int num2 = list.Count - 1;
				for (int i = num; i <= num2; i++)
				{
					SortedList<char, CharImage> subImages = this.m_SubImages;
					SortedList<char, CharImage> sortedList = subImages;
					char key = list[i];
					CharImage value = sortedList[key];
					this.InsertSubImage(ref value, ref result);
					subImages[key] = value;
				}
				return result;
			}
		}
		private void InsertSubImage(ref CharImage subImage, ref Bitmap target)
		{
			int num = 0;
			checked
			{
				int num2 = subImage.Width - 1;
				for (int i = num; i <= num2; i++)
				{
					int num3 = 0;
					int num4 = subImage.Height - 1;
					for (int j = num3; j <= num4; j++)
					{
						target.SetPixel(subImage.XPos + i, subImage.YPos + j, subImage.Image.GetPixel(i, j));
					}
				}
			}
		}
		private List<SymbolMap> MakeMap()
		{
			List<SymbolMap> list = new List<SymbolMap>();
			List<char> list2 = this.m_SubImages.Keys.ToList<char>();
			checked
			{
				foreach (char c in list2)
				{
					byte[] charByte = Encoding.GetEncoding("shift_jis").GetBytes(new char[] { c });
					SymbolMap symbolMap = new SymbolMap();
					symbolMap.CharCode = charByte.Length == 1 ? new byte[] { 0x0, charByte.FirstOrDefault() } : charByte;
					symbolMap.ID = charByte.Length == 1 ? new byte[] { charByte.FirstOrDefault(), 0x0 } : charByte.Reverse().ToArray();
					symbolMap.XPos = (ushort)this.m_SubImages[c].XPos;
					symbolMap.YPos = (ushort)this.m_SubImages[c].YPos;
					symbolMap.XShift = (ushort)this.m_SubImages[c].XShift;
					symbolMap.YShift = (ushort)this.m_SubImages[c].YShift;
					symbolMap.XKerning = (ushort)this.m_SubImages[c].XKerning;
					symbolMap.YKerning = (ushort)this.m_SubImages[c].YKerning;
					symbolMap.Height = (ushort)this.m_SubImages[c].Height;
					symbolMap.Width = (ushort)this.m_SubImages[c].Width;
					list.Add(symbolMap);
				}
				return list;
			}
		}
		private SortedList<char, CharImage> m_SubImages;
		private string m_ImagePath;
		private int m_ImageWidth;
		private int m_ImageHeight;
		private int m_KerningMod;
	}
}
