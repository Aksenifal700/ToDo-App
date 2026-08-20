using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.TaskItem;
using TodoApp.Interfaces.Entities;

namespace TodoApp.DataAccess.Database.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TaskItemRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        
        return task is null
            ? null
            :_mapper.Map<TaskItemDto>(task);
    }

    public async Task<List<TaskItemDto>> GetByUserIdAsync(Guid userId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        
        return _mapper.Map<List<TaskItemDto>>(tasks);
    }
    
    public async Task<TaskItemDto> AddAsync(CreateTaskItemDto dto, Guid userId)
    {
       var task = _mapper.Map<TaskItem>(dto);
       task.Id = Guid.NewGuid();
       task.UserId = userId;
       task.CreatedAt = DateTime.UtcNow;
       task.IsCompleted = false;
       
       await _context.Tasks.AddAsync(task);
       await _context.SaveChangesAsync();
       
       return _mapper.Map<TaskItemDto>(task);
    }

    public async Task<TaskItemDto> UpdateAsync(Guid id, UpdateTaskItemDto dto, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task is null)
            return null;
        
        _mapper.Map(dto, task);
        task.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return _mapper.Map<TaskItemDto>(task);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task is null)
            return false;
        
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }
    

    public async Task<(List<TaskItemDto> Items, int TotalCount)> GetPagedAsync(
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
        
        var dtoItems = _mapper.Map<List<TaskItemDto>>(items);
        return (dtoItems, totalCount);
    }
    
    public async Task<bool> ExistsAsync(Guid id, Guid userId)
    {
       return await _context.Tasks
            .AnyAsync(t => t.Id == id && t.UserId == userId );
    }
    
}