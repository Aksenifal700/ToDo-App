using AutoMapper;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.Entities;
using TodoApp.Interfaces.IRepositories;
using TodoApp.Interfaces.IServices;

namespace TodoApp.BusinessLogic.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<CategoryDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null || category.UserId != userId)
            return null;
        
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<List<CategoryDto>> GetByUserIdAsync(Guid userId)
    {
        var categories = await _categoryRepository.GetByUserIdAsync(userId);
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, Guid userId)
    {
        var existingCategory = await _categoryRepository.GetByUserIdAsync(userId);
        if (existingCategory is not null)
            throw new Exception("Category already exists");
        
        var category = _mapper.Map<Category>(dto);
        category.Id = Guid.NewGuid();
        category.UserId = userId;
        category.CreatedAt = DateTime.Now;
        
        await _categoryRepository.AddAsync(category);
        
        return _mapper.Map<CategoryDto>(category);
            
    }
    
    public async Task UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null || category.UserId != userId)
            throw new Exception("Category not found");

        _mapper.Map(dto, category); 

        await _categoryRepository.Update(category);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        
        if(category is null || category.UserId != userId)
            throw new Exception("Category not found");
        
        await _categoryRepository.Delete(category);
    }
}