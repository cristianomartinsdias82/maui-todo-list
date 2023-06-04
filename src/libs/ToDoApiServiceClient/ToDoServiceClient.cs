using System.Net.Http.Json;
using System.Text.Json;

namespace ToDoApiServiceClient
{
    internal sealed class ToDoServiceClient : IToDoServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions? _serializerOptions;

        public ToDoServiceClient(
            HttpClient httpClient,
            JsonSerializerOptions? serializerOptions)
        {
            _httpClient = httpClient;
            _serializerOptions = serializerOptions;
        }

        public async Task<Guid?> AddAsync(ToDo toDo, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync(string.Empty, toDo, _serializerOptions, cancellationToken);

            result.EnsureSuccessStatusCode();

            var contentResult = await result.Content.ReadAsStringAsync(cancellationToken);

            var successfullySavedToDo = JsonSerializer.Deserialize<ToDo>(contentResult);

            return successfullySavedToDo!.Id;
        }

        public async Task<ListToDoResult> GetAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetAsync(string.Empty, cancellationToken);

            result.EnsureSuccessStatusCode();

            var contentResult = await result.Content.ReadAsStringAsync(cancellationToken);

            var listToDoResult = JsonSerializer.Deserialize<ListToDoResult>(contentResult, _serializerOptions);

            return listToDoResult!;
        }

        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync(id.ToString(), cancellationToken);

            result.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> UpdateAsync(ToDo toDo, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync(toDo.Id.ToString(), toDo, _serializerOptions, cancellationToken);

            result.EnsureSuccessStatusCode();

            return true;
        }
    }
}