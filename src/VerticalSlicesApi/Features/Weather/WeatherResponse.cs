namespace VerticalSlicesApi.Features.Weather;

public record WeatherResponse(
    string City,
    DateOnly Date,
    int TemperatureC,
    int TemperatureMinC,
    string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC * 9.0 / 5);
    public int TemperatureMinF => 32 + (int)(TemperatureMinC * 9.0 / 5);
}
