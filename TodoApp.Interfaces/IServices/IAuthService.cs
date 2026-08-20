using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.Interfaces.IServices;

public interface IAuthService
{
    Task<LoginResultDto> LoginAsync(LoginDto dto);
    
    Task<LoginResultDto> RegisterAsync(RegisterDto dto);
}