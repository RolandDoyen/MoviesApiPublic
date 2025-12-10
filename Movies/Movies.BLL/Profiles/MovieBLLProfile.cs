using AutoMapper;
using Movies.BLL.DTO;
using Movies.DAL.DAO;

namespace Movies.BLL.Profiles
{
    public class MovieBLLProfile : Profile
    {
        public MovieBLLProfile()
        {
            CreateMap<Movie, MovieDTO>();

            CreateMap<MovieDTO, Movie>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}