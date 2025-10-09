namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using AutoMapper;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController(IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
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
        var item = _mapper.Map<ToDoItem>(request);

        try
        {
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(x => x.ToDoItemId) + 1;
            items.Add(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        var responseDto = _mapper.Map<ToDoItemGetResponseDto>(item);

        return CreatedAtAction(nameof(ReadById), new { toDoItemId = item.ToDoItemId }, responseDto);
    }

    [HttpGet]
    public IActionResult Read() //api/ToDoItems GET
    {
        try
        {
            if (items == null)
            {
                return Problem("Seznam úkolů nenalezen", statusCode: StatusCodes.Status404NotFound);
            }
            var response = _mapper.Map<List<ToDoItemGetResponseDto>>(items);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId) //api/ToDoItems/<id> GET
    {
        try
        {
            var item = items.Find(x => x.ToDoItemId == toDoItemId);

            if (item == null)
            {
                return Problem(
                    detail: $"Úkol s ID {toDoItemId} nebyl nalezen.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var responseDto = _mapper.Map<ToDoItemGetResponseDto>(item);

            return Ok(responseDto);
        }
        catch (Exception ex)
        {
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return Ok(); //200
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        return Ok(); //200
    }
}

