namespace TodoApp.Interfaces.DTOs.Auth;

public class TokenGenerationDto
{
    public string Email { get; set; }
    public Guid UserId { get; set; }
}