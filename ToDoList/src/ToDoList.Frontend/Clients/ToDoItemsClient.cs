namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient : IToDoItemsClient
{
    private readonly HttpClient httpClient;

    public ToDoItemsClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemViews = new List<ToDoItemView>();
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");

        toDoItemViews = response.Select(dto => new ToDoItemView
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        }).ToList();

        return toDoItemViews;
    }

    public async Task<ToDoItemView?> ReadItemByIdAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{id}");

        var toDoItemView = new ToDoItemView
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
            IsCompleted = response.IsCompleted
        };

        return toDoItemView;
    }

    public async Task UpdateAsync(ToDoItemView item)
    {
        // try{}
        var itemRequest = new ToDoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted, CategoryId: null);
        var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
    }

}
