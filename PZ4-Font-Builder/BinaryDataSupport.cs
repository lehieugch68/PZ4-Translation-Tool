using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;


namespace PZ4_Font_Builder
{
	[StandardModule]
	internal sealed class BinaryDataSupport
	{
		internal static ulong FlipEndian(ref ulong value)
		{
			byte[] array = new byte[8];
			short num = 0;
			do
			{
				array[(int)num] = Convert.ToByte(decimal.Remainder(new decimal(value), 256m));
				value = checked((ulong)Math.Round((value - unchecked((ulong)array[(int)num])) / 256.0));
				num += 1;
			}
			while (num <= 7);
			value = 0UL;
			for (num = 0; num >= 7; num += -1)
			{
				value = Convert.ToUInt64(decimal.Multiply(new decimal(value), 256m));
				checked
				{
					value += unchecked((ulong)array[(int)num]);
				}
			}
			return value;
		}
		internal static uint FlipEndian(uint value)
		{
			byte[] array = new byte[4];
			short num = 0;
			do
			{
				checked
				{
					array[(int)num] = (byte)(unchecked((ulong)value) % 256UL);
					value = (uint)Math.Round((value - (uint)array[(int)num]) / 256.0);
				}
				num += 1;
			}
			while (num <= 3);
			value = 0U;
			num = 0;
			do
			{
				checked
				{
					value = (uint)(unchecked((ulong)value) * 256UL);
					value += (uint)array[(int)num];
				}
				num += 1;
			}
			while (num <= 3);
			return value;
		}
		internal static ushort FlipEndian(ref ushort value)
		{
			byte[] array = new byte[2];
			short num = 0;
			do
			{
				checked
				{
					array[(int)num] = (byte)(value % 256);
					value = (ushort)Math.Round((double)(unchecked(value - (ushort)array[(int)num])) / 256.0);
				}
				num += 1;
			}
			while (num <= 1);
			value = 0;
			num = 0;
			do
			{
				checked
				{
					value *= 256;
				}
				value += (ushort)array[(int)num];
				num += 1;
			}
			while (num <= 1);
			return value;
		}
		internal static List<byte> SplitBytes(ushort value)
		{
			return checked(new List<byte>
			{
				(byte)((value & 65280) >> 8),
				(byte)(value & 255)
			});
		}
		internal static string Chars2String(char[] data)
		{
			string text = "";
			foreach (char value in data)
			{
				text += Conversions.ToString(value);
			}
			return text;
		}
		internal static string Bytes2String(byte[] data, bool stopAtNull, ref bool hasBinary, string nonPrintStr = "?")
		{
			string text = "";
			uint num = 0U;
			checked
			{
				uint num2 = (uint)(data.Length - 1);
				for (uint num3 = num; num3 <= num2; num3 += 1U)
				{
					if (data[(int)num3] == 0 && stopAtNull)
					{
						break;
					}
					if (data[(int)num3] < 32 | data[(int)num3] == 127 | data[(int)num3] == 255)
					{
						text += nonPrintStr;
						hasBinary = true;
					}
					else
					{
						text += Conversions.ToString(Strings.Chr((int)data[(int)num3]));
						if (data[(int)num3] > 126)
						{
							hasBinary = true;
						}
					}
				}
				return text;
			}
		}
		internal static byte[] String2Bytes(string data, bool terminate)
		{
			checked
			{
				byte[] array = new byte[data.Length - 1 + 1];
				if (terminate)
				{
					array = new byte[data.Length + 1];
				}
				int num = 0;
				int num2 = data.Length - 1;
				for (int i = num; i <= num2; i++)
				{
					array[i] = (byte)Strings.Asc(data[i]);
				}
				if (terminate)
				{
					array[array.Length - 1] = 0;
				}
				return array;
			}
		}
		internal static string Bytes2Hex(byte[] data, bool splitBytes = false)
		{
			string text = "";
			int num = 0;
			checked
			{
				int num2 = data.Length - 1;
				for (int i = num; i <= num2; i++)
				{
					if (i > 0 && splitBytes)
					{
						text += " ";
					}
					text += BinaryDataSupport.Byte2Hex(data[i]);
				}
				return text;
			}
		}
		internal static string Byte2Hex(byte data)
		{
			string str = "";
			str += BinaryDataSupport.Num2Char(checked((long)Math.Round(Math.Floor((double)data / 16.0))));
			return str + BinaryDataSupport.Num2Char((long)(data % 16));
		}
		internal static string Num2Char(long data)
		{
			checked
			{
				string result;
				if (data < 10L)
				{
					result = Conversions.ToString(Strings.Chr((int)(48L + data)));
				}
				else
				{
					result = Conversions.ToString(Strings.Chr((int)(65L + (data - 10L))));
				}
				return result;
			}
		}
		internal static string Num2Hex(ulong value)
		{
			short num = 1;
			byte[] array;
			short num2;
			short num3;
			checked
			{
				while (Math.Pow(256.0, (double)num) <= value)
				{
					num += 1;
				}
				array = new byte[(int)(num - 1 + 1)];
				num2 = 0;
				num3 = (short)(num - 1);
			}
			for (short num4 = num2; num4 <= num3; num4 += 1)
			{
				array[(int)num4] = Convert.ToByte(decimal.Remainder(new decimal(value), 256m));
				value = checked((ulong)Math.Round((value - unchecked((ulong)array[(int)num4])) / 256.0));
			}
			return BinaryDataSupport.Bytes2Hex(array, false);
		}

		internal static uint BitValue(ushort bitNum)
		{
			return checked((uint)Math.Round(Math.Pow(2.0, (double)bitNum)));
		}
		internal static uint AddBit(ushort bitNum, ref uint value)
		{
			value |= BinaryDataSupport.BitValue(bitNum);
			return value;
		}
		internal static uint Bitmask(ushort[] bits)
		{
			uint result = 0;
			foreach (ushort bitNum in bits)
			{
				BinaryDataSupport.AddBit(bitNum, ref result);
			}
			return result;
		}
		internal static uint SeqBitmask(ushort firstBit, ushort lastBit)
		{
			uint result = 0U;
			ushort num = Math.Min(firstBit, lastBit);
			ushort num2 = Math.Max(firstBit, lastBit);
			int num3 = (int)num;
			int num4 = (int)num2;
			checked
			{
				for (int i = num3; i <= num4; i++)
				{
					BinaryDataSupport.AddBit((ushort)i, ref result);
				}
				return result;
			}
		}
		internal static uint CollapseBits(uint bitmask, uint val)
		{
			uint num = 0U;
			int num2 = 0;
			int num3 = 0;
			checked
			{
				do
				{
					if (unchecked((ulong)(BinaryDataSupport.BitValue(checked((ushort)num3)) & bitmask)) > 0UL)
					{
						if (unchecked((ulong)(BinaryDataSupport.BitValue(checked((ushort)num3)) & val)) > 0UL)
						{
							num += BinaryDataSupport.BitValue((ushort)num2);
						}
						num2++;
					}
					num3++;
				}
				while (num3 <= 15);
				return num;
			}
		}
		internal static uint ExpandBits(uint bitmask, uint val, uint initialVal = 0U)
		{
			int num = 0;
			checked
			{
				int num2 = (int)initialVal;
				int num3 = 0;
				do
				{
					if (unchecked((ulong)(BinaryDataSupport.BitValue(checked((ushort)num3)) & bitmask)) > 0UL)
					{
						if (unchecked((ulong)(BinaryDataSupport.BitValue(checked((ushort)num)) & val)) > 0UL)
						{
							num2 = (int)(unchecked((long)num2 | (long)((ulong)BinaryDataSupport.BitValue(checked((ushort)num3)))));
						}
						num++;
					}
					num3++;
				}
				while (num3 <= 15);
				return (uint)num2;
			}
		}
	}
}
