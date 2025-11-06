namespace ToDoList.Persistence.Repositories;


using ToDoList.Domain.Models;

public interface IRepository<T> where T : class
{
    public void Create(ToDoItem item);
    public IEnumerable<ToDoItem> Read();
    public ToDoItem? ReadById(int id);
    public void Update(ToDoItem item);
    public void Delete(int id);
}


