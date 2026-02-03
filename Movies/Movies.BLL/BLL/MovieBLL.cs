using AutoMapper;
using Movies.BLL.DTO;
using Movies.BLL.Interfaces;
using Movies.Core.Exceptions;
using Movies.DAL.DAO;
using Movies.DAL.Interfaces;

namespace Movies.BLL.BLL
{
    /// <inheritdoc cref="IMovieBLL"/>
    public class MovieBLL : IMovieBLL
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieBLL"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        /// <param name="movieRepository">The repository for movie data persistence.</param>
        public MovieBLL(IMapper mapper, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        /// <inheritdoc />
        public async Task CreateAsync(MovieDTO movieDTO)
        {
            if (await _movieRepository.ExistsAsync(movieDTO.Title, movieDTO.Year ?? 0))
                throw new MovieAlreadyExistsException($"A movie with the title '{movieDTO.Title}' and year {movieDTO.Year} already exists.");

            var movieEntity = _mapper.Map<Movie>(movieDTO);
            await _movieRepository.AddAsync(movieEntity);
            await _movieRepository.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<MovieDTO>> GetAllAsync()
        {
            var moviesEntity = await _movieRepository.GetAllAsync();
            return _mapper.Map<List<MovieDTO>>(moviesEntity);
        }

        /// <inheritdoc />
        public async Task<MovieDTO> GetByIdAsync(Guid id)
        {
            var movieEntity = await _movieRepository.GetByIdAsync(id);
            if (movieEntity == null)
                throw new MovieNotFoundException($"The movie with ID '{id}' was not found.");

            return _mapper.Map<MovieDTO>(movieEntity);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Guid id, MovieDTO updatedMovieDTO)
        {
            var movieEntity = await _movieRepository.GetByIdAsync(id);
            if (movieEntity == null)
                throw new MovieNotFoundException($"The movie with ID '{id}' was not found.");

            if (movieEntity.Title != updatedMovieDTO.Title || movieEntity.Year != updatedMovieDTO.Year)
            {
                if (await _movieRepository.ExistsAsync(updatedMovieDTO.Title, updatedMovieDTO.Year ?? 0))
                    throw new MovieAlreadyExistsException($"A movie with the title '{updatedMovieDTO.Title}' and year {updatedMovieDTO.Year} already exists.");
            }

            _mapper.Map(updatedMovieDTO, movieEntity);
            await _movieRepository.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            var movieEntity = await _movieRepository.GetByIdAsync(id);
            if (movieEntity == null)
                throw new MovieNotFoundException($"The movie with ID '{id}' was not found.");

            _movieRepository.Delete(movieEntity);
            await _movieRepository.SaveChangesAsync();
        }
    }
}