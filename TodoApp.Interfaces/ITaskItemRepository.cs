using TodoApp.Interfaces.DTOs.TaskItem;

namespace TodoApp.Interfaces;

public interface ITaskItemRepository
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
    
    Task<TaskItemDto> AddAsync(CreateTaskItemDto dto, Guid userId);

    Task<TaskItemDto> UpdateAsync(Guid id, UpdateTaskItemDto dto, Guid userId);
    
    Task DeleteAsync(Guid id, Guid userId);
    
    Task<bool> ExistsAsync(Guid id, Guid userId);
}