using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;

namespace ToDoList.Test.UnitTests;

public class ToDoItemsControllerDeleteTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void DeleteById_WhenItemExists_ReturnsNoContent()
    {
        // Arrange
        int existingId = 1;
        var existingItem = CreateValidToDoItem(id: existingId);
        RepositoryMock.ReadById(existingId).Returns(existingItem);

        // Act
        var result = Controller.DeleteById(existingId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        RepositoryMock.Received(1).ReadById(existingId);
        RepositoryMock.Received(1).Delete(existingId);
    }

    [Fact]
    public void DeleteById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        RepositoryMock.ReadById(nonExistentId).Returns((ToDoItem?)null);

        // Act
        var result = Controller.DeleteById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);

        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Contains($"Úkol s ID {nonExistentId} nebyl nalezen", problemDetails.Detail);

        RepositoryMock.DidNotReceive().Delete(Arg.Any<int>());
    }

    [Fact]
    public void DeleteById_WhenDeleteThrows_ReturnsInternalServerError()
    {
        // Arrange
        int existingId = 1;
        var existingItem = CreateValidToDoItem(id: existingId);

        RepositoryMock.ReadById(existingId).Returns(existingItem);
        RepositoryMock
            .When(r => r.Delete(existingId))
            .Do(_ => throw new Exception("Database error"));

        // Act
        var result = Controller.DeleteById(existingId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    public void DeleteById_WhenReadThrows_ReturnsInternalServerError()
    {
        // Arrange
        int anyId = 1;

        RepositoryMock
            .When(r => r.ReadById(anyId))
            .Do(_ => throw new Exception("Database error"));

        // Act
        var result = Controller.DeleteById(anyId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        RepositoryMock.DidNotReceive().Delete(Arg.Any<int>());
    }
}
