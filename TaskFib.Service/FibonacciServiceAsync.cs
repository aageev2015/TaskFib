using System.Numerics;
using TaskFib.Service.Contract;

namespace TaskFib.Service
{
    public class FibonacciServiceAsync(IIterationsWorkloadAsync iterationsWorkload) : ISequenceValueServiceAsync<BigInteger>
    {
        private readonly IIterationsWorkloadAsync _iterationsWorkload = iterationsWorkload;

        public async Task<BigInteger> Get(int index, CancellationToken ct)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            var result = GenerateFibonacci(index);

            await _iterationsWorkload.RunWorkload(ct);

            return result;
        }

        private BigInteger GenerateFibonacci(int index)
        {
            if (index >= 0 && index <= 1)
            {
                return index;
            }

            BigInteger val0 = 1;
            BigInteger val1 = 1;
            BigInteger tmp;

            for (var i = 2; i < index; i++)
            {
                tmp = val1;
                val1 = val0 + val1;
                val0 = tmp;
            }

            return val1;
        }
    }
}
