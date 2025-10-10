namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using AutoMapper;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController(IMapper mapper) : BaseApiController(mapper)
{
    private static List<ToDoItem> items = new()
{
    new ToDoItem
    {
        ToDoItemId = 1,
        Name = "Nakoupit potraviny",
        Description = "Koupit mléko, vejce a chléb",
        IsCompleted = false
    },
    new ToDoItem
    {
        ToDoItemId = 2,
        Name = "Uklidit kuchyň",
        Description = "Uklidit nádobí a utřít stůl",
        IsCompleted = true
    },
    new ToDoItem
    {
        ToDoItemId = 3,
        Name = "Zavolat doktorovi",
        Description = "Objednat se na kontrolu",
        IsCompleted = false
    }
};

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //pouzijeme DTO - Data Transfer Object
    {
        return ExecuteWithExceptionHandling(() =>
        {
            var item = Mapper.Map<ToDoItem>(request);
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(x => x.ToDoItemId) + 1;
            items.Add(item);

            var responseDto = Mapper.Map<ToDoItemGetResponseDto>(item);
            return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.ToDoItemId }, responseDto);
        });
    }

    [HttpGet]
    public IActionResult Read() //api/ToDoItems GET
    {
        return ExecuteWithExceptionHandling(() =>
        {
            if (items == null)
            {
                return Problem("Seznam úkolů nenalezen", statusCode: StatusCodes.Status404NotFound);
            }

            var response = Mapper.Map<List<ToDoItemGetResponseDto>>(items);
            return Ok(response);
        });
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId) //api/ToDoItems/<id> GET
    {
        return ExecuteWithExceptionHandling(() =>
        {
            var item = items.Find(x => x.ToDoItemId == toDoItemId);

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
            var existingItem = items.Find(x => x.ToDoItemId == toDoItemId);

            if (existingItem == null)
            {
                return Problem(
                    detail: $"Úkol s ID {toDoItemId} nebyl nalezen.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            int index = items.FindIndex(x => x.ToDoItemId == toDoItemId);
            var updatedItem = Mapper.Map<ToDoItem>(request);
            updatedItem.ToDoItemId = toDoItemId;
            items[index] = updatedItem;

            return NoContent();
        });
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        return ExecuteWithExceptionHandling(() =>
        {
            var toDoItem = items.Find(item => item.ToDoItemId == toDoItemId);

            if (toDoItem == null)
            {
                return Problem(
                    detail: $"Úkol s ID {toDoItemId} nebyl nalezen.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            items.Remove(toDoItem);
            return NoContent();
        });
    }
}

