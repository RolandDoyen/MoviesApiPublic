using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Models;
using Movies.BLL.DTO;
using Movies.BLL.Interfaces;
using Movies.Core.Exceptions;

namespace Movies.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieBLL _movieBLL;
        private readonly IMapper _mapper;

        public MovieController(ILogger<MovieController> logger, IMovieBLL movieBLL, IMapper mapper)
        {
            _logger = logger;
            _movieBLL = movieBLL;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movieRequestModel">The movie data to create.</param>
        /// <returns>Confirmation message or conflict if the movie already exists.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieRequestModel movieRequestModel)
        {
            try
            {
                var movieDTO = _mapper.Map<MovieDTO>(movieRequestModel);
                await _movieBLL.CreateAsync(movieDTO);

                _logger.LogInformation("Create endpoint called.");
                return Ok("Movie created successfully.");
            }
            catch (MovieAlreadyExistsException)
            {
                _logger.LogWarning("Movie already exists: {Title} {Year}", movieRequestModel.Title, movieRequestModel.Year);
                return Conflict("A movie with the same title and year already exists.");
            }
            catch
            {
                _logger.LogError("Error occurred while creating movie.");
                return StatusCode(500, "An error occurred while creating the movie.");
            }
        }

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>List of movies.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var movieDTOs = await _movieBLL.GetAllAsync();
                var movieResponseModels = _mapper.Map<List<MovieResponseModel>>(movieDTOs);

                _logger.LogInformation("GetAll endpoint called.");
                return Ok(movieResponseModels);
            }
            catch
            {
                _logger.LogError("Error occurred while retrieving movies.");
                return StatusCode(500, "An error occurred while retrieving movies.");
            }
        }

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="id">The movie's unique identifier.</param>
        /// <returns>The requested movie or NotFound if it doesn't exist.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var movieDTO = await _movieBLL.GetByIdAsync(id);
                var movieResponseModel = _mapper.Map<MovieResponseModel>(movieDTO);

                _logger.LogInformation("GetById endpoint called for id: {Id}", id);
                return Ok(movieResponseModel);
            }
            catch (MovieNotFoundException)
            {
                _logger.LogWarning("Movie not found with id: {Id}", id);
                return NotFound("Movie not found.");
            }
            catch
            {
                _logger.LogError("Error occurred while retrieving movie with id: {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the movie.");
            }
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The movie's unique identifier.</param>
        /// <param name="updatedMovieRequestModel">Updated movie data.</param>
        /// <returns>Confirmation message or NotFound if the movie doesn't exist.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MovieRequestModel updatedMovieRequestModel)
        {
            try
            {
                var movieUpdatedDTO = _mapper.Map<MovieDTO>(updatedMovieRequestModel);
                await _movieBLL.UpdateAsync(id, movieUpdatedDTO);

                _logger.LogInformation("Update endpoint called for id: {Id}", id);
                return Ok("Movie updated successfully.");
            }
            catch (MovieNotFoundException)
            {
                _logger.LogWarning("Movie not found with id: {Id}", id);
                return NotFound("Movie not found.");
            }
            catch
            {
                _logger.LogError("Error occurred while updating movie with id: {Id}", id);
                return StatusCode(500, "An error occurred while updating the movie.");
            }
        }

        /// <summary>
        /// Deletes a movie by its ID.
        /// </summary>
        /// <param name="id">The movie's unique identifier.</param>
        /// <returns>Confirmation message or NotFound if the movie doesn't exist.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _movieBLL.DeleteAsync(id);

                _logger.LogInformation("Delete endpoint called for id: {Id}", id);
                return Ok("Movie deleted successfully.");
            }
            catch (MovieNotFoundException)
            {
                _logger.LogWarning("Movie not found with id: {Id}", id);
                return NotFound("Movie not found.");
            }
            catch
            {
                _logger.LogError("Error occurred while deleting movie with id: {Id}", id);
                return StatusCode(500, "An error occurred while deleting the movie.");
            }
        }
    }
}