using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PZ4_Font_Builder
{
    public class GDLG
    {
        private string[] _Messages;
        public string[] Messages
        {
            get
            {
                return _Messages;
            }
            set
            {
                _Messages = value;
            }
        }
        private Page[] _Pages;
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
        public GDLG(string file)
        {
            _RootFile = file;
        }
        #region Structure
        private struct Header
        {
            public int Magic;
            public int Unk;
            public short PageCount;
            public short MessageCount;
            public int PageDataOffset;
            public int PageTableOffset;
            public int MessageDataOffset;
            public int MessageTableOffset;
        }
        private struct DialogMessage
        {
            public int Pointer;
            public byte[] Data;
            public string Text;
        }
        private struct Page
        {
            public int Pointer;
            public int Title;
            public int DialogCount;
            public int DialogOffset;
            public PageDialog[] PageDialogs;
        }
        private struct PageDialog
        {
            public int Pointer;
            public short Magic;
            public ushort MessageCount;
            public byte[] Unk;
            public DialogMapping[] Messages;
        }
        private struct DialogMapping
        {
            public int Pointer;
            public int Index;
        }
        #endregion
        private static Header ReadHeader(ref BinaryReader reader)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            Header header = new Header();
            header.Magic = reader.ReadInt32();
            if (header.Magic != 0x474C4447) throw new Exception("Unsupported file type.");
            header.Unk = reader.ReadInt32();
            header.PageCount = reader.ReadInt16();
            header.MessageCount = reader.ReadInt16();
            header.PageDataOffset = reader.ReadInt32();
            header.PageTableOffset = reader.ReadInt32();
            header.MessageDataOffset = reader.ReadInt32();
            header.MessageTableOffset = reader.ReadInt32();
            return header;
        }
        private static DialogMessage[] ReadEntries(ref BinaryReader reader, Header header)
        {
            List<DialogMessage> list = new List<DialogMessage>();
            reader.BaseStream.Seek(header.MessageTableOffset, SeekOrigin.Begin);
            for (int i = 0; i < header.MessageCount; i++)
            {
                DialogMessage entry = new DialogMessage();
                entry.Pointer = reader.ReadInt32();
                list.Add(entry);
            }
            reader.BaseStream.Seek(header.MessageDataOffset, SeekOrigin.Begin);
            DialogMessage[] result = list.ToArray();
            for (int i = 0; i < header.MessageCount; i++)
            {
                List<byte> bytes = new List<byte>();
                byte b = reader.ReadByte();
                while (b != 0 && reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    if (b == 0x8D) break;
                    bytes.Add(b);
                    b = reader.ReadByte();
                }
                result[i].Data = bytes.ToArray();
                result[i].Text = TextDecode(bytes);

            }
            return result;
        }
        private static Page[] ReadPages(ref BinaryReader reader, Header header)
        {
            Page[] result = new Page[header.PageCount];
            reader.BaseStream.Seek(header.PageTableOffset, SeekOrigin.Begin);
            for (int i = 0; i < header.PageCount; i++)
            {
                result[i].Pointer = reader.ReadInt32();
                long nextPage = reader.BaseStream.Position;
                reader.BaseStream.Seek(header.PageDataOffset + result[i].Pointer, SeekOrigin.Begin);
                result[i].Title = reader.ReadInt32();
                result[i].DialogCount = reader.ReadInt32();
                result[i].DialogOffset = reader.ReadInt32();
                reader.BaseStream.Seek(header.PageDataOffset + result[i].Pointer + result[i].DialogOffset, SeekOrigin.Begin);
                result[i].PageDialogs = new PageDialog[result[i].DialogCount];
                for (int x = 0; x < result[i].DialogCount; x++)
                {
                    result[i].PageDialogs[x].Pointer = reader.ReadInt32();
                    long nextDialog = reader.BaseStream.Position;
                    reader.BaseStream.Seek(header.PageDataOffset + result[i].Pointer + result[i].PageDialogs[x].Pointer, SeekOrigin.Begin);
                    long tableStart = reader.BaseStream.Position;
                    result[i].PageDialogs[x].Magic = reader.ReadInt16();
                    result[i].PageDialogs[x].MessageCount = reader.ReadUInt16();
                    result[i].PageDialogs[x].Unk = reader.ReadBytes(0x1C);
                    result[i].PageDialogs[x].Messages = new DialogMapping[result[i].PageDialogs[x].MessageCount];
                    for (int y = 0; y < result[i].PageDialogs[x].MessageCount; y++)
                    {
                        result[i].PageDialogs[x].Messages[y].Pointer = reader.ReadInt32();
                        long nextPointer = reader.BaseStream.Position;
                        reader.BaseStream.Seek(tableStart + result[i].PageDialogs[x].Messages[y].Pointer, SeekOrigin.Begin);
                        result[i].PageDialogs[x].Messages[y].Index = reader.ReadInt32();
                        reader.BaseStream.Position = nextPointer;
                    }
                    reader.BaseStream.Position = nextDialog;
                }
                reader.BaseStream.Position = nextPage;
            }
            return result;
        }
        private static byte[] TextEncode(string source)
        {
            List<byte> list = Encoding.GetEncoding("shift_jis").GetBytes(source).ToList();
            list.Add(0);
            for (int i = 0; i <= list.Count - 1; i++)
            {
                list[i] ^= 141;
            }
            return list.ToArray();
        }
        private static string TextDecode(List<byte> sourceBytes)
        {
            List<byte> list = new List<byte>();
            foreach (byte b in sourceBytes)
            {
                list.Add((byte)(b ^ 141));
            }
            string decoded = Encoding.GetEncoding("shift_jis").GetString(list.ToArray());
            return decoded;
        }
        public void GetStrings(string txt)
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(_RootFile));
            Header header = ReadHeader(ref reader);
            DialogMessage[] entries = ReadEntries(ref reader, header);
            Page[] pages = ReadPages(ref reader, header);

            List<string> result = new List<string>();
            int index = 0;
            using (StreamReader sr = new StreamReader(txt))
            {
                while (!sr.EndOfStream)
                {
                    string line = string.Empty;
                    while (!line.StartsWith("#PAGE=") && !sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                    }
                    if (sr.EndOfStream) break;
                    int pageIndex = int.Parse(line.Split('=')[1]);
                    string title = sr.ReadLine().Split('=')[1].Trim();
                    int titleIndex = int.Parse(sr.ReadLine().Split('=')[1]);
                    result.Add(title);
                    pages[pageIndex].Title = index++;
                    if (entries[titleIndex + 1].Text == "NON")
                    {
                        result.Add(entries[titleIndex + 1].Text);
                        index++;
                    }
                    for (int i = 0; i < pages[pageIndex].DialogCount; i++)
                    {
                        while (!line.StartsWith("/*INDEX="))
                        {
                            line = sr.ReadLine();
                        }
                        int count = 0;
                        int tableIndex = int.Parse(line.Split('=')[1]);
                        line = sr.ReadLine();
                        while (!line.StartsWith("*/"))
                        {
                            result.Add(line);
                            count++;
                            line = sr.ReadLine();
                        }
                        pages[pageIndex].PageDialogs[tableIndex].Messages = new DialogMapping[count];
                        pages[pageIndex].PageDialogs[tableIndex].MessageCount = (ushort)count;
                        for (int x = 0; x < count; x++)
                        {
                            pages[pageIndex].PageDialogs[i].Messages[x].Index = index++;
                        }
                    }
                    while (!line.StartsWith("#END"))
                    {
                        line = sr.ReadLine();
                    }
                }
            }

            reader.Close();
            _Pages = pages;
            _Messages = result.ToArray();
        }
        public byte[] Build()
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(_RootFile));
            Header header = ReadHeader(ref reader);
            //TextEntry[] entries = ReadEntries(ref reader, header);            
            header.MessageCount = (short)_Messages.Length;
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(header.Magic);
                writer.Write(header.Unk);
                writer.Write(header.PageCount);
                writer.Write(header.MessageCount);
                reader.BaseStream.Position = writer.BaseStream.Position;
                writer.Write(reader.ReadBytes(0x14));
                writer.Write(new byte[header.PageCount * 4]);
                if ((header.PageCount * 4) % 0x20 != 0) writer.Write(new byte[0x20 - ((header.PageCount * 4) % 0x20)]);
                header.MessageTableOffset = (int)writer.BaseStream.Position;
                writer.Write(new byte[header.MessageCount * 4]);
                if ((header.MessageCount * 4) % 0x10 != 0) writer.Write(new byte[0x10 - ((header.MessageCount * 4) % 0x10)]);
                header.PageDataOffset = (int)writer.BaseStream.Position;
                writer.BaseStream.Position = 0xC;
                writer.Write(header.PageDataOffset);
                writer.BaseStream.Position = header.PageDataOffset;
                long pagePointer = 0;
                for (int i = 0; i < header.PageCount; i++)
                {
                    _Pages[i].Pointer = (int)pagePointer;
                    long pageOffset = writer.BaseStream.Position;
                    writer.BaseStream.Position = header.PageTableOffset + (i * 4);
                    writer.Write(_Pages[i].Pointer);
                    writer.BaseStream.Position = pageOffset;
                    writer.Write(_Pages[i].Title);
                    writer.Write(_Pages[i].DialogCount);
                    writer.Write(_Pages[i].DialogOffset);
                    writer.Write(new byte[0x14]);
                    long dialogTableOffset = writer.BaseStream.Position;
                    writer.Write(new byte[_Pages[i].DialogCount * 4]);
                    if ((_Pages[i].DialogCount * 4) % 0x10 != 0) writer.Write(new byte[(0x10 - ((_Pages[i].DialogCount * 4) % 0x10))]);
                    for (int x = 0; x < _Pages[i].DialogCount; x++)
                    {
                        long dialogOffset = writer.BaseStream.Position;
                        writer.BaseStream.Position = dialogTableOffset + (x * 4);
                        writer.Write((int)(dialogOffset - pageOffset));
                        writer.BaseStream.Position = dialogOffset;
                        writer.Write(_Pages[i].PageDialogs[x].Magic);
                        writer.Write(_Pages[i].PageDialogs[x].MessageCount);
                        writer.Write(_Pages[i].PageDialogs[x].Unk);
                        long messageTableOffset = writer.BaseStream.Position;
                        writer.Write(new byte[_Pages[i].PageDialogs[x].MessageCount * 4]);
                        if ((_Pages[i].PageDialogs[x].MessageCount * 4) % 0x10 != 0) writer.Write(new byte[0x10 - ((_Pages[i].PageDialogs[x].MessageCount * 4) % 0x10)]);
                        for (int y = 0; y < _Pages[i].PageDialogs[x].MessageCount; y++)
                        {
                            long messageOffset = writer.BaseStream.Position;
                            writer.BaseStream.Position = messageTableOffset + (y * 4);
                            writer.Write((int)(messageOffset - dialogOffset));
                            writer.BaseStream.Position = messageOffset;
                            writer.Write(_Pages[i].PageDialogs[x].Messages[y].Index);
                            writer.Write(new byte[0xC]);
                        }
                    }
                    pagePointer += writer.BaseStream.Position - pageOffset;
                }
                header.MessageDataOffset = (int)writer.BaseStream.Position;
                writer.BaseStream.Position = 0x14;
                writer.Write(header.MessageDataOffset);
                writer.BaseStream.Position = header.MessageDataOffset;
                for (int i = 0; i < header.MessageCount; i++)
                {
                    byte[] encoded = TextEncode(_Messages[i]);
                    long textPointer = writer.BaseStream.Position;
                    writer.BaseStream.Position = header.MessageTableOffset + (i * 4);
                    writer.Write((int)(textPointer - header.MessageDataOffset));
                    writer.BaseStream.Position = textPointer;
                    writer.Write(encoded);
                }

                if (writer.BaseStream.Length % 0x10 != 0)
                {
                    int padding = (int)(0x10 - (writer.BaseStream.Length % 0x10));
                    writer.Write(new byte[padding]);
                }
            }
            reader.Close();
            return stream.ToArray();
        }
    }
}
