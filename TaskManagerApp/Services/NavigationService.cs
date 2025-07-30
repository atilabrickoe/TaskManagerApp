namespace TaskManagerApp.Services
{
    public class NavigationService : INavigationService
    {
        public Task NavigationTO(string route)
        {
            return Shell.Current.GoToAsync(route);
        }
    }
}
