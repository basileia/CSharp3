using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

namespace ToDoList.Test.UnitTests;

public class ToDoItemsControllerGetTests : ToDoItemsControllerTestBase
{
    [Fact]
    public async Task Read_ReturnsAllItems_WithCategory()
    {
        // Arrange
        var toDoItems = new List<ToDoItem>
            {
                new() { Id = 1, Name = "Nakoupit potraviny", Description = "Koupit mléko", IsCompleted = false },
                new() { Id = 2, Name = "Uklidit", Description = "Uklidit stůl", IsCompleted = true }
            };

        var expectedDtos = CreateValidGetResponseDtoList();

        RepositoryMock.ReadAllIncludingCategoryAsync().Returns(Task.FromResult<IEnumerable<ToDoItem>>(toDoItems));
        MapperMock.Map<IEnumerable<ToDoItemGetResponseDto>>(toDoItems).Returns(expectedDtos);

        var controller = CreateController();

        // Act
        var result = await controller.Read();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDtos = Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(okResult.Value);

        Assert.Equal(expectedDtos.Count, ((List<ToDoItemGetResponseDto>)actualDtos).Count);
        await RepositoryMock.Received(1).ReadAllIncludingCategoryAsync();
    }

    [Fact]
    public async Task Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var someItem = new ToDoItem { Name = "Some Task", Description = "Some Description", IsCompleted = false };
        RepositoryMock.ReadAllIncludingCategoryAsync().Returns(Task.FromResult<IEnumerable<ToDoItem>>([someItem]));
        MapperMock.Map<IEnumerable<ToDoItemGetResponseDto>>(Arg.Any<IEnumerable<ToDoItem>>())
                    .Returns(CreateValidGetResponseDtoList());

        var controller = CreateController();

        // Act
        var result = await controller.Read();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(okResult.Value);
        await RepositoryMock.Received(1).ReadAllIncludingCategoryAsync();
    }

    [Fact]
    public async Task Read_WhenRepositoryThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.ReadAllIncludingCategoryAsync().ThrowsAsync(new Exception("Unexpected error"));

        var controller = CreateController();

        // Act
        var result = await controller.Read();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task ReadById_WhenItemExists_ReturnsOkWithMappedItem()
    {
        // Arrange
        int itemId = 1;
        var toDoItem = CreateValidToDoItem();
        var expectedDto = CreateValidGetResponseDto(id: itemId);

        RepositoryMock.ReadByIdIncludingCategoryAsync(itemId).Returns(Task.FromResult<ToDoItem?>(toDoItem));
        MapperMock.Map<ToDoItemGetResponseDto>(toDoItem).Returns(expectedDto);

        var controller = CreateController();

        // Act
        var result = await controller.ReadById(itemId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);

        Assert.Equal(expectedDto.Id, actualDto.Id);
        Assert.Equal(expectedDto.Name, actualDto.Name);
        Assert.Equal(expectedDto.Description, actualDto.Description);
        Assert.Equal(expectedDto.IsCompleted, actualDto.IsCompleted);
        Assert.Equal(expectedDto.CategoryId, actualDto.CategoryId);
        Assert.Equal(expectedDto.CategoryName, actualDto.CategoryName);
    }

    [Fact]
    public async Task ReadById_WhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        RepositoryMock.ReadByIdIncludingCategoryAsync(nonExistentId).Returns(Task.FromResult<ToDoItem?>(null));

        var controller = CreateController();

        // Act
        var result = await controller.ReadById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }

    [Fact]
    public async Task ReadById_UnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        int id = 1;

        RepositoryMock
            .ReadByIdIncludingCategoryAsync(id)
            .ThrowsAsync(new Exception("Unexpected error"));

        var controller = CreateController();

        // Act
        var result = await controller.ReadById(id);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}
