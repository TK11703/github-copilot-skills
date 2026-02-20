using Microsoft.Extensions.Caching.Memory;

namespace VerticalSlicesApi.Features.Todo;

public static class CreateTodo
{
    // Note: IDs are sequential and in-memory only; they reset on application restart.
    private static int _nextId;

    public record Request(string Title, string? Description);

    public record Response(int Id, string Title, string? Description, bool IsCompleted);

    public static IResult Handle(Request request, IMemoryCache cache)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest("Title is required.");
        }

        var items = TodoCacheHelper.GetItems(cache);
        var newItem = new TodoItem(Interlocked.Increment(ref _nextId), request.Title, request.Description, false);
        var updated = items.Append(newItem).ToList();
        TodoCacheHelper.SetItems(cache, updated);

        return Results.Created($"/todos/{newItem.Id}", new Response(newItem.Id, newItem.Title, newItem.Description, newItem.IsCompleted));
    }

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
