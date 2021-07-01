using System;

namespace SmScanner.Core.Exceptions
{
    internal class InvalidInputException : Exception
    {
        public InvalidInputException(string input)
            : base($"'{input}' is not a valid input.")
        {

        }
    }
}
