namespace TodoApp.Interfaces.DTOs.TaskItem;

public class UpdateTaskItemDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? CategoryId { get; set; }
}