using Media.Api.Contracts;
using Media.Api.Models;

namespace Media.Api.Services;

public class InMemoryMovieRepository : IMovieRepository
{
  private readonly List<Movie> _movies = new();
  public Task<Movie> AddMovieAsync(Movie movie)
  {
    movie.Id = _movies.Count + 1;
    _movies.Add(movie);
    return Task.FromResult(movie);
  }

  public Task<Movie?> GetMovieAsync(int id)
  {
    var movie = _movies.FirstOrDefault(m => m.Id == id);
    return Task.FromResult(movie);
  }

  public Task<IEnumerable<Movie>> GetMoviesAsync()
  {
    return Task.FromResult(_movies.AsEnumerable());
  }
  public Task<bool> UpdateMovieAsync(int id, Movie movie)
  {
    ArgumentNullException.ThrowIfNull(movie);
    if (movie.Id != id)
    {
      throw new ArgumentException("The movie id does not match the id in the request");
    }
    var existingMovie = _movies.FirstOrDefault(m => m.Id == id);
    if (existingMovie is null)
    {
      return Task.FromResult(false);
    }
    existingMovie.Title = movie.Title;
    existingMovie.Director = movie.Director;
    existingMovie.Duration = movie.Duration;
    return Task.FromResult(true);
  }

  public Task<bool> DeleteMovieAsync(int id)
  {
    var movie = _movies.FirstOrDefault(m => m.Id == id);
    if (movie is null)
    {
      return Task.FromResult(false);
    }
    _movies.Remove(movie);
    return Task.FromResult(true);
  }
}
