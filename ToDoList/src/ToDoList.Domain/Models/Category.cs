namespace ToDoList.Domain.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<ToDoItem>? ToDoItems { get; set; }
}
