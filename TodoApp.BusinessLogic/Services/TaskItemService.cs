using TodoApp.BusinessLogic.Exceptions;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.TaskItem;
using TodoApp.Interfaces.IServices;

namespace TodoApp.BusinessLogic.Services;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ICachedQueryService _cachedQuery;

    public TaskItemService( ITaskItemRepository taskItemRepository, ICachedQueryService cachedQuery)
    {
        _taskItemRepository = taskItemRepository;
        _cachedQuery = cachedQuery;
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _cachedQuery.GetOrSetAsync(
            $"task:{userId}:{id}",
            () => _taskItemRepository.GetByIdAsync(id, userId));
    }
    
    public async Task<List<TaskItemDto>> GetByUserIdAsync(Guid userId)
    {
        var result = await _cachedQuery.GetOrSetAsync(
            $"task:{userId}",
            async () => (List<TaskItemDto>?)await _taskItemRepository.GetByUserIdAsync(userId));

        return result ?? [];
    }

    public async Task<(List<TaskItemDto> Items, int TotalCount)> GetPagedAsync(
        Guid userId, int pageNumber, int pageSize,
        string? searchTerm = null, Guid? categoryId = null,
        bool? isCompleted = null)
    {
        return await _taskItemRepository.GetPagedAsync(
            userId, 
            pageNumber, 
            pageSize, 
            searchTerm, 
            categoryId,  
            isCompleted);
    }

    public async Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto, Guid userId)
    {
        var result = await _taskItemRepository.AddAsync(dto, userId);
        await _cachedQuery.InvalidateAsync($"task:{userId}");
        return result;
    }

    public async Task<TaskItemDto> UpdateAsync(Guid id, UpdateTaskItemDto dto, Guid userId)
    {
        var result = await _taskItemRepository.UpdateAsync(id, dto, userId);
        if (result is null)
            throw new NotFoundException("Task item not found");

        await _cachedQuery.InvalidateAsync($"task:{userId}:{id}", $"tasks:{userId}");
        return result;
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var deleted = await _taskItemRepository.DeleteAsync(id, userId);
        if (!deleted)
            throw new NotFoundException("Task item not found");

        await _cachedQuery.InvalidateAsync($"task:{userId}:{id}", $"tasks:{userId}");
    }
}