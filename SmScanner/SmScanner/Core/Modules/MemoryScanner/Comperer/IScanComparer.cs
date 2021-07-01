using SmScanner.Core.Enums;

namespace SmScanner.Core.Modules.MemoryScanner.Comperer
{
	public interface IScanComparer
	{
		ScanCompareType CompareType { get; }
	}
}
