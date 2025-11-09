namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class ToDoItemsControllerDeleteTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void DeleteById_WhenItemExists_ReturnsNoContent_AndRemovesItem()
    {
        // Arrange
        var existingItem = AddItemToDb(CreateValidToDoItem("Úkol k odstranění", "Test", false));

        // Act
        var result = Controller.DeleteById(existingItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var deletedItem = GetItemFromDb(existingItem.ToDoItemId);
        Assert.Null(deletedItem);
    }

    [Fact]
    public void DeleteById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        const int nonExistentId = -1;

        // Act
        var result = Controller.DeleteById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);

        var items = Repository.Read();
        Assert.All(items, i => Assert.NotEqual(nonExistentId, i.ToDoItemId));
    }

    [Fact]
    public void DeleteById_WhenCalledTwiceOnSameItem_SecondCallReturnsNotFound()
    {
        // Arrange
        var item = AddItemToDb(CreateValidToDoItem("Duplicitní smazání", "Test", false));

        // Act
        var firstResult = Controller.DeleteById(item.ToDoItemId);
        var secondResult = Controller.DeleteById(item.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(firstResult);

        var notFoundResult = Assert.IsType<ObjectResult>(secondResult);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}
