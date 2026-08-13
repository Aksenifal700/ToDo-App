namespace TodoApp.Interfaces.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<TaskItem> Tasks { get; set; }
}