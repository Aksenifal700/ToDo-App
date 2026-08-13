using TodoApp.Interfaces.Entities;

namespace TodoApp.Interfaces.IRepositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id);
    
    Task<List<Category>> GetByUserIdAsync(Guid userId);
    
    Task<Category?> GetByNameAndUserIdAsync(string name, Guid userId);
    
    Task AddAsync(Category entity);
    
    Task Update(Category entity);
    
    Task Delete(Category entity);
    
    Task<bool> ExistsAsync(Guid id, Guid userId);
    
}