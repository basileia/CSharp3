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
    public void UpdateById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        var updateDto = CreateValidUpdateDto();

        RepositoryMock.ReadById(nonExistentId).Returns((ToDoItem?)null);

        var controller = CreateController();

        // Act
        var result = controller.UpdateById(nonExistentId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);

        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Contains($"Úkol s ID {nonExistentId} nebyl nalezen", problemDetails.Detail);

        MapperMock.DidNotReceive().Map(Arg.Any<ToDoItemUpdateRequestDto>(), Arg.Any<ToDoItem>());
        RepositoryMock.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void UpdateById_WithValidDto_ReturnsNoContent()
    {
        // Arrange
        var existingItem = CreateValidToDoItem();
        var updateDto = CreateValidUpdateDto();

        RepositoryMock.ReadById(existingItem.ToDoItemId).Returns(existingItem);

        var controller = CreateController();

        // Act
        var result = controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        MapperMock.Received(1).Map(updateDto, existingItem);
        RepositoryMock.Received(1).Update(existingItem);
    }

    [Fact]
    public void UpdateById_UpdatesItemValuesCorrectly()
    {
        // Arrange
        var existingItem = CreateValidToDoItem();
        var updateDto = CreateValidUpdateDto(
            name: "Aktualizovaný úkol",
            description: "Nový popis",
            isCompleted: true
        );

        RepositoryMock.ReadById(existingItem.ToDoItemId).Returns(existingItem);

        MapperMock.When(m => m.Map(updateDto, existingItem))
                  .Do(_ =>
                  {
                      existingItem.Name = updateDto.Name;
                      existingItem.Description = updateDto.Description;
                      existingItem.IsCompleted = updateDto.IsCompleted;
                  });

        var controller = CreateController();

        // Act
        var result = controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(updateDto.Name, existingItem.Name);
        Assert.Equal(updateDto.Description, existingItem.Description);
        Assert.Equal(updateDto.IsCompleted, existingItem.IsCompleted);

        RepositoryMock.Received(1).Update(existingItem);
    }

    [Fact]
    public void UpdateById_WhenRepositoryThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var existingItem = CreateValidToDoItem();
        var updateDto = CreateValidUpdateDto();

        RepositoryMock.ReadById(existingItem.ToDoItemId).Returns(existingItem);
        RepositoryMock
            .When(r => r.Update(existingItem))
            .Do(r => throw new Exception("Database error"));

        var controller = CreateController();

        // Act
        var result = controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public void UpdateById_WhenReadByIdThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        int itemId = 1;
        var updateDto = CreateValidUpdateDto();

        RepositoryMock.ReadById(itemId).Throws(new Exception("Database connection failed"));

        var controller = CreateController();

        // Act
        var result = controller.UpdateById(itemId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        RepositoryMock.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }
}
