using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

namespace ToDoList.Test;

public class ToDoItemsControllerCreateTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void Create_WithValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = CreateValidCreateDto();
        var createdItem = CreateValidToDoItem(id: 1);
        var responseDto = CreateValidGetResponseDto(id: 1);

        MapperMock
            .Setup(m => m.Map<ToDoItem>(It.IsAny<ToDoItemCreateRequestDto>()))
            .Returns(createdItem);
        MapperMock
            .Setup(m => m.Map<ToDoItemGetResponseDto>(It.IsAny<ToDoItem>()))
            .Returns(responseDto);

        // Act
        var result = Controller.Create(createDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(Controller.ReadById), createdAtActionResult.ActionName);
    }

    [Fact]
    public void Create_WithValidDto_ReturnsCreatedItemInResponse()
    {
        // Arrange
        var createDto = CreateValidCreateDto();
        var createdItem = CreateValidToDoItem(1, createDto.Name, createDto.Description, createDto.IsCompleted);
        var expectedDto = CreateValidGetResponseDto(1, createDto.Name, createDto.Description, createDto.IsCompleted);

        MapperMock
            .Setup(m => m.Map<ToDoItem>(It.IsAny<ToDoItemCreateRequestDto>()))
            .Returns(createdItem);

        MapperMock
            .Setup(m => m.Map<ToDoItemGetResponseDto>(It.IsAny<ToDoItem>()))
            .Returns(expectedDto);

        // Act
        var result = Controller.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var actualDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);
        Assert.Equal(expectedDto.ToDoItemId, expectedDto.ToDoItemId);
        Assert.Equal(expectedDto.Name, actualDto.Name);
        Assert.Equal(expectedDto.Description, actualDto.Description);
        Assert.Equal(expectedDto.IsCompleted, actualDto.IsCompleted);
    }

    [Fact]
    public void Create_WithNewItem_SetsCorrectId()
    {
        // Arrange
        var createDto = CreateValidCreateDto();

        MapperMock
            .Setup(m => m.Map<ToDoItem>(It.IsAny<ToDoItemCreateRequestDto>()))
            .Returns((ToDoItemCreateRequestDto dto) =>
                new ToDoItem
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    IsCompleted = dto.IsCompleted
                });

        MapperMock
            .Setup(m => m.Map<ToDoItemGetResponseDto>(It.IsAny<ToDoItem>()))
            .Returns((ToDoItem x) => CreateValidGetResponseDto(x.ToDoItemId, x.Name, x.Description, x.IsCompleted));

        var nextId = GetNextId();

        // Act
        var result = Controller.Create(createDto);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(((CreatedAtActionResult)result).Value);

        // Assert
        Assert.Equal(nextId, dto.ToDoItemId);
    }
}
