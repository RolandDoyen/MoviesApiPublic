using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Movies.API.Middleware;
using Movies.API.Models;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Movies.Tests.Integration.Middleware
{
    public class ExceptionMiddlewareTests : IClassFixture<MiddlewareTestFactory>
    {
        private readonly HttpClient _client;

        public ExceptionMiddlewareTests(MiddlewareTestFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Middleware_NoException_Returns200()
        {
            var response = await _client.GetAsync("/ok");

            Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Everything is fine", content);
        }

        [Fact]
        public async Task Middleware_WhenMovieNotFoundException_Returns404()
        {
            var response = await _client.GetAsync("/throw-notfound");

            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.NotNull(error);
            Assert.Equal("Movie not found", error.Message);
            Assert.Equal(404, error.StatusCode);
        }

        [Fact]
        public async Task Middleware_WhenMovieAlreadyExistsException_Returns409()
        {
            var response = await _client.GetAsync("/throw-alreadyexists");

            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.NotNull(error);
            Assert.Equal("Movie already exists", error.Message);
            Assert.Equal(409, error.StatusCode);
        }

        [Fact]
        public async Task Middleware_WhenGenericException_Returns500()
        {
            var response = await _client.GetAsync("/throw-generic");

            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.NotNull(error);
            Assert.Equal("An unexpected error occurred on the server.", error.Message);
            Assert.Equal(500, error.StatusCode);
        }

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