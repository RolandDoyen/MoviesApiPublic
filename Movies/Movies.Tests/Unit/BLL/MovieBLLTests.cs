using AutoMapper;
using Moq;
using Movies.BLL.BLL;
using Movies.BLL.DTO;
using Movies.Core.Exceptions;
using Movies.DAL.DAO;
using Movies.DAL.Interfaces;
using Xunit;

namespace Movies.Tests.Unit.BLL
{
    public class MovieBLLTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly MovieBLL _bll;
        private readonly Mock<IMovieRepository> _mockRepository;

        public MovieBLLTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IMovieRepository>();
            _bll = new MovieBLL(_mockMapper.Object, _mockRepository.Object);
        }

        #region GetByIdAsync()
        [Fact]
        public async Task GetByIdAsync_WhenMovieExists_ReturnsMovieDTO()
        {
            var movieId = Guid.NewGuid();
            var movieEntity = new Movie { Id = movieId };
            var movieDTO = new MovieDTO { Id = movieId };

            _mockRepository.Setup(x => x.GetByIdAsync(movieId)).ReturnsAsync(movieEntity);
            _mockMapper.Setup(x => x.Map<MovieDTO>(movieEntity)).Returns(movieDTO);

            var result = await _bll.GetByIdAsync(movieId);

            Assert.NotNull(result);
            Assert.Equal(movieId, result.Id);

            _mockRepository.Verify(x => x.GetByIdAsync(movieId), Times.Once);
            _mockMapper.Verify(x => x.Map<MovieDTO>(movieEntity), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenMovieDoesNotExist_ThrowsMovieNotFoundException()
        {
            var movieId = Guid.NewGuid();

            _mockRepository.Setup(x => x.GetByIdAsync(movieId)).ReturnsAsync((Movie)null);

            await Assert.ThrowsAsync<MovieNotFoundException>(() => _bll.GetByIdAsync(movieId));

            _mockRepository.Verify(x => x.GetByIdAsync(movieId), Times.Once);
        }
        #endregion
    }
}
