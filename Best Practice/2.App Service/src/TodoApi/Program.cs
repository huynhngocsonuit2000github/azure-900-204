using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var b = WebApplication.CreateBuilder(args);
b.Services.AddDbContext<AppDb>(o => o.UseInMemoryDatabase("todos"));
b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen();

// CORS
const string CorsDev = "CorsDev";
b.Services.AddCors(o =>
{
    o.AddPolicy(CorsDev, p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = b.Build();

app.UseCors(CorsDev);

app.UseSwagger();
app.UseSwaggerUI();

app.UseSwagger(); app.UseSwaggerUI();
app.MapGet("/healthz", () => Results.Ok("OK"));
app.MapGet("/sayhi", () => Results.Ok("Hello everything"));

// CRUD
var grp = app.MapGroup("/api/todos");
grp.MapGet("/", async (AppDb db) => await db.Todos.AsNoTracking().ToListAsync());
grp.MapGet("/{id:int}", async (int id, AppDb db) =>
    await db.Todos.FindAsync(id) is { } t ? Results.Ok(t) : Results.NotFound());
grp.MapPost("/", async (Todo todo, AppDb db) =>
{ db.Todos.Add(todo); await db.SaveChangesAsync(); return Results.Created($"/api/todos/{todo.Id}", todo); });
grp.MapPut("/{id:int}", async (int id, Todo input, AppDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    todo.Title = input.Title; todo.IsDone = input.IsDone;
    await db.SaveChangesAsync(); return Results.NoContent();
});
grp.MapDelete("/{id:int}", async (int id, AppDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    db.Remove(todo); await db.SaveChangesAsync(); return Results.NoContent();
});

app.Run();
