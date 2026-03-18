/**
 * get-weather-by-city.js — Fetches real-time weather data for a given city.
 *
 * Step 1: Geocodes the city name to coordinates using the Open-Meteo geocoding API.
 * Step 2: Fetches a 5-day daily forecast using the Open-Meteo forecast API.
 * Returns a structured object the AI uses to build the weather response.
 */

const GEOCODING_URL = 'https://geocoding-api.open-meteo.com/v1/search';
const FORECAST_URL = 'https://api.open-meteo.com/v1/forecast';

/**
 * Maps a WMO weather interpretation code to a human-readable summary.
 * @param {number} code
 * @returns {string}
 */
function wmoCodeToSummary(code) {
  if (code === 0) return 'Clear sky';
  if (code === 1) return 'Mainly clear';
  if (code === 2) return 'Partly cloudy';
  if (code === 3) return 'Overcast';
  if (code === 45 || code === 48) return 'Foggy';
  if (code >= 51 && code <= 55) return 'Drizzle';
  if (code >= 61 && code <= 65) return 'Rainy';
  if (code >= 71 && code <= 77) return 'Snowy';
  if (code >= 80 && code <= 82) return 'Rain showers';
  if (code === 85 || code === 86) return 'Snow showers';
  if (code === 95) return 'Thunderstorm';
  if (code === 96 || code === 99) return 'Thunderstorm with hail';
  return 'Unknown';
}

/**
 * Builds a human-readable label for a geocoding result, e.g. "Austin, Texas, United States".
 * @param {{ name: string, admin1?: string, country?: string }} location
 * @returns {string}
 */
function locationLabel(location) {
  return [location.name, location.admin1, location.country].filter(Boolean).join(', ');
}

/**
 * Fetches weather data for the given city name.
 * If multiple distinct locations share the name, returns an ambiguous result
 * listing the options so the caller can ask the user to clarify.
 * Pass the 1-based `selectionIndex` on a follow-up call to resolve the ambiguity.
 *
 * @param {string} city - The city name to look up
 * @param {number} [selectionIndex] - 1-based index of the desired location from a previous ambiguous result
 * @returns {Promise<
 *   { city: string, forecast: Array<{ date: string, tempMaxC: number, tempMinC: number, conditions: string }> } |
 *   { ambiguous: true, options: string[] } |
 *   { error: string }
 * >}
 */
async function getWeather(city, selectionIndex) {
  // Step 1: Geocode the city name — request up to 5 results to detect ambiguity
  const geoUrl = `${GEOCODING_URL}?name=${encodeURIComponent(city)}&count=5&language=en&format=json`;
  const geoResponse = await fetch(geoUrl);

  if (!geoResponse.ok) {
    return { error: `Geocoding request failed with status ${geoResponse.status}` };
  }

  const geoData = await geoResponse.json();
  const results = geoData?.results;

  if (!results || results.length === 0) {
    return { error: `City "${city}" could not be found.` };
  }

  // If a selection index was provided, use that result directly
  if (selectionIndex !== undefined) {
    const idx = Number(selectionIndex) - 1;
    if (idx < 0 || idx >= results.length) {
      return { error: `Invalid selection. Please choose a number between 1 and ${results.length}.` };
    }
    const location = results[idx];
    const { latitude, longitude } = location;
    const resolvedCity = locationLabel(location);
    return fetchForecast(latitude, longitude, resolvedCity);
  }

  // If multiple distinct locations share the same name, ask the user to clarify
  if (results.length > 1) {
    const uniqueLabels = [...new Set(results.map(locationLabel))];
    if (uniqueLabels.length > 1) {
      return { ambiguous: true, city, options: uniqueLabels };
    }
  }

  const location = results[0];
  const { latitude, longitude } = location;
  const resolvedCity = locationLabel(location);

  return fetchForecast(latitude, longitude, resolvedCity);
}

/**
 * Fetches a 5-day forecast for the given coordinates and returns the structured result.
 * @param {number} latitude
 * @param {number} longitude
 * @param {string} resolvedCity
 */
async function fetchForecast(latitude, longitude, resolvedCity) {
  const forecastUrl =
    `${FORECAST_URL}?latitude=${latitude}&longitude=${longitude}` +
    '&daily=temperature_2m_max,temperature_2m_min,weathercode&timezone=auto&forecast_days=5';

  const forecastResponse = await fetch(forecastUrl);

  if (!forecastResponse.ok) {
    return { error: `Forecast request failed with status ${forecastResponse.status}` };
  }

  const forecastData = await forecastResponse.json();
  const daily = forecastData?.daily;

  if (!daily) {
    return { error: 'No forecast data returned by the weather service.' };
  }

  // Guard against inconsistent list lengths from the API
  const count = Math.min(
    daily.time?.length ?? 0,
    daily.temperature_2m_max?.length ?? 0,
    daily.temperature_2m_min?.length ?? 0,
    daily.weathercode?.length ?? 0
  );

  const forecast = Array.from({ length: count }, (_, i) => ({
    date: daily.time[i],
    tempMaxC: Math.round(daily.temperature_2m_max[i]),
    tempMinC: Math.round(daily.temperature_2m_min[i]),
    conditions: wmoCodeToSummary(daily.weathercode[i]),
  }));

  return { city: resolvedCity, forecast };
}

module.exports = { getWeather };
