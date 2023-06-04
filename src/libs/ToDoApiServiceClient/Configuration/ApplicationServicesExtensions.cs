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
            JsonSerializerOptions? options = default/*,
            ServiceLifetime lifetime = ServiceLifetime.Singleton*/)
        {
            ArgumentNullException.ThrowIfNull(configureClient);

            services.AddHttpClient<IToDoServiceClient, ToDoServiceClient>(configureClient);
            if (options is not null)
                services.TryAddTransient(serviceProvider => options);

            return services;

            //return lifetime switch
            //{
            //    ServiceLifetime.Singleton => services.AddSingleton<IToDoServiceClient, ToDoServiceClient>(),
            //    ServiceLifetime.Scoped => services.AddScoped<IToDoServiceClient, ToDoServiceClient>(),
            //    ServiceLifetime.Transient => services.AddTransient<IToDoServiceClient, ToDoServiceClient>(),
            //    _ => throw new ArgumentException("Invalid lifetime argument!")
            //};
        }
    }
}