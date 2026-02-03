namespace Movies.API.Models
{
    /// <summary>
    /// Represents a standardized error structure returned by the API when an operation fails.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// The HTTP status code associated with the error (e.g., 404, 409, 500).
        /// </summary>
        /// <example>409</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// A human-readable message describing the error.
        /// </summary>
        /// <example>A movie with the same title and year already exists.</example>
        public string Message { get; set; }

        /// <summary>
        /// The date and time (UTC) when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="message">The detailed error message.</param>
        public ErrorResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
