using Microsoft.AspNetCore.Http;
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
            var createdItem = CreateValidToDoItem(1, createDto.Name, createDto.Description, createDto.IsCompleted);
            var responseDto = CreateValidGetResponseDto(1, createDto.Name, createDto.Description, createDto.IsCompleted);

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            MapperMock.Map<ToDoItemGetResponseDto>(Arg.Any<ToDoItem>()).Returns(responseDto);

            var controller = CreateController();

            // Act
            var result = controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.ReadById), createdAtActionResult.ActionName);
            Assert.Equal(responseDto, createdAtActionResult.Value);
        }

        [Fact]
        public void Create_WithValidDto_ReturnsCreatedItemInResponse()
        {
            // Arrange
            var createDto = CreateValidCreateDto();
            var createdItem = CreateValidToDoItem(1, createDto.Name, createDto.Description, createDto.IsCompleted);
            var expectedDto = CreateValidGetResponseDto(1, createDto.Name, createDto.Description, createDto.IsCompleted);

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            MapperMock.Map<ToDoItemGetResponseDto>(createdItem).Returns(expectedDto);

            var controller = CreateController();

            // Act
            var result = controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);

            Assert.Equal(expectedDto.ToDoItemId, actualDto.ToDoItemId);
            Assert.Equal(expectedDto.Name, actualDto.Name);
            Assert.Equal(expectedDto.Description, actualDto.Description);
            Assert.Equal(expectedDto.IsCompleted, actualDto.IsCompleted);

            RepositoryMock.Received(1).Create(Arg.Any<ToDoItem>());
        }

        [Fact]
        public void Create_ReturnsCorrectRouteValues()
        {
            // Arrange
            var createDto = CreateValidCreateDto();
            var createdItem = CreateValidToDoItem(id: 123);
            var responseDto = CreateValidGetResponseDto(id: 123);

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            MapperMock.Map<ToDoItemGetResponseDto>(createdItem).Returns(responseDto);

            var controller = CreateController();

            // Act
            var result = controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("ReadById", createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.Equal(123, createdResult.RouteValues["toDoItemId"]);
        }

        [Fact]
        public void Create_WhenRepositoryThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var createDto = CreateValidCreateDto();
            var createdItem = CreateValidToDoItem();

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            RepositoryMock.When(x => x.Create(Arg.Any<ToDoItem>()))
                .Do(x => throw new Exception("Database connection failed"));

            var controller = CreateController();

            // Act
            var result = controller.Create(createDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }
    }
}
