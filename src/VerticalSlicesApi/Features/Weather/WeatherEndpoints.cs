namespace VerticalSlicesApi.Features.Weather;

public static class WeatherEndpoints
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weather", GetWeatherForecast)
            .WithName("GetWeatherForecast")
            .WithSummary("Get a 5-day weather forecast for the specified city")
            .WithTags("Weather");

        return app;
    }

    private static IResult GetWeatherForecast(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return Results.BadRequest("A city name is required.");
        }

        var forecast = Enumerable.Range(1, 5).Select(index => new WeatherResponse(
            city,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        )).ToArray();

        return Results.Ok(forecast);
    }
}
