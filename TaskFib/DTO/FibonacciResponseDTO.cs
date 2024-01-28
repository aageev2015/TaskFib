namespace TaskFib.WebApi.DTO
{
    public class FibonacciResponseDTO
    {
        public IEnumerable<string>? Values { get; init; }
        public bool IsTimeout { get; init; }
    }
}
