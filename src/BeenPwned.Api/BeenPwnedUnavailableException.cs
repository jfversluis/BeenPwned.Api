using System;

namespace BeenPwned.Api
{
    public class BeenPwnedUnavailableException : Exception
    {
        public BeenPwnedUnavailableException(string message)
            : base(message)
        { }
    }
}