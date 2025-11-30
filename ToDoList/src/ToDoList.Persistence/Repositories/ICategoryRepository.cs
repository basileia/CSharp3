namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public interface ICategoryRepository : IBaseRepository<Category>
{
    public Task<bool> ExistsByNormalizedNameAsync(string normalizedName);
}
