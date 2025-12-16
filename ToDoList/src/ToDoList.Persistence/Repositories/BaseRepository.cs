namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<TEntity>(ToDoItemsContext context) : IBaseRepository<TEntity>
    where TEntity : class
{
    protected ToDoItemsContext Context { get; } = context;
    protected DbSet<TEntity> DbSet { get; } = context.Set<TEntity>();

    public virtual async Task CreateAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> ReadAllAsync() =>
        await DbSet.AsNoTracking().ToListAsync();

    public virtual async Task UpdateAsync(TEntity entity)
    {
        Context.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}
