using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PZ4_Font_Builder
{
    public class CustomEncoding
    {
		private int GetCharCode()
        {
			_CharCode++;
			if (_CharCode == 65439) _CharCode = 12353;
			if (_CharCode == 12435) _CharCode = 12449;
			return _CharCode;
		}
		private char[] _Skip = new char[]
		{
			'#', (char)129, (char)130, (char)131
		};
		private Dictionary<char, string> _Replace = new Dictionary<char, string>
		{
			{ (char)129, "#r#" }, { (char)130, "#b#" }, { (char)131, "#g#" }
		};
		private int _CharCode = 65377;
		public string[] _Messages;
		private Dictionary<char, int> _CharCodes;
		public Dictionary<char, int> CharCodes
        {
			get
            {
				return _CharCodes;
            }
        }
		private Dictionary<char, int> _GhostListCharCodes;
		public Dictionary<char, int> GhostListCharCodes
		{
			get
			{
				return _GhostListCharCodes;
			}
		}
		public CustomEncoding()
        {

        }

		public void Build(string[] translation, bool ghostList)
		{
			_CharCodes = new Dictionary<char, int>();
			_GhostListCharCodes = new Dictionary<char, int>();
			char[] chars = string.Join("", translation).ToCharArray().Distinct().Where(c => !_Skip.Contains(c)).ToArray();
			Dictionary<char, char> dict = new Dictionary<char, char>();
			for (int i = 0; i < chars.Length; i++)
            {
				int oldCharCode = (int)chars[i];
				if (oldCharCode > 126)
                {
					int newCharCode = GetCharCode();
					dict.Add((char)oldCharCode, (char)newCharCode);
					_CharCodes.Add((char)newCharCode, oldCharCode);
				}
				else
                {
					_CharCodes.Add((char)oldCharCode, oldCharCode);
				}
			}
			Dictionary<char, char> titleDict = new Dictionary<char, char>();
			if (ghostList)
			{
				string title = Array.Find(translation, s => s.StartsWith("{GhostListTitle}")).Replace("{GhostListTitle}", "");
				foreach (KeyValuePair<char, string> entry in _Replace)
				{
					title = title.Replace(entry.Value, entry.Key.ToString());
				}
				char[] titleChars = title.ToCharArray().Distinct().Where(c => !_Skip.Contains(c)).ToArray();
				for (int i = 0; i < titleChars.Length; i++)
				{
					int oldCharCode = (int)chars[i];
					int newCharCode = GetCharCode();
					
					titleDict.Add((char)oldCharCode, (char)newCharCode);
					_GhostListCharCodes.Add((char)newCharCode, oldCharCode);
				}
			}
			for (int i = 0; i < translation.Length; i++)
			{
				Dictionary<char, char> dictionary = dict;
				if (translation[i].StartsWith("{GhostListTitle}") && ghostList)
				{
					translation[i] = translation[i].Replace("{GhostListTitle}", "");
					dictionary = titleDict;
				}
				char[] temp = translation[i].ToCharArray();
				for (int x = 0; x < temp.Length; x++)
				{
					char c;
					if (dictionary.TryGetValue(temp[x], out c)) temp[x] = c;
				}
				translation[i] = string.Join("", temp);
				foreach (KeyValuePair<char, string> entry in _Replace)
				{
					translation[i] = translation[i].Replace(entry.Key.ToString(), entry.Value);
				}
			}
			_Messages = translation;
		}
	}
}
