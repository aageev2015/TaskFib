using System.Numerics;

namespace TaskFib.Service.Contract
{
    public interface IFibonacciServiceAsync
    {
        Task<List<BigInteger>> GetSequence(int fromIndex, int toIndex, int timeLimitMs, int memLimitBytes);
    }
}
