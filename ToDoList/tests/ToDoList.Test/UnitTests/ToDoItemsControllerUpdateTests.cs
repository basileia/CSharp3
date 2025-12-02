using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

namespace ToDoList.Test.UnitTests;

public class ToDoItemsControllerUpdateTests : ToDoItemsControllerTestBase
{
    [Fact]
    public async Task UpdateById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        var updateDto = CreateValidUpdateDto();

        RepositoryMock.ReadByIdAsync(nonExistentId).Returns(Task.FromResult<ToDoItem?>(null));
        var controller = CreateController();

        // Act
        var result = await controller.UpdateById(nonExistentId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);

        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Contains($"Úkol s ID {nonExistentId} nebyl nalezen", problemDetails.Detail);

        MapperMock.DidNotReceive().Map(Arg.Any<ToDoItemUpdateRequestDto>(), Arg.Any<ToDoItem>());
        await RepositoryMock.DidNotReceive().UpdateAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task UpdateById_WithValidDto_ReturnsNoContent()
    {
        // Arrange
        var existingItem = CreateValidToDoItem();
        var updateDto = CreateValidUpdateDto();

        RepositoryMock.ReadByIdAsync(existingItem.ToDoItemId).Returns(Task.FromResult<ToDoItem?>(existingItem));

        var controller = CreateController();

        // Act
        var result = await controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        MapperMock.Received(1).Map(updateDto, existingItem);
        await RepositoryMock.Received(1).UpdateAsync(existingItem);
    }

    [Fact]
    public async Task UpdateById_UpdatesItemValuesCorrectly()
    {
        // Arrange
        var existingItem = CreateValidToDoItem();
        var updateDto = CreateValidUpdateDto(
            name: "Aktualizovaný úkol",
            description: "Nový popis",
            isCompleted: true
        );

        RepositoryMock.ReadByIdAsync(existingItem.ToDoItemId).Returns(Task.FromResult<ToDoItem?>(existingItem));

        MapperMock.When(m => m.Map(updateDto, existingItem))
                  .Do(_ =>
                  {
                      existingItem.Name = updateDto.Name;
                      existingItem.Description = updateDto.Description;
                      existingItem.IsCompleted = updateDto.IsCompleted;
                  });

        var controller = CreateController();

        // Act
        var result = await controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(updateDto.Name, existingItem.Name);
        Assert.Equal(updateDto.Description, existingItem.Description);
        Assert.Equal(updateDto.IsCompleted, existingItem.IsCompleted);

        await RepositoryMock.Received(1).UpdateAsync(existingItem);
    }

    [Fact]
    public async Task UpdateById_WhenRepositoryThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var existingItem = CreateValidToDoItem();
        var updateDto = CreateValidUpdateDto();

        RepositoryMock.ReadByIdAsync(existingItem.ToDoItemId).Returns(Task.FromResult<ToDoItem?>(existingItem));
        RepositoryMock
            .When(r => r.UpdateAsync(existingItem))
            .Do(r => throw new Exception("Database error"));

        var controller = CreateController();

        // Act
        var result = await controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task UpdateById_WhenReadByIdThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        int itemId = 1;
        var updateDto = CreateValidUpdateDto();

        RepositoryMock.ReadByIdAsync(itemId).ThrowsAsync(new Exception("Database connection failed"));

        var controller = CreateController();

        // Act
        var result = await controller.UpdateById(itemId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        await RepositoryMock.DidNotReceive().UpdateAsync(Arg.Any<ToDoItem>());
    }
}
