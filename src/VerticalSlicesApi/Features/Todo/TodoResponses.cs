namespace VerticalSlicesApi.Features.Todo;

public record TodoResponse(int Id, string Title, string? Description, bool IsCompleted);

public record TodoListResponse(IReadOnlyList<TodoResponse> Items, int TotalCount);
