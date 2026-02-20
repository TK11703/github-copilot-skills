namespace VerticalSlicesApi.Features.Weather;

public static class WeatherEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weather", GetWeatherForecast)
            .WithName("GetWeatherForecast")
            .WithSummary("Get a 5-day weather forecast for the specified city using Open-Meteo")
            .WithTags("Weather");

        return app;
    }

    private static async Task<IResult> GetWeatherForecast(string city, WeatherService weatherService)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return Results.BadRequest("A city name is required.");
        }

        try
        {
            var forecast = await weatherService.GetForecastAsync(city);

            if (forecast is null)
            {
                return Results.NotFound($"Could not find weather data for '{city}'. Please check the city name and try again.");
            }

            return Results.Ok(forecast);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or System.Text.Json.JsonException)
        {
            return Results.Problem(
                detail: "The weather service is currently unavailable. Please try again later.",
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }
    }
}
