namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly ToDoItemsContext context;
    protected readonly DbSet<TEntity> dbSet;

    public BaseRepository(ToDoItemsContext context)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
    }

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
