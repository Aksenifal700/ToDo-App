using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetByIdAsync(Guid userId);
    
    Task<UserDto?> GetByEmailAsync(string email);
    
    Task<UserDto?> CreateUserAsync(RegisterDto dto, byte[] passwordHash, byte[] passwordSalt);
    
    Task<bool> ExistsByEmailAsync(string email);
    
}