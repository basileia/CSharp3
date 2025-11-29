namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext context) : BaseRepository<ToDoItem>(context), IRepositoryAsync
{
    public async Task<IEnumerable<ToDoItem>> ReadAllIncludingCategoryAsync() =>
    await dbSet
        .AsNoTracking()
        .Include(t => t.Category)
        .ToListAsync();

    public async Task<ToDoItem?> ReadByIdIncludingCategoryAsync(int id) =>
    await dbSet
        .AsNoTracking()
        .Include(t => t.Category)
        .FirstOrDefaultAsync(item => item.Id == id);
}

