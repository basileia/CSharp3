namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class ToDoItemsControllerDeleteTests : ToDoItemsControllerTestBase
{
    [Fact]
    public async Task DeleteById_WhenItemExists_ReturnsNoContent_AndRemovesItem()
    {
        // Arrange
        var existingItem = await AddItemToDbAsync(CreateValidToDoItem("Úkol k odstranění", "Test", false));
        var controller = CreateController();

        // Act
        var result = await controller.DeleteById(existingItem.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var deletedItem = await GetItemFromDbAsync(existingItem.Id);
        Assert.Null(deletedItem);
    }

    [Fact]
    public async Task DeleteById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        const int nonExistentId = -1;
        var controller = CreateController();

        // Act
        var result = await controller.DeleteById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }

    [Fact]
    public async Task DeleteById_WhenCalledTwiceOnSameItem_SecondCallReturnsNotFound()
    {
        // Arrange
        var item = await AddItemToDbAsync(CreateValidToDoItem("Duplicitní smazání", "Test", false));
        var controller = CreateController();

        // Act
        var firstResult = await controller.DeleteById(item.Id);
        var secondResult = await controller.DeleteById(item.Id);

        // Assert
        Assert.IsType<NoContentResult>(firstResult);

        var notFoundResult = Assert.IsType<ObjectResult>(secondResult);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}
