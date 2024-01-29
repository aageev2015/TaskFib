namespace TaskFib.WebApi.Cache
{
    public class SequenceCacheItem<T> where T : struct
    {
        private uint _calculationInProcess = 0;
        public bool IsNewOneTimeRead { get => 0 == Interlocked.CompareExchange(ref _calculationInProcess, 1, 0); }

        private T? _value;
        public T? Value
        {
            get => _value;
            set
            {
                if (_value != null)
                {
                    throw new SequenceCacheItemAlreadySetException();
                }
                _value = value;
            }
        }

    }
}
