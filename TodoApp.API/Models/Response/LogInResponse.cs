namespace TodoApp.API.Models.Resposne;

public class LogInResponse
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } 
}