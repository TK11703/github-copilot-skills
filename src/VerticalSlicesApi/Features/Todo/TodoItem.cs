namespace VerticalSlicesApi.Features.Todo;

public record TodoItem(int Id, string Title, string? Description, bool IsCompleted);
