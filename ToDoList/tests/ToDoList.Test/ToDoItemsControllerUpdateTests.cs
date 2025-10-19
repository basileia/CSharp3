using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

namespace ToDoList.Test;

public class ToDoItemsControllerUpdateTests : ToDoItemsControllerTestBase
{
    [Fact]
    public void UpdateById_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        var updateDto = CreateValidUpdateDto();

        // Act
        var result = Controller.UpdateById(nonExistentId, updateDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
    }

    [Fact]
    public void UpdateById_WithValidDto_ReturnsNoContent()
    {
        // Arrange
        var existingItem = CreateValidToDoItem();
        var updateDto = CreateValidUpdateDto();

        MapperMock
            .Setup(m => m.Map<ToDoItem>(It.IsAny<ToDoItemUpdateRequestDto>()))
            .Returns((ToDoItemUpdateRequestDto dto) =>
            {
                return new ToDoItem
                {
                    ToDoItemId = existingItem.ToDoItemId,
                    Name = dto.Name,
                    Description = dto.Description,
                    IsCompleted = dto.IsCompleted
                };
            });

        // Act
        var result = Controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void UpdateById_UpdatesItemValues()
    {
        // Arrange
        var existingItem = CreateValidToDoItem();
        var updateDto = CreateValidUpdateDto(
            name: "Aktualizovaný úkol",
            description: "Nový popis",
            isCompleted: true
        );

        MapperMock
            .Setup(m => m.Map<ToDoItem>(It.IsAny<ToDoItemUpdateRequestDto>()))
            .Returns((ToDoItemUpdateRequestDto dto) => new ToDoItem
            {
                ToDoItemId = existingItem.ToDoItemId,
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted
            });

        // Act
        var result = Controller.UpdateById(existingItem.ToDoItemId, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var itemsField = typeof(ToDoItemsController)
            .GetField("items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        var currentItems = (List<ToDoItem>)itemsField.GetValue(null);
        var updatedItem = currentItems.Single(x => x.ToDoItemId == existingItem.ToDoItemId);

        Assert.Equal(updateDto.Name, updatedItem.Name);
        Assert.Equal(updateDto.Description, updatedItem.Description);
        Assert.Equal(updateDto.IsCompleted, updatedItem.IsCompleted);
    }
}
