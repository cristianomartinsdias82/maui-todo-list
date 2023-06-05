using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;

namespace ToDoApiServiceClient.Configuration
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddToDoApiServiceClient(
            this IServiceCollection services,
            Action<HttpClient> configureClient,
            JsonSerializerOptions? options = default)
        {
            ArgumentNullException.ThrowIfNull(configureClient);

            services.AddHttpClient<IToDoServiceClient, ToDoServiceClient>(configureClient);
            if (options is not null)
                services.TryAddSingleton(serviceProvider => options);

            return services;
        }
    }
}