namespace ToDoList.Domain.DTOs;

public record ToDoItemUpdateRequestDto()
{
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsCompleted { get; init; }
}
