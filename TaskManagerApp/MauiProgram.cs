using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskManagerApp.Pages;
using TaskManagerApp.Services;
using TaskManagerApp.ViewModels;
using System.Reflection;


namespace TaskManagerApp
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
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(basePath!, "appsettings.json"), optional: false, reloadOnChange: true)
                .Build();

            builder.Configuration.AddConfiguration(config);

            #if DEBUG
                builder.Logging.AddDebug();
            #endif

            builder.Services.AddHttpClient();

            // Register services
            builder.Services.AddHttpClient<IApiService, ApiService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskItemService, TaskItemService>();
            builder.Services.AddTransient<INavigationService, NavigationService>();
            builder.Services.AddTransient(typeof(IHandleApiResponseService<>), typeof(HandleApiResponseService<>));
            builder.Services.AddSingleton<RabbitMqListenerService>();

            // Register view + viewmodel
            builder.Services.AddTransient<CreateUserViewModel>();
            builder.Services.AddSingleton<UserTaskManagerViewModel>();
            builder.Services.AddTransient<AddRandomUsersPage>();
            builder.Services.AddTransient<AddRandomUsersViewModel>();
            builder.Services.AddTransient<TaskEditPage>();
            builder.Services.AddTransient<TaskEditViewModel>();





            return builder.Build();
        }
    }
}
