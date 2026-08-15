namespace TodoApp.API.Models.Resposne;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}