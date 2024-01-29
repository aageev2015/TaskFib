namespace TaskFib.WebApi.Cache
{
    [Serializable]
    internal class SequenceCacheItemAlreadySetException : Exception
    {
        public SequenceCacheItemAlreadySetException()
        {
        }

        public SequenceCacheItemAlreadySetException(string? message) : base(message)
        {
        }

        public SequenceCacheItemAlreadySetException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}