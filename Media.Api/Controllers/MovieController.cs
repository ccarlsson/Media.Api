using Media.Api.Contracts;
using Media.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Media.Api.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
  private readonly IMovieRepository _movieRepository;

  public MovieController(IMovieRepository movieRepository)
  {
    _movieRepository = movieRepository;
  }

  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var movies = await _movieRepository.GetMoviesAsync();
    return Ok(movies);
  }

  [HttpGet("{id:int}", Name = "GetMovie")]
  public async Task<IActionResult> Get(int id)
  {
    var movie = await _movieRepository.GetMovieAsync(id);
    if (movie is null)
      return NotFound();
    return Ok(movie);
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] Movie movie)
  {
    var createdMovie = await _movieRepository.AddMovieAsync(movie);
    return CreatedAtRoute("GetMovie", new { id = createdMovie.Id }, createdMovie);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var deleted = await _movieRepository.DeleteMovieAsync(id);
    if (!deleted)
      return NotFound();
    return NoContent();
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Put(int id, [FromBody] Movie movie)
  {
    try
    {
      var updated = await _movieRepository.UpdateMovieAsync(id, movie);
      if (!updated)
        return NotFound();
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }


}
