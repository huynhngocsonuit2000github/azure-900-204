using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Controllers;

[ApiController]
[Route("todos")]
public class TodoController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(TodoRepository.Todos);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var todo = TodoRepository.Todos.FirstOrDefault(t => t.Id == id);
        return todo is null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    public IActionResult Create([FromBody] TodoItem todo)
    {
        todo.Id = TodoRepository.NextId++;
        TodoRepository.Todos.Add(todo);
        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] TodoItem updated)
    {
        var todo = TodoRepository.Todos.FirstOrDefault(t => t.Id == id);
        if (todo is null) return NotFound();

        todo.Title = updated.Title;
        todo.IsCompleted = updated.IsCompleted;
        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var todo = TodoRepository.Todos.FirstOrDefault(t => t.Id == id);
        if (todo is null) return NotFound();

        TodoRepository.Todos.Remove(todo);
        return NoContent();
    }
}
