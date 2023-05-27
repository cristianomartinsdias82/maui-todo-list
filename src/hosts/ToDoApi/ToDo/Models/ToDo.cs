using Microsoft.EntityFrameworkCore;

namespace ToDoApi.ToDo.Models
{
    public class ToDo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Details { get; set; }
        public bool Done { get; set; }
    }
}