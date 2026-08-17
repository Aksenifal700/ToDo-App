using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.Entities;

namespace TodoApp.DataAccess.Database.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CategoryRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<CategoryDto?> GetByIdAsync(Guid guid, Guid id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
        
        return category is null
            ? null
            : _mapper.Map<CategoryDto>(category);
    }

    public async Task<List<CategoryDto>> GetByUserIdAsync(Guid userId)
    {
       var categories = await _context.Categories
           .Where(c => c.UserId == userId)
           .OrderBy(c => c.Name)
           .ToListAsync();
       
       return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetByNameAndUserIdAsync(string name, Guid userId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == name && c.UserId == userId);
        
        return category is null
            ? null
            : _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> AddAsync(CreateCategoryDto dto, Guid userId)
    {
        var category = _mapper.Map<Category>(dto);
        category.Id = Guid.NewGuid();
        category.UserId = userId;
        category.CreatedAt = DateTime.UtcNow;

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (category is null)
            throw new Exception("Category not found");

        _mapper.Map(dto, category);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (category is null)
            throw new Exception("Category not found");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id, Guid userId)
    {
        return await _context.Categories
            .AnyAsync(t => t.Id == id && t.UserId == userId);
    }
}