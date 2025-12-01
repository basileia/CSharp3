namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class ToDoItemControllerGetTests : ToDoItemsControllerTestBase
{
    [Fact]
    public async Task Read_ReturnsAllItems()
    {
        // Arrange
        var item1 = await AddItemToDbAsync(CreateValidToDoItem(name: "Task 1"));
        var item2 = await AddItemToDbAsync(CreateValidToDoItem(name: "Task 2"));

        var controller = CreateController();

        // Act
        var result = await controller.Read();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDtos = Assert.IsType<List<ToDoItemGetResponseDto>>(okResult.Value);

        Assert.NotEmpty(actualDtos);
        Assert.Contains(actualDtos, dto => dto.Name == "Task 1");
        Assert.Contains(actualDtos, dto => dto.Name == "Task 2");

        // Cleanup
        await RemoveItemFromDbAsync(item1.Id);
        await RemoveItemFromDbAsync(item2.Id);
    }

    [Fact]
    public async Task ReadById_WhenItemExists_ReturnsOkWithMappedItem()
    {
        // Arrange
        var newItem = await AddItemToDbAsync(CreateValidToDoItem(name: "Integration Test Item"));

        var controller = CreateController();

        // Act
        var result = await controller.ReadById(newItem.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);

        Assert.Equal(newItem.Id, dto.Id);
        Assert.Equal(newItem.Name, dto.Name);
        Assert.Equal(newItem.Description, dto.Description);
        Assert.Equal(newItem.IsCompleted, dto.IsCompleted);

        // Cleanup
        await RemoveItemFromDbAsync(newItem.Id);
    }

    [Fact]
    public async Task ReadById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = -1;
        var controller = CreateController();

        // Act
        var result = await controller.ReadById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }

    [Theory]
    [InlineData("Buy milk", "Go to store")]
    [InlineData("Write code", "Finish integration test")]
    public async Task ReadById_WithValidItems_ReturnsOkWithCorrectData(string name, string description)
    {
        // Arrange
        var newItem = await AddItemToDbAsync(CreateValidToDoItem(name: name, description: description));
        var controller = CreateController();

        // Act
        var result = await controller.ReadById(newItem.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);

        Assert.Equal(newItem.Id, dto.Id);
        Assert.Equal(name, dto.Name);
        Assert.Equal(description, dto.Description);
        Assert.Equal(newItem.IsCompleted, dto.IsCompleted);

        // Cleanup
        await RemoveItemFromDbAsync(newItem.Id);
    }
}
