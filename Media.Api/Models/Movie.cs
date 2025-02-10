﻿namespace Media.Api.Models;

public class Movie
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Director { get; set; } = string.Empty;
  public int Duration { get; set; }
}
