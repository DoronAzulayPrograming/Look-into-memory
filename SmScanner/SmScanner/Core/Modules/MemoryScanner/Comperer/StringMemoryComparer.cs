using SmScanner.Core.Enums;
using SmScanner.Core.Extensions;
using System;
using System.Text;

namespace SmScanner.Core.Modules.MemoryScanner.Comperer
{
	public class StringMemoryComparer : ISimpleScanComparer
	{
		public ScanCompareType CompareType => ScanCompareType.Equal;
		public bool CaseSensitive { get; }
		public Encoding Encoding { get; }
		public string Value { get; }
		public int ValueSize { get; }

		public StringMemoryComparer(string value, Encoding encoding, bool caseSensitive)
		{
			Value = value;
			Encoding = encoding;
			CaseSensitive = caseSensitive;
			ValueSize = Value.Length * Encoding.GuessByteCountPerChar();
		}

		public bool Compare(byte[] data, int index, out ScanResult result)
		{
			result = null;

			var value = Encoding.GetString(data, index, ValueSize);

			if (!Value.Equals(value, CaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}

			result = new StringScanResult(value, Encoding);

			return true;
		}

		public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
		{
			return Compare(data, index, out result);
		}
	}
}
