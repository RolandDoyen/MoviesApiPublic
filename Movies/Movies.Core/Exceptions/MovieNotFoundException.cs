namespace Movies.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested movie is not found in the database.
    /// </summary>
    public class MovieNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class with a default message.
        /// </summary>
        public MovieNotFoundException() : base("The movie was not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MovieNotFoundException(string message) : base(message) { }
    }
}