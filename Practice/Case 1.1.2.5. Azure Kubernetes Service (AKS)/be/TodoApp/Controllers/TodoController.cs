using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Data;

namespace TodoApi.Controllers;

[ApiController]
[Route("todos")]
public class TodoController : ControllerBase
{
    private readonly AppDbContext _db;

    public TodoController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
    {
        var items = await TodoRepository.GetAllAsync(_db);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> Get(int id)
    {
        var item = await TodoRepository.GetByIdAsync(_db, id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Post(TodoItem item)
    {
        var created = await TodoRepository.AddAsync(_db, item);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, TodoItem item)
    {
        var updated = await TodoRepository.UpdateAsync(_db, id, item);
        return updated == null ? NotFound() : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await TodoRepository.DeleteAsync(_db, id);
        return deleted ? NoContent() : NotFound();
    }
}
