using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using TaskFib.Service.Contract;
using TaskFib.WebApi.DTO;
using TaskFib.WebApi.Exceptions;
using TaskFib.WebApi.Filters;
using TaskFib.WebApi.Utilities;

namespace TaskFib.WebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/fibonacci")]
    public class FibonacciController(
            ISubsequenceServiceAsync<BigInteger> subsequenceService,
            [FromKeyedServices(ServiceKeys.ValuesCache)] ISubsequenceServiceAsync<BigInteger> subsequenceCachedService
        ) : ControllerBase
    {
        #region DI

        private readonly ISubsequenceServiceAsync<BigInteger> _subsequenceService = subsequenceService;
        private readonly ISubsequenceServiceAsync<BigInteger> _subsequenceCachedService = subsequenceCachedService;

        #endregion DI

        [TeaPotActionFilter]
        [HttpGet(@"{fromIndex}/{toIndex}")]
        public async Task<FibonacciResponseDTO> Get(
            int fromIndex,
            int toIndex,
            [FromQuery] int timeLimitMs = 10000,
            [FromQuery] long memLimitBytes = -1,
            [FromQuery] bool useCache = false)
        {
            var service = useCache ? _subsequenceCachedService : _subsequenceService;

            var serviceResult = await service.GetSubsequence(
                fromIndex, toIndex,
                timeLimitMs, memLimitBytes);

            if (serviceResult.Count == 0)
            {
                throw new SingleValueTimeoutException();
            }

            var result = serviceResult.ToFibonacciResponseDTO(toIndex - fromIndex + 1);

            return result;
        }
    }
}
