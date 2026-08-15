using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.BusinessLogic.IServices;

public interface IJwtTokenGenerator
{
    public string GenerateJwtToken(TokenGenerationDto dto);
}