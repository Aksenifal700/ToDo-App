namespace TodoApp.API.Models.Request.Auth;

public class LogInRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}