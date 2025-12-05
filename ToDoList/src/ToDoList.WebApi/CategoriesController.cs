namespace ToDoList.WebApi;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(IMapper mapper, ICategoryRepository repository)
    : BaseApiController<Category, ICategoryRepository>(mapper, repository)
{
    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateRequestDto request)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var normalizedName = Category.Normalize(request.Name);

            var exists = await Repository.ExistsByNormalizedNameAsync(normalizedName);
            if (exists)
            {
                return Problem(
                    detail: $"Kategorie s názvem '{request.Name}' už existuje.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var category = Mapper.Map<Category>(request);
            category.NormalizedName = normalizedName;
            await Repository.CreateAsync(category);

            var response = Mapper.Map<CategoryGetResponseDto>(category);
            return CreatedAtAction(nameof(ReadById), new { categoryId = category.Id }, response);
        });
    }

    [HttpGet]
    public async Task<IActionResult> Read()
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var categories = await Repository.ReadAllAsync();
            var response = Mapper.Map<IEnumerable<CategoryGetResponseDto>>(categories);
            return Ok(response);
        });
    }

    [HttpGet("{categoryId:int}")]
    public async Task<IActionResult> ReadById(int categoryId)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var category = await Repository.ReadByIdAsync(categoryId);

            if (category == null)
            {
                return Problem(
                    detail: $"Kategorie s ID {categoryId} nebyla nalezena.",
                    statusCode: StatusCodes.Status404NotFound);
            }
            return Ok(Mapper.Map<CategoryGetResponseDto>(category));
        });
    }

    [HttpPut("{categoryId:int}")]
    public async Task<IActionResult> Update(int categoryId, [FromBody] CategoryUpdateRequestDto request)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var category = await Repository.ReadByIdAsync(categoryId);
            if (category == null)
            {
                return Problem(
                    detail: $"Kategorie s ID {categoryId} nebyla nalezena.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            var normalizedName = Category.Normalize(request.Name);

            if (normalizedName == category.NormalizedName)
            {
                return NoContent();
            }

            var exists = await Repository.ExistsByNormalizedNameAsync(normalizedName);
            if (exists)
            {
                return Problem(
                    detail: $"Kategorie s názvem '{request.Name}' už existuje.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            Mapper.Map(request, category);
            category.NormalizedName = normalizedName;
            await Repository.UpdateAsync(category);

            return NoContent();
        });
    }

    [HttpDelete("{categoryId:int}")]
    public async Task<IActionResult> Delete(int categoryId)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var category = await Repository.ReadByIdAsync(categoryId);
            if (category == null)
                return Problem(
                    detail: $"Kategorie s ID {categoryId} nebyla nalezena.",
                    statusCode: StatusCodes.Status404NotFound);

            await Repository.DeleteAsync(category);
            return NoContent();
        });
    }
}

