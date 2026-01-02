using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Movies.DAL.DAO;
using System.Linq.Expressions;
using System.Text.Json;

namespace Movies.DAL
{
    /// <summary>
    /// Database context for the Movies application.
    /// Handles entity configuration and database connection.
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">Options for configuring the DbContext.</param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the entity models and their relationships.
        /// Sets JSON conversion for lists, unique indexes, and primary keys.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entities.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure JSON conversion and comparer for list properties
            var comparer = GetListComparer();
            void ConfigureJsonList(Expression<Func<Movie, List<string>>> prop)
            {
                modelBuilder.Entity<Movie>()
                    .Property(prop)
                    .HasConversion(
                        x => JsonSerializer.Serialize(x, new JsonSerializerOptions()),
                        x => JsonSerializer.Deserialize<List<string>>(x, new JsonSerializerOptions()) ?? new List<string>()
                    )
                    .Metadata.SetValueComparer(comparer);
            }

            ConfigureJsonList(m => m.Styles);
            ConfigureJsonList(m => m.Actors);
            ConfigureJsonList(m => m.Realisators);
            ConfigureJsonList(m => m.Scenarists);
            ConfigureJsonList(m => m.Producers);

            // Unique index on Title + Year
            modelBuilder.Entity<Movie>()
                .HasIndex(x => new { x.Title, x.Year })
                .IsUnique();

            // Primary key
            modelBuilder.Entity<Movie>().HasKey(x => x.Id);
        }

        /// <summary>
        /// Provides a comparer for List<string> to enable proper change tracking in EF Core.
        /// </summary>
        /// <returns>A ValueComparer for List<string>.</returns>
        private static ValueComparer<List<string>> GetListComparer()
        {
            return new ValueComparer<List<string>>(
                (x, y) => x.SequenceEqual(y),
                z => z.Aggregate(0, (a, b) => HashCode.Combine(a, b.GetHashCode())),
                z => z.ToList()
            );
        }

        /// <summary>
        /// Gets or sets the Movies DbSet.
        /// </summary>
        public DbSet<Movie> Movies { get; set; }
    }
}