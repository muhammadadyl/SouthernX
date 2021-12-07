using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace SouthernCross.WebApi.Filters
{
    public class UnhandledExceptionFilter : IAsyncExceptionFilter
    {
        private const string UnhandledError = "urn:sx:api:unhandled-error";

        private readonly ILogger<UnhandledExceptionFilter> _logger;

        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception");

            const int defaultStatusCode = StatusCodes.Status500InternalServerError;

            context.Result = new ContentResult
            {
                Content = JsonSerializer.Serialize(new ProblemDetails { Detail = UnhandledError, Status = defaultStatusCode }),
                StatusCode = defaultStatusCode,
                ContentType = "application/json+problem"
            };

            return Task.CompletedTask;
        }
    }
}
