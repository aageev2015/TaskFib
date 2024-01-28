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
        #region DI

        private readonly ISubsequenceServiceAsync<BigInteger> _subsequenceService;

        public FibonacciController(ISubsequenceServiceAsync<BigInteger> subsequenceService)
        {
            _subsequenceService = subsequenceService;
        }

        #endregion DI

        [HttpGet(@"{fromIndex}/{toIndex}")]
        public async Task<IActionResult> Get(
            int fromIndex,
            int toIndex,
            [FromQuery] int timeLimitMs = 10000,
            [FromQuery] long memLimitBytes = -1)
        {
            var memLimitBytesNormalized = (memLimitBytes < 0) ? long.MaxValue : memLimitBytes;

            var serviceResult = await _subsequenceService.GetSubsequence(
                fromIndex, toIndex,
                timeLimitMs, memLimitBytesNormalized);

            if (serviceResult.Count == 0)
            {
                throw new SingleValueTimeoutException();
            }

            var result = serviceResult.ToFibonacciResponseDTO(toIndex - fromIndex + 1);

            return new JsonResult(result);
        }
    }
}
