using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movies.BLL.DTO;
using Movies.BLL.Interfaces;
using Movies.Core.Exceptions;
using Movies.DAL;
using Movies.DAL.DAO;

namespace Movies.BLL.BLL
{
    /// <summary>
    /// Business logic layer for managing movies.
    /// </summary>
    public class MovieBLL : IMovieBLL
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieBLL"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs and entities.</param>
        public MovieBLL(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new movie in the database.
        /// </summary>
        /// <param name="movieDTO">The movie data to create.</param>
        /// <exception cref="MovieAlreadyExistsException">Thrown if a movie with the same title and year already exists.</exception>
        public async Task CreateAsync(MovieDTO movieDTO)
        {
            var entity = await _context.Movies.FirstOrDefaultAsync(m => m.Title == movieDTO.Title && m.Year == movieDTO.Year);
            if (entity != null)
                throw new MovieAlreadyExistsException();

            movieDTO.Id = Guid.NewGuid();
            var newMovieEntity = _mapper.Map<Movie>(movieDTO);
            await _context.Movies.AddAsync(newMovieEntity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all movies from the database.
        /// </summary>
        /// <returns>List of all movies as DTOs.</returns>
        public async Task<List<MovieDTO>> GetAllAsync()
        {
            var moviesEntity = await _context.Movies.ToListAsync();
            var moviesDTO = _mapper.Map<List<MovieDTO>>(moviesEntity);
            return moviesDTO;
        }

        /// <summary>
        /// Retrieves a movie by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the movie.</param>
        /// <returns>The movie DTO.</returns>
        /// <exception cref="MovieNotFoundException">Thrown if the movie does not exist.</exception>
        public async Task<MovieDTO> GetByIdAsync(Guid id)
        {
            var movieEntity = await _context.Movies.FindAsync(id);
            if (movieEntity == null)
                throw new MovieNotFoundException();

            var movieDTO = _mapper.Map<MovieDTO>(movieEntity);
            return movieDTO;
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The unique identifier of the movie to update.</param>
        /// <param name="updatedMovieDTO">The updated movie data.</param>
        /// <exception cref="MovieNotFoundException">Thrown if the movie does not exist.</exception>
        public async Task UpdateAsync(Guid id, MovieDTO updatedMovieDTO)
        {
            var movieEntity = await _context.Movies.FindAsync(id);
            if (movieEntity == null)
                throw new MovieNotFoundException();

            var updatedMovie = _mapper.Map(updatedMovieDTO, movieEntity);
            _context.Movies.Update(updatedMovie);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a movie by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the movie to delete.</param>
        /// <exception cref="MovieNotFoundException">Thrown if the movie does not exist.</exception>
        public async Task DeleteAsync(Guid id)
        {
            var movieEntity = await _context.Movies.FindAsync(id);
            if (movieEntity == null)
                throw new MovieNotFoundException();

            _context.Movies.Remove(movieEntity);
            await _context.SaveChangesAsync();
        }
    }
}