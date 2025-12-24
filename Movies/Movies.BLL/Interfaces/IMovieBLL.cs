using Movies.BLL.DTO;

namespace Movies.BLL.Interfaces
{
    /// <summary>
    /// Defines the business logic operations for managing movies.
    /// Acts as a bridge between the API controllers and the data access layer,
    /// handling validation and business rules.
    /// </summary>
    public interface IMovieBLL
    {
        /// <summary>
        /// Creates a new movie entry after validating that it doesn't already exist.
        /// </summary>
        /// <param name="movieDTO">The movie data transfer object containing the movie details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Movies.Core.Exceptions.MovieAlreadyExistsException">
        /// Thrown when a movie with the same title and year is already present in the database.
        /// </exception>
        Task CreateAsync(MovieDTO movieDTO);

        /// <summary>
        /// Retrieves all movies from the database.
        /// </summary>
        /// <returns>A list of all movies as DTOs.</returns>
        Task<List<MovieDTO>> GetAllAsync();

        /// <summary>
        /// Retrieves a movie by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the movie.</param>
        /// <returns>The movie DTO.</returns>
        /// <exception cref="Movies.Core.Exceptions.MovieNotFoundException">
        /// Thrown if the movie with the specified ID does not exist.
        /// </exception>
        Task<MovieDTO> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates an existing movie with new data after validating its existence.
        /// </summary>
        /// <param name="id">The unique identifier of the movie to update.</param>
        /// <param name="updatedMovieDTO">The data transfer object containing updated movie details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns> 
        /// <exception cref="Movies.Core.Exceptions.MovieNotFoundException">
        /// Thrown if the movie with the specified ID does not exist.
        /// </exception>
        /// <exception cref="Movies.Core.Exceptions.MovieAlreadyExistsException">
        /// Thrown when a movie with the same title and year is already present in the database.
        /// </exception>
        Task UpdateAsync(Guid id, MovieDTO updatedMovieDTO);

        /// <summary>
        /// Deletes a movie by its unique identifier after validating its existence.
        /// </summary>
        /// <param name="id">The unique identifier of the movie to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Movies.Core.Exceptions.MovieNotFoundException">
        /// Thrown if the movie with the specified ID does not exist.
        /// </exception>
        Task DeleteAsync(Guid id);
    }
}