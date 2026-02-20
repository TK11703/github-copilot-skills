namespace VerticalSlicesApi.Features.Todo;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/todos").WithTags("Todo");

        group.MapGet("/", GetAllTodosHandler.Handle)
            .WithName("GetAllTodos")
            .WithSummary("Get all todo items");

        group.MapGet("/{id:int}", GetTodoByIdHandler.Handle)
            .WithName("GetTodoById")
            .WithSummary("Get a todo item by ID");

        group.MapPost("/", CreateTodoHandler.Handle)
            .WithName("CreateTodo")
            .WithSummary("Create a new todo item");

        group.MapPut("/{id:int}", UpdateTodoHandler.Handle)
            .WithName("UpdateTodo")
            .WithSummary("Update an existing todo item");

        group.MapDelete("/{id:int}", DeleteTodoHandler.Handle)
            .WithName("DeleteTodo")
            .WithSummary("Delete a todo item");

        return app;
    }
}

