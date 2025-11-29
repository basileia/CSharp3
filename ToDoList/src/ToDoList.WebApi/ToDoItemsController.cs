namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using AutoMapper;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController(IMapper mapper, IRepositoryAsync repository) : BaseApiController<ToDoItem>(mapper, repository)
{
    [HttpPost]
    public async Task<IActionResult> Create(ToDoItemCreateRequestDto request)
    {
        return await ExecuteWithExceptionHandling(async () =>
            {
                var item = Mapper.Map<ToDoItem>(request);
                await Repository.CreateAsync(item);

                var responseDto = Mapper.Map<ToDoItemGetResponseDto>(item);

                return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.Id }, responseDto);
            });
    }

    [HttpGet]
    public async Task<IActionResult> Read()
    {
        return await ExecuteWithExceptionHandling(async () =>
            {
                var items = await Repository.ReadAllIncludingCategoryAsync();
                var response = Mapper.Map<IEnumerable<ToDoItemGetResponseDto>>(items);
                return Ok(response);
            });
    }

    [HttpGet("{toDoItemId:int}")]
    public async Task<IActionResult> ReadById(int toDoItemId)
    {
        return await ExecuteWithExceptionHandling(async () =>
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
    }

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

                Mapper.Map(request, existingItem);
                await Repository.UpdateAsync(existingItem);
                return NoContent();
            });
    }

    [HttpDelete("{toDoItemId:int}")]
    public async Task<IActionResult> DeleteById(int toDoItemId)
    {
        return await ExecuteWithExceptionHandling(async () =>
            {
                var toDoItem = await Repository.ReadByIdAsync(toDoItemId);

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

