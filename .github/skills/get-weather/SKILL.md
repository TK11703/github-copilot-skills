---
name: get-weather
description: Retrieves real-time weather information for a specified city. Triggered by natural language weather questions or the /weather command.
---

# Get Weather by City

This skill retrieves the current weather information for a specified city using a public weather API.

## Description

Executes an API call to fetch real-time weather data including temperature, conditions, and other meteorological information based on city name.

## Trigger Phrases

Use this skill whenever the user asks about current weather using phrases like (case-insensitive):
- `What's the weather in <city>?`
- `What's the weather in <city> right now?`
- `How's the weather in <city>?`
- `Weather in <city>`
- `/weather <cityName>` (command)

Extract the city name from the user's message to use as input.

## How It Works

1. Execute the `getWeather` function exported from [script](./scripts/get-weather-by-city.js).
2. Call: `getWeather("<extracted city name>")`
3. The script returns one of three JSON shapes — handle each as described below.

### Return shapes

**Success** — weather was found for an unambiguous city:
```json
{ "city": "Austin, Texas, United States", "forecast": [ ... ] }
```

**Ambiguous** — multiple distinct locations share the same name:
```json
{ "ambiguous": true, "options": ["Springfield, Illinois, United States", "Springfield, Missouri, United States", "..."] }
```

**Error** — city not found or API failure:
```json
{ "error": "City \"xyz\" could not be found." }
```

## Required Response

**On success:** Return output using the [template](./TEMPLATE.md)

**On ambiguous:** Ask the user to clarify which location they mean by listing the options:
"I found multiple cities named <name>. Which did you mean?"
Followed by a numbered list of the options from `result.options`.

**On error:** Report the error message from `result.error` and suggest the user try a more specific city name.

## Usage
