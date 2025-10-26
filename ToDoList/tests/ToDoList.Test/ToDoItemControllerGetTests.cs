using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

namespace ToDoList.Test;

public class ToDoItemControllerGetTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void Read_ReturnsAllItems()
    {
        // Arrange
        var expectedDtos = CreateValidGetResponseDtoList();
        MapperMock
            .Setup(m => m.Map<List<ToDoItemGetResponseDto>>(It.IsAny<List<ToDoItem>>()))
            .Returns(expectedDtos);

        // Act
        var result = Controller.Read();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDtos = Assert.IsType<List<ToDoItemGetResponseDto>>(okResult.Value);
        Assert.Equal(expectedDtos.Count, actualDtos.Count);
        Assert.Equal(expectedDtos[0].ToDoItemId, actualDtos[0].ToDoItemId);
    }

    [Fact]
    public void ReadById_WhenItemExists_ReturnsOkWithMappedItem()
    {
        // Arrange
        var itemId = 1;
        var expectedDto = CreateValidGetResponseDto(id: itemId);
        MapperMock
            .Setup(m => m.Map<ToDoItemGetResponseDto>(It.IsAny<ToDoItem>()))
            .Returns(expectedDto);

        // Act
        var result = Controller.ReadById(itemId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);
        Assert.Equal(expectedDto.ToDoItemId, actualDto.ToDoItemId);
        Assert.Equal(expectedDto.Name, actualDto.Name);
    }

    [Fact]
    public void ReadById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        var result = Controller.ReadById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void ReadById_WithValidIds_ReturnsOkWithCorrectItem(int itemId)
    {
        // Arrange
        var expectedDto = CreateValidGetResponseDto(id: itemId);
        MapperMock
            .Setup(m => m.Map<ToDoItemGetResponseDto>(It.IsAny<ToDoItem>()))
            .Returns(expectedDto);

        // Act
        var result = Controller.ReadById(itemId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);

        Assert.Equal(expectedDto.ToDoItemId, actualDto.ToDoItemId);
        Assert.Equal(expectedDto.Name, actualDto.Name);
        Assert.Equal(expectedDto.Description, actualDto.Description);
        Assert.Equal(expectedDto.IsCompleted, actualDto.IsCompleted);
    }
}
