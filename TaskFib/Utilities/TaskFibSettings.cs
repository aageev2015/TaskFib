namespace TaskFib.WebApi.Utilities
{
    public class TaskFibSettings
    {
        public const string AppSettingGroupName = "TaskFib";

        public int SleepWorkloadDelayMS { get; set; } = 0;
        public int SequenceValueCacheExpirationSeconds { get; set; } = 0;
    }
}
