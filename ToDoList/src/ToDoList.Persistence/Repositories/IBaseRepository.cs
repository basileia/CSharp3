namespace ToDoList.Persistence.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    public Task CreateAsync(TEntity entity);
    public Task<IEnumerable<TEntity>> ReadAllAsync();
    public Task<TEntity?> ReadByIdAsync(int id);
    public Task UpdateAsync(TEntity entity);
    public Task DeleteAsync(TEntity entity);
}
