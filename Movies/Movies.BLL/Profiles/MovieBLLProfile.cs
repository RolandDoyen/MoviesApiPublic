using AutoMapper;
using Movies.BLL.DTO;
using Movies.DAL.DAO;

namespace Movies.BLL.Profiles
{
    /// <summary>
    /// Configuration profile for AutoMapper in the Business Logic Layer.
    /// Manages translations between Database Entities and DTOs.
    /// </summary>
    public class MovieBLLProfile : Profile
    {
        /// <summary>
        /// Initializes mapping rules for the Movie domain.
        /// </summary>
        public MovieBLLProfile()
        {
            /// <summary>
            /// Maps a database Entity to a DTO.
            /// Used when retrieving data to be processed by the BLL or sent to the API.
            /// </summary>
            CreateMap<Movie, MovieDTO>();

            /// <summary>
            /// Maps a DTO to a database Entity.
            /// The Id is ignored to ensure the primary key remains managed by the database or explicit logic.
            /// </summary>
            CreateMap<MovieDTO, Movie>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}