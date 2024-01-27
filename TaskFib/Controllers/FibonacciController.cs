using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace TaskFib.WebApi.Controllers
{
    [ApiController]
    [Route("api/fibonacci")]
    public class FibonacciController : ControllerBase
    {
        private readonly ILogger<FibonacciController> _logger;

        public FibonacciController(ILogger<FibonacciController> logger)
        {
            _logger = logger;
        }

        [HttpGet(@"{fromIndex}/{toIndex}")]
        public IActionResult Get(
            int fromIndex,
            int toIndex,
            [FromQuery] int timeLimitMs = 10000,
            [FromQuery] int memLimitBytes = 1048576)
        {
            var data = ((BigInteger[])[int.MinValue, 1, fromIndex, toIndex, int.MaxValue, ((BigInteger)int.MaxValue) * int.MaxValue])
                .Select(val => val.ToString());
            return new JsonResult(data);
        }
    }
}
