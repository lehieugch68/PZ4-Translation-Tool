﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PZ4_Font_Builder
{
    public class CustomEncoding
    {
		private char[] _Skip = new char[]
		{
			'#', (char)128, (char)129, (char)130
		};
		private Dictionary<char, string> _Replace = new Dictionary<char, string>
		{
			{ (char)128, "#r#" }, { (char)129, "#b#" }, { (char)130, "#g#" }
		};
		private char[] _Number = new char[]
		{
			'-', '/', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '[', ']', 'x'
		};
		private int _CodeNumber = 0x20;
		private string[] _Messages;
		public string[] Message
        {
			get
            {
				return _Messages;
            }
        }
		private Dictionary<char, int> _CharCode;
		public Dictionary<char, int> CharCode
        {
			get
            {
				return _CharCode;
            }
        }
		public CustomEncoding()
        {

        }

		public void Build(string translation, bool number)
		{
			string[] original = File.ReadAllLines(translation);
			string[] input = original.Where(line => !line.StartsWith("{Copy}")).ToArray();
			_CharCode = new Dictionary<char, int>();
			int codeNumber = _CodeNumber;
			for (int i = 0; i < input.Length; i++)
			{
				foreach (KeyValuePair<char, string> entry in _Replace)
				{
					input[i] = input[i].Replace(entry.Value, entry.Key.ToString());
				}
			}
			char[] chars = string.Join("", input).ToCharArray().Where(c => !_Number.Contains(c)).ToArray();
			char[] uniqueChars = chars.Distinct().Where(c => !_Skip.Contains(c)).ToArray();
			Dictionary<char, char> dict = new Dictionary<char, char>();
			for (int i = 0; i < uniqueChars.Length; i++)
			{
				int charCode = (int)uniqueChars[i];
				int newCode = codeNumber++;
				if (codeNumber == 0x7F) codeNumber = 65377;
				if (codeNumber == 65439) codeNumber = 12353;
				while (_Skip.Contains((char)newCode) || _Number.Contains((char)newCode))
				{
					newCode = codeNumber++;
				};
				dict.Add((char)charCode, (char)newCode);
				_CharCode.Add((char)newCode, charCode);
			}
			if (number)
			{
				foreach (var n in _Number)
				{
					int charCode = (int)n;
					_CharCode.Add((char)charCode, charCode);
				}
			}
			int index = 0;
			for (int i = 0; i < original.Length; i++)
			{
				if (original[i].StartsWith("{Copy}")) continue;
				char[] line = input[index++].ToCharArray();
				for (int x = 0; x < line.Length; x++)
				{
					char c;
					if (dict.TryGetValue(line[x], out c)) line[x] = c;
				}
				original[i] = string.Join("", line);
				foreach (KeyValuePair<char, string> entry in _Replace)
				{
					original[i] = original[i].Replace(entry.Key.ToString(), entry.Value);
				}
			}
			_Messages = original;
		}
	}
}
