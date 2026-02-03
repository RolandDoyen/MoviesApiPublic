namespace Movies.BLL.DTO
{
    /// <summary>
    /// Data Transfer Object representing a movie.
    /// </summary>
    public class MovieDTO
    {
        /// <summary>
        /// Unique identifier of the movie.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the movie.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Average rating of the movie.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Short synopsis of the movie.
        /// </summary>
        public string Sypnosis { get; set; } = string.Empty;

        /// <summary>
        /// Release year of the movie.
        /// </summary>
        public int? Year { get; set; }

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