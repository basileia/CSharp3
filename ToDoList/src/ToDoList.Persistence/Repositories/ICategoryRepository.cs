namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public interface ICategoryRepository : IBaseRepository<Category>
{
    public Task<bool> ExistsByNormalizedNameAsync(string normalizedName);
    public Task<Category?> ReadByIdAsync(int id);
}
