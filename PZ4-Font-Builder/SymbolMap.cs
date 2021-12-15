using System;
using System.Diagnostics;
using System.IO;

namespace PZ4_Font_Builder
{
	public class SymbolMap
	{
		[DebuggerNonUserCode]
		public SymbolMap()
		{
		}
		public bool ReadData(ref BinaryReader reader)
		{
			this.CharCode = reader.ReadBytes(2);
			ushort num = reader.ReadUInt16();
			this.XPos = BinaryDataSupport.FlipEndian(ref num);
			num = reader.ReadUInt16();
			this.YPos = BinaryDataSupport.FlipEndian(ref num);
			num = reader.ReadUInt16();
			this.Width = BinaryDataSupport.FlipEndian(ref num);
			num = reader.ReadUInt16();
			this.Height = BinaryDataSupport.FlipEndian(ref num);
			num = reader.ReadUInt16();
			this.XShift = BinaryDataSupport.FlipEndian(ref num);
			num = reader.ReadUInt16();
			this.YShift = BinaryDataSupport.FlipEndian(ref num);
			num = reader.ReadUInt16();
			this.XKerning = BinaryDataSupport.FlipEndian(ref num);
			num = reader.ReadUInt16();
			this.YKerning = BinaryDataSupport.FlipEndian(ref num);
			this.ID = reader.ReadBytes(2);
			return true;
		}
		public void WriteData(ref BinaryWriter writer)
		{
			writer.Write(this.CharCode);
			writer.Write(BinaryDataSupport.FlipEndian(ref this.XPos));
			writer.Write(BinaryDataSupport.FlipEndian(ref this.YPos));
			writer.Write(BinaryDataSupport.FlipEndian(ref this.Width));
			writer.Write(BinaryDataSupport.FlipEndian(ref this.Height));
			writer.Write(BinaryDataSupport.FlipEndian(ref this.XShift));
			writer.Write(BinaryDataSupport.FlipEndian(ref this.YShift));
			writer.Write(BinaryDataSupport.FlipEndian(ref this.XKerning));
			writer.Write(BinaryDataSupport.FlipEndian(ref this.YKerning));
			writer.Write(this.ID);
		}
		public byte[] CharCode;
		public ushort XPos;
		public ushort YPos;
		public ushort Width;
		public ushort Height;
		public ushort XShift;
		public ushort YShift;
		public ushort XKerning;
		public ushort YKerning;
		public byte[] ID;
	}
}
