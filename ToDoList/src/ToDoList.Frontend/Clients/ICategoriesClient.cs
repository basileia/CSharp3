namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.Common;
using ToDoList.Frontend.Models;

public interface ICategoriesClient
{
    public Task<List<CategoryView>> GetCategoriesAsync();
    public Task<Result<string>> CreateCategoryAsync(CategoryCreateModel category);
    public Task<Result<bool>> DeleteCategoryAsync(int id);
    public Task<Result<bool>> UpdateAsync(CategoryView category);
    public Task<Result<CategoryView>> ReadCategoryByIdAsync(int id);
}
