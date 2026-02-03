using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Models;
using Movies.BLL.DTO;
using Movies.BLL.Interfaces;

namespace Movies.API.Controllers.v1
{
    /// <summary>
    /// Manages movie operations for version 1.0 (Deprecated).
    /// Use version 2.0 for new developments.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
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
        /// <returns>A success message if the movie is created.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        /// <returns>A list of movies.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
        /// <returns>The requested movie.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovieResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var movieDTO = await _movieBLL.GetByIdAsync(id);

            return Ok(_mapper.Map<MovieResponseModel>(movieDTO));
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The movie's unique identifier.</param>
        /// <param name="updatedMovieRequestModel">Updated movie data.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
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
        /// <returns>Confirmation message.</returns>
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