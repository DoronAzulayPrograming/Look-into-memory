using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmScanner.Core.Extensions
{
	public static class StringExtension
	{
		[Pure]
		[DebuggerStepThrough]
		public static string SmEndianReverse(this string s)
		{
			char t;
			char[] charArray = s.ToCharArray();
            for (int i = 0; i < charArray.Length-1; i+=2)
            {
				t = charArray[i];
				charArray[i] = charArray[i+1];
				charArray[i+1] = t;
			}
			return new string(charArray);
		}
		[Pure]
		[DebuggerStepThrough]
		public static string SmReverse(this string s)
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}
		[Pure]
		[DebuggerStepThrough]
		public static List<string> SmGetAddressList(this string text, string format)
		{
			string instracionsBytes = string.Empty;
			List<string> instracions = new List<string>();

			int indexOf = text.IndexOf("0x");
			while (indexOf != -1)
			{
				int lastIndex = text.SmFindAddressInString(indexOf + 2);
				instracions.Add(long.Parse(text.SmSubstring(indexOf, lastIndex).Trim().Substring(2),System.Globalization.NumberStyles.HexNumber).ToString("X08"));
				indexOf = text.IndexOf("0x", lastIndex);
			}

			instracions.AddRange(instracions.Select(s => new string($"{s:X016}")).ToList());

			return instracions;
		}
		[Pure]
		[DebuggerStepThrough]
		public static int SmFindAddressInString(this string text, int start_index)
		{
			int last_index = -1;
			string text_lower_case = text.ToLower();
			for (int i = start_index; i < text_lower_case.Length; i++)
			{
				if (text_lower_case[i] == ' ' && last_index == -1) continue;
				if ((text_lower_case[i] >= '0' && text_lower_case[i] <= '9') || (text_lower_case[i] >= 'a' && text_lower_case[i] <= 'f'))
					last_index = i;
				else
					break;
			}

			return last_index;
		}
		[Pure]
		[DebuggerStepThrough]
		public static bool SmEqualsToOperator(this string text)
		{
			if (text == null) return false;
			if (text.Length <= 0) return false;

            switch (text[0])
            {
				case '+': return true;
				case '-': return true;
				case '*': return true;
				case '/': return true;
				default:  return false;
			}
		}
		[Pure]
		[DebuggerStepThrough]
		public static bool SmEqualsToOperator(this string text, char letter)
		{
			if (text == null) return false;
			if (text.Length <= 0) return false;

			bool status = false;
			int i;
			for (i = 0; i < text.Length; i++) if (text[i] == letter) { status = true; break; }

			return status;
		}
		[Pure]
		[DebuggerStepThrough]
		public static int SmIndexOf(this string text, char letter)
		{
			if (text == null) return -1;
			if (text.Length <= 0) return -1;

			bool status = false;
			int i;
			for (i = 0; i < text.Length; i++) if (text[i] == letter) { status = true; break; }

			if (!status) return -1;

			return i;
		}
		[Pure]
		[DebuggerStepThrough]
		public static int SmIndexOf(this string text, string oneLetter)
		{
			if (text == null) return -1;
			if (oneLetter == null) return -1;
			if (text.Length <= 0) return -1;
			if (oneLetter.Length < 1) return -1;

			char letter = oneLetter[0];

			return text.SmIndexOf(letter);
		}
		[Pure]
		[DebuggerStepThrough]
		public static string SmSubstring(this string text, int start)
		{
			if (text.Length <= 0) return string.Empty;
			if (start > text.Length - 1) return string.Empty;

			string str = string.Empty;

			for (int i = start; i < text.Length; i++) str += text[i];

			return str;
		}
		//[Pure]
		//[DebuggerStepThrough]
		public static string SmSubstring(this string text, int start, int end)
		{
			if (text.Length <= 0) return string.Empty;
			if (start > text.Length - 1) return string.Empty;
			if (end > text.Length - 1) return string.Empty;

			int len = text.Length - end;
			string str = string.Empty;

            if (text.Contains("esi+ecx+0x1"))
            {
				int ss = 1;
            }
			for (int i = start; i <= end; i++) str += text[i];

			return str;
		}


		[Pure]
		[DebuggerStepThrough]
		public static bool IsPrintable(this char c)
		{
			return (' ' <= c && c <= '~' || '\xA1' <= c && c <= '\xFF') && c != '\xFFFD' /* Unicode REPLACEMENT CHARACTER � */;
		}

		[DebuggerStepThrough]
		public static IEnumerable<char> InterpretAsSingleByteCharacter(this IEnumerable<byte> source)
		{
			Contract.Requires(source != null);

			return source.Select(b => (char)b);
		}

		[DebuggerStepThrough]
		public static IEnumerable<char> InterpretAsDoubleByteCharacter(this IEnumerable<byte> source)
		{
			Contract.Requires(source != null);

			var bytes = source.ToArray();
			var chars = new char[bytes.Length / 2];
			Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return chars;
		}

		[DebuggerStepThrough]
		public static bool IsPrintableData(this IEnumerable<char> source)
		{
			Contract.Requires(source != null);

			return CalculatePrintableDataThreshold(source) >= 1.0f;
		}

		[DebuggerStepThrough]
		public static bool IsLikelyPrintableData(this IEnumerable<char> source)
		{
			Contract.Requires(source != null);

			return CalculatePrintableDataThreshold(source) >= 0.75f;
		}

		[DebuggerStepThrough]
		public static float CalculatePrintableDataThreshold(this IEnumerable<char> source)
		{
			var doCountValid = true;
			var countValid = 0;
			var countAll = 0;

			foreach (var c in source)
			{
				countAll++;

				if (doCountValid)
				{
					if (c.IsPrintable())
					{
						countValid++;
					}
					else
					{
						doCountValid = false;
					}
				}
			}

			if (countAll == 0)
			{
				return 0.0f;
			}

			return countValid / (float)countAll;
		}

		[Pure]
		[DebuggerStepThrough]
		public static string LimitLength(this string s, int length)
		{
			Contract.Requires(s != null);
			Contract.Ensures(Contract.Result<string>() != null);

			if (s.Length <= length)
			{
				return s;
			}
			return s.Substring(0, length);
		}

		private static readonly Regex hexadecimalValueRegex = new Regex("^(0x|h)?([0-9A-F]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		public static bool TryGetHexString(this string s, out string value)
		{
			Contract.Requires(s != null);

			var match = hexadecimalValueRegex.Match(s);
			value = match.Success ? match.Groups[2].Value : null;

			return match.Success;
		}
	}
}
