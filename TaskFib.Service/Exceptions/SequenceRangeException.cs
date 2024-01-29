namespace TaskFib.Service.Exceptions
{
    [Serializable]
    public class SequenceRangeException : System.Exception
    {

        public int FromIndex { get; init; }
        public int ToIndex { get; init; }

        public SequenceRangeException()
        {
        }

        public SequenceRangeException(string? message) : base(message)
        {
        }

        public SequenceRangeException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        public SequenceRangeException(int fromIndex, int toIndex, string? message = null) : this(message)
        {
            FromIndex = fromIndex;
            ToIndex = toIndex;
        }
    }
}