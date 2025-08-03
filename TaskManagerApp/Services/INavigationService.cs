namespace TaskManagerApp.Services
{
    public interface INavigationService
    {
        Task NavigationTO(string route, bool resetStack = false);
    }
}
