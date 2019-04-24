using System;
using System.Collections.Generic;
using System.Linq;

namespace NullFight.Exceptions
{
    public class ResultException : Exception
    {
        public ResultException(string message, Exception innerException = null) : base(message, innerException) { }
        public static Exception FindFirstNonResultException(Exception exception)
        {
            if (exception?.InnerException != null && (exception is FriendlyResultException || exception is ResultException))
                return FindFirstNonResultException(exception.InnerException);

            if (exception is FriendlyResultException || exception is ResultException)
                return null;
            return exception;
        }

        public static FriendlyResultException[] GatherFriendlyExceptions(Exception exception)
        {
            var friendlyExceptions = new List<FriendlyResultException>();
            if (exception is FriendlyResultException resultException)
                friendlyExceptions.Add(resultException);
            if (exception.InnerException != null)
                friendlyExceptions.AddRange(GatherFriendlyExceptions(exception.InnerException));
            return friendlyExceptions.ToArray();
        }
        public static string GetConcatenatedFriendlyMessage(Exception exception)
        {
            return GatherFriendlyExceptions(exception).Aggregate(string.Empty, (c, x) => string.IsNullOrWhiteSpace(c) ? $"'{x.Message}'" : $"{c} -> '{x.Message}'");
        }

        public static Exception RemoveUnnecessaryResultExceptions(Exception exception)
        {
            if (exception?.InnerException == null)
                return exception;
            switch (exception)
            {
                case FriendlyResultException _:
                    return new FriendlyResultException(exception.Message, FindFirstNonResultException(exception));
                case ResultException _:
                    return new ResultException(exception.Message, FindFirstNonResultException(exception));
                default:
                    return exception;
            }
        }
    }
}