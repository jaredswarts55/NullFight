using System.Runtime.Serialization;

namespace NullFight
{
    /// <summary>
    /// This is a DataContract serializable version of a result object. The unknown type of 'Exception'
    /// makes this a tough cookie to crack when it comes to sending this over the wire using that type of
    /// serialization.
    /// The property containing the exception was very rarely a base exception.
    /// </summary>
    /// <typeparam name="T">Value Type</typeparam>
    [DataContract]
    public struct RemotingResult<T>
    {
        public RemotingResult(bool hasValue, T value, string errorMessage)
        {
            HasValue = hasValue;
            Value = value;
            ErrorMessage = errorMessage;
        }
        public RemotingResult(Result<T> result)
        {
            HasValue = result.HasValue;
            if (result.HasValue)
            {
                Value = result.Unwrap();
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.UnwrapException().Message;
                Value = default(T);
            }
        }
        [DataMember]
        public bool HasValue { get; private set; }
        [DataMember]
        public T Value { get; private set; }
        [DataMember]
        public string ErrorMessage { get; private set; }

        // Used to return an error without specifying the type
        public static implicit operator RemotingResult<T>(RemotingResult<object> myResult)
        {
            return new RemotingResult<T>(false, default(T), myResult.ErrorMessage);
        }
        public Result<T> ToResult()
        {
            return HasValue ? FunctionalExtensions.ResultValue(Value) : FunctionalExtensions.ResultError<T>(ErrorMessage);
        }


    }
}
