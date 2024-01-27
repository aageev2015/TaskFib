using NSubstitute;
using System.Numerics;
using TaskFib.Service.Contract;

namespace TaskFib.Service.Tests.Fixtures
{
    // As correct Fibonacci values taken from
    // https://r-knott.surrey.ac.uk/Fibonacci/fibtable.html

    [TestFixture]
    public class FibonacciServiceAsyncTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private FibonacciServiceAsync _service;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        [SetUp]
        public void SetUp()
        {
            var iterationsWorkload = Substitute.For<IIterationsWorkloadAsync>();
            _service = new FibonacciServiceAsync(iterationsWorkload);
        }

        [Test]
        public async Task When_RangeSingleStartingIndex_Then_ReturnCorrectSingleFibonacciValue()
        {
            Assert.That(
                await _service.GetSequence(1, 1, 2000, int.MaxValue),
                Is.EquivalentTo((List<BigInteger>)[1]));

            Assert.That(
                await _service.GetSequence(2, 2, 2000, int.MaxValue),
                Is.EquivalentTo((List<BigInteger>)[1]));

            Assert.That(
                await _service.GetSequence(3, 3, 2000, int.MaxValue),
                Is.EquivalentTo((List<BigInteger>)[2]));

            Assert.That(
                await _service.GetSequence(6, 6, 2000, int.MaxValue),
                Is.EquivalentTo((List<BigInteger>)[8]));
        }

        [Test]
        public async Task When_RangeSingleMidIndex_Then_ReturnCorrectSingleFibonacciValue()
        {
            Assert.That(
                await _service.GetSequence(17, 17, 2000, int.MaxValue),
                Is.EquivalentTo((List<BigInteger>)[1597]));

            Assert.That(
                await _service.GetSequence(46, 46, 2000, int.MaxValue),
                Is.EquivalentTo((List<BigInteger>)[1836311903]));
        }

        [Test]
        public async Task When_RangeSingleLargeIndex_Then_ReturnCorrectSingleFibonacciValue()
        {
            List<BigInteger> a = [1, 2, 3];
            Assert.That(
                await _service.GetSequence(49, 49, 2000, int.MaxValue),
                Is.EquivalentTo((BigInteger[])[(BigInteger)13 * 97 * 6168709]));

            Assert.That(
                await _service.GetSequence(99, 99, 2000, int.MaxValue),
                Is.EquivalentTo((BigInteger[])[(BigInteger)2 * 17 * 89 * 197 * 19801 * 18546805133]));
        }

        [Test]
        public async Task When_LowRangeProvided_Then_ReturnCorrectFibonacciValues()
        {
            Assert.That(
               await _service.GetSequence(1, 2, 2000, int.MaxValue),
               Is.EquivalentTo((List<BigInteger>)[1, 1]));


            Assert.That(
               await _service.GetSequence(2, 6, 2000, int.MaxValue),
               Is.EquivalentTo((List<BigInteger>)[1, 2, 3, 5, 8]));
        }

        [Test]
        public async Task When_MidRangeProvided_Then_ReturnCorrectFibonacciValues()
        {
            Assert.That(
               await _service.GetSequence(40, 46, 2000, int.MaxValue),
               Is.EquivalentTo((List<BigInteger>)[102334155, 165580141, 267914296, 433494437, 701408733, 1134903170, 1836311903]));
        }

        [Test]
        public async Task When_LargeRangeProvided_Then_ReturnCorrectFibonacciValues()
        {
            Assert.That(
               await _service.GetSequence(96, 99, 2000, int.MaxValue),
               Is.EquivalentTo((BigInteger[])[
                   (BigInteger)128 * 9 * 7 * 23 * 47 * 769 * 1103 * 2207 * 3167,
                   (BigInteger)193 * 389 * 3084989 * 361040209,
                   (BigInteger)13 * 29 * 97 * 6168709 * 599786069,
                   (BigInteger)2 * 17 * 89 * 197 * 19801 * 18546805133
                ]));
        }

        [Test]
        public void When_RangeLessOrEqZero_Then_IndexOutOfRangeException()
        {
            Assert.CatchAsync<IndexOutOfRangeException>(async () => await _service.GetSequence(0, 1, 2000, int.MaxValue));
            Assert.CatchAsync<IndexOutOfRangeException>(async () => await _service.GetSequence(-1, 0, 2000, int.MaxValue));
            Assert.CatchAsync<IndexOutOfRangeException>(async () => await _service.GetSequence(-5, -4, 2000, int.MaxValue));
        }

        [Test]
        public void When_FromGraterTo_Then_ArgumentException()
        {
            Assert.CatchAsync<ArgumentException>(async () => await _service.GetSequence(5, 4, 2000, int.MaxValue));
            Assert.CatchAsync<ArgumentException>(async () => await _service.GetSequence(1, 0, 2000, int.MaxValue));
            Assert.CatchAsync<ArgumentException>(async () => await _service.GetSequence(-4, -5, 2000, int.MaxValue));
        }


    }
}
