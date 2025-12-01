using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;

namespace ToDoList.Test.UnitTests;

public class ToDoItemsControllerDeleteTests : ToDoItemsControllerTestBase
{
    [Fact]
    public async Task DeleteById_WhenItemExists_ReturnsNoContent()
    {
        // Arrange
        int existingId = 1;
        var existingItem = CreateValidToDoItem(id: existingId);
        RepositoryMock.ReadByIdIncludingCategoryAsync(existingId).Returns(Task.FromResult<ToDoItem?>(existingItem));

        var controller = CreateController();

        // Act
        var result = await controller.DeleteById(existingId);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        await RepositoryMock.Received(1).ReadByIdIncludingCategoryAsync(existingId);
        await RepositoryMock.Received(1).DeleteAsync(existingItem);
    }

    [Fact]
    public async Task DeleteById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        RepositoryMock.ReadByIdIncludingCategoryAsync(nonExistentId).Returns(Task.FromResult<ToDoItem?>(null));

        var controller = CreateController();

        // Act
        var result = await controller.DeleteById(nonExistentId);

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

        await RepositoryMock.DidNotReceive().DeleteAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task DeleteById_WhenDeleteThrows_ReturnsInternalServerError()
    {
        // Arrange
        int existingId = 1;
        var existingItem = CreateValidToDoItem(id: existingId);

        RepositoryMock.ReadByIdIncludingCategoryAsync(existingId).Returns(Task.FromResult<ToDoItem?>(existingItem));
        RepositoryMock
            .When(r => r.DeleteAsync(existingItem))
            .Do(_ => throw new Exception("Database error"));

        var controller = CreateController();

        // Act
        var result = await controller.DeleteById(existingId);

        // Assert
        var objectResult = result.Should()
        .BeOfType<ObjectResult>()
        .Which;

        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    public async Task DeleteById_WhenReadThrows_ReturnsInternalServerError()
    {
        // Arrange
        int anyId = 1;

        RepositoryMock
            .When(r => r.ReadByIdIncludingCategoryAsync(anyId))
            .Do(_ => throw new Exception("Database error"));

        var controller = CreateController();

        // Act
        var result = await controller.DeleteById(anyId);

        // Assert
        var objectResult = result.Should()
        .BeOfType<ObjectResult>()
        .Which;

        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        await RepositoryMock.DidNotReceive().DeleteAsync(Arg.Any<ToDoItem>());
    }
}
