namespace ToDoList.Domain.DTOs;

public record ToDoItemGetResponseDto(
    int Id,
    string Name,
    string Description,
    bool IsCompleted,
    int? CategoryId,
    string? CategoryName);
