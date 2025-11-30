namespace ToDoList.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Length(1, 50)]
    public required string Name { get; set; }

    public ICollection<ToDoItem>? ToDoItems { get; set; }
}
