namespace ToDoList.Frontend.Models;

public class ToDoItemUpdate
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int? CategoryId { get; set; }
}
