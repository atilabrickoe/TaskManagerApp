using System.Text.Json;
using TaskManagerApp.Models;
using TaskManagerApp.Pages;
using TaskManagerApp.Services;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp
{
    public partial class App : Application
    {
        private readonly UserTaskManagerViewModel _userTaskManagerViewModel;
        public App(RabbitMqListenerService listener, UserTaskManagerViewModel userTaskManagerViewModel)
        {
            InitializeComponent();

            _userTaskManagerViewModel = userTaskManagerViewModel;

            // Start the background listener
            Task.Run(async () => await listener.StartAsync());

            // Subscribe to RabbitMQ messages
            listener.OnMessageReceived += (s, msg) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    HandleNotification(msg);
                });
            };
        }
        private void HandleNotification(string jsonMessage)
        {
            var notification = JsonSerializer.Deserialize<NotificationMessage>(jsonMessage);

            if (notification == null)
                return;

            var user = _userTaskManagerViewModel.Users.FirstOrDefault(u => u.Id == notification.UserId);
            if (user != null)
            {
                user.Notification = true;
                user.MenssageNotification = notification.Message;
            }
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
