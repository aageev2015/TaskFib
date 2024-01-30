using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Numerics;
using TaskFib.Service.Contract;
using TaskFib.WebApi.Cache;
using TaskFib.WebApi.Utilities;

namespace TaskFib.WebApi.Tests.Fixtures
{
    [TestFixture]
    public class SequenceValueCachedSimpleProxyServiceTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ISequenceValueServiceAsync<BigInteger> _sourceService;
        private MemoryCache _memoryCache;
        private SequenceValueCachedSimpleProxyService<BigInteger> _testingCachedProxy;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        [SetUp]
        public void SetUp()
        {
            _sourceService = Substitute.For<ISequenceValueServiceAsync<BigInteger>>();
            _sourceService.Get(5, Arg.Any<CancellationToken>()).Returns((BigInteger)999);

            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            _testingCachedProxy = new SequenceValueCachedSimpleProxyService<BigInteger>(
                _sourceService,
                _memoryCache,
                Options.Create(new TaskFibSettings() { SequenceValueCacheExpirationSeconds = 1 })
            );

        }

        [Test]
        public async Task When_SameIndexGet_Then_CachedValueMustBeRequested()
        {
            var result1 = await _testingCachedProxy.Get(5);
            await _sourceService.Received().Get(5, Arg.Any<CancellationToken>());
            _sourceService.ClearReceivedCalls();
            var result2 = await _testingCachedProxy.Get(5);
            await _sourceService.DidNotReceive().Get(Arg.Any<int>(), Arg.Any<CancellationToken>());

            Assert.That(result1, Is.EqualTo((BigInteger)999));
            Assert.That(result2, Is.EqualTo((BigInteger)999));
        }

        [Test]
        public async Task When_CacheTimeElapsed_Then_CachedValueMustBeRequested()
        {
            var result1 = await _testingCachedProxy.Get(5);
            await _sourceService.Received().Get(5, Arg.Any<CancellationToken>());
            _sourceService.ClearReceivedCalls();
            await Task.Delay(1100);
            var result2 = await _testingCachedProxy.Get(5);
            await _sourceService.Received().Get(5, Arg.Any<CancellationToken>());

            Assert.That(result1, Is.EqualTo((BigInteger)999));
            Assert.That(result2, Is.EqualTo((BigInteger)999));
        }

    }
}
