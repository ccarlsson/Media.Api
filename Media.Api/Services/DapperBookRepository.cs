using Dapper;
using Media.Api.Contracts;
using Media.Api.Models;
using Microsoft.Data.SqlClient;

namespace Media.Api.Services;

public class DapperBookRepository(IConfiguration configuration) : IBookRepository
{
  private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");
  public async Task<Book> AddBookAsync(Book book)
  {
    using var sqlClient = new SqlConnection(_connectionString);
    const string query = "INSERT INTO Books (Title, Author, Pages) VALUES (@Title, @Author, @Pages); SELECT SCOPE_IDENTITY();";
    return await sqlClient.QuerySingleAsync<Book>(query, book);
  }

  public async Task<bool> DeleteBookAsync(int id)
  {
    using var sqlClient = new SqlConnection(_connectionString);
    const string query = "DELETE FROM Books WHERE Id = @Id";
    return await sqlClient.ExecuteAsync(query, new { Id = id }) > 0;
  }

  public Task<Book?> GetBookAsync(int id)
  {
    using SqlConnection sqlClient = new (_connectionString);
    const string query = "SELECT * FROM Books WHERE Id = @Id";
    return sqlClient.QuerySingleOrDefaultAsync<Book>(query, new { Id = id });
  }

  public Task<IEnumerable<Book>> GetBooksAsync()
  {
    using SqlConnection sqlClient = new(_connectionString);
    const string query = "SELECT * FROM Books";
    return sqlClient.QueryAsync<Book>(query);
  }

  public async Task<bool> UpdateBookAsync(int id, Book book)
  {
    using var sqlClient = new SqlConnection(_connectionString);
    const string query = "UPDATE Books SET Title = @Title, Author = @Author, Pages = @Pages WHERE Id = @Id";
    return await sqlClient.ExecuteAsync(query, new { book.Title, book.Author, book.Pages, Id = id }) > 0;
  }
}
