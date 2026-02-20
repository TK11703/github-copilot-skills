using Microsoft.Extensions.Caching.Memory;

namespace VerticalSlicesApi.Features.Todo;

public static class CreateTodoHandler
{
    // Note: IDs are sequential and in-memory only; they reset on application restart.
    private static int _nextId = 0;

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
}
