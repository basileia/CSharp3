namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext context) : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext context = context;
    private readonly DbSet<ToDoItem> dbSet = context.Set<ToDoItem>();

    public async Task CreateAsync(ToDoItem item)
    {
        await dbSet.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoItem>> ReadAllAsync() =>
    await dbSet
        .AsNoTracking()
        .Include(t => t.Category)
        .ToListAsync();

    public async Task<ToDoItem?> ReadByIdAsync(int id) =>
    await dbSet
        .AsNoTracking()
        .Include(t => t.Category)
        .FirstOrDefaultAsync(item => item.Id == id);

    public async Task UpdateAsync(ToDoItem item)
    {
        context.Update(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"ToDoItem with ID {id} not found");

        if (item != null)
        {
            dbSet.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}

