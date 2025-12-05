namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Models;

public interface IToDoItemsClient
{
    public Task<List<ToDoItemView>> ReadItemsAsync();
    public Task<ToDoItemView?> ReadItemByIdAsync(int id);
    public Task UpdateAsync(ToDoItemView item);
}
