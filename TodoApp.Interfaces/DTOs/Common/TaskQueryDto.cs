namespace TodoApp.Interfaces.DTOs.Common;

public class TaskQueryDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; }
    public bool? IsCompleted { get; set; }
}