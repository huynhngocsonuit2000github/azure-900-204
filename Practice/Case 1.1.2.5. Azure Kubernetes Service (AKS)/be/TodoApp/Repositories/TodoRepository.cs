using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Data;

namespace TodoApi.Repositories;

public static class TodoRepository
{
    public static async Task<List<TodoItem>> GetAllAsync(AppDbContext db)
    {
        return await db.TodoItems.AsNoTracking().ToListAsync();
    }

    public static async Task<TodoItem?> GetByIdAsync(AppDbContext db, int id)
    {
        return await db.TodoItems.FindAsync(id);
    }

    public static async Task<TodoItem> AddAsync(AppDbContext db, TodoItem item)
    {
        db.TodoItems.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public static async Task<TodoItem?> UpdateAsync(AppDbContext db, int id, TodoItem updatedItem)
    {
        var item = await db.TodoItems.FindAsync(id);
        if (item == null) return null;

        item.Title = updatedItem.Title;
        item.IsCompleted = updatedItem.IsCompleted;

        await db.SaveChangesAsync();
        return item;
    }

    public static async Task<bool> DeleteAsync(AppDbContext db, int id)
    {
        var item = await db.TodoItems.FindAsync(id);
        if (item == null) return false;

        db.TodoItems.Remove(item);
        await db.SaveChangesAsync();
        return true;
    }
} 
