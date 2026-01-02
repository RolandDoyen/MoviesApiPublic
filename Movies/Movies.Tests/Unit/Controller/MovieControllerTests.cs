using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Movies.API.Controllers.v2;
using Movies.API.Models;
using Movies.BLL.DTO;
using Movies.BLL.Interfaces;
using Movies.Core.Exceptions;
using Xunit;

namespace Movies.Tests.Unit.Controller
{
    public class MovieControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMovieBLL> _mockBLL;
        private readonly Mock<ILogger<MovieController>> _mockLogger;
        private readonly MovieController _controller;

        public MovieControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockBLL = new Mock<IMovieBLL>();
            _mockLogger = new Mock<ILogger<MovieController>>();
            _controller = new MovieController(_mockLogger.Object, _mockBLL.Object, _mockMapper.Object);
        }

        #region GetById()
        [Fact]
        public async Task GetById_WhenFound_Returns200Ok()
        {
            var movieId = Guid.NewGuid();
            var movieDTO = new MovieDTO { Id = movieId };
            var movieResponse = new MovieResponseModel { Id = movieId };

            _mockBLL.Setup(x => x.GetByIdAsync(movieId)).ReturnsAsync(movieDTO);
            _mockMapper.Setup(x => x.Map<MovieResponseModel>(movieDTO)).Returns(movieResponse);

            var result = await _controller.GetById(movieId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModel = Assert.IsType<MovieResponseModel>(okResult.Value);
            Assert.Equal(movieId, responseModel.Id);

            _mockBLL.Verify(x => x.GetByIdAsync(movieId), Times.Once);
            _mockMapper.Verify(x => x.Map<MovieResponseModel>(movieDTO), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenNotFound_ThrowsException()
        {
            var movieId = Guid.NewGuid();
            _mockBLL.Setup(x => x.GetByIdAsync(movieId)).ThrowsAsync(new MovieNotFoundException("Movie not found"));

            await Assert.ThrowsAsync<MovieNotFoundException>(() => _controller.GetById(movieId));

            _mockBLL.Verify(x => x.GetByIdAsync(movieId), Times.Once);
        }
        #endregion
    }
}
