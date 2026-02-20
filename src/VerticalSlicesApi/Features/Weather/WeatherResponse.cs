namespace VerticalSlicesApi.Features.Weather;

public record WeatherResponse(
    string City,
    DateOnly Date,
    int TemperatureC,
    string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC * 9.0 / 5);
}
