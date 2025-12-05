namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<TEntity>(ToDoItemsContext context) : IBaseRepository<TEntity> where TEntity : class
{
    private readonly ToDoItemsContext context = context;
    private readonly DbSet<TEntity> dbSet = context.Set<TEntity>();

    public virtual async Task CreateAsync(TEntity entity)
    {
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> ReadAllAsync() =>
        await dbSet.AsNoTracking().ToListAsync();

    public virtual async Task UpdateAsync(TEntity entity)
    {
        context.Update(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }
}
