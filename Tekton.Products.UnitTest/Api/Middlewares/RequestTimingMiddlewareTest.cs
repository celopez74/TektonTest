using Microsoft.AspNetCore.Http;
using Moq;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Tekton.Products.Api.Middlewares;
using Tekton.Products.Infrastructure.Services;

public class RequestTimingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_LogsRequestTiming()
    {
        // Arrange
        var loggerMock = new Mock<ILoggerService>();
        var context = new DefaultHttpContext();
        var middleware = new RequestTimingMiddleware(async (innerHttpContext) =>
        {
            // Simulate some processing delay
            await Task.Delay(50);
            await innerHttpContext.Response.WriteAsync("Hello World");
        }, loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        loggerMock.Verify(log => log.Log(It.Is<string>(msg => msg.Contains("Request") && msg.Contains("took"))), Times.Once);
    }
}
