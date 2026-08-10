using TodoApp.DataAccess.Database.Entities;

namespace TodoApp.Interfaces.IRepositories;

public interface ITaskItemRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    
    Task<List<TaskItem>> GetByUserIdAsync(Guid userId);

    Task<(List<TaskItem> Items, int TotalCount)> GetPagedAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? categoryId = null,
        bool? isCompleted = null);
    
    Task AddAsync(TaskItem entity);

    void Update(TaskItem entity);
    
    void Delete(TaskItem entity);
    
    Task<bool> ExistsAsync(Guid id, Guid userId);
    
    Task SaveChangesAsync(); 

}