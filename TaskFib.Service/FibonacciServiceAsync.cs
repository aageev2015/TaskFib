using System.Numerics;
using TaskFib.Service.Contract;

namespace TaskFib.Service
{
    public sealed class FibonacciServiceAsync : IFibonacciServiceAsync
    {
        public async Task<List<BigInteger>> GetSequence(int fromIndex, int toIndex, int timeLimitMs, int memLimitBytes)
        {
            if (fromIndex > toIndex)
            {
                throw new ArgumentException();
            }

            if (fromIndex <= 0 || toIndex <= 0)
            {
                throw new IndexOutOfRangeException();
            }

            await Task.Delay(0);
            throw new NotImplementedException();
        }
    }
}
