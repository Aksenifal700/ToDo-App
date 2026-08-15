using TodoApp.Interfaces.DTOs.Category;

namespace TodoApp.BusinessLogic.IServices;

public interface ICategoryService
{
    Task<CategoryDto?> GetByIdAsync (Guid id, Guid userId);

    Task<List<CategoryDto>> GetByUserIdAsync(Guid userId);
    
    Task<CategoryDto> CreateAsync (CreateCategoryDto dto, Guid userId);
    
    Task <CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId);
    
    Task DeleteAsync(Guid id, Guid userId);
}