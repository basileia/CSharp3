namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ToDoList.Persistence;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected readonly IMapper Mapper;
    protected readonly ToDoItemsContext Context;

    protected BaseApiController(IMapper mapper, ToDoItemsContext context)
    {
        Mapper = mapper;
        Context = context;
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
