using System;
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
			'#', (char)129, (char)130, (char)131
		};
		private Dictionary<char, string> _Replace = new Dictionary<char, string>
		{
			{ (char)129, "#r#" }, { (char)130, "#b#" }, { (char)131, "#g#" }
		};
		private char[] _Number = new char[]
		{
			'-', '/', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '[', ']', 'x', ':', '×', '∞', ' '
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
		private Dictionary<char, int> _GhostListCharCode;
		public Dictionary<char, int> GhostListCharCode
		{
			get
			{
				return _GhostListCharCode;
			}
		}
		public CustomEncoding()
        {

        }

		public void Build(string translation, bool number, bool ghost)
		{
			string[] original = File.ReadAllLines(translation);
			string[] input = original.Where(line => !line.StartsWith("{Copy}") && !line.StartsWith("{Title}")).ToArray();
			_CharCode = new Dictionary<char, int>();
			_GhostListCharCode = new Dictionary<char, int>();
			int codeNumber = _CodeNumber;
			for (int i = 0; i < input.Length; i++)
			{
				foreach (KeyValuePair<char, string> entry in _Replace)
				{
					input[i] = input[i].Replace(entry.Value, entry.Key.ToString());
				}
			}
			char[] chars = string.Join("", input).ToCharArray();
			if (number)	chars = chars.Where(c => !_Number.Contains(c)).ToArray();
			char[] uniqueChars = chars.Distinct().Where(c => !_Skip.Contains(c)).ToArray();
			Dictionary<char, char> dict = new Dictionary<char, char>();
			for (int i = 0; i < uniqueChars.Length; i++)
			{
				int charCode = (int)uniqueChars[i];
				int newCode = codeNumber++;
				if (codeNumber == 0x7F) codeNumber = 65377;
				if (codeNumber == 65439) codeNumber = 12353;
				if (codeNumber == 12435) codeNumber = 12449;
				while (_Skip.Contains((char)newCode))
				{
					newCode = codeNumber++;
				};
				if (number)
                {
					while (_Number.Contains((char)newCode))
					{
						newCode = codeNumber++;
					};
				}
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
			string title = string.Empty;
			Dictionary<char, char> titleDict = new Dictionary<char, char>();
			if (ghost)
            {
				title = Array.Find(original, s => s.StartsWith("{Title}")).Replace("{Title}", "");
				foreach (KeyValuePair<char, string> entry in _Replace)
				{
					title = title.Replace(entry.Value, entry.Key.ToString());
				}
				char[] titleChars = title.ToCharArray().Distinct().Where(c => !_Skip.Contains(c)).ToArray();
				for (int i = 0; i < titleChars.Length; i++)
				{
					int charCode = (int)titleChars[i];
					int newCode = codeNumber++;
					if (codeNumber == 0x7F) codeNumber = 65377;
					if (codeNumber == 65439) codeNumber = 12353;
					if (codeNumber == 12435) codeNumber = 12449;
					while (_Skip.Contains((char)newCode))
					{
						newCode = codeNumber++;
					};
					titleDict.Add((char)charCode, (char)newCode);
					_GhostListCharCode.Add((char)newCode, charCode);
				}
			}
			int index = 0;
			for (int i = 0; i < original.Length; i++)
			{
				if (original[i].StartsWith("{Copy}")) continue;
				string temp = string.Empty;
				Dictionary<char, char> dictionary = dict;
				if (original[i].StartsWith("{Title}") && ghost)
				{
					temp = title;
					dictionary = titleDict;
				}
				else temp = input[index++];
				char[] line = temp.ToCharArray();
				for (int x = 0; x < line.Length; x++)
				{
					char c;
					if (dictionary.TryGetValue(line[x], out c)) line[x] = c;
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
