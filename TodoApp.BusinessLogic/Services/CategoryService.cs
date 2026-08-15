using TodoApp.BusinessLogic.IServices;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.Entities;

namespace TodoApp.BusinessLogic.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository; 
    }
    
    public async Task<CategoryDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id, userId);
        return category;
    }

    public async Task<List<CategoryDto>> GetByUserIdAsync(Guid userId)
    {
        var categories = await _categoryRepository.GetByUserIdAsync(userId);
        return categories;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, Guid userId)
    {
        var existingCategory = await _categoryRepository.GetByUserIdAsync(userId);
        if (existingCategory is not null)
            throw new Exception("Category already exists");
        
        return await _categoryRepository.AddAsync(dto, userId);
    }
    
    public async Task UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId)
    {
       await _categoryRepository.UpdateAsync(id, dto, userId);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
       await _categoryRepository.DeleteAsync(id, userId);
    }
}