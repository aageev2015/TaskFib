using NSubstitute;
using System.Numerics;
using TaskFib.Service.Contract;

namespace TaskFib.Service.Tests.Fixtures
{
    // As correct Fibonacci values taken from
    // https://r-knott.surrey.ac.uk/Fibonacci/fibtable.html

    [TestFixture]
    public class FibonacciServiceAsyncLogicTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IIterationsWorkloadAsync _iterationsWorkload;
        private CancellationToken _cancelTokenMock;
        private FibonacciServiceAsync _service;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        [SetUp]
        public void SetUp()
        {
            _iterationsWorkload = Substitute.For<IIterationsWorkloadAsync>();
            _cancelTokenMock = new CancellationToken();
            _service = new FibonacciServiceAsync(_iterationsWorkload);
        }

        [Test]
        public async Task When_IndexEqZero_Then_ReturnZeroValue()
        {
            Assert.That(await _service.Get(0, _cancelTokenMock), Is.EqualTo((BigInteger)0));
        }

        [Test]
        public async Task When_StartingIndex_Then_ReturnCorrectFibonacciValue()
        {
            Assert.That(await _service.Get(0, _cancelTokenMock), Is.EqualTo((BigInteger)0));
            Assert.That(await _service.Get(1, _cancelTokenMock), Is.EqualTo((BigInteger)1));
            Assert.That(await _service.Get(2, _cancelTokenMock), Is.EqualTo((BigInteger)1));
            Assert.That(await _service.Get(6, _cancelTokenMock), Is.EqualTo((BigInteger)8));
        }

        [Test]
        public async Task When_MiddleIndex_Then_ReturnCorrectFibonacciValue()
        {
            Assert.That(await _service.Get(17, _cancelTokenMock), Is.EqualTo((BigInteger)1597));
            Assert.That(await _service.Get(46, _cancelTokenMock), Is.EqualTo((BigInteger)1836311903));
        }

        [Test]
        public async Task When_LargeIndex_Then_ReturnCorrectFibonacciValue()
        {
            Assert.That(await _service.Get(49, _cancelTokenMock),
                Is.EqualTo((BigInteger)13 * 97 * 6168709));
            Assert.That(await _service.Get(99, _cancelTokenMock),
                Is.EqualTo((BigInteger)2 * 17 * 89 * 197 * 19801 * 18546805133));
        }

        [Test]
        public void When_IndexLessZero_Then_IndexOutOfRangeException()
        {
            Assert.CatchAsync<IndexOutOfRangeException>(async () => await _service.Get(-1, _cancelTokenMock));
            Assert.CatchAsync<IndexOutOfRangeException>(async () => await _service.Get(-100, _cancelTokenMock));
        }
    }
}
