namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class ToDoItemsControllerCreateTests : ToDoItemsControllerTestBase
{
    [Fact]
    public async Task Create_WithValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = CreateValidCreateDto();
        var controller = CreateController();

        // Act
        var result = await controller.Create(createDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdDto = Assert.IsType<ToDoItemGetResponseDto>(createdAtActionResult.Value);
        Assert.Equal(nameof(controller.ReadById), createdAtActionResult.ActionName);

        // Cleanup
        var createdItem = await Repository.ReadByIdAsync(createdDto.ToDoItemId);
        if (createdItem != null)
        {
            await RemoveItemFromDbAsync(createdItem.ToDoItemId);
        }
    }

    [Fact]
    public async Task Create_WithValidDto_ReturnsCreatedItemInResponse()
    {
        // Arrange
        var createDto = CreateValidCreateDto();
        var controller = CreateController();

        // Act
        var result = await controller.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var actualDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);

        Assert.Equal(createDto.Name, actualDto.Name);
        Assert.Equal(createDto.Description, actualDto.Description);
        Assert.Equal(createDto.IsCompleted, actualDto.IsCompleted);
        Assert.True(actualDto.ToDoItemId > 0);

        // Cleanup
        await RemoveItemFromDbAsync(actualDto.ToDoItemId);
    }

    [Fact]
    public async Task Create_WithNullName_ReturnsObjectResult500()
    {
        // Arrange
        var createDto = new ToDoItemCreateRequestDto(Name: null!, Description: "", IsCompleted: false);
        var controller = CreateController();

        // Act
        var result = await controller.Create(createDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
    }
}
