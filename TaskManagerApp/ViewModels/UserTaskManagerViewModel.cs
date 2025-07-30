using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TaskManagerApp.Models;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels
{
    public partial class UserTaskManagerViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        private User? selectedUser;

        private readonly IUserService _userService;

        public UserTaskManagerViewModel(IUserService userService)
        {
            _userService = userService;
            LoadData();
        }

        private async void LoadData()
        {
            var userTaskResponse = await _userService.GetAllUsers(true);

            if (userTaskResponse.Success)
            {

                Users = new ObservableCollection<User>(userTaskResponse.Data);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", userTaskResponse.Message, "OK");
            }

        }
    }
}