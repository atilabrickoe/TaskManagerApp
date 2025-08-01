using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private readonly ITaskItemService _taskItemService;

        public UserTaskManagerViewModel(IUserService userService, ITaskItemService taskItemService)
        {
            _userService = userService;
            _taskItemService = taskItemService;
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
        [RelayCommand]
        private async Task DeleteTask(TaskItem taskItem)
        {
            if (taskItem != null)
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert(
                    "Confirm",
                    $"Term certeza que deseja deletar a tarefa: '{taskItem.Title}'?",
                    "Sim", "Não");

                if (!confirm)
                    return;

                var result = await _taskItemService.DeleteTaskAsync(taskItem.Id);
                if (result.Success)
                {
                    Users.FirstOrDefault(u => u.Id == SelectedUser.Id)?.Tasks.Remove(taskItem);
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Tarefa deletada com sucesso.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", result.Message, "OK");
                }
            }
        }
    }
}