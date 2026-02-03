using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Movies.API.Middleware;
using Xunit;

namespace Movies.Tests.Unit.Middleware
{
    public class ExceptionMiddlewareUnitTests
    {

        [Fact]
        public async Task InvokeAsync_WhenExceptionOccurs_LogsErrorMessage()
        {
            var mockNext = new Mock<RequestDelegate>();
            var exception = new Exception("Real technical error!");
            mockNext.Setup(next => next(It.IsAny<HttpContext>())).Throws(exception);
            var mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
            var middleware = new ExceptionMiddleware(mockNext.Object, mockLogger.Object);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.InvokeAsync(context);

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((y, z) => y.ToString().Contains("An unhandled exception occurred")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
