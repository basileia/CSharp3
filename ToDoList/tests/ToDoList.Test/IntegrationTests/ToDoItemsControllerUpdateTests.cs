namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class ToDoItemsControllerUpdateTests : ToDoItemsControllerTestBase
{
    [Fact]
    public async Task UpdateById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = -1;
        var updateDto = CreateValidUpdateDto();
        var controller = CreateController();

        // Act
        var result = await controller.UpdateById(nonExistentId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }

    [Fact]
    public async Task UpdateById_WithValidDto_ReturnsNoContentAndUpdatesItem()
    {
        // Arrange
        var existingItem = await AddItemToDbAsync(CreateValidToDoItem(
            name: "Původní úkol",
            description: "Původní popis",
            isCompleted: false));

        var updateDto = CreateValidUpdateDto(
            name: "Aktualizovaný úkol",
            description: "Nový popis",
            isCompleted: true);

        var controller = CreateController();

        // Act
        var result = await controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var updatedItem = await GetItemFromDbAsync(existingItem.ToDoItemId);
        Assert.NotNull(updatedItem);
        Assert.Equal(updateDto.Name, updatedItem.Name);
        Assert.Equal(updateDto.Description, updatedItem.Description);
        Assert.Equal(updateDto.IsCompleted, updatedItem.IsCompleted);

        // Cleanup
        await RemoveItemFromDbAsync(existingItem.ToDoItemId);
    }

    [Fact]
    public async Task UpdateById_WithInvalidDto_ReturnsObjectResult500()
    {
        // Arrange
        var existingItem = await AddItemToDbAsync(CreateValidToDoItem());
        var invalidDto = new ToDoItemUpdateRequestDto(
            Name: null!,
            Description: "Popis",
            IsCompleted: true);

        var controller = CreateController();

        // Act
        var result = await controller.UpdateById(existingItem.ToDoItemId, invalidDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);

        // Cleanup
        await RemoveItemFromDbAsync(existingItem.ToDoItemId);
    }
}
