using System.Numerics;
using TaskFib.Service.Contract;

namespace TaskFib.Service
{
    public sealed class FibonacciServiceAsync(IIterationsWorkloadAsync iterationsWorkload) : IFibonacciServiceAsync
    {
        private readonly IIterationsWorkloadAsync _iterationsWorkload = iterationsWorkload;

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


            var result = new List<BigInteger>(toIndex - fromIndex + 1);
            await foreach (var value in genFibonacci(fromIndex, toIndex))
            {
                result.Add(value);
            }
            return result;
        }

        private async IAsyncEnumerable<BigInteger> genFibonacci(int fromIndex, int toIndex)
        {
            BigInteger val0 = 0;
            BigInteger val1 = 1;
            BigInteger tmp;

            for (var i = 1; i < fromIndex; i++)
            {
                tmp = val1;
                val1 = val0 + val1;
                val0 = tmp;

                await _iterationsWorkload.RunWorkload();
            }

            yield return val1;
            await _iterationsWorkload.RunWorkload();

            for (var i = fromIndex; i < toIndex; i++)
            {
                tmp = val1;
                val1 = val0 + val1;
                val0 = tmp;

                yield return val1;
                await _iterationsWorkload.RunWorkload();
            }
        }
    }
}
