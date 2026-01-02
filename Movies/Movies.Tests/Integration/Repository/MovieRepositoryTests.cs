using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Movies.DAL;
using Movies.DAL.DAO;
using Movies.DAL.Repositories;
using Xunit;

namespace Movies.Tests.Integration.Repository
{
    public class MovieRepositoryTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly MovieRepository _repository;
        private readonly SqliteConnection _connection;

        public MovieRepositoryTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new DataContext(options);
            _context.Database.EnsureCreated();

            _repository = new MovieRepository(_context);
        }

        #region GetByIdAsync()
        [Fact]
        public async Task GetByIdAsync_WhenMovieExists_ReturnsMovie()
        {
            var movieId = Guid.NewGuid();
            var movieEntity = new Movie { Id = movieId, Title = "Interstellar" };
            _context.Movies.Add(movieEntity);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(movieId);

            Assert.NotNull(result);
            Assert.Equal("Interstellar", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_WhenMovieDoesNotExist_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }
        #endregion

        public void Dispose()
        {
            _connection.Close();
            _context.Dispose();
        }
    }
}