using Movies.API.Models;
using Movies.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace Movies.API.Middleware
{
    /// <summary>
    /// Global middleware for handling exceptions across the entire API pipeline.
    /// It catches unhandled exceptions, logs them, and returns a standardized JSON error response.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate in the request pipeline.</param>
        /// <param name="logger">The logger instance for recording errors.</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to process the current HTTP context.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the caught exception by logging the details and writing a 
        /// <see cref="ErrorResponse"/> to the HTTP response stream.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="exception">The exception that was thrown.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;

            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            switch (exception)
            {
                case MovieAlreadyExistsException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = exception.Message;
                    break;

                case MovieNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred on the server.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            var response = new ErrorResponse(statusCode, message);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
