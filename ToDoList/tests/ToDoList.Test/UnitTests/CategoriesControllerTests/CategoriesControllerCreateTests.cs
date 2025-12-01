namespace ToDoList.Test.UnitTests.CategoriesControllerTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class CategoriesControllerCreateTests : CategoriesControllerTestBase
{
    [Fact]
    public async Task Create_WhenCategoryDoesNotExist_ShouldCreateAndReturnCreatedResult()
    {
        // Arrange
        var request = CreateCreateDto("Práce");
        string normalizedName = Category.Normalize(request.Name);

        RepositoryMock.ExistsByNormalizedNameAsync(normalizedName)
            .Returns(false);

        var mappedCategory = CreateCategory(request.Name);
        Mapper.Map<Category>(request).Returns(mappedCategory);

        var expectedResponse = new CategoryGetResponseDto(1, request.Name);
        Mapper.Map<CategoryGetResponseDto>(Arg.Any<Category>())
            .Returns(expectedResponse);

        var controller = CreateController();

        // Act
        var result = await controller.Create(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.Value.Should().Be(expectedResponse);

        await RepositoryMock.Received(1).CreateAsync(
            Arg.Is<Category>(c =>
                c.Name == request.Name &&
                c.NormalizedName == normalizedName));
    }

    [Fact]
    public async Task Create_WhenCategoryAlreadyExists_ShouldReturnBadRequest()
    {
        // Arrange
        var request = CreateCreateDto("Práce");
        string normalizedName = Category.Normalize(request.Name);

        RepositoryMock.ExistsByNormalizedNameAsync(normalizedName)
            .Returns(true);

        var controller = CreateController();

        // Act
        var result = await controller.Create(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(400);

        Mapper.DidNotReceive().Map<Category>(Arg.Any<CategoryCreateRequestDto>());
        await RepositoryMock.DidNotReceive().CreateAsync(Arg.Any<Category>());
    }
}
