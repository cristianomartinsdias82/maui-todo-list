using SharedKernel;
using ToDoApiServiceClient;

namespace TodoApp.ServiceClientFacade
{
    internal sealed class ToDoApiServiceFacade : IToDoApiServiceFacade
    {
        private readonly IToDoServiceClient _serviceClient;

        public ToDoApiServiceFacade(IToDoServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<Result<ListToDoResult>> GetToDoListAsync(CancellationToken cancellationToken = default)
        {
            if (!ConnectivityIsOk)
                return await Task.FromResult(new Result<ListToDoResult> { Value = new ListToDoResult { ToDos = Enumerable.Empty<ToDo>() }, Message = "Internet is unavailable." });

            try
            {
                var result = await _serviceClient.GetAsync(cancellationToken);

                return new Result<ListToDoResult> { Value = result, Succeeded = true };
            }
            catch(Exception e)
            {
                return new Result<ListToDoResult> { Exception = e, Message = "An error occurred while attempting to perform the operation." };
            }
        }

        public async Task<Result> DeleteToDoAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (!ConnectivityIsOk)
                return await Task.FromResult(new Result<ListToDoResult> { Value = new ListToDoResult { ToDos = Enumerable.Empty<ToDo>() }, Message = "Internet is unavailable." });

            try
            {
                var result = await _serviceClient.RemoveAsync(id, cancellationToken);

                return new Result { Succeeded = true };
            }
            catch (Exception e)
            {
                return new Result { Exception = e, Message = "An error occurred while attempting to perform the operation." };
            }
        }

        public async Task<Result<Guid?>> SaveToDoAsync(ToDo toDo, CancellationToken cancellationToken = default)
        {
            if (!ConnectivityIsOk)
                return await Task.FromResult(new Result<Guid?> { Value = default, Message = "Internet is unavailable." });

            Guid id = toDo.Id;
            try
            {
                if (id == Guid.Empty)
                {
                    var newlyCreatedToDoId = await _serviceClient.AddAsync(toDo, cancellationToken);
                    if (newlyCreatedToDoId.HasValue)
                        id = newlyCreatedToDoId.Value;
                }
                else
                    await _serviceClient.UpdateAsync(toDo, cancellationToken);

                return new Result<Guid?> { Value = id, Succeeded = true };
            }
            catch (Exception e)
            {
                return new Result<Guid?> { Exception = e, Message = "An error occurred while attempting to perform the operation." };
            }
        }

        private bool ConnectivityIsOk => Connectivity.Current.NetworkAccess is NetworkAccess.Internet;
    }
}