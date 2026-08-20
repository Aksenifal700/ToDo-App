using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.Interfaces;

public interface IJwtTokenGenerator
{
    public string GenerateJwtToken(TokenGenerationDto dto);
}