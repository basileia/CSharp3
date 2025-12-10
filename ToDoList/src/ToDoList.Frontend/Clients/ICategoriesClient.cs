namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.Common;
using ToDoList.Frontend.Models;

public interface ICategoriesClient
{
    public Task<List<CategoryView>> GetCategoriesAsync();
    public Task<Result<string>> CreateCategoryAsync(CategoryCreateModel category);
}
