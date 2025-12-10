namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.Common;
using ToDoList.Frontend.Models;

public interface IToDoItemsClient
{
    public Task<List<ToDoItemView>> ReadItemsAsync();
    public Task<Result<ToDoItemView>> ReadItemByIdAsync(int id);
    public Task<Result<bool>> UpdateAsync(ToDoItemView item);
    public Task<Result<bool>> DeleteAsync(int id);
}
