namespace ToDoList.Frontend.Clients;

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
            IsCompleted = dto.IsCompleted
        }).ToList();
    }

    public async Task<ToDoItemView?> ReadItemByIdAsync(int id)
    {
        var response = await httpClient
            .GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{id}");

        if (response is null)
        {
            return null;
        }

        return new ToDoItemView
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
            IsCompleted = response.IsCompleted
        };
    }

    public async Task<bool> UpdateAsync(ToDoItemView item)
    {
        var itemRequest = new ToDoItemUpdateRequestDto(
            item.Name,
            item.Description,
            item.IsCompleted,
            CategoryId: null
        );

        try
        {
            var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/ToDoItems/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
