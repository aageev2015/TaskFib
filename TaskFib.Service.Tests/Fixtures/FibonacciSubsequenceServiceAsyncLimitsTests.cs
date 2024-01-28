using System.Runtime;
using System.Runtime.CompilerServices;
using TaskFib.Service.Contract;

namespace TaskFib.Service.Tests.Fixtures
{
    // As correct Fibonacci values taken from
    // https://r-knott.surrey.ac.uk/Fibonacci/fibtable.html

    [SingleThreaded]
    [TestFixture(Description = "Async timing tests. False negative possible")]
    public class FibonacciSubsequenceServiceAsyncLimitsTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private DelayWorkload _iterationsWorkload;
        private FibonacciServiceAsync _service;
        private SubsequenceServiceAsync _rangeService;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        private class DelayWorkload(int delayMs = 0) : IIterationsWorkloadAsync
        {
            public int DelayMs { get; set; } = delayMs;

            public async Task RunWorkload(CancellationToken ct)
            {
                try
                {
                    await Task.Delay(DelayMs, ct);
                }
                catch
                {
                }
            }
        }

        [SetUp]
        public void SetUp()
        {
            _iterationsWorkload = new DelayWorkload();
            _service = new FibonacciServiceAsync(_iterationsWorkload);
            _rangeService = new SubsequenceServiceAsync(_service);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [Test]
        public async Task When_HitMemoryLimit_Then_ReturnPartialFibonacciValue()
        {
            GCLatencyMode oldMode = GCSettings.LatencyMode;
            try
            {
                GCSettings.LatencyMode = GCLatencyMode.LowLatency;

                _iterationsWorkload.DelayMs = 0;
                var memoryFrom = GC.GetTotalMemory(true);
                await _rangeService.GetSubsequence(1000, 1000, 2000, long.MaxValue);
                var memoryTo = GC.GetTotalMemory(false);
                var singleItemMemoryAllocated = memoryTo - memoryFrom;
                memoryFrom = GC.GetTotalMemory(true);
                memoryTo = memoryFrom + singleItemMemoryAllocated * 3 / 2;

                var result1 = await _rangeService.GetSubsequence(999, 1000, 2000, memoryTo);
                var result2 = await _rangeService.GetSubsequence(999, 1000, 2000, long.MaxValue);

                Assert.That(result1.Count, Is.EqualTo(1));
                Assert.That(result2.Count, Is.EqualTo(2));
            }
            finally
            {
                GCSettings.LatencyMode = oldMode;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [Test]
        public async Task When_HitTimeLimit_Then_ReturnPartialFibonacciValue()
        {
            GCLatencyMode oldMode = GCSettings.LatencyMode;
            try
            {
                GCSettings.LatencyMode = GCLatencyMode.LowLatency;

                GC.Collect();
                _iterationsWorkload.DelayMs = 100;

                var result1 = await _rangeService.GetSubsequence(5, 50, 400, long.MaxValue);
                var result2 = await _rangeService.GetSubsequence(5, 50, 190, long.MaxValue);
                var result3 = await _rangeService.GetSubsequence(5, 50, 20, long.MaxValue);

                Assert.That(result1.Count, Is.EqualTo(3));
                Assert.That(result2.Count, Is.EqualTo(1));
                Assert.That(result3.Count, Is.EqualTo(0));
            }
            finally
            {
                GCSettings.LatencyMode = oldMode;
            }
        }

    }


}
