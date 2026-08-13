using Microsoft.EntityFrameworkCore;
using TodoApp.Interfaces.Entities;
using TodoApp.Interfaces.IRepositories;

namespace TodoApp.DataAccess.Database.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Category>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByNameAndUserIdAsync(string name, Guid userId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == name && c.UserId == userId);
    }

    public async Task AddAsync(Category entity)
    {
        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Category entity)
    {
        _context.Categories.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Category entity)
    {
        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync();

    }

    public async Task<bool> ExistsAsync(Guid id, Guid userId)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == id && c.UserId == userId);
    }
    
}