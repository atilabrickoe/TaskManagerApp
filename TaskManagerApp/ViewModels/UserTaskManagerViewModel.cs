using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManagerApp.Models;
using TaskManagerApp.Pages;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels
{
    public partial class UserTaskManagerViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        private User? selectedUser;

        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        private readonly ITaskItemService _taskItemService;
        private readonly INavigationService _navigationService;
        private readonly IReceiveNotificationService _receiveNotificationService;

        public UserTaskManagerViewModel(IServiceProvider serviceProvider, IUserService userService, ITaskItemService taskItemService, INavigationService navigationService, IReceiveNotificationService receiveNotificationService)
        {
            _userService = userService;
            _taskItemService = taskItemService;
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _receiveNotificationService = receiveNotificationService;
            LoadData();
        }

        private async void LoadData()
        {
            await _receiveNotificationService.ReceiveNotificationAsync();
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
        private async Task AddUser()
        {
            var createRandomUsersPage = _serviceProvider.GetRequiredService<AddRandomUsersPage>();
            var createRandomUsersVM = (AddRandomUsersViewModel)createRandomUsersPage.BindingContext;

            await Application.Current.MainPage.Navigation.PushModalAsync(createRandomUsersPage);

            var amount = await createRandomUsersVM.ResultCompletionSource.Task;
            if (amount == null)
                return;

            var randonUserResponse = await _userService.CreateRandomAsync(amount.Value);
            if (randonUserResponse.Success)
            {
                foreach (var user in randonUserResponse.Data)
                {
                    Users.Add(user);
                }
                await Application.Current.MainPage.DisplayAlert("Sucesso", $"{amount} usuários criados com sucesso.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", randonUserResponse.Message, "OK");
            }
        }
        [RelayCommand]
        private async Task LogOut()
        {
            SecureStorage.RemoveAll();
            await _navigationService.NavigationTO(nameof(CreateUserPage));
            await Application.Current.MainPage.DisplayAlert("Sucesso", "Log Out realizado com sucesso.", "OK");
        }
        [RelayCommand]
        private async Task AddTask()
        {
            var currentUserId = await SecureStorage.GetAsync("CurrentUserId");

            await Shell.Current.GoToAsync(nameof(TaskEditPage), true,
                                          new Dictionary<string, object>
                                          {
                                            { "CurrentUserId", currentUserId},
                                            { "TaskId", string.Empty }
                                          });

        }

        [RelayCommand]
        private async Task EditTask(TaskItem taskItem)
        {
            var currentUserId = await SecureStorage.GetAsync("CurrentUserId");

            await Shell.Current.GoToAsync(nameof(TaskEditPage), true,
                                          new Dictionary<string, object>
                                          {
                                            { "CurrentUserId", currentUserId},
                                            { "TaskId", taskItem.Id.ToString() }
                                          });
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

                var result = await _taskItemService.DeleteTaskByIdAsync(taskItem.Id);
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