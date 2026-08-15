namespace TodoApp.API.Models.Request.Task;

public class UpdateTaskRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsCompleted { get; set; }
}