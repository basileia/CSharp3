using AutoMapper;
using Moq;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

namespace ToDoList.Test;

public abstract class ToDoItemsControllerTestBase
{
    protected readonly Mock<IMapper> MapperMock;
    protected readonly ToDoItemsController Controller;

    protected ToDoItemsControllerTestBase()
    {
        MapperMock = new Mock<IMapper>();
        Controller = new ToDoItemsController(MapperMock.Object);
    }

    protected static ToDoItemCreateRequestDto CreateValidCreateDto(
        string name = "Test Task",
        string description = "Test Description",
        bool isCompleted = false)
    {
        return new ToDoItemCreateRequestDto(name, description, isCompleted);
    }

    protected static ToDoItemUpdateRequestDto CreateValidUpdateDto(
        string name = "Updated Task",
        string description = "Updated Description",
        bool isCompleted = true)
    {
        return new ToDoItemUpdateRequestDto(name, description, isCompleted);
    }

    protected static ToDoItemGetResponseDto CreateValidGetResponseDto(
        int id = 1,
        string name = "Test Item",
        string description = "Test Description",
        bool isCompleted = false)
    {
        return new ToDoItemGetResponseDto(id, name, description, isCompleted);
    }

    protected static List<ToDoItemGetResponseDto> CreateValidGetResponseDtoList()
    {
        return new List<ToDoItemGetResponseDto>
        {
            new ToDoItemGetResponseDto(1, "Nakoupit potraviny", "Koupit mléko, vejce a chléb", false),
            new ToDoItemGetResponseDto(2, "Uklidit kuchyň", "Uklidit nádobí a utřít stůl", true),
            new ToDoItemGetResponseDto(3, "Zavolat doktorovi", "Objednat se na kontrolu", false)
        };
    }

    protected static ToDoItem CreateValidToDoItem(
        int id = 1,
        string name = "Test Item",
        string description = "Test Description",
        bool isCompleted = false)
    {
        return new ToDoItem
        {
            ToDoItemId = id,
            Name = name,
            Description = description,
            IsCompleted = isCompleted
        };
    }
}
