namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public interface IRepositoryAsync : IBaseRepository<ToDoItem>
{
    public Task<IEnumerable<ToDoItem>> ReadAllIncludingCategoryAsync();
    public Task<ToDoItem?> ReadByIdIncludingCategoryAsync(int id);
}


