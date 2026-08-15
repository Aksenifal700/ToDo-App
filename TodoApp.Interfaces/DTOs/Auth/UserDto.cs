namespace TodoApp.Interfaces.DTOs.Auth;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}