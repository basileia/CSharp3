using AutoMapper;
using ToDoList.Domain.Mapping;
using ToDoList.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI
    builder.Services.AddControllers();
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    builder.Services.AddScoped<IMapper, Mapper>();
    builder.Services.AddDbContext<ToDoItemsContext>();
}

var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline)
    app.MapControllers();
}

app.Run();
