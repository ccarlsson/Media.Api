using Media.Api.Contracts;
using Media.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Media.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
  private readonly IBookRepository _bookRepository;

  public BooksController(IBookRepository bookRepository)
  {
    _bookRepository = bookRepository;
  }

  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var books = await _bookRepository.GetBooksAsync();
    return Ok(books);
  }

  [HttpGet("{id:int}", Name = "GetBook")]
  public async Task<IActionResult> Get(int id)
  {
    var book = await _bookRepository.GetBookAsync(id);
    if (book is null)
      return NotFound();

    return Ok(book);
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] Book book)
  {
    var createdBook = await _bookRepository.AddBookAsync(book);

    return CreatedAtRoute("GetBook", new { id = createdBook.Id }, createdBook);
  }


  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var deleted = await _bookRepository.DeleteBookAsync(id);
    if (!deleted)
      return NotFound();

    return NoContent();

  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Put(int id, [FromBody] Book book)
  {
    try
    {
      var updated = await _bookRepository.UpdateBookAsync(id, book);
      if (!updated)
        return NotFound();

      return NoContent();
    }
    catch (ArgumentException ex)
    {
      return BadRequest(ex.Message);
    }
  }
}
