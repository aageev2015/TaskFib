using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using TaskFib.Service.Contract;
using TaskFib.WebApi.DTO;
using TaskFib.WebApi.Utilities;

namespace TaskFib.WebApi.Controllers
{
    [ApiController]
    [Route("api/fibonacci")]
    public class FibonacciController : ControllerBase
    {
        private readonly ILogger<FibonacciController> _logger;
        private readonly ISubsequenceServiceAsync<BigInteger> _subsequenceService;

        public FibonacciController(ILogger<FibonacciController> logger, ISubsequenceServiceAsync<BigInteger> subsequenceService)
        {
            _logger = logger;
            _subsequenceService = subsequenceService;
        }

        [HttpGet(@"{fromIndex}/{toIndex}")]
        public async Task<IActionResult> Get(
            int fromIndex,
            int toIndex,
            [FromQuery] int timeLimitMs = 10000,
            [FromQuery] long memLimitBytes = -1)
        {
            var memLimitBytesNormalized = (memLimitBytes < 0) ? long.MaxValue : memLimitBytes;

            var serviceResult = await _subsequenceService.GetSubsequence(fromIndex, toIndex, timeLimitMs, memLimitBytesNormalized);

            var count = serviceResult.Count;
            if (count == 0)
            {
                throw new SingleValueTimeoutException();
            }
            var expectedCount = toIndex - fromIndex + 1;

            var result = new FibonacciResponseDTO()
            {
                Values = serviceResult.Select(value => value.ToString()),
                IsTimeout = count < expectedCount
            };

            return new JsonResult(result);
        }
    }
}
