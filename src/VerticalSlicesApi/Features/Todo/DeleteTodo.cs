using Microsoft.Extensions.Caching.Memory;

namespace VerticalSlicesApi.Features.Todo;

public static class DeleteTodo
{
    public static IResult Handle(int id, IMemoryCache cache)
    {
        var items = TodoCacheHelper.GetItems(cache);
        var existing = items.FirstOrDefault(x => x.Id == id);
        if (existing is null)
        {
            return Results.NotFound($"Todo item with ID {id} was not found.");
        }

        var updated = items.Where(x => x.Id != id).ToList();
        TodoCacheHelper.SetItems(cache, updated);

        return Results.NoContent();
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/todos/{id:int}", Handle)
                .WithName("DeleteTodo")
                .WithSummary("Delete a todo item")
                .WithTags("Todo");
        }
    }
}
