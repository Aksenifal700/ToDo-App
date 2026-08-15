namespace TodoApp.Interfaces.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    public ICollection<Category> Categories { get; set; }
    public ICollection<TaskItem> TaskItems { get; set; }
}