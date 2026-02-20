using Microsoft.Extensions.Caching.Memory;

namespace VerticalSlicesApi.Features.Todo;

public static class GetAllTodos
{
    public record Response(int Id, string Title, string? Description, bool IsCompleted);

    public record ListResponse(IReadOnlyList<Response> Items, int TotalCount);

    public static IResult Handle(IMemoryCache cache)
    {
        var items = TodoCacheHelper.GetItems(cache);
        var responses = items.Select(x => new Response(x.Id, x.Title, x.Description, x.IsCompleted)).ToList();
        return Results.Ok(new ListResponse(responses, responses.Count));
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/todos", Handle)
                .WithName("GetAllTodos")
                .WithSummary("Get all todo items")
                .WithTags("Todo");
        }
    }
}
