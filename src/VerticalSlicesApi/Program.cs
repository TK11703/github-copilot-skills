using VerticalSlicesApi.Features.Todo;
using VerticalSlicesApi.Features.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Register feature endpoints (vertical slices)
app.MapWeatherEndpoints();
app.MapTodoEndpoints();

app.Run();
