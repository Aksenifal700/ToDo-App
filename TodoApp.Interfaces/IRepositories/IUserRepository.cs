using TodoApp.DataAccess.Database.Entities;

namespace TodoApp.Interfaces.IRepositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    
    Task<User?> GetByEmailAsync(string email);
    
    Task AddAsync(User user);
    
    Task<bool> ExistsByEmailAsync(string email);
    
}