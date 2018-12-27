using System;

namespace NullFight.Exceptions
{
    public class FriendlyResultException : ResultException
    {
        public FriendlyResultException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}