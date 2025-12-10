namespace Movies.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a movie that already exists in the database.
    /// </summary>
    public class MovieAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieAlreadyExistsException"/> class.
        /// </summary>
        public MovieAlreadyExistsException()
            : base() { }
    }
}