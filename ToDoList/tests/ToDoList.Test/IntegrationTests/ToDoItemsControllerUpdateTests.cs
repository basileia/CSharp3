namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class ToDoItemsControllerUpdateTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void UpdateById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = -1;
        var updateDto = CreateValidUpdateDto();

        // Act
        var result = Controller.UpdateById(nonExistentId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }

    [Fact]
    public void UpdateById_WithValidDto_ReturnsNoContentAndUpdatesItem()
    {
        // Arrange
        var existingItem = AddItemToDb(CreateValidToDoItem(
            name: "Původní úkol",
            description: "Původní popis",
            isCompleted: false));

        var updateDto = CreateValidUpdateDto(
            name: "Aktualizovaný úkol",
            description: "Nový popis",
            isCompleted: true);

        // Act
        var result = Controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var updatedItem = GetItemFromDb(existingItem.ToDoItemId);
        Assert.NotNull(updatedItem);
        Assert.Equal(updateDto.Name, updatedItem.Name);
        Assert.Equal(updateDto.Description, updatedItem.Description);
        Assert.Equal(updateDto.IsCompleted, updatedItem.IsCompleted);

        // Cleanup
        RemoveItemFromDb(existingItem.ToDoItemId);
    }

    [Fact]
    public void UpdateById_WithInvalidDto_ReturnsObjectResult500()
    {
        // Arrange
        var existingItem = AddItemToDb(CreateValidToDoItem());
        var invalidDto = new ToDoItemUpdateRequestDto(
            Name: null!,
            Description: "Popis",
            IsCompleted: true);

        // Act
        var result = Controller.UpdateById(existingItem.ToDoItemId, invalidDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);

        // Cleanup
        RemoveItemFromDb(existingItem.ToDoItemId);
    }
}
