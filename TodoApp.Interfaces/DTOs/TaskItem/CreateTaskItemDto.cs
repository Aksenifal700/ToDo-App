namespace TodoApp.Interfaces.DTOs.TaskItem;

public class CreateTaskItemDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? CategoryId { get; set; }
}