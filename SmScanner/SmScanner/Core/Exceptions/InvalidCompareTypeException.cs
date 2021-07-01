using SmScanner.Core.Enums;
using System;

namespace SmScanner.Core.Exceptions
{
	public class InvalidCompareTypeException : Exception
	{
		public InvalidCompareTypeException(ScanCompareType type)
			: base($"{type} is not valid in the current state.")
		{

		}
	}
}
