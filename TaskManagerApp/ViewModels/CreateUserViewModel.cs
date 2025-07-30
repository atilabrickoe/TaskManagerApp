using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagerApp.Pages;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels
{
    public partial class CreateUserViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        public CreateUserViewModel(IUserService userService, INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _userService = userService;
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task CreateUser()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos", "OK");
                return;
            }

            var result = await _userService.CreateUserAsync(UserName, Password);

            if (result.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Sucesso", "Usuário Criado!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", result.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos", "OK");
                return;
            }
            try
            {
                var result = await _userService.LoginAsync(UserName, Password);
                if (result.Success)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Usuário logado!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", result.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }

            //var loginPage = _serviceProvider.GetService<LoginPage>();
            //if (loginPage != null)
            //{
            //    await _navigationService.PushAsync(loginPage);
            //}
            //else
            //{
            //    await Application.Current.MainPage.DisplayAlert("Erro", "Página de login não encontrada.", "OK");
            //}
        }
    }
}
