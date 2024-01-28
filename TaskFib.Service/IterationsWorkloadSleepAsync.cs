using TaskFib.Service.Contract;

namespace TaskFib.Service
{
    public class IterationsWorkloadSleepAsync(int delay) : IIterationsWorkloadAsync
    {
        private int _delay = delay;

        public async Task RunWorkload(CancellationToken ct)
        {
            try
            {
                await Task.Delay(_delay, ct);
            }
            catch
            {
            }
        }
    }
}
