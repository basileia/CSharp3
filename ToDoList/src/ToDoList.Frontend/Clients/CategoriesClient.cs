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

    public async Task<Result<bool>> DeleteCategoryAsync(int id)
    {
        HttpResponseMessage response;

        try
        {
            response = await httpClient.DeleteAsync($"api/Categories/{id}");
        }
        catch
        {
            return Result<bool>.Fail("Nepodařilo se spojit se serverem.");
        }

        if (response.IsSuccessStatusCode)
        {
            return Result<bool>.Ok();
        }

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        return Result<bool>.Fail(problem?.Detail ?? "Neznámá chyba.");
    }

    public async Task<Result<bool>> UpdateAsync(CategoryView category)
    {
        var categoryRequest = new CategoryUpdateRequestDto(
                    category.Name
                );

        HttpResponseMessage response;

        try
        {
            response = await httpClient.PutAsJsonAsync($"api/Categories/{category.Id}", categoryRequest);
        }
        catch
        {
            return Result<bool>.Fail("Nepodařilo se spojit se serverem.");
        }

        if (response.IsSuccessStatusCode)
        {
            return Result<bool>.Ok();
        }

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        return Result<bool>.Fail(problem?.Detail ?? "Neznámá chyba.");
    }

    public async Task<Result<CategoryView>> ReadCategoryByIdAsync(int id)
    {
        HttpResponseMessage response;

        try
        {
            response = await httpClient.GetAsync($"api/Categories/{id}");
        }
        catch
        {
            return Result<CategoryView>.Fail("Nepodařilo se spojit se serverem.");
        }

        if (response.IsSuccessStatusCode)
        {
            var dto = await response.Content.ReadFromJsonAsync<CategoryGetResponseDto>();

            var category = new CategoryView
            {
                Name = dto.Name,
                Id = dto.Id
            };

            return Result<CategoryView>.Ok(category);
        }
        else
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            return Result<CategoryView>.Fail(problem?.Detail ?? "Neznámá chyba.");
        }
    }
}
