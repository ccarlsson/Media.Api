using Media.Api.Contracts;
using Media.Api.Models;
using Microsoft.Data.SqlClient;
namespace Media.Api.Services;
public class SQLMovieRepository(IConfiguration configuration) : IMovieRepository
{
  private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

  public async Task<Movie> AddMovieAsync(Movie movie)
  {
    const string query = "INSERT INTO Movies (Title, Director, Duration) VALUES (@Title, @Director, @Duration); SELECT SCOPE_IDENTITY();";
    using var sqlClient = new SqlConnection(_connectionString);
    using var sqlCommand = new SqlCommand(query, sqlClient);
    sqlCommand.Parameters.AddWithValue("@Title", movie.Title);
    sqlCommand.Parameters.AddWithValue("@Author", movie.Director);
    sqlCommand.Parameters.AddWithValue("@Pages", movie.Duration);
    await sqlClient.OpenAsync();
    var id = await sqlCommand.ExecuteScalarAsync();
    movie.Id = Convert.ToInt32(id);
    return movie;
  }

  public async Task<bool> DeleteMovieAsync(int id)
  {
    const string query = "DELETE FROM Movies WHERE Id = @Id";
    using var sqlClient = new SqlConnection(_connectionString);
    using var sqlCommand = new SqlCommand(query, sqlClient);
    sqlCommand.Parameters.AddWithValue("@Id", id);
    await sqlClient.OpenAsync();
    return await sqlCommand.ExecuteNonQueryAsync() > 0;
  }

  public async Task<Movie?> GetMovieAsync(int id)
  {
    const string query = "SELECT * FROM Movies WHERE Id = @Id";
    using var sqlClient = new SqlConnection(_connectionString);
    using var sqlCommand = new SqlCommand(query, sqlClient);
    sqlCommand.Parameters.AddWithValue("@Id", id);
    await sqlClient.OpenAsync();
    using var reader = await sqlCommand.ExecuteReaderAsync();
    if (await reader.ReadAsync())
    {
      return new Movie
      {
        Id = reader.GetInt32(0),
        Title = reader.GetString(1),
        Director= reader.GetString(2),
        Duration = reader.GetInt32(3)
      };
    }
    return null;
  }

  public async Task<IEnumerable<Movie>> GetMoviesAsync()
  {
    const string query = "SELECT * FROM Movies";
    using var sqlClient = new SqlConnection(_connectionString);
    using var sqlCommand = new SqlCommand(query, sqlClient);
    await sqlClient.OpenAsync();
    using var reader = await sqlCommand.ExecuteReaderAsync();
    var movies = new List<Movie>();
    while (await reader.ReadAsync())
    {
      movies.Add(new Movie
      {
        Id = reader.GetInt32(0),
        Title = reader.GetString(1),
        Director = reader.GetString(2),
        Duration = reader.GetInt32(3)
      });
    }
    return movies;
  }

  public async Task<bool> UpdateMovieAsync(int id, Movie movie)
  {
    if (movie == null)
    {
      throw new ArgumentNullException(nameof(movie));
    }
    if (id != movie.Id)
    {
      throw new ArgumentException("The Id property must match the id parameter");
    }

    const string query = "UPDATE Movies SET Title = @Title, Director = @Director, Duration = @Duration WHERE Id = @Id";
    using var sqlClient = new SqlConnection(_connectionString);
    using var sqlCommand = new SqlCommand(query, sqlClient);
    sqlCommand.Parameters.AddWithValue("@Id", id);
    sqlCommand.Parameters.AddWithValue("@Title", movie.Title);
    sqlCommand.Parameters.AddWithValue("@Director", movie.Director);
    sqlCommand.Parameters.AddWithValue("@Duration", movie.Duration);
    await sqlClient.OpenAsync();
    return await sqlCommand.ExecuteNonQueryAsync() > 0;
  }
}
