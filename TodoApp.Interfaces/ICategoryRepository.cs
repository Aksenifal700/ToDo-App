using TodoApp.Interfaces.DTOs.Category;

namespace TodoApp.Interfaces;

public interface ICategoryRepository
{
    Task<CategoryDto?> GetByIdAsync(Guid guid, Guid id);
    
    Task<List<CategoryDto>> GetByUserIdAsync(Guid userId);
    
    Task<CategoryDto?> GetByNameAndUserIdAsync(string name, Guid userId);
    
    Task<CategoryDto> AddAsync(CreateCategoryDto dto, Guid userId);
    
    Task <CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId);
    
    Task<bool> DeleteAsync(Guid id, Guid userId);
    
    Task<bool> ExistsAsync(Guid id, Guid userId);
    
}