namespace TodoApp.Interfaces.DTOs.Auth;

public class LoginResultDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Email { get; set; }
}