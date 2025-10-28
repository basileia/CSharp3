namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;

public class ToDoItemsControllerCreateTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void Create_WithValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = CreateValidCreateDto();

        // Act
        var result = Controller.Create(createDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(Controller.ReadById), createdAtActionResult.ActionName);

        // Cleanup
        var createdItem = Context.ToDoItems.FirstOrDefault(x => x.Name == createDto.Name);
        if (createdItem != null)
        {
            RemoveItemFromDb(createdItem.ToDoItemId);
        }
    }

    [Fact]
    public void Create_WithValidDto_ReturnsCreatedItemInResponse()
    {
        // Arrange
        var createDto = CreateValidCreateDto();

        // Act
        var result = Controller.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var actualDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);

        Assert.Equal(createDto.Name, actualDto.Name);
        Assert.Equal(createDto.Description, actualDto.Description);
        Assert.Equal(createDto.IsCompleted, actualDto.IsCompleted);
        Assert.True(actualDto.ToDoItemId > 0);

        // Cleanup
        RemoveItemFromDb(actualDto.ToDoItemId);
    }

    [Fact]
    public void Create_WithNullName_ReturnsObjectResult500()
    {
        // Arrange
        var createDto = new ToDoItemCreateRequestDto(Name: null!, Description: "", IsCompleted: false);

        // Act
        var result = Controller.Create(createDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
    }
}
