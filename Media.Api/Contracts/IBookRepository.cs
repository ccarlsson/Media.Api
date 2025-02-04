using Media.Api.Models;

namespace Media.Api.Contracts;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book?> GetBookAsync(int id);
    Task<Book> AddBookAsync(Book book);
    Task<bool> UpdateBookAsync(int id, Book book);
    Task<bool> DeleteBookAsync(int id);
}
