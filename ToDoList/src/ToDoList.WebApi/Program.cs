using AutoMapper;
using ToDoList.Domain.Mapping;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();

    builder.Services.AddScoped<IMapper, Mapper>();
}

var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline)
    app.MapControllers();
}

app.Run();
