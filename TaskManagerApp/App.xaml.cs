using TaskManagerApp.Pages;
using TaskManagerApp.Services;

namespace TaskManagerApp
{
    public partial class App : Application
    {
        public App(RabbitMqListenerService listener)
        {
            InitializeComponent();

            // Start the background listener
            Task.Run(async () => await listener.StartAsync());

            // Subscribe to messages
            listener.OnMessageReceived += (s, msg) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.DisplayAlert("Notification", msg, "OK");
                });
            };
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}