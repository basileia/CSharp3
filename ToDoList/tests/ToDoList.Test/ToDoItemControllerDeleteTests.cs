using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Test;

public class ToDoItemControllerDeleteTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void DeleteById_WhenItemExists_ReturnsNoContent()
    {
        // Arrange
        int nextId = GetNextId();

        var existingItem = CreateValidToDoItem(nextId, "Testovací úkol");
        AddItem(existingItem);

        // Act
        var result = Controller.DeleteById(existingItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var updatedItems = GetCurrentItems();
        Assert.DoesNotContain(updatedItems, x => x.ToDoItemId == existingItem.ToDoItemId);
    }

    [Fact]
    public void DeleteById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = GetNextId() + 100;

        // Act
        var result = Controller.DeleteById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        var items = GetCurrentItems();
        Assert.All(items, i => Assert.NotEqual(nonExistentId, i.ToDoItemId));
    }
}
