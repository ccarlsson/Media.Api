using Media.Api.Contracts;
using Media.Api.Models;
using Microsoft.Data.SqlClient;
namespace Media.Api.Services;
public class SQLBookRepository(IConfiguration configuration) : IBookRepository
{
  private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

  public async Task<Book> AddBookAsync(Book book)
    {
        const string query = "INSERT INTO Books (Title, Author, Pages) VALUES (@Title, @Author, @Pages); SELECT SCOPE_IDENTITY();";
        using var sqlClient = new SqlConnection(_connectionString);
        using var sqlCommand = new SqlCommand(query, sqlClient);
        sqlCommand.Parameters.AddWithValue("@Title", book.Title);
        sqlCommand.Parameters.AddWithValue("@Author", book.Author);
        sqlCommand.Parameters.AddWithValue("@Pages", book.Pages);
        await sqlClient.OpenAsync();
        var id = await sqlCommand.ExecuteScalarAsync();
        book.Id = Convert.ToInt32(id);
        return book;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        const string query = "DELETE FROM Books WHERE Id = @Id";
        using var sqlClient = new SqlConnection(_connectionString);
        using var sqlCommand = new SqlCommand(query, sqlClient);
        sqlCommand.Parameters.AddWithValue("@Id", id);
        await sqlClient.OpenAsync();
        return await sqlCommand.ExecuteNonQueryAsync() > 0;
    }

    public async Task<Book?> GetBookAsync(int id)
    {
        const string query = "SELECT * FROM Books WHERE Id = @Id";
        using var sqlClient = new SqlConnection(_connectionString);
        using var sqlCommand = new SqlCommand(query, sqlClient);
        sqlCommand.Parameters.AddWithValue("@Id", id);
        await sqlClient.OpenAsync();
        using var reader = await sqlCommand.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                Pages = reader.GetInt32(3)
            };
        }
        return null;
    }

    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        const string query = "SELECT * FROM Books";
        using var sqlClient = new SqlConnection(_connectionString);
        using var sqlCommand = new SqlCommand(query, sqlClient);
        await sqlClient.OpenAsync();
        using var reader = await sqlCommand.ExecuteReaderAsync();
        var books = new List<Book>();
        while (await reader.ReadAsync())
        {
            books.Add(new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                Pages = reader.GetInt32(3)
            });
        }
        return books;
    }

    public async Task<bool> UpdateBookAsync(int id, Book book)
    {
        if (book == null)
        {
            throw new ArgumentNullException(nameof(book));
        }
        if (id != book.Id)
        {
            throw new ArgumentException("The Id property must match the id parameter");
        }

        const string query = "UPDATE Books SET Title = @Title, Author = @Author, Pages = @Pages WHERE Id = @Id";
        using var sqlClient = new SqlConnection(_connectionString);
        using var sqlCommand = new SqlCommand(query, sqlClient);
        sqlCommand.Parameters.AddWithValue("@Id", id);
        sqlCommand.Parameters.AddWithValue("@Title", book.Title);
        sqlCommand.Parameters.AddWithValue("@Author", book.Author);
        sqlCommand.Parameters.AddWithValue("@Pages", book.Pages);
        await sqlClient.OpenAsync();
        return await sqlCommand.ExecuteNonQueryAsync() > 0;
    }
}
