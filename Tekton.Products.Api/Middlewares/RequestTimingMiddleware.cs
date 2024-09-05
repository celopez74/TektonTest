using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Tekton.Products.Infrastructure.Services;

namespace Tekton.Products.Api.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();
            var responseTime = stopwatch.ElapsedMilliseconds;

            var message = $"Request {context.Request.Method} {context.Request.Path} took {responseTime} ms";
            _logger.Log(message);
        }
    }
}