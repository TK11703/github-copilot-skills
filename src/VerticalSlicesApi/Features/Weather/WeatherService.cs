namespace VerticalSlicesApi.Features.Weather;

public class WeatherService(HttpClient httpClient)
{
    private const string GeocodingUrl = "https://geocoding-api.open-meteo.com/v1/search";
    private const string ForecastUrl = "https://api.open-meteo.com/v1/forecast";

    public async Task<GetWeatherForecastHandler.Response[]?> GetForecastAsync(string city)
    {
        // Step 1: resolve city name to coordinates
        var geoResponse = await httpClient.GetFromJsonAsync<GeocodingResponse>(
            $"{GeocodingUrl}?name={Uri.EscapeDataString(city)}&count=1&language=en&format=json");

        var location = geoResponse?.Results?.FirstOrDefault();
        if (location is null)
        {
            return null;
        }

        // Step 2: fetch 5-day daily forecast
        var forecastResponse = await httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(
            $"{ForecastUrl}?latitude={location.Latitude}&longitude={location.Longitude}" +
            "&daily=temperature_2m_max,temperature_2m_min,weathercode&timezone=auto&forecast_days=5");

        if (forecastResponse?.Daily is null)
        {
            return null;
        }

        var daily = forecastResponse.Daily;

        // Guard against inconsistent list lengths from the API
        var count = Math.Min(
            Math.Min(daily.Time.Count, daily.TemperatureMax.Count),
            Math.Min(daily.TemperatureMin.Count, daily.WeatherCode.Count));

        return Enumerable.Range(0, count).Select(i => new GetWeatherForecastHandler.Response(
            city,
            DateOnly.Parse(daily.Time[i]),
            (int)Math.Round(daily.TemperatureMax[i]),
            (int)Math.Round(daily.TemperatureMin[i]),
            WmoCodeToSummary(daily.WeatherCode[i])
        )).ToArray();
    }

    private static string WmoCodeToSummary(int code) => code switch
    {
        0 => "Clear sky",
        1 => "Mainly clear",
        2 => "Partly cloudy",
        3 => "Overcast",
        45 or 48 => "Foggy",
        51 or 53 or 55 => "Drizzle",
        61 or 63 or 65 => "Rainy",
        71 or 73 or 75 or 77 => "Snowy",
        80 or 81 or 82 => "Rain showers",
        85 or 86 => "Snow showers",
        95 => "Thunderstorm",
        96 or 99 => "Thunderstorm with hail",
        _ => "Unknown"
    };

    // Open-Meteo geocoding response models
    private record GeocodingResponse(
        [property: System.Text.Json.Serialization.JsonPropertyName("results")]
        List<GeocodingResult>? Results);

    private record GeocodingResult(
        [property: System.Text.Json.Serialization.JsonPropertyName("latitude")]
        double Latitude,
        [property: System.Text.Json.Serialization.JsonPropertyName("longitude")]
        double Longitude);

    // Open-Meteo forecast response models
    private record OpenMeteoForecastResponse(
        [property: System.Text.Json.Serialization.JsonPropertyName("daily")]
        DailyForecast? Daily);

    private record DailyForecast(
        [property: System.Text.Json.Serialization.JsonPropertyName("time")]
        List<string> Time,
        [property: System.Text.Json.Serialization.JsonPropertyName("temperature_2m_max")]
        List<double> TemperatureMax,
        [property: System.Text.Json.Serialization.JsonPropertyName("temperature_2m_min")]
        List<double> TemperatureMin,
        [property: System.Text.Json.Serialization.JsonPropertyName("weathercode")]
        List<int> WeatherCode);
}

