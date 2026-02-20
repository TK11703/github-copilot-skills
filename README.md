# github-copilot-skills
A repo that contains some C# .Net code and some JavaScript files. This repo will be used to learn and demonstrate some usages of GitHub Copilot skills.

---

## VerticalSlicesApi

A .NET 10 minimal API project built with a **vertical slices** architecture. No MVC controllers — all routing is discovered and registered automatically from the executing assembly via the `IEndpoint` interface.

### Project Structure

```
src/VerticalSlicesApi/
├── Program.cs
├── IEndpoint.cs                           ← endpoint interface
├── EndpointExtensions.cs                  ← assembly-scan registration + MapEndpoints
├── Features/
│   ├── Weather/
│   │   ├── GetWeatherForecast.cs          ← nested Response record + Handle() + Endpoint : IEndpoint
│   │   ├── WeatherService.cs              ← Open-Meteo HTTP client
│   │   └── weather.http
│   └── Todo/
│       ├── GetAllTodos.cs                 ← nested Response, ListResponse + Handle() + Endpoint : IEndpoint
│       ├── GetTodoById.cs                 ← nested Response + Handle() + Endpoint : IEndpoint
│       ├── CreateTodo.cs                  ← nested Request, Response + Handle() + Endpoint : IEndpoint
│       ├── UpdateTodo.cs                  ← nested Request, Response + Handle() + Endpoint : IEndpoint
│       ├── DeleteTodo.cs                  ← Handle() + Endpoint : IEndpoint
│       ├── TodoCacheHelper.cs             ← internal shared cache helpers
│       ├── TodoItem.cs
│       └── todo.http
```

### Features

- **Weather** — `GET /weather?city={city}` returns a real 5-day forecast by querying the [Open-Meteo](https://open-meteo.com/) API (free, no API key required). The city name is geocoded to coordinates, then daily max/min temperatures and WMO weather condition codes are fetched and mapped to human-readable summaries (e.g. `Partly cloudy`, `Rainy`, `Thunderstorm`).
- **Todo** — full CRUD (`GET`, `POST`, `PUT`, `DELETE`) over `/todos`; data persisted in `IMemoryCache` with a 1-hour sliding expiration; no database required

### Design Notes

- **`IEndpoint` interface** — declares a single `void MapEndpoint(IEndpointRouteBuilder app)` method. Any class implementing it is automatically discovered and registered.
- **Per-operation classes** — each endpoint operation is a `public static class` (e.g. `GetWeatherForecast`, `CreateTodo`) that owns its nested `Request`/`Response` records, a `Handle()` method, and a `public sealed class Endpoint : IEndpoint` that registers the route.
- **Assembly-scan registration** — `EndpointExtensions.AddEndpoints(Assembly)` scans the executing assembly for all `IEndpoint` implementations and registers them as singletons. `EndpointExtensions.MapEndpoints(WebApplication)` resolves and invokes each. Adding a new feature requires no changes outside the feature's own class.
- `WeatherService` is registered as a typed `HttpClient` (`AddHttpClient<WeatherService>()`) and injected directly into the handler.
- Weather responses include both daily high (`TemperatureC`/`TemperatureF`) and daily low (`TemperatureMinC`/`TemperatureMinF`).
- Weather endpoint returns `404` when the city cannot be geocoded and `503` when the upstream service is unavailable.
- Todo IDs use `Interlocked.Increment` for thread-safe generation.
- HTTP test files are included inside each feature folder for quick manual testing.

### Running the API

```bash
cd src/VerticalSlicesApi
dotnet run
# API available at http://localhost:5123
```

### Endpoint Summary

| Method   | Route             | Description                                  |
|----------|-------------------|----------------------------------------------|
| `GET`    | `/weather`        | Get a 5-day forecast — `?city=London`        |
| `GET`    | `/todos`          | Get all todo items                           |
| `GET`    | `/todos/{id}`     | Get a todo item by ID                        |
| `POST`   | `/todos`          | Create a new todo item                       |
| `PUT`    | `/todos/{id}`     | Update an existing todo item                 |
| `DELETE` | `/todos/{id}`     | Delete a todo item                           |

### Example: Wiring in `Program.cs`

```csharp
// Discover and register all IEndpoint implementations, then map them
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
// ...
app.MapEndpoints();
```

### Example: Operation class structure

```csharp
public static class CreateTodo
{
    public record Request(string Title, string? Description);
    public record Response(int Id, string Title, string? Description, bool IsCompleted);

    public static IResult Handle(Request request, IMemoryCache cache) { ... }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/todos", Handle)
                .WithName("CreateTodo")
                .WithSummary("Create a new todo item")
                .WithTags("Todo");
        }
    }
}
```

