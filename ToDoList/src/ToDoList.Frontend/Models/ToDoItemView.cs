namespace ToDoList.Frontend.Models;

using System.ComponentModel.DataAnnotations;
using ToDoList.Frontend.Components.Pages;

public class ToDoItemView
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Název je povinný")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Popis je povinný")]
    [StringLength(250, ErrorMessage = "Popis může mít maximálně 250 znaků")]
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
