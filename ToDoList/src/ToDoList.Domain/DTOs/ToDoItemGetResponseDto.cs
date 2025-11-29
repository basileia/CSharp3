namespace ToDoList.Domain.DTOs;

public record ToDoItemGetResponseDto(
    int ToDoItemId,
    string Name,
    string Description,
    bool IsCompleted,
    int? CategoryId,
    string? CategoryName);
