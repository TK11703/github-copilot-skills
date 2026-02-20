using Microsoft.Extensions.Caching.Memory;

namespace VerticalSlicesApi.Features.Todo;

public static class GetTodoById
{
    public record Response(int Id, string Title, string? Description, bool IsCompleted);

    public static IResult Handle(int id, IMemoryCache cache)
    {
        var items = TodoCacheHelper.GetItems(cache);
        var item = items.FirstOrDefault(x => x.Id == id);
        return item is null
            ? Results.NotFound($"Todo item with ID {id} was not found.")
            : Results.Ok(new Response(item.Id, item.Title, item.Description, item.IsCompleted));
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/todos/{id:int}", Handle)
                .WithName("GetTodoById")
                .WithSummary("Get a todo item by ID")
                .WithTags("Todo");
        }
    }
}
