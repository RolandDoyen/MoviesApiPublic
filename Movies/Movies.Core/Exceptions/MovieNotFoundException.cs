namespace Movies.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested movie is not found in the database.
    /// </summary>
    public class MovieNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class.
        /// </summary>
        public MovieNotFoundException()
            : base() { }
    }
}