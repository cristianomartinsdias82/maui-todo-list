using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.Json;
using ToDoApiServiceClient.Configuration;
using TodoApp.Pages;
using TodoApp.ServiceClientFacade;

namespace TodoApp.Infrastructure
{
    internal static class ApplicationServicesExtensions
    {
        //How to use appsettings file in Maui projects?
        //One of the answers is here: https://montemagno.com/dotnet-maui-appsettings-json-configuration/
        public static MauiAppBuilder ConfigureEmbeddedSettings(this MauiAppBuilder builder)
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("TodoApp.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Configuration.AddConfiguration(config);

            return builder;
        }

        public static MauiAppBuilder RegisterServiceClients(this MauiAppBuilder builder)
        {
            var platform = DeviceInfo.Platform == DevicePlatform.Android ? DevicePlatform.Android.ToString() : "Others";
            const string SectionName = "ToDoApiSettings:Devices";
            var scheme = builder.Configuration[$"{SectionName}:{platform}:Scheme"];
            var host = builder.Configuration[$"{SectionName}:{platform}:Host"];
            var port = builder.Configuration[$"{SectionName}:{platform}:Port"];
            var requestTimeout = double.Parse(builder.Configuration[$"{SectionName}:{platform}:RequestTimeout"]);

            builder.Services.AddToDoApiServiceClient(
                config =>
                {
                    config.BaseAddress = new Uri($"{scheme}://{host}:{port}");
                    config.Timeout = TimeSpan.FromSeconds(requestTimeout);
                },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            builder.Services.AddSingleton<IToDoApiServiceFacade, ToDoApiServiceFacade>();

            return builder;
        }

        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<MaintainToDoPage>();

            return builder;
        }
    }
}