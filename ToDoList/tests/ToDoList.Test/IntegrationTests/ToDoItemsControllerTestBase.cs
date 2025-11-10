namespace ToDoList.Test.IntegrationTests;

using AutoMapper;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Persistence;
using ToDoList.Domain.Mapping;
using ToDoList.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

public abstract class ToDoItemsControllerTestBase : IDisposable
{
    protected IMapper Mapper { get; }
    protected IRepository<ToDoItem> Repository { get; }
    protected ToDoItemsController Controller { get; }
    protected ToDoItemsContext DbContext { get; }
    private readonly string dbPath = "../../../IntegrationTests/data/localdb_test.db";

    protected ToDoItemsControllerTestBase()
    {
        string className = GetType().Name;
        dbPath = $"../../../IntegrationTests/data/localdb_test_{className}.db";

        string? folder = Path.GetDirectoryName(dbPath);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder!);
        }

        DbContext = new ToDoItemsContext($"Data Source={dbPath}");
        DbContext.Database.EnsureCreated();

        Repository = new ToDoItemsRepository(DbContext);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        Mapper = config.CreateMapper();

        Controller = new ToDoItemsController(Mapper, Repository);
    }

    protected ToDoItem AddItemToDb(ToDoItem item)
    {
        Repository.Create(item);
        DbContext.Entry(item).State = EntityState.Detached;
        return item;
    }

    protected ToDoItem? GetItemFromDb(int id) =>
        Repository.ReadById(id);

    protected void RemoveItemFromDb(int id)
    {
        var item = Repository.ReadById(id);
        if (item != null)
        {
            Repository.Delete(item.ToDoItemId);
        }
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

    protected static ToDoItem CreateValidToDoItem(
        string name = "Test Item",
        string description = "Test Description",
        bool isCompleted = true)
    {
        return new ToDoItem
        {
            Name = name,
            Description = description,
            IsCompleted = isCompleted
        };
    }

    public void Dispose()
    {
        var items = Repository.Read();
        foreach (var item in items)
        {
            Repository.Delete(item.ToDoItemId);
        }

        GC.SuppressFinalize(this);
    }
}

