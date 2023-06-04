namespace ToDoApiServiceClient
{
    public interface IToDoServiceClient
    {
        Task<Guid?> AddAsync(
            ToDo toDo,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            ToDo toDo,
            CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<ListToDoResult> GetAsync(CancellationToken cancellationToken = default);
    }
}