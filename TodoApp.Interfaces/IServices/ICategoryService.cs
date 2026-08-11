using TodoApp.Interfaces.DTOs.Category;

namespace TodoApp.Interfaces.IServices;

public interface ICategoryService
{
    Task<CategoryDto?> GetByIdAsync (Guid id, Guid userId);

    Task<List<CategoryDto>> GetByUserIdAsync(Guid userId);
    
    Task<CategoryDto> CreateAsync (CreateCategoryDto dto, Guid userId);
    
    Task UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId);
    
    Task DeleteAsync(Guid id, Guid userId);
}