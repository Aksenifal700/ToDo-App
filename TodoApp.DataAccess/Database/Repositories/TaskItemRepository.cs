using Microsoft.EntityFrameworkCore;
using TodoApp.Interfaces.Entities;
using TodoApp.Interfaces.IRepositories;

namespace TodoApp.DataAccess.Database.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly AppDbContext _context;

    public TaskItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<TaskItem>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Title)
            .ToListAsync();
    }

    public async Task<(List<TaskItem> Items, int TotalCount)> GetPagedAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? categoryId = null,
        bool? isCompleted = null)
    {
        var query = _context.Tasks
            .Where(t => t.UserId == userId);

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t =>
                t.Title.Contains(searchTerm) || 
                (t.Description != null && t.Description.Contains(searchTerm))); 
        }
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(t => t.CreatedAt)   
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }

    public async Task AddAsync(TaskItem entity)
    {
        await _context.Tasks.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(TaskItem entity)
    {
        _context.Tasks.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(TaskItem entity)
    {
        _context.Tasks.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id, Guid userId)
    {
        return await _context.Tasks
            .AnyAsync(t => t.Id == id && t.UserId == userId);
    }
    
}