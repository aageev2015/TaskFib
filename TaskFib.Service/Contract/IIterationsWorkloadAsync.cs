﻿namespace TaskFib.Service.Contract
{
    public interface IIterationsWorkloadAsync
    {
        Task RunWorkload(CancellationToken ct = default);
    }
}
