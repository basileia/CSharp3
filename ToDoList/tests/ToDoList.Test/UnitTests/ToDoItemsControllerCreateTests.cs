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
        public async Task Create_WithValidDtoAndNoCategory_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = CreateValidCreateDto();
            var createdItem = CreateValidToDoItem(1, createDto.Name, createDto.Description, createDto.IsCompleted);
            var responseDto = CreateValidGetResponseDto(1, createDto.Name, createDto.Description, createDto.IsCompleted);

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            MapperMock.Map<ToDoItemGetResponseDto>(createdItem).Returns(responseDto);

            var controller = CreateController();

            // Act
            var result = await controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.ReadById), createdAtActionResult.ActionName);
            Assert.Equal(responseDto, createdAtActionResult.Value);

            await RepositoryMock.Received(1).CreateAsync(Arg.Any<ToDoItem>());
        }

        [Fact]
        public async Task Create_WithValidDtoAndExistingCategory_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = CreateValidCreateDto(categoryId: 42);
            var category = new Category { Id = 42, Name = "TestCat" };
            var createdItem = CreateValidToDoItem(1, createDto.Name, createDto.Description, createDto.IsCompleted, createDto.CategoryId);
            var responseDto = CreateValidGetResponseDto(1, createDto.Name, createDto.Description, createDto.IsCompleted, createDto.CategoryId, category.Name);

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            MapperMock.Map<ToDoItemGetResponseDto>(createdItem).Returns(responseDto);

            CategoryRepositoryMock.ReadByIdAsync(42).Returns(category);

            var controller = CreateController();

            // Act
            var result = await controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.ReadById), createdAtActionResult.ActionName);
            Assert.Equal(responseDto, createdAtActionResult.Value);

            await RepositoryMock.Received(1).CreateAsync(Arg.Any<ToDoItem>());
        }

        [Fact]
        public async Task Create_WithNonExistingCategory_ReturnsBadRequest()
        {
            // Arrange
            var createDto = CreateValidCreateDto(categoryId: 99);
            CategoryRepositoryMock.ReadByIdAsync(99).Returns((Category)null);

            var controller = CreateController();

            // Act
            var result = await controller.Create(createDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Contains("Kategorie s ID 99 neexistuje", problemDetails.Detail);

            await RepositoryMock.DidNotReceive().CreateAsync(Arg.Any<ToDoItem>());
        }

        [Fact]
        public async Task Create_WhenRepositoryThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var createDto = CreateValidCreateDto();
            var createdItem = CreateValidToDoItem();

            MapperMock.Map<ToDoItem>(createDto).Returns(createdItem);
            RepositoryMock.When(x => x.CreateAsync(Arg.Any<ToDoItem>()))
                .Do(x => throw new Exception("Database connection failed"));

            var controller = CreateController();

            // Act
            var result = await controller.Create(createDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }
    }
}
