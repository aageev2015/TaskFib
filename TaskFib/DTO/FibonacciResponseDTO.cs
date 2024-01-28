using System.Numerics;

namespace TaskFib.WebApi.DTO
{
    public class FibonacciResponseDTO
    {
        public IEnumerable<string>? Values { get; init; }
        public bool IsTimeoutReached { get; init; }
    }

    public static class FibonacciResponseDTOExtension
    {
        public static FibonacciResponseDTO ToFibonacciResponseDTO(this List<BigInteger> values, int expectedCount) =>
            new FibonacciResponseDTO()
            {
                Values = values.Select(value => value.ToString()),
                IsTimeoutReached = values.Count < expectedCount
            };
    }
}
