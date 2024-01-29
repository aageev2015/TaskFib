namespace TaskFib.Service.Exceptions
{
    [Serializable]
    public class SequenceLimitValueException : System.Exception
    {
        public string? LimitName { get; init; }

        public SequenceLimitValueException()
        {
        }

        public SequenceLimitValueException(string? message) : base(message)
        {
        }

        public SequenceLimitValueException(string? message, ArgumentException? innerException) : base(message, innerException)
        {
        }

        public SequenceLimitValueException(string? message, string limitName) : this(message)
        {
            LimitName = limitName;
        }

    }
}