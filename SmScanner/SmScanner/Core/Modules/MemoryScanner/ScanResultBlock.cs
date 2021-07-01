using System;
using SmScanner.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SmScanner.Core.Modules.MemoryScanner
{
	internal class ScanResultBlock
	{
		public IntPtr Start { get; }
		public IntPtr End { get; }
		public int Size => End.Sub(Start).ToInt32();
		public IReadOnlyList<ScanResult> Results { get; }

		public ScanResultBlock(IntPtr start, IntPtr end, IReadOnlyList<ScanResult> results)
		{
			Contract.Requires(results != null);

			Start = start;
			End = end;
			Results = results;
		}
	}
}
