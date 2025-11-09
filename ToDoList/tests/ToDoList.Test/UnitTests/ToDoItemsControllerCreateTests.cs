using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

namespace ToDoList.Test.UnitTests
{
    public class ToDoItemsControllerCreateTests : ToDoItemsControllerTestBase
    {
        [Fact]
        public void Create_WithValidDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = CreateValidCreateDto();
            var createdItem = CreateValidToDoItem(createDto.Name, createDto.Description, createDto.IsCompleted);
            var responseDto = CreateValidGetResponseDto(1, createDto.Name, createDto.Description, createDto.IsCompleted);

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            MapperMock.Map<ToDoItemGetResponseDto>(Arg.Any<ToDoItem>()).Returns(responseDto);

            // Act
            var result = Controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(Controller.ReadById), createdAtActionResult.ActionName);
            Assert.Equal(responseDto, createdAtActionResult.Value);
        }

        [Fact]
        public void Create_WithValidDto_ReturnsCreatedItemInResponse()
        {
            // Arrange
            var createDto = CreateValidCreateDto();
            var createdItem = CreateValidToDoItem(createDto.Name, createDto.Description, createDto.IsCompleted);
            var expectedDto = CreateValidGetResponseDto(1, createDto.Name, createDto.Description, createDto.IsCompleted);

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            MapperMock.Map<ToDoItemGetResponseDto>(createdItem).Returns(expectedDto);

            // Act
            var result = Controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);

            Assert.Equal(expectedDto.ToDoItemId, actualDto.ToDoItemId);
            Assert.Equal(expectedDto.Name, actualDto.Name);
            Assert.Equal(expectedDto.Description, actualDto.Description);
            Assert.Equal(expectedDto.IsCompleted, actualDto.IsCompleted);

            RepositoryMock.Received(1).Create(Arg.Any<ToDoItem>());
        }
    }
}
