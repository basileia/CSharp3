var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "Test");
app.MapGet("/czechitas", () => "Hello Czechitas!");
app.MapGet("/secti/{a:int}/{b:int}", (int a, int b) => $"Výsledek {a} + {b} = {(a + b)}!");

app.Run();
