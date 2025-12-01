namespace ToDoList.Test.IntegrationTests;

using AutoMapper;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Persistence;
using ToDoList.Domain.Mapping;
using ToDoList.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

public abstract class ToDoItemsControllerTestBase : IAsyncDisposable
{
    protected IMapper Mapper { get; }
    protected IRepositoryAsync Repository { get; }
    protected ToDoItemsController Controller { get; }
    protected ICategoryRepository CategoryRepository { get; }
    protected ToDoItemsContext DbContext { get; }
    private readonly string dbPath = "../../../IntegrationTests/data/localdb_test.db";

    protected ToDoItemsControllerTestBase()
    {
        string className = GetType().Name;
        dbPath = $"../../../IntegrationTests/data/localdb_test_{className}_{Guid.NewGuid()}.db";

        string? folder = Path.GetDirectoryName(dbPath);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder!);
        }

        DbContext = new ToDoItemsContext($"Data Source={dbPath}");
        DbContext.Database.EnsureCreated();

        Repository = new ToDoItemsRepository(DbContext);
        CategoryRepository = new CategoryRepository(DbContext);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        Mapper = config.CreateMapper();

        Controller = new ToDoItemsController(Mapper, Repository, CategoryRepository);
    }

    protected ToDoItemsController CreateController()
    {
        DbContext.ChangeTracker.Clear();

        var repository = new ToDoItemsRepository(DbContext);
        var categoryRepository = new CategoryRepository(DbContext);
        return new ToDoItemsController(Mapper, repository, categoryRepository);
    }
    protected async Task<ToDoItem> AddItemToDbAsync(ToDoItem item)
    {
        await Repository.CreateAsync(item);
        DbContext.Entry(item).State = EntityState.Detached;
        return item;
    }

    protected async Task<ToDoItem?> GetItemFromDbAsync(int id) =>
        await Repository.ReadByIdIncludingCategoryAsync(id);

    protected async Task RemoveItemFromDbAsync(int id)
    {
        DbContext.ChangeTracker.Clear();

        var item = await Repository.ReadByIdIncludingCategoryAsync(id);
        if (item != null)
        {
            await Repository.DeleteAsync(item);
        }
    }

    protected static Category CreateValidCategory(string name = "Default Category")
        => new()
        {
            Name = name
        };

    protected async Task<Category> AddCategoryToDbAsync(Category category)
    {
        await CategoryRepository.CreateAsync(category);
        DbContext.Entry(category).State = EntityState.Detached;
        return category;
    }
    protected async Task RemoveCategoryFromDbAsync(int id)
    {
        DbContext.ChangeTracker.Clear();

        var entity = await CategoryRepository.ReadByIdAsync(id);
        if (entity != null)
        {
            await CategoryRepository.DeleteAsync(entity);
        }
    }

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

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
        if (File.Exists(dbPath))
        {
            File.Delete(dbPath);
        }
        GC.SuppressFinalize(this);
    }
}

