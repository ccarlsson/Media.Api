using Media.Api.Contracts;
using Media.Api.Services;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Books
// builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddScoped<IBookRepository, SQLBookRepository>();
// builder.Services.AddScoped<IBookRepository, DapperBookRepository>(); // Uncomment to use Dapper (doest work atm)

// Movies
// builder.Services.AddSingleton<IMovieRepository, InMemoryMovieRepository>();
builder.Services.AddScoped<IMovieRepository, SQLMovieRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference(options =>
  {
    options.Theme = ScalarTheme.DeepSpace;
  });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
