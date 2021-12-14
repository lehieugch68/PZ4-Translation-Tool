using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualBasic;

namespace PZ4_Font_Builder
{
	public class CharImage
	{
		public CharImage()
		{
			this.m_Image = null;
			this.m_ImagePath = "";
		}
		public int XPos
		{
			get
			{
				return this.m_XPos;
			}
			set
			{
				this.m_XPos = value;
			}
		}
		public int YPos
		{
			get
			{
				return this.m_YPos;
			}
			set
			{
				this.m_YPos = value;
			}
		}
		public int XKerning
		{
			get
			{
				return this.m_XKerning;
			}
			set
			{
				this.m_XKerning = value;
			}
		}
		public int YKerning
		{
			get
			{
				return this.m_YKerning;
			}
			set
			{
				this.m_YKerning = value;
			}
		}
		public int XShift
		{
			get
			{
				return this.m_XShift;
			}
			set
			{
				this.m_XShift = value;
			}
		}
		public int YShift
		{
			get
			{
				return this.m_YShift;
			}
			set
			{
				this.m_YShift = value;
			}
		}
		public int Width
		{
			get
			{
				return this.Image.Width;
			}
		}
		public int Height
		{
			get
			{
				return this.Image.Height;
			}
		}
		public Bitmap Image
		{
			get
			{
				if (this.m_Image == null)
				{
					return new Bitmap(1, 1);
				}
				return this.m_Image;
			}
			set
			{
				this.m_Image = value;
			}
		}
		public bool LoadImage(string imagePath, int kerningMod = 0)
		{
			if (File.Exists(imagePath))
			{
				this.m_Image = new Bitmap(imagePath);
				this.m_ImagePath = imagePath;
				this.m_XKerning = checked(this.m_Image.Width + kerningMod);
				this.m_YKerning = this.m_Image.Height;
				return true;
			}
			return false;
		}
		public void TrimImage()
		{
			Rectangle rect = default(Rectangle);
			bool flag = true;
			int num = 0;
			checked
			{
				int num2 = this.m_Image.Width - 1;
				for (int i = num; i <= num2; i++)
				{
					int num3 = 0;
					int num4 = this.m_Image.Height - 1;
					for (int j = num3; j <= num4; j++)
					{
						if (!this.IsBlack(this.m_Image.GetPixel(i, j)))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						rect.X = i;
						break;
					}
				}
				flag = true;
				int num5 = 0;
				int num6 = this.m_Image.Height - 1;
				for (int j = num5; j <= num6; j++)
				{
					int num7 = 0;
					int num8 = this.m_Image.Width - 1;
					for (int i = num7; i <= num8; i++)
					{
						if (!this.IsBlack(this.m_Image.GetPixel(i, j)))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						rect.Y = j;
						break;
					}
				}
				flag = true;
				for (int i = this.m_Image.Width - 1; i >= 0; i += -1)
				{
					int num9 = 0;
					int num10 = this.m_Image.Height - 1;
					for (int j = num9; j <= num10; j++)
					{
						if (!this.IsBlack(this.m_Image.GetPixel(i, j)))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						rect.Width = i - rect.X + 1;
						break;
					}
				}
				flag = true;
				for (int j = this.m_Image.Height - 1; j >= 0; j += -1)
				{
					int num11 = 0;
					int num12 = this.m_Image.Width - 1;
					for (int i = num11; i <= num12; i++)
					{
						if (!this.IsBlack(this.m_Image.GetPixel(i, j)))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						rect.Height = j - rect.Y + 1;
						break;
					}
				}
				if (rect.Width == 0)
				{
					rect.Width = 1;
				}
				if (rect.Height == 0)
				{
					rect.Height = 1;
				}
				this.m_XShift = rect.X;
				this.m_YShift = rect.Y;
				this.m_Image = this.m_Image.Clone(rect, this.m_Image.PixelFormat);
			}
		}
		private bool IsBlack(Color pixelColor)
		{
			return pixelColor.R == 0 & pixelColor.G == 0 & pixelColor.B == 0;
		}

		private Bitmap m_Image;
		private string m_ImagePath;
		private int m_XPos;
		private int m_YPos;
		private int m_XShift;
		private int m_YShift;
		private int m_XKerning;
		private int m_YKerning;
	}
}
