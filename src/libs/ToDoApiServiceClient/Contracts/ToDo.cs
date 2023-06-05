namespace ToDoApiServiceClient;

public class ToDo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Details { get; set; }
    public bool Done { get; set; }
}