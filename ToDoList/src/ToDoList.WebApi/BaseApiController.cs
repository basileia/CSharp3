namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.Models;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected IMapper Mapper { get; }
    protected IRepository<ToDoItem> Repository { get; }

    protected BaseApiController(IMapper mapper, IRepository<ToDoItem> repository)
    {
        Mapper = mapper;
        Repository = repository;
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
