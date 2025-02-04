using Media.Api.Contracts;
using Media.Api.Models;

namespace Media.Api.Services;

public class InMemoryBookRepository : IBookRepository
{
  private readonly List<Book> _books = [];
  public Task<Book> AddBookAsync(Book book)
  {
    ArgumentNullException.ThrowIfNull(book);

    book.Id = _books.Count + 1;
    _books.Add(book);
    return Task.FromResult(book);
  }

  public Task<bool> DeleteBookAsync(int id)
  {
    var book = _books.FirstOrDefault(b => b.Id == id);
    if (book is null)
    {
      return Task.FromResult(false);
    }
    _books.Remove(book);
    return Task.FromResult(true);
  }

  public Task<Book?> GetBookAsync(int id)
  {
    var book = _books.FirstOrDefault(b => b.Id == id);
    return Task.FromResult(book);
  }

  public Task<IEnumerable<Book>> GetBooksAsync()
  {
    return Task.FromResult(_books.AsEnumerable());
  }

  public Task<bool> UpdateBookAsync(int id, Book book)
  {
    ArgumentNullException.ThrowIfNull(book);
    if (book.Id != id)
    {
      throw new ArgumentException("The book id does not match the id in the request");
    }

    var existingBook = _books.FirstOrDefault(b => b.Id == id);
    if (existingBook is null)
    {
      return Task.FromResult(false);
    }
    existingBook.Title = book.Title;
    existingBook.Author = book.Author;
    existingBook.Pages = book.Pages;

    return Task.FromResult(true);
  }
}
