using Movies.BLL.DTO;

namespace Movies.BLL.Interfaces
{
    public interface IMovieBLL
    {
        Task CreateAsync(MovieDTO movieDTO);
        Task<List<MovieDTO>> GetAllAsync();
        Task<MovieDTO> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, MovieDTO updatedMovieDTO);
        Task DeleteAsync(Guid id);
    }
}