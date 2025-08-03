namespace TaskManagerApp.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavigationTO(string route, bool resetStack = false)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Task.Delay(1000);

                var finalRoute = resetStack ? $"//{route}" : route;
                
                await Shell.Current.GoToAsync(finalRoute);
            });
        }
    }
}
