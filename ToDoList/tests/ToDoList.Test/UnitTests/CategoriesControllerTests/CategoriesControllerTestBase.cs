namespace ToDoList.Test.UnitTests.CategoriesControllerTests;

using AutoMapper;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public abstract class CategoriesControllerTestBase
{
    protected ICategoryRepository RepositoryMock { get; }
    protected IMapper Mapper { get; }

    protected CategoriesControllerTestBase()
    {
        RepositoryMock = Substitute.For<ICategoryRepository>();
        Mapper = Substitute.For<IMapper>();
    }

    protected CategoriesController CreateController() =>
        new(Mapper, RepositoryMock);

    protected static Category CreateCategory(
        string name = "Práce")
        => new()
        {
            Name = name,
            NormalizedName = Category.Normalize(name)
        };

    protected static CategoryCreateRequestDto CreateCreateDto(string name = "Práce")
        => new(name);

    protected static CategoryUpdateRequestDto CreateUpdateDto(string name = "Domácnost")
        => new(name);
}
