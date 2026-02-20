using System.Reflection;
using VerticalSlicesApi;
using VerticalSlicesApi.Features.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Map all endpoints discovered from the executing assembly
app.MapEndpoints();

app.Run();
