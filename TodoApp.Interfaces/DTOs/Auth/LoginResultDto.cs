namespace TodoApp.Interfaces.DTOs.Auth;

public class LoginResultDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Email { get; set; }
}