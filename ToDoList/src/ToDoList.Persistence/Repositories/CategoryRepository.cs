namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class CategoryRepository(ToDoItemsContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    public async Task<bool> ExistsByNormalizedNameAsync(string normalizedName) => await dbSet.AnyAsync(c => c.NormalizedName == normalizedName);

    public async Task<Category?> ReadByIdAsync(int id) =>
    await dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
}
