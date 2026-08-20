using TodoApp.Interfaces.DTOs.TaskItem;

namespace TodoApp.Interfaces.IServices;

public interface ITaskItemService
{
    Task<TaskItemDto?> GetByIdAsync(Guid id, Guid userId);
    
    Task<List<TaskItemDto>> GetByUserIdAsync(Guid userId);
    
    Task<(List<TaskItemDto> Items, int TotalCount)> GetPagedAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? categoryId = null,
        bool? isCompleted = null);
    
    Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto, Guid userId);
    
    Task<TaskItemDto> UpdateAsync(Guid id, UpdateTaskItemDto dto, Guid userId);
    
    Task DeleteAsync(Guid id, Guid userId);  
    
}