using TodoApp.BusinessLogic.Exceptions;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.Entities;
using TodoApp.Interfaces.IServices;

namespace TodoApp.BusinessLogic.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICachedQueryService _cachedQuery;

    public CategoryService(ICategoryRepository categoryRepository, ICachedQueryService cachedQuery)
    {
        _categoryRepository = categoryRepository;
        _cachedQuery = cachedQuery;
    }
    
    public async Task<CategoryDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var result = await _cachedQuery.GetOrSetAsync(
            $"category:{userId}:{id}", 
            () => _categoryRepository.GetByIdAsync(id, userId));
        
        if(result is null)
            throw new NotFoundException("Category not found");
        
        return result;
    }

    public async Task<List<CategoryDto>> GetByUserIdAsync(Guid userId)
    {
        var result = await _cachedQuery.GetOrSetAsync(
            $"category:{userId}",
             async () => (List<CategoryDto>?) await _categoryRepository.GetByUserIdAsync(userId));
        
        return result ?? [];
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, Guid userId)
    {
        var existingCategory = await _categoryRepository.GetByNameAndUserIdAsync(dto.Name, userId);
        if (existingCategory is not null)
            throw new AlreadyExistsException($"category with name '{dto.Name}' already exists");
        
        var created = await _categoryRepository.AddAsync(dto, userId);
        
        await _cachedQuery.InvalidateAsync($"category:{userId}");

        return created;
    }
    
    public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId)
    {
        var result = await _categoryRepository.UpdateAsync(id, dto, userId);
        if (result is null)
            throw new NotFoundException("category not found");
        
        await _cachedQuery.InvalidateAsync($"category:{userId}:{id}",$"categories:{userId}");

        return result;
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var deleted = await _categoryRepository.DeleteAsync(id, userId);
        if (!deleted)
            throw new NotFoundException("Category not found");
        
        await _cachedQuery.InvalidateAsync($"category:{userId}:{id}", $"categories:{userId}");
    }
}