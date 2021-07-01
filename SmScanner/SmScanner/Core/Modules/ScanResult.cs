﻿using SmScanner.Core.Enums;
using SmScanner.Core.Extensions;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace SmScanner.Core.Modules
{
	public abstract class ScanResult
	{
		public abstract ScanValueType ValueType { get; }

		public IntPtr Address { get; set; }

		public abstract int ValueSize { get; }

		public abstract ScanResult Clone();
	}

	public class ByteScanResult : ScanResult, IEquatable<ByteScanResult>
	{
		public override ScanValueType ValueType => ScanValueType.Byte;

		public override int ValueSize => sizeof(byte);

		public byte Value { get; }

		public ByteScanResult(byte value)
		{
			Value = value;
		}

		public override ScanResult Clone()
		{
			return new ByteScanResult(Value) { Address = Address };
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ByteScanResult);
		}

		public bool Equals(ByteScanResult other)
		{
			return other != null && Address == other.Address && Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode() * 19 + Value.GetHashCode();
		}
	}

	public class ShortScanResult : ScanResult, IEquatable<ShortScanResult>
	{
		public override ScanValueType ValueType => ScanValueType.Short;

		public override int ValueSize => sizeof(short);

		public short Value { get; }

		public ShortScanResult(short value)
		{
			Value = value;
		}

		public override ScanResult Clone()
		{
			return new ShortScanResult(Value) { Address = Address };
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ShortScanResult);
		}

		public bool Equals(ShortScanResult other)
		{
			return other != null && Address == other.Address && Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode() * 19 + Value.GetHashCode();
		}
	}

	public class IntegerScanResult : ScanResult, IEquatable<IntegerScanResult>
	{
		public override ScanValueType ValueType => ScanValueType.Integer;

		public override int ValueSize => sizeof(int);

		public int Value { get; }

		public IntegerScanResult(int value)
		{
			Value = value;
		}

		public override ScanResult Clone()
		{
			return new IntegerScanResult(Value) { Address = Address };
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as IntegerScanResult);
		}

		public bool Equals(IntegerScanResult other)
		{
			return other != null && Address == other.Address && Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode() * 19 + Value.GetHashCode();
		}
	}

	public class LongScanResult : ScanResult, IEquatable<LongScanResult>
	{
		public override ScanValueType ValueType => ScanValueType.Long;

		public override int ValueSize => sizeof(long);

		public long Value { get; }

		public LongScanResult(long value)
		{
			Value = value;
		}

		public override ScanResult Clone()
		{
			return new LongScanResult(Value) { Address = Address };
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as LongScanResult);
		}

		public bool Equals(LongScanResult other)
		{
			return other != null && Address == other.Address && Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode() * 19 + Value.GetHashCode();
		}
	}

	public class FloatScanResult : ScanResult, IEquatable<FloatScanResult>
	{
		public override ScanValueType ValueType => ScanValueType.Float;

		public override int ValueSize => sizeof(float);

		public float Value { get; }

		public FloatScanResult(float value)
		{
			Value = value;
		}

		public override ScanResult Clone()
		{
			return new FloatScanResult(Value) { Address = Address };
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as FloatScanResult);
		}

		public bool Equals(FloatScanResult other)
		{
			return other != null && Address == other.Address && Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode() * 19 + Value.GetHashCode();
		}
	}

	public class DoubleScanResult : ScanResult, IEquatable<DoubleScanResult>
	{
		public override ScanValueType ValueType => ScanValueType.Double;

		public override int ValueSize => sizeof(double);

		public double Value { get; }

		public DoubleScanResult(double value)
		{
			Value = value;
		}

		public override ScanResult Clone()
		{
			return new DoubleScanResult(Value) { Address = Address };
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as DoubleScanResult);
		}

		public bool Equals(DoubleScanResult other)
		{
			return other != null && Address == other.Address && Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode() * 19 + Value.GetHashCode();
		}
	}

	public class ArrayOfBytesScanResult : ScanResult, IEquatable<ArrayOfBytesScanResult>
	{
		public override ScanValueType ValueType => ScanValueType.ArrayOfBytes;

		public override int ValueSize => Value.Length;

		public byte[] Value { get; }

		public ArrayOfBytesScanResult(byte[] value)
		{
			Contract.Requires(value != null);

			Value = value;
		}

		public override ScanResult Clone()
		{
			return new ArrayOfBytesScanResult(Value) { Address = Address };
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ArrayOfBytesScanResult);
		}

		public bool Equals(ArrayOfBytesScanResult other)
		{
			return other != null && Address == other.Address && Value.SequenceEqual(other.Value);
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode() * 19 + Value.GetHashCode();
		}
	}

	public class StringScanResult : ScanResult, IEquatable<StringScanResult>
	{
		public override ScanValueType ValueType => ScanValueType.String;

		public override int ValueSize => Value.Length * Encoding.GuessByteCountPerChar();

		public string Value { get; }

		public Encoding Encoding { get; }

		public StringScanResult(string value, Encoding encoding)
		{
			Contract.Requires(value != null);
			Contract.Requires(encoding != null);

			Value = value;
			Encoding = encoding;
		}

		public override ScanResult Clone()
		{
			return new StringScanResult(Value, Encoding) { Address = Address };
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as StringScanResult);
		}

		public bool Equals(StringScanResult other)
		{
			return other != null && Address == other.Address && Value == other.Value && Encoding.Equals(other.Encoding);
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode() * 19 + Value.GetHashCode() * 19 + Encoding.GetHashCode();
		}
	}

	public class RegexStringScanResult : StringScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Regex;

		public RegexStringScanResult(string value, Encoding encoding)
			: base(value, encoding)
		{

		}

		public override ScanResult Clone()
		{
			return new RegexStringScanResult(Value, Encoding) { Address = Address };
		}
	}
}
