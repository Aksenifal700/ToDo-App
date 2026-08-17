using TodoApp.BusinessLogic.IServices;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.TaskItem;

namespace TodoApp.BusinessLogic.Services;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;

    public TaskItemService( ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(id, userId);
        if (taskItem is null)
            return null;
        
        return taskItem;
    }
    
    public async Task<List<TaskItemDto>> GetByUserIdAsync(Guid userId)
    {
        var taskItems = await _taskItemRepository.GetByUserIdAsync(userId);
        return taskItems;
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
        return await _taskItemRepository.AddAsync(dto, userId);
    }

    public async Task<TaskItemDto> UpdateAsync(Guid id, UpdateTaskItemDto dto, Guid userId)
    {
        return await _taskItemRepository.UpdateAsync(id, dto, userId);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        await _taskItemRepository.DeleteAsync(id, userId);
    }
}