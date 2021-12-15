using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace PZ4_Font_Builder
{
    public class STRIMAG2
    {
        private string _RootFile;
        public string RootFile
        {
            get
            {
                return _RootFile;
            }
            set
            {
                _RootFile = value;
            }
        }
        private struct Header
        {
            public byte[] Magic;
            public int PageLineCount;
            public int LineTextCount;
            public uint CharCount;
        }
        private Header ReadHeader(ref BinaryReader reader)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            Header header = new Header();
            header.Magic = reader.ReadBytes(8);
            reader.BaseStream.Position += 8;
            header.PageLineCount = reader.ReadInt16();
            header.LineTextCount = reader.ReadInt16();
            header.CharCount = reader.ReadUInt32();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            return header;
        }
        public STRIMAG2(string file)
        {
            _RootFile = file;
        }
        public byte[] Build(List<SymbolMap> glyphs, Bitmap bitmap)
        {
            MemoryStream result = new MemoryStream();
            using (FileStream stream = File.OpenRead(_RootFile))
            {
                BinaryReader reader = new BinaryReader(stream);
                Header header = this.ReadHeader(ref reader);
                BinaryWriter writer = new BinaryWriter(result);
                writer.Write(reader.ReadBytes(24));
                uint charCount = BinaryDataSupport.FlipEndian((uint)glyphs.Count);
                writer.Write(charCount);
                reader.BaseStream.Position += 4;
                writer.Write(reader.ReadBytes(132));
                foreach (SymbolMap glyph in glyphs)
                {
                    glyph.WriteData(ref writer);
                }
                if (writer.BaseStream.Length % 0x20 != 0)
                {
                    int padding = (int)(0x20 - (writer.BaseStream.Length % 0x20));
                    writer.Write(new byte[padding]);
                }
                writer.Write(Encoding.ASCII.GetBytes("GCT0"));
                writer.Write(new byte[4]);
                ushort num = (ushort)bitmap.Width;
                writer.Write(BinaryDataSupport.FlipEndian(ref num));
                num = (ushort)bitmap.Height;
                writer.Write(BinaryDataSupport.FlipEndian(ref num));
                writer.Write(new byte[4]);
                writer.Write(new byte[] { 0x0, 0x0, 0x0, 0x40 });
                writer.Write(new byte[44]);
                byte[] imageData = GCT0.WriteI4(bitmap);
                writer.Write(imageData);
                reader.Close();
                writer.Close();
            }
            return result.ToArray();
        }
    }
}
