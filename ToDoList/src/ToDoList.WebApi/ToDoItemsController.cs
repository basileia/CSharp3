namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using AutoMapper;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController(IMapper mapper, IRepository<ToDoItem> repository) : BaseApiController(mapper, repository)
{
    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request)
    {
        return ExecuteWithExceptionHandling(() =>
            {
                var item = Mapper.Map<ToDoItem>(request);
                repository.Create(item);

                var responseDto = Mapper.Map<ToDoItemGetResponseDto>(item);

                return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.ToDoItemId }, responseDto);
            });
    }

    [HttpGet]
    public IActionResult Read()
    {
        var items = Repository.Read();
        var response = Mapper.Map<IEnumerable<ToDoItemGetResponseDto>>(items);
        return Ok(response);
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId)
    {
        return ExecuteWithExceptionHandling(() =>
            {
                var item = Repository.ReadById(toDoItemId);

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
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return ExecuteWithExceptionHandling(() =>
            {
                var existingItem = Repository.ReadById(toDoItemId);
                if (existingItem == null)
                {
                    return Problem(
                        detail: $"Úkol s ID {toDoItemId} nebyl nalezen.",
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                Mapper.Map(request, existingItem);
                Repository.Update(existingItem);
                return NoContent();
            });
    }
    /*
                    [HttpDelete("{toDoItemId:int}")]
                    public IActionResult DeleteById(int toDoItemId)
                    {
                        return ExecuteWithExceptionHandling(() =>
                            {
                                var toDoItem = Context.ToDoItems.Find(toDoItemId);
                                if (toDoItem == null)
                                {
                                    return Problem(
                                        detail: $"Úkol s ID {toDoItemId} nebyl nalezen.",
                                        statusCode: StatusCodes.Status404NotFound
                                    );
                                }

                                Context.ToDoItems.Remove(toDoItem);
                                Context.SaveChanges();

                                return NoContent();
                            });
                    } */
}

