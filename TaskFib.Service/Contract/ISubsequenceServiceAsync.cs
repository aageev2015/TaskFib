namespace TaskFib.Service.Contract
{
    public interface ISubsequenceServiceAsync<T> where T : struct
    {
        Task<List<T>> GetSubsequence(int fromIndex, int toIndex, int timeLimitMs, long memLimitBytes);
    }
}
