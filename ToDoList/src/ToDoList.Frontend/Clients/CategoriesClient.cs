namespace ToDoList.Frontend.Clients;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Domain.Common;
using ToDoList.Frontend.Models;

public class CategoriesClient(HttpClient httpClient) : ICategoriesClient
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<List<CategoryView>> GetCategoriesAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<Category>>("api/Categories")
                       ?? [];

        return response.Select(c => new CategoryView
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    }

    public async Task<Result<string>> CreateCategoryAsync(CategoryCreateModel category)
    {
        var dto = new CategoryCreateRequestDto(category.Name);

        HttpResponseMessage response;

        try
        {
            response = await httpClient.PostAsJsonAsync("api/Categories", dto);
        }
        catch
        {
            return Result<string>.Fail("Nepodařilo se spojit se serverem.");
        }

        if (response.IsSuccessStatusCode)
        {
            return Result<string>.Ok();
        }

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        return Result<string>.Fail(problem?.Detail ?? "Neznámá chyba.");
    }
}
