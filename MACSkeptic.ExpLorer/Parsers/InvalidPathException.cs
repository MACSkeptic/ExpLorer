using System;

namespace MACSkeptic.ExpLorer.Parsers
{
    public class InvalidPathException : Exception
    {
        public InvalidPathException(string message)
            : base (message)
        {
        }
    }
}