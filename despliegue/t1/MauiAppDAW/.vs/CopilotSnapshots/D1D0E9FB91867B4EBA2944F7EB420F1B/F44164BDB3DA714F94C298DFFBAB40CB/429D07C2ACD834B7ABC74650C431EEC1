using Microsoft.Extensions.Logging;
using MauiAppDAW.Services;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppDAW
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Configure a single HttpClient instance with a SocketsHttpHandler
            var handler = new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromSeconds(10),
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                EnableMultipleHttp2Connections = false
            };

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://swapi.dev/api/"),
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Register SwapiService as singleton using the configured HttpClient and external image base URLs
            builder.Services.AddSingleton(new SwapiService(httpClient, "https://swudb.com", "https://starwars-databank.vercel.app"));

            // Register SearchHistoryService
            builder.Services.AddSingleton<SearchHistoryService>();

            // Register pages for DI
            builder.Services.AddTransient<Views.MainPage>();
            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<Views.SimpleLoginPage>();
            builder.Services.AddTransient<Views.SearchChoicePage>();
            builder.Services.AddTransient<Views.StarshipsPage>();
            builder.Services.AddTransient<Views.PlanetsPage>();
            builder.Services.AddTransient<Views.HistoryPage>();

            return builder.Build();
        }
    }
}
