namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using AutoMapper;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController(IMapper mapper, IRepositoryAsync repository, ICategoryRepository categoryRepository) : BaseApiController<ToDoItem, IRepositoryAsync>(mapper, repository)
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private async Task<ActionResult<Category?>> ValidateCategory(int categoryId)
    {
        var category = await categoryRepository.ReadByIdAsync(categoryId);
        if (category == null)
        {
            return Problem(
                detail: $"Kategorie s ID {categoryId} neexistuje.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        return category;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToDoItemCreateRequestDto request) => await ExecuteWithExceptionHandling(async () =>
     {
         if (request.CategoryId is not null)
         {
             var categoryResult = await ValidateCategory(request.CategoryId.Value);
             if (categoryResult.Result != null)
             {
                 return categoryResult.Result;
             }
             var category = categoryResult.Value;
         }

         var item = Mapper.Map<ToDoItem>(request);
         await Repository.CreateAsync(item);

         var responseDto = Mapper.Map<ToDoItemGetResponseDto>(item);
         return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.Id }, responseDto);
     });

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> Read() => await ExecuteWithExceptionHandling<IEnumerable<ToDoItemGetResponseDto>>(async () =>
    {
        var items = await Repository.ReadAllIncludingCategoryAsync();
        var response = Mapper.Map<IEnumerable<ToDoItemGetResponseDto>>(items);
        return Ok(response);
    });

    [HttpGet("{toDoItemId:int}")]
    public async Task<ActionResult<ToDoItemGetResponseDto>> ReadById(int toDoItemId) => await ExecuteWithExceptionHandling<ToDoItemGetResponseDto>(async () =>
    {
        var item = await Repository.ReadByIdIncludingCategoryAsync(toDoItemId);

        if (item == null)
        {
            return Problem(
                detail: $"Úkol s ID {toDoItemId} nebyl nalezen.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var responseDto = Mapper.Map<ToDoItemGetResponseDto>(item);
        return Ok(responseDto);
    });

    [HttpPut("{toDoItemId:int}")]
    public async Task<IActionResult> UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return await ExecuteWithExceptionHandling(async () =>
            {
                var existingItem = await Repository.ReadByIdIncludingCategoryAsync(toDoItemId);

                if (existingItem == null)
                {
                    return Problem(
                        detail: $"Úkol s ID {toDoItemId} nebyl nalezen.",
                        statusCode: StatusCodes.Status404NotFound
                    );
                }
                if (request.CategoryId is not null)
                {
                    var categoryResult = await ValidateCategory(request.CategoryId.Value);
                    if (categoryResult.Result != null)
                        return categoryResult.Result;

                    var category = categoryResult.Value;
                }

                Mapper.Map(request, existingItem);
                existingItem.Category = null;
                await Repository.UpdateAsync(existingItem);
                return NoContent();
            });
    }

    [HttpDelete("{toDoItemId:int}")]
    public async Task<IActionResult> DeleteById(int toDoItemId)
    {
        return await ExecuteWithExceptionHandling(async () =>
            {
                var toDoItem = await Repository.ReadByIdIncludingCategoryAsync(toDoItemId);

                if (toDoItem == null)
                {
                    return Problem(
                        detail: $"Úkol s ID {toDoItemId} nebyl nalezen.",
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                await Repository.DeleteAsync(toDoItem);
                return NoContent();
            });
    }
}

