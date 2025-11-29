namespace ToDoList.Domain.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ToDoItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ToDoItemId { get; set; }
    [Length(1, 50)]
    public string Name { get; set; } = null!;
    [StringLength(250)]
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}

