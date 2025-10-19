namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected readonly IMapper Mapper;

    protected BaseApiController(IMapper mapper)
    {
        Mapper = mapper;
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
