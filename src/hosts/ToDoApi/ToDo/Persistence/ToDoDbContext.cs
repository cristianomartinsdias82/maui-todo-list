using Microsoft.EntityFrameworkCore;

namespace ToDoApi.ToDo.Persistence
{
    public sealed class ToDoDbContext : DbContext
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ToDoDbContext(DbContextOptions options) : base(options)
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            
        }

        public DbSet<Models.ToDo> ToDos { get; set; }
    }
}
