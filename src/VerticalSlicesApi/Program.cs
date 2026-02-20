using VerticalSlicesApi;
using VerticalSlicesApi.Features.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<WeatherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Register all feature endpoints (vertical slices)
app.MapAllEndpoints();

app.Run();
