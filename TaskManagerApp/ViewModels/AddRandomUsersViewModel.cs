using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Pages;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels
{
    public partial class AddRandomUsersViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public AddRandomUsersViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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
