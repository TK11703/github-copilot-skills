namespace VerticalSlicesApi.Features.Weather;

public static class GetWeatherForecastHandler
{
    public record Response(
        string City,
        DateOnly Date,
        int TemperatureC,
        int TemperatureMinC,
        string Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC * 9.0 / 5);
        public int TemperatureMinF => 32 + (int)(TemperatureMinC * 9.0 / 5);
    }

    public static async Task<IResult> Handle(string city, WeatherService weatherService)
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
