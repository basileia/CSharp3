namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext context) : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext context = context;
    private readonly DbSet<ToDoItem> dbSet = context.Set<ToDoItem>();

    public void Create(ToDoItem item)
    {
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    public IEnumerable<ToDoItem> ReadAll() => dbSet.AsNoTracking().ToList();

    public ToDoItem? ReadById(int id) => dbSet.AsNoTracking().FirstOrDefault(item => item.ToDoItemId == id);

    public void Update(ToDoItem item)
    {
        var foundItem = context.ToDoItems.Find(item.ToDoItemId) ?? throw new ArgumentOutOfRangeException("Item not found");
        context.Entry(item).CurrentValues.SetValues(item);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = dbSet.Find(id);
        if (item != null)
        {
            dbSet.Remove(item);
            context.SaveChanges();
        }
    }
}

