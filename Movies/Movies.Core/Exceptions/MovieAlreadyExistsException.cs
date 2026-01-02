namespace Movies.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a movie with the same unique identifiers (e.g., Title and Year) 
    /// already exists in the persistent storage.
    /// </summary>
    public class MovieAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieAlreadyExistsException"/> class with a default error message.
        /// </summary>
        public MovieAlreadyExistsException()
            : base("A movie with the same title and year already exists.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieAlreadyExistsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error, including specific movie details.</param>
        public MovieAlreadyExistsException(string message)
            : base(message) { }
    }
}