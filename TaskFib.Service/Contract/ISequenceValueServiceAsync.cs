namespace TaskFib.Service.Contract
{
    public interface ISequenceValueServiceAsync<T> where T : struct
    {
        Task<T> Get(int index, CancellationToken ct);
    }
}
