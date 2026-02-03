using AutoMapper;
using Movies.API.Models;
using Movies.BLL.DTO;

namespace Movies.API.Profiles
{
    /**
     * Defines the mapping configurations between API models (Models) 
     * and Business Logic Layer objects (DTOs).
     * This profile ensures a clean separation between the external API contracts 
     * and the internal data structures.
     */
    public class MovieAPIProfile : Profile
    {
        /**
         * Initializes the mapping rules for Movie-related objects.
         */
        public MovieAPIProfile()
        {
            // Maps incoming request data from the client to a DTO for processing.
            CreateMap<MovieRequestModel, MovieDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Maps processed business data back to a response model for the client.
            CreateMap<MovieDTO, MovieResponseModel>();
        }
    }
}