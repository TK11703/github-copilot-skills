using VerticalSlicesApi.Features.Todo;
using VerticalSlicesApi.Features.Weather;

namespace VerticalSlicesApi;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWeatherEndpoints();
        app.MapTodoEndpoints();

        return app;
    }
}
