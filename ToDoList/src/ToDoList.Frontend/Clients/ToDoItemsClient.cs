namespace ToDoList.Frontend.Clients;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Common;
using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var response = await httpClient
            .GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems")
            ?? [];

        return response.Select(dto => new ToDoItemView
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            CategoryName = dto.CategoryName
        }).ToList();
    }

    public async Task<Result<ToDoItemView>> ReadItemByIdAsync(int id)
    {
        HttpResponseMessage response;

        try
        {
            response = await httpClient.GetAsync($"api/ToDoItems/{id}");
        }
        catch
        {
            return Result<ToDoItemView>.Fail("Nepodařilo se spojit se serverem.");
        }

        if (response.IsSuccessStatusCode)
        {
            var dto = await response.Content.ReadFromJsonAsync<ToDoItemGetResponseDto>();

            var item = new ToDoItemView
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                CategoryName = dto.CategoryName
            };

            return Result<ToDoItemView>.Ok(item);
        }
        else
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            return Result<ToDoItemView>.Fail(problem?.Detail ?? "Neznámá chyba.");
        }
    }

    public async Task<Result<bool>> UpdateAsync(ToDoItemView item)
    {
        var itemRequest = new ToDoItemUpdateRequestDto(
            item.Name,
            item.Description,
            item.IsCompleted,
            item.CategoryId
        );

        HttpResponseMessage response;

        try
        {
            response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
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

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        HttpResponseMessage response;

        try
        {
            response = await httpClient.DeleteAsync($"api/ToDoItems/{id}");
        }
        catch
        {
            return Result<bool>.Fail("Nepodařilo se spojit se serverem.");
        }

        if (response.IsSuccessStatusCode)
            return Result<bool>.Ok();

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        return Result<bool>.Fail(problem?.Detail ?? "Neznámá chyba.");
    }

    public async Task<Result<ToDoItemView>> CreateAsync(ToDoItemView item)
    {
        var itemRequest = new ToDoItemCreateRequestDto(
            item.Name,
            item.Description,
            item.IsCompleted,
            item.CategoryId
        );

        HttpResponseMessage response;

        try
        {
            response = await httpClient.PostAsJsonAsync("api/ToDoItems", itemRequest);
        }
        catch
        {
            return Result<ToDoItemView>.Fail("Nepodařilo se spojit se serverem.");
        }

        if (response.IsSuccessStatusCode)
        {
            var createdItem = await response.Content.ReadFromJsonAsync<ToDoItemGetResponseDto>();
            return Result<ToDoItemView>.Ok();
        }

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        return Result<ToDoItemView>.Fail(problem?.Detail ?? "Neznámá chyba.");
    }
}
