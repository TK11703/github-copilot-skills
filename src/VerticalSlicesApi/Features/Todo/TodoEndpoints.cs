using Microsoft.Extensions.Caching.Memory;

namespace VerticalSlicesApi.Features.Todo;

public static class TodoEndpoints
{
    private const string CacheKey = "todo_items";
    private static int _nextId = 0;

    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/todos").WithTags("Todo");

        group.MapGet("/", GetAllTodos)
            .WithName("GetAllTodos")
            .WithSummary("Get all todo items");

        group.MapGet("/{id:int}", GetTodoById)
            .WithName("GetTodoById")
            .WithSummary("Get a todo item by ID");

        group.MapPost("/", CreateTodo)
            .WithName("CreateTodo")
            .WithSummary("Create a new todo item");

        group.MapPut("/{id:int}", UpdateTodo)
            .WithName("UpdateTodo")
            .WithSummary("Update an existing todo item");

        group.MapDelete("/{id:int}", DeleteTodo)
            .WithName("DeleteTodo")
            .WithSummary("Delete a todo item");

        return app;
    }

    private static IResult GetAllTodos(IMemoryCache cache)
    {
        var items = GetCachedItems(cache);
        var responses = items.Select(MapToResponse).ToList();
        return Results.Ok(new TodoListResponse(responses, responses.Count));
    }

    private static IResult GetTodoById(int id, IMemoryCache cache)
    {
        var items = GetCachedItems(cache);
        var item = items.FirstOrDefault(x => x.Id == id);
        return item is null
            ? Results.NotFound($"Todo item with ID {id} was not found.")
            : Results.Ok(MapToResponse(item));
    }

    private static IResult CreateTodo(CreateTodoRequest request, IMemoryCache cache)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest("Title is required.");
        }

        var items = GetCachedItems(cache);
        var newItem = new TodoItem(Interlocked.Increment(ref _nextId), request.Title, request.Description, false);
        var updated = items.Append(newItem).ToList();
        SetCachedItems(cache, updated);

        return Results.Created($"/todos/{newItem.Id}", MapToResponse(newItem));
    }

    private static IResult UpdateTodo(int id, UpdateTodoRequest request, IMemoryCache cache)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest("Title is required.");
        }

        var items = GetCachedItems(cache);
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

        SetCachedItems(cache, updated);

        var updatedItem = updated.First(x => x.Id == id);
        return Results.Ok(MapToResponse(updatedItem));
    }

    private static IResult DeleteTodo(int id, IMemoryCache cache)
    {
        var items = GetCachedItems(cache);
        var existing = items.FirstOrDefault(x => x.Id == id);
        if (existing is null)
        {
            return Results.NotFound($"Todo item with ID {id} was not found.");
        }

        var updated = items.Where(x => x.Id != id).ToList();
        SetCachedItems(cache, updated);

        return Results.NoContent();
    }

    private static List<TodoItem> GetCachedItems(IMemoryCache cache)
    {
        return cache.GetOrCreate(CacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromHours(1);
            return new List<TodoItem>();
        }) ?? [];
    }

    private static void SetCachedItems(IMemoryCache cache, List<TodoItem> items)
    {
        cache.Set(CacheKey, items, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(1)
        });
    }

    private static TodoResponse MapToResponse(TodoItem item) =>
        new(item.Id, item.Title, item.Description, item.IsCompleted);
}
