using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagerApp.Pages;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels
{
    public partial class AddRandomUsersViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        public AddRandomUsersViewModel(INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
        }

        [ObservableProperty]
        private string count = string.Empty;

        public TaskCompletionSource<int?> ResultCompletionSource { get; } = new();

        [RelayCommand]
        private async Task Confirm()
        {
            if (int.TryParse(Count, out int value) && value > 0)
            {
                ResultCompletionSource.SetResult(value);
                var vm = _serviceProvider.GetService<UserTaskManagerViewModel>();
                vm.LoadDataAsync();
                await _navigationService.NavigationTO(nameof(UserTaskManagerPage));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Erro",
                    "Digite um número válido.",
                    "OK");
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            ResultCompletionSource.SetResult(null);
            await _navigationService.NavigationTO(nameof(UserTaskManagerPage));
        }
    }
}
