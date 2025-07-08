using TodoApi.Models;

namespace TodoApi.Repositories;

public static class TodoRepository
{
    public static List<TodoItem> Todos { get; } = new List<TodoItem>()
    {
        new TodoItem() { Id = 1, Title = "Work", IsCompleted = true },
        new TodoItem() { Id = 2, Title = "Home", IsCompleted = false },
        new TodoItem() { Id = 3, Title = "Office", IsCompleted = false },
    };
    public static int NextId { get; set; } = 1;
}
