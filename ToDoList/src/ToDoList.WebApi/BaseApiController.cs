namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ToDoList.Persistence.Repositories;

[ApiController]
public class BaseApiController<T> : ControllerBase where T : class
{
    protected IMapper Mapper { get; }
    protected IRepositoryAsync<T> Repository { get; }

    protected BaseApiController(IMapper mapper, IRepositoryAsync<T> repository)
    {
        Mapper = mapper;
        Repository = repository;
    }

    protected async Task<IActionResult> ExecuteWithExceptionHandling(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
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
