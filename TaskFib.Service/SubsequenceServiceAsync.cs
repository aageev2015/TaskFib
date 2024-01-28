using TaskFib.Service.Contract;

namespace TaskFib.Service
{
    public class SubsequenceServiceAsync<T>(ISequenceValueServiceAsync<T> valueService) :
        ISubsequenceServiceAsync<T> where T : struct
    {
        private readonly ISequenceValueServiceAsync<T> _valueService = valueService;

        public async Task<List<T>> GetSubsequence(int fromIndex, int toIndex, int timeLimitMs, long memLimitBytes)
        {
            if (fromIndex > toIndex)
            {
                throw new ArgumentException();
            }

            if (fromIndex < 0 || toIndex < 0)
            {
                throw new IndexOutOfRangeException();
            }

            using var cancelSource = new CancellationTokenSource(timeLimitMs);
            var cancelToken = cancelSource.Token;

            var result = new List<T>(toIndex - fromIndex + 1);
            for (var index = fromIndex; index <= toIndex; index++)
            {
                var value = await _valueService.Get(index, cancelToken);
                if (cancelToken.IsCancellationRequested)
                {
                    break;
                }
                if (GC.GetTotalMemory(false) > memLimitBytes)
                {
                    break;
                }
                result.Add(value);
            }

            return result;
        }
    }
}
