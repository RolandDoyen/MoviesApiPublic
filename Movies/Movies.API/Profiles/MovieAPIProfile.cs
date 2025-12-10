using AutoMapper;
using Movies.API.Models;
using Movies.BLL.DTO;

namespace Movies.API.Profiles
{
    public class MovieAPIProfile : Profile
    {
        public MovieAPIProfile()
        {
            CreateMap<MovieRequestModel, MovieDTO>();
            CreateMap<MovieDTO, MovieResponseModel>();
        }
    }
}