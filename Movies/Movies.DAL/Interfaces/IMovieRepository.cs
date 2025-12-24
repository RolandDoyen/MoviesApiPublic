using Movies.DAL.DAO;

namespace Movies.DAL.Interfaces
{
    /// <summary>
    /// Interface for movie data access operations.
    /// </summary>
    public interface IMovieRepository
    {
        /// <summary>
        /// Retrieves all movie entities.
        /// </summary>
        Task<IEnumerable<Movie>> GetAllAsync();

        /// <summary>
        /// Finds a movie entity by its unique identifier.
        /// </summary>
        Task<Movie?> GetByIdAsync(Guid id);

        /// <summary>
        /// Checks if a movie exists by title and year.
        /// </summary>
        Task<bool> ExistsAsync(string title, int year);

        /// <summary>
        /// Adds a new movie entity to the database.
        /// </summary>
        Task AddAsync(Movie movie);

        /// <summary>
        /// Removes a movie entity from the database.
        /// </summary>
        void Delete(Movie movie);

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        Task SaveChangesAsync();
    }
}
