using SharedKernel;
using ToDoApiServiceClient;

namespace TodoApp.ServiceClientFacade
{
    public interface IToDoApiServiceFacade
    {
        Task<Result<ListToDoResult>> GetToDoListAsync(CancellationToken cancellationToken = default);
        Task<Result> DeleteToDoAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<Guid?>> SaveToDoAsync(ToDo toDo, CancellationToken cancellationToken = default);
    }
}