using SmScanner.Core.Interfaces;
using SmScanner.Core.Memory;
using SmScanner.Core.Modules.MemoryScanner;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace SmScanner.Core.Extensions
{
	public static class IRemoteMemoryReaderExtension
	{
		public static sbyte ReadRemoteInt8(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(sbyte));

			return (sbyte)data[0];
		}

		public static byte ReadRemoteUInt8(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(byte));

			return data[0];
		}

		public static short ReadRemoteInt16(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(short));

			return BitConverter.ToInt16(data, 0);
		}

		public static ushort ReadRemoteUInt16(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(ushort));

			return BitConverter.ToUInt16(data, 0);
		}

		public static int ReadRemoteInt32(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(int));

			return BitConverter.ToInt32(data, 0);
		}

		public static uint ReadRemoteUInt32(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(uint));

			return BitConverter.ToUInt32(data, 0);
		}

		public static long ReadRemoteInt64(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(long));

			return BitConverter.ToInt64(data, 0);
		}

		public static ulong ReadRemoteUInt64(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(ulong));

			return BitConverter.ToUInt64(data, 0);
		}

		public static float ReadRemoteFloat(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(float));

			return BitConverter.ToSingle(data, 0);
		}

		public static double ReadRemoteDouble(this IRemoteMemoryReader reader, IntPtr address)
		{
			var data = reader.ReadRemoteMemory(address, sizeof(double));

			return BitConverter.ToDouble(data, 0);
		}

		public static IntPtr ReadRemoteIntPtr(this IRemoteMemoryReader reader, IntPtr address)
		{
#if RECLASSNET64
			return (IntPtr)reader.ReadRemoteInt64(address);
#else
			return (IntPtr)reader.ReadRemoteInt32(address);
#endif
		}

		public static string ReadRemoteString(this IRemoteMemoryReader reader, IntPtr address, Encoding encoding, int length)
		{
			Contract.Requires(encoding != null);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			var data = reader.ReadRemoteMemory(address, length * encoding.GuessByteCountPerChar());

			try
			{
				var sb = new StringBuilder(encoding.GetString(data));
				for (var i = 0; i < sb.Length; ++i)
				{
					if (sb[i] == '\0')
					{
						sb.Length = i;
						break;
					}
					if (!sb[i].IsPrintable())
					{
						sb[i] = '.';
					}
				}
				return sb.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

		public static string ReadRemoteStringUntilFirstNullCharacter(this IRemoteMemoryReader reader, IntPtr address, Encoding encoding, int length)
		{
			Contract.Requires(encoding != null);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			var data = reader.ReadRemoteMemory(address, length * encoding.GuessByteCountPerChar());

			// TODO We should cache the pattern per encoding.
			var index = PatternScanner.FindPattern(BytePattern.From(new byte[encoding.GuessByteCountPerChar()]), data);
			if (index == -1)
			{
				index = data.Length;
			}

			try
			{
				return encoding.GetString(data, 0, Math.Min(index, data.Length));
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
