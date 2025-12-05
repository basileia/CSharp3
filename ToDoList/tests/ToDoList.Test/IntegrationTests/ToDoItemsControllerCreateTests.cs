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
        await RemoveItemFromDbAsync(createdDto.Id);
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
        Assert.True(actualDto.Id > 0);

        // Cleanup
        await RemoveItemFromDbAsync(actualDto.Id);
    }

    [Fact]
    public async Task Create_WithNullName_ReturnsObjectResult500()
    {
        // Arrange
        var createDto = new ToDoItemCreateRequestDto(Name: null!, Description: "", IsCompleted: false, CategoryId: null);
        var controller = CreateController();

        // Act
        var result = await controller.Create(createDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
    }

    [Fact]
    public async Task Create_WithExistingCategory_ReturnsCreatedItem()
    {
        // Arrange
        var category = CreateValidCategory(name: "Práce");
        await AddCategoryToDbAsync(category);

        var createDto = CreateValidCreateDto(
            name: "Úkol s kategorií",
            description: "Test popis",
            isCompleted: false,
            categoryId: category.Id
        );

        var controller = CreateController();

        // Act
        var result = await controller.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);

        Assert.Equal(createDto.Name, dto.Name);
        Assert.Equal(createDto.Description, dto.Description);
        Assert.Equal(createDto.IsCompleted, dto.IsCompleted);
        Assert.Equal(category.Id, dto.CategoryId);

        // Cleanup
        await RemoveItemFromDbAsync(dto.Id);
        await RemoveCategoryFromDbAsync(category.Id);
    }

    [Fact]
    public async Task Create_WithNonExistingCategory_ReturnsBadRequest()
    {
        // Arrange
        int nonExistentCategoryId = -1;
        var createDto = CreateValidCreateDto(
            name: "Úkol s neexistující kategorií",
            description: "Test popis",
            isCompleted: false,
            categoryId: nonExistentCategoryId
        );

        var controller = CreateController();

        // Act
        var result = await controller.Create(createDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Contains($"Kategorie s ID {nonExistentCategoryId} neexistuje", problemDetails.Detail);
    }
}
