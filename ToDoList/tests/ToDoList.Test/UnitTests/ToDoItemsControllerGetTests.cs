using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

namespace ToDoList.Test.UnitTests;

public class ToDoItemsControllerGetTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void Read_ReturnsAllItems()
    {
        // Arrange
        var toDoItems = new List<ToDoItem>
            {
                new() { ToDoItemId = 1, Name = "Nakoupit potraviny", Description = "Koupit mléko", IsCompleted = false },
                new() { ToDoItemId = 2, Name = "Uklidit", Description = "Uklidit stůl", IsCompleted = true }
            };

        var expectedDtos = CreateValidGetResponseDtoList();

        RepositoryMock.ReadAll().Returns(toDoItems);
        MapperMock.Map<IEnumerable<ToDoItemGetResponseDto>>(toDoItems).Returns(expectedDtos);

        // Act
        var result = Controller.Read();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDtos = Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(okResult.Value);

        Assert.Equal(expectedDtos.Count, ((List<ToDoItemGetResponseDto>)actualDtos).Count);
        RepositoryMock.Received(1).ReadAll();
    }

    [Fact]
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var someItem = new ToDoItem { Name = "Some Task", Description = "Some Description", IsCompleted = false };
        RepositoryMock.ReadAll().Returns([someItem]);
        MapperMock.Map<IEnumerable<ToDoItemGetResponseDto>>(Arg.Any<IEnumerable<ToDoItem>>())
                    .Returns(CreateValidGetResponseDtoList());

        // Act
        var result = Controller.Read();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(okResult.Value);
        RepositoryMock.Received(1).ReadAll();
    }

    [Fact]
    public void ReadById_WhenItemExists_ReturnsOkWithMappedItem()
    {
        // Arrange
        int itemId = 1;
        var toDoItem = CreateValidToDoItem();
        var expectedDto = CreateValidGetResponseDto(id: itemId);

        RepositoryMock.ReadById(itemId).Returns(toDoItem);
        MapperMock.Map<ToDoItemGetResponseDto>(toDoItem).Returns(expectedDto);

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

    [Fact]
    public void ReadById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        RepositoryMock.ReadById(nonExistentId).Returns((ToDoItem?)null);

        // Act
        var result = Controller.ReadById(nonExistentId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }
}
