namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.Models;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected readonly IMapper Mapper;
    protected readonly IRepository<ToDoItem> Repository;

    protected BaseApiController(IMapper mapper, IRepository<ToDoItem> repository)
    {
        Mapper = mapper;
        repository = repository;

    }

    protected IActionResult ExecuteWithExceptionHandling(Func<IActionResult> action)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}
