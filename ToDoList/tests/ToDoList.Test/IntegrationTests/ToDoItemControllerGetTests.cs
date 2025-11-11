namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class ToDoItemControllerGetTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void Read_ReturnsAllItems()
    {
        // Arrange
        var item1 = AddItemToDb(CreateValidToDoItem(name: "Task 1"));
        var item2 = AddItemToDb(CreateValidToDoItem(name: "Task 2"));

        // Act
        var result = Controller.Read();

        if (result is ObjectResult obj)
        {
            Console.WriteLine($"Status code: {obj.StatusCode}, value: {obj.Value}");
        }
        else
        {
            Console.WriteLine($"Result type: {result.GetType().Name}");
        }

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDtos = Assert.IsType<List<ToDoItemGetResponseDto>>(okResult.Value);

        Assert.NotEmpty(actualDtos);
        Assert.Contains(actualDtos, dto => dto.Name == "Task 1");
        Assert.Contains(actualDtos, dto => dto.Name == "Task 2");

        // Cleanup
        RemoveItemFromDb(item1.ToDoItemId);
        RemoveItemFromDb(item2.ToDoItemId);
    }

    [Fact]
    public void ReadById_WhenItemExists_ReturnsOkWithMappedItem()
    {
        // Arrange
        var newItem = AddItemToDb(CreateValidToDoItem(name: "Integration Test Item"));

        // Act
        var result = Controller.ReadById(newItem.ToDoItemId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);

        Assert.Equal(newItem.ToDoItemId, dto.ToDoItemId);
        Assert.Equal(newItem.Name, dto.Name);
        Assert.Equal(newItem.Description, dto.Description);
        Assert.Equal(newItem.IsCompleted, dto.IsCompleted);

        // Cleanup
        RemoveItemFromDb(newItem.ToDoItemId);
    }

    [Fact]
    public void ReadById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = -1;

        // Act
        var result = Controller.ReadById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }

    [Theory]
    [InlineData("Buy milk", "Go to store")]
    [InlineData("Write code", "Finish integration test")]
    public void ReadById_WithValidItems_ReturnsOkWithCorrectData(string name, string description)
    {
        // Arrange
        var newItem = AddItemToDb(CreateValidToDoItem(name: name, description: description));

        // Act
        var result = Controller.ReadById(newItem.ToDoItemId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);

        Assert.Equal(newItem.ToDoItemId, dto.ToDoItemId);
        Assert.Equal(name, dto.Name);
        Assert.Equal(description, dto.Description);
        Assert.Equal(newItem.IsCompleted, dto.IsCompleted);

        // Cleanup
        RemoveItemFromDb(newItem.ToDoItemId);
    }
}
