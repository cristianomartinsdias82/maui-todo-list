namespace ToDoApi.ToDo.ViewModels
{
    public record struct ToDoViewModelItem
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Title { get; init; }
        public string? Details { get; init; }
        public bool Done { get; init; }
    }
}
