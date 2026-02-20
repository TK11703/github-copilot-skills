namespace VerticalSlicesApi.Features.Todo;

public record CreateTodoRequest(string Title, string? Description);

public record UpdateTodoRequest(string Title, string? Description, bool IsCompleted);
