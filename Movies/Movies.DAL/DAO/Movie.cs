using System.ComponentModel.DataAnnotations;

namespace Movies.DAL.DAO
{
    /// <summary>
    /// Represents a movie entity in the database.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Unique identifier of the movie.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the movie.
        /// Maximum length: 200 characters. Required field.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Average rating of the movie.
        /// Must be between 0 and 10, with 1 decimal precision.
        /// </summary>
        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
        public int Rating { get; set; }

        /// <summary>
        /// Short synopsis of the movie.
        /// Maximum length: 1000 characters.
        /// </summary>
        [MaxLength(1000)]
        public string Sypnosis { get; set; } = string.Empty;

        /// <summary>
        /// Release year of the movie.
        /// Must be realistic (between 1930 and 2030).
        /// </summary>
        [Range(1930, 2030, ErrorMessage = "Year must be realistic")]
        public int Year { get; set; }

        /// <summary>
        /// List of genres or styles associated with the movie.
        /// </summary>
        public List<string> Styles { get; set; } = new();

        /// <summary>
        /// Duration of the movie in minutes.
        /// </summary>
        public int Length { get; set; } = 0;

        /// <summary>
        /// URL link to the movie's trailer.
        /// </summary>
        public string TrailerLink { get; set; } = string.Empty;

        /// <summary>
        /// List of directors (realisators) of the movie.
        /// </summary>
        public List<string> Realisators { get; set; } = new();

        /// <summary>
        /// List of screenwriters (scenarists) of the movie.
        /// </summary>
        public List<string> Scenarists { get; set; } = new();

        /// <summary>
        /// List of actors in the movie.
        /// </summary>
        public List<string> Actors { get; set; } = new();

        /// <summary>
        /// List of producers of the movie.
        /// </summary>
        public List<string> Producers { get; set; } = new();
    }
}