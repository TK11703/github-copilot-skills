namespace VerticalSlicesApi.Features.Weather;

public static class WeatherEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weather", GetWeatherForecastHandler.Handle)
            .WithName("GetWeatherForecast")
            .WithSummary("Get a 5-day weather forecast for the specified city using Open-Meteo")
            .WithTags("Weather");

        return app;
    }
}
