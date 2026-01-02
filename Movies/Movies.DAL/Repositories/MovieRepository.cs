using Microsoft.EntityFrameworkCore;
using Movies.DAL.DAO;
using Movies.DAL.Interfaces;

namespace Movies.DAL.Repositories
{
    /// <summary>
    /// Repository for managing movie data using Entity Framework Core.
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;
        public MovieRepository(DataContext context) => _context = context;

        /// <inheritdoc />
        public async Task<IEnumerable<Movie>> GetAllAsync() => await _context.Movies.ToListAsync();

        /// <inheritdoc />
        public async Task<Movie?> GetByIdAsync(Guid id) => await _context.Movies.FindAsync(id);

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(string title, int year) => await _context.Movies.AnyAsync(x => x.Title == title && x.Year == year);

        /// <inheritdoc />
        public async Task AddAsync(Movie movie) => await _context.Movies.AddAsync(movie);

        /// <inheritdoc />
        public void Delete(Movie movie) => _context.Movies.Remove(movie);

        /// <inheritdoc />
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
