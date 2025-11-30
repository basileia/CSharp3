namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public class CategoryRepository(ToDoItemsContext context) : BaseRepository<Category>(context), ICategoryRepository
{
}
