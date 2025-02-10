using Media.Api.Models;

namespace Media.Api.Contracts;

public interface IMovieRepository
{
  Task<IEnumerable<Movie>> GetMoviesAsync();
  Task<Movie?> GetMovieAsync(int id);
  Task<Movie> AddMovieAsync(Movie movie);
  Task<bool> UpdateMovieAsync(int id, Movie movie);
  Task<bool> DeleteMovieAsync(int id);
}
