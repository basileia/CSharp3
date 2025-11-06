namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext context) : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext context = context;
    private readonly DbSet<ToDoItem> dbSet = context.Set<ToDoItem>();

    public void Create(ToDoItem item)
    {
        dbSet.Add(item);
        context.SaveChanges();
    }

    public IEnumerable<ToDoItem> Read() => dbSet.AsNoTracking().ToList();
    public ToDoItem? ReadById(int id) => dbSet.AsNoTracking().FirstOrDefault(item => item.ToDoItemId == id);
}

