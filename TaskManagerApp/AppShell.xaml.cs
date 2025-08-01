using TaskManagerApp.Pages;

namespace TaskManagerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CreateUserPage), typeof(CreateUserPage));
            Routing.RegisterRoute(nameof(UserTaskManagerPage), typeof(UserTaskManagerPage));
            Routing.RegisterRoute(nameof(AddRandomUsersPage), typeof(AddRandomUsersPage));
            Routing.RegisterRoute(nameof(TaskEditPage), typeof(TaskEditPage));
        }
    }
}
