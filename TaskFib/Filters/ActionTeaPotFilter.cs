using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskFib.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TeaPotActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Query.Keys.Contains("makecofee"))
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status418ImATeapot,
                    Content = "I can't make coffee, sorry. I'm teapot"
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
