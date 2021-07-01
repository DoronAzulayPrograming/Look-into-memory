using System;

namespace SmScanner.Core.Exceptions
{
    public class DriverException : Exception
    {
        public DriverException(string input)
            : base($"Smdkd driver '{input}'.")
        {

        }
    }
}
