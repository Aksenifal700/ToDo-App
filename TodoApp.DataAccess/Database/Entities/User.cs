namespace TodoApp.DataAccess.Database.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    
    public ICollection<Category> Categories { get; set; }
    public ICollection<TaskItem> TaskItems { get; set; }
}