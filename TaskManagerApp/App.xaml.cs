using TaskManagerApp.Pages;

namespace TaskManagerApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var startPage = _serviceProvider.GetRequiredService<CreateUserPage>();
            return new Window(startPage);
        }
    }
}