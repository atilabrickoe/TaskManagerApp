using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using TaskManagerApp.Pages;
using TaskManagerApp.Services;
using TaskManagerApp.ViewModels;

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

            // Register view + viewmodel
            //builder.Services.AddTransient<CreateUserPage>();
            //builder.Services.AddTransient<UserTaskManagerPage>();
            builder.Services.AddTransient<CreateUserViewModel>();
            builder.Services.AddTransient<UserTaskManagerViewModel>();



            return builder.Build();
        }
    }
}
