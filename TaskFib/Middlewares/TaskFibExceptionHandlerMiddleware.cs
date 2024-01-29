using TaskFib.Service.Exceptions;
using TaskFib.WebApi.Exceptions;

namespace TaskFib.WebApi.Middlewares
{
    public class TaskFibExceptionHandlerMiddleware(RequestDelegate next, ILogger<TaskFibExceptionHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<TaskFibExceptionHandlerMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SingleValueTimeoutException ex)
            {
                _logger.LogError(ex, "Single value timeout");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    Status = StatusCodes.Status408RequestTimeout,
                    ErrorMessage = "Not enough time to prepare single value"
                });
            }
            catch (SequenceLimitValueException ex)
            {
                _logger.LogError(ex, $"Sequence limit {ex.LimitName} not valid");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    Status = context.Response.StatusCode,
                    ErrorMessage = $"Limit {ex.LimitName} not valid"
                });
            }
            catch (SequenceRangeException ex)
            {
                _logger.LogError(ex, "Sequence index range not valid");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    Status = context.Response.StatusCode,
                    ErrorMessage = "Index range not valid"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Undefined exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    Status = context.Response.StatusCode,
                    ErrorMessage = "Something wrong"
                });
            }
        }
    }
}
