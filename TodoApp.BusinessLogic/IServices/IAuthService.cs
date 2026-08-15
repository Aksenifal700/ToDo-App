using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.BusinessLogic.IServices;

public interface IAuthService
{
    Task<LoginResultDto> LoginAsync(LoginDto dto);
    
    Task<LoginResultDto> RegisterAsync(RegisterDto dto);
}