namespace ToDoList.Frontend.Models;

using System.ComponentModel.DataAnnotations;

public class CategoryCreateModel
{
    [Required(ErrorMessage = "Název kategorie je povinný.")]
    [StringLength(100, ErrorMessage = "Název může mít maximálně 100 znaků.")]
    public string Name { get; set; }
}
