using Microsoft.Extensions.Caching.Memory;

namespace VerticalSlicesApi.Features.Todo;

public static class UpdateTodo
{
    public record Request(string Title, string? Description, bool IsCompleted);

    public record Response(int Id, string Title, string? Description, bool IsCompleted);

    public static IResult Handle(int id, Request request, IMemoryCache cache)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest("Title is required.");
        }

        var items = TodoCacheHelper.GetItems(cache);
        var existing = items.FirstOrDefault(x => x.Id == id);
        if (existing is null)
        {
            return Results.NotFound($"Todo item with ID {id} was not found.");
        }

        var updated = items
            .Select(x => x.Id == id
                ? new TodoItem(x.Id, request.Title, request.Description, request.IsCompleted)
                : x)
            .ToList();

        TodoCacheHelper.SetItems(cache, updated);

        var updatedItem = updated.First(x => x.Id == id);
        return Results.Ok(new Response(updatedItem.Id, updatedItem.Title, updatedItem.Description, updatedItem.IsCompleted));
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("/todos/{id:int}", Handle)
                .WithName("UpdateTodo")
                .WithSummary("Update an existing todo item")
                .WithTags("Todo");
        }
    }
}
