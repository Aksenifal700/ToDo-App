namespace TodoApp.DataAccess.Database.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? CategoryId { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    public Category? Category { get; set; }
}