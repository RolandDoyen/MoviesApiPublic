using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Models;
using Movies.BLL.DTO;
using Movies.BLL.Interfaces;

namespace Movies.API.Controllers.v2
{
    /// <summary>
    /// Manages movie operations for version 2.0.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieBLL _movieBLL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieController"/> class.
        /// </summary>
        public MovieController(ILogger<MovieController> logger, IMovieBLL movieBLL, IMapper mapper)
        {
            _logger = logger;
            _movieBLL = movieBLL;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movieRequestModel">The movie data transfer object containing title, year, etc.</param>
        /// <response code="200">Movie created successfully.</response>
        /// <response code="400">The request is invalid (e.g., validation errors).</response>
        /// <response code="409">A movie with the same unique properties already exists.</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] MovieRequestModel movieRequestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movieDTO = _mapper.Map<MovieDTO>(movieRequestModel);
            await _movieBLL.CreateAsync(movieDTO);

            _logger.LogInformation("Movie created: {Title}", movieRequestModel.Title);

            return Ok("Movie created successfully.");
        }

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <response code="200">Returns the complete list of movies.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var movieDTOs = await _movieBLL.GetAllAsync();
            var movieResponseModels = _mapper.Map<List<MovieResponseModel>>(movieDTOs);

            return Ok(movieResponseModels);
        }

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="id">The movie's unique identifier.</param>
        /// <response code="200">Returns the requested movie.</response>
        /// <response code="404">The movie with the specified ID was not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovieResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var movieDTO = await _movieBLL.GetByIdAsync(id);
            var movieResponseModel = _mapper.Map<MovieResponseModel>(movieDTO);

            return Ok(movieResponseModel);
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The movie's unique identifier.</param>
        /// <param name="updatedMovieRequestModel">Updated movie data.</param>
        /// <response code="200">Movie updated successfully.</response>
        /// <response code="400">The request is invalid (e.g., validation errors).</response>
        /// <response code="404">The movie with the specified ID was not found.</response>
        /// <response code="409">Update failed due to a conflict (e.g., title already exists for another movie).</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] MovieRequestModel updatedMovieRequestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movieUpdatedDTO = _mapper.Map<MovieDTO>(updatedMovieRequestModel);
            await _movieBLL.UpdateAsync(id, movieUpdatedDTO);

            return Ok("Movie updated successfully.");
        }

        /// <summary>
        /// Deletes a movie by its ID.
        /// </summary>
        /// <param name="id">The movie's unique identifier.</param>
        /// <response code="200">Movie deleted successfully.</response>
        /// <response code="404">The movie with the specified ID was not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _movieBLL.DeleteAsync(id);

            return Ok("Movie deleted successfully.");
        }
    }
}