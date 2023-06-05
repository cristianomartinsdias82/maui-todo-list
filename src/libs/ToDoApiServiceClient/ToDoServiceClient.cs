using System.Net.Http.Json;
using System.Text.Json;

namespace ToDoApiServiceClient
{
    internal sealed class ToDoServiceClient : IToDoServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions? _serializerOptions;
        private const string ApiRoute = "api/todo";

        public ToDoServiceClient(
            HttpClient httpClient,
            JsonSerializerOptions? serializerOptions)
        {
            _httpClient = httpClient;
            _serializerOptions = serializerOptions;
        }

        public async Task<Guid?> AddAsync(ToDo toDo, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync(ApiRoute, toDo, _serializerOptions, cancellationToken);

            result.EnsureSuccessStatusCode();

            var contentResult = await result.Content.ReadAsStringAsync(cancellationToken);

            var successfullySavedToDo = JsonSerializer.Deserialize<ToDo>(contentResult, _serializerOptions);

            return successfullySavedToDo!.Id;
        }

        public async Task<ListToDoResult> GetAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetAsync(ApiRoute, cancellationToken);

            result.EnsureSuccessStatusCode();

            var contentResult = await result.Content.ReadAsStringAsync(cancellationToken);

            var listToDoResult = JsonSerializer.Deserialize<ListToDoResult>(contentResult, _serializerOptions);

            return listToDoResult!;
        }

        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync($"{ApiRoute}/{id}", cancellationToken);

            result.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> UpdateAsync(ToDo toDo, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync($"{ApiRoute}/{toDo.Id}", toDo, _serializerOptions, cancellationToken);

            result.EnsureSuccessStatusCode();

            return true;
        }
    }
}