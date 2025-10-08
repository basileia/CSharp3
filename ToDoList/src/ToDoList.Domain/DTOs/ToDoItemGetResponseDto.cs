using System;

namespace ToDoList.Domain.DTOs;

public record ToDoItemGetResponseDto()
{
    public int ToDoItemId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsCompleted { get; init; }
}
