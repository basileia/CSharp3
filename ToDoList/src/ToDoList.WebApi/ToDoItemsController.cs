namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using AutoMapper;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly IMapper _mapper;
    private static List<ToDoItem> items = [];

    public ToDoItemsController(IMapper mapper)
    {
        _mapper = mapper;
    }

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
            throw new Exception("Neco se opravdu nepovedlo.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
        return Ok(); //200
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

