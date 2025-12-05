namespace ToDoList.Test.UnitTests;

using System.Collections.Generic;
using AutoMapper;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
using NSubstitute;

public abstract class ToDoItemsControllerTestBase
{
    protected IMapper MapperMock;
    protected IRepositoryAsync RepositoryMock;
    protected ICategoryRepository CategoryRepositoryMock;

    protected ToDoItemsControllerTestBase()
    {
        MapperMock = Substitute.For<IMapper>();
        RepositoryMock = Substitute.For<IRepositoryAsync>();
        CategoryRepositoryMock = Substitute.For<ICategoryRepository>();
    }

    protected ToDoItemsController CreateController()
        => new(MapperMock, RepositoryMock, CategoryRepositoryMock);

    protected static ToDoItemCreateRequestDto CreateValidCreateDto(
        string name = "Test Task",
        string description = "Test Description",
        bool isCompleted = false,
        int? categoryId = null)
    {
        return new ToDoItemCreateRequestDto(name, description, isCompleted, categoryId);
    }

    protected static ToDoItemUpdateRequestDto CreateValidUpdateDto(
        string name = "Updated Task",
        string description = "Updated Description",
        bool isCompleted = true,
        int? categoryId = null)
    {
        return new ToDoItemUpdateRequestDto(name, description, isCompleted, categoryId);
    }

    protected static ToDoItemGetResponseDto CreateValidGetResponseDto(
        int id = 1,
        string name = "Test Item",
        string description = "Test Description",
        bool isCompleted = false,
        int? categoryId = null,
        string? categoryName = null)
    {
        return new ToDoItemGetResponseDto(id, name, description, isCompleted, categoryId, categoryName);
    }

    protected static List<ToDoItemGetResponseDto> CreateValidGetResponseDtoList()
    {
        return new List<ToDoItemGetResponseDto>
        {
            new ToDoItemGetResponseDto(1, "Nakoupit potraviny", "Koupit mléko, vejce a chléb", false, null, null),
            new ToDoItemGetResponseDto(2, "Uklidit kuchyň", "Uklidit nádobí a utřít stůl", true, null, null),
            new ToDoItemGetResponseDto(3, "Zavolat doktorovi", "Objednat se na kontrolu", false, null, null)
        };
    }

    protected static ToDoItem CreateValidToDoItem(
        int id = 1,
        string name = "Test Item",
        string description = "Test Description",
        bool isCompleted = false,
        int? categoryId = null)
    {
        return new ToDoItem
        {
            Id = id,
            Name = name,
            Description = description,
            IsCompleted = isCompleted,
            CategoryId = categoryId
        };
    }
}

