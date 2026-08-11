namespace TodoApp.Interfaces.DTOs.TaskItem;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool isCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
}