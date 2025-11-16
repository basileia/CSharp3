using FluentAssertions;
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

        var controller = CreateController();

        // Act
        var result = controller.DeleteById(existingId);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        RepositoryMock.Received(1).ReadById(existingId);
        RepositoryMock.Received(1).Delete(existingId);
    }

    [Fact]
    public void DeleteById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        RepositoryMock.ReadById(nonExistentId).Returns((ToDoItem?)null);

        var controller = CreateController();

        // Act
        var result = controller.DeleteById(nonExistentId);

        // Assert
        var objectResult = result.Should()
        .BeOfType<ObjectResult>()
        .Which;

        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var problemDetails = objectResult.Value.Should()
            .BeOfType<ProblemDetails>()
            .Which;

        problemDetails.Detail.Should()
            .Contain($"Úkol s ID {nonExistentId} nebyl nalezen");

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

        var controller = CreateController();

        // Act
        var result = controller.DeleteById(existingId);

        // Assert
        var objectResult = result.Should()
        .BeOfType<ObjectResult>()
        .Which;

        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    public void DeleteById_WhenReadThrows_ReturnsInternalServerError()
    {
        // Arrange
        int anyId = 1;

        RepositoryMock
            .When(r => r.ReadById(anyId))
            .Do(_ => throw new Exception("Database error"));

        var controller = CreateController();

        // Act
        var result = controller.DeleteById(anyId);

        // Assert
        var objectResult = result.Should()
        .BeOfType<ObjectResult>()
        .Which;

        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        RepositoryMock.DidNotReceive().Delete(Arg.Any<int>());
    }
}
