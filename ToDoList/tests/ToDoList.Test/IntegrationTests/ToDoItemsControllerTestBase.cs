namespace ToDoList.Test.IntegrationTests;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Persistence;
using ToDoList.Domain.Mapping;

public abstract class ToDoItemsControllerTestBase : IDisposable
{
    protected IMapper Mapper { get; }
    protected ToDoItemsContext Context { get; }
    protected ToDoItemsController Controller { get; }
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

        Context = new ToDoItemsContext($"Data Source={dbPath}");
        Context.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        Mapper = config.CreateMapper();

        Controller = new ToDoItemsController(Mapper, Context);
    }

    protected ToDoItem AddItemToDb(ToDoItem item)
    {
        Context.ToDoItems.Add(item);
        Context.SaveChanges();
        return item;
    }

    protected ToDoItem? GetItemFromDb(int id) =>
        Context.ToDoItems.Find(id);

    protected void RemoveItemFromDb(int id)
    {
        var item = Context.ToDoItems.Find(id);
        if (item != null)
        {
            Context.ToDoItems.Remove(item);
            Context.SaveChanges();
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
        Context.Database.ExecuteSqlRaw("DELETE FROM ToDoItems;");
        GC.SuppressFinalize(this);
    }
}

