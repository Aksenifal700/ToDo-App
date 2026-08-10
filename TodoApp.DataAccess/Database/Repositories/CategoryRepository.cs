using Microsoft.EntityFrameworkCore;
using TodoApp.DataAccess.Database.Entities;
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
    }

    public void Update(Category entity)
    {
        _context.Categories.Update(entity);
    }

    public void Delete(Category entity)
    {
        _context.Categories.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Guid id, Guid userId)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}