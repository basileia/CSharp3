namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ToDoList.Persistence.Repositories;

[ApiController]
public class BaseApiController<TEntity, TRepository> : ControllerBase
    where TEntity : class
    where TRepository : IBaseRepository<TEntity>
{
    protected IMapper Mapper { get; }
    protected TRepository Repository { get; }

    protected BaseApiController(IMapper mapper, TRepository repository)
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
