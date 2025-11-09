namespace ToDoList.Test.UnitTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
using NSubstitute;

public abstract class ToDoItemsControllerTestBase
{
    protected readonly IMapper MapperMock;
    protected readonly ToDoItemsController Controller;
    protected readonly IRepository<ToDoItem> RepositoryMock;

    protected ToDoItemsControllerTestBase()
    {
        MapperMock = Substitute.For<IMapper>();
        RepositoryMock = Substitute.For<IRepository<ToDoItem>>();
        Controller = new ToDoItemsController(MapperMock, RepositoryMock);
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
        string name = "Test Item",
        string description = "Test Description",
        bool isCompleted = false)
    {
        return new ToDoItem
        {
            Name = name,
            Description = description,
            IsCompleted = isCompleted
        };
    }

    protected int GetNextId()
    {
        var itemsField = typeof(ToDoItemsController)
            .GetField("items", BindingFlags.NonPublic | BindingFlags.Static);

        var currentItems = (List<ToDoItem>)itemsField.GetValue(null);
        return currentItems.Count > 0 ? currentItems.Max(x => x.ToDoItemId) + 1 : 1;
    }

    protected static List<ToDoItem> GetCurrentItems()
    {
        var itemsField = typeof(ToDoItemsController)
            .GetField("items", BindingFlags.NonPublic | BindingFlags.Static);

        return (List<ToDoItem>)itemsField.GetValue(null)!;
    }

    protected static void AddItem(ToDoItem item)
    {
        var items = GetCurrentItems();
        items.Add(item);
    }
}

