using System;

namespace NullFight.Exceptions
{
    public class ResultException : Exception
    {
        public ResultException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}