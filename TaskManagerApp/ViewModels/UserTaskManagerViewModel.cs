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

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                if (SetProperty(ref selectedUser, value) && value != null)
                {
                    value.Notification = false;
                    value.MenssageNotification = string.Empty;
                }
            }
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        private readonly ITaskItemService _taskItemService;
        private readonly INavigationService _navigationService;

        public UserTaskManagerViewModel(IServiceProvider serviceProvider, IUserService userService, ITaskItemService taskItemService, INavigationService navigationService)
        {
            _userService = userService;
            _taskItemService = taskItemService;
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
        }

        public async void LoadDataAsync()
        {
            var userTaskResponse = await _userService.GetAllUsers(true);

            if (userTaskResponse.Success)
            {
                FillUsers(userTaskResponse);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", userTaskResponse.Message, "OK");
            }

        }

        private void FillUsers(Services.Responses.Response<List<User>> userTaskResponse)
        {
            foreach (var user in userTaskResponse.Data)
            {
                var existingUser = Users.FirstOrDefault(u => u.Id == user.Id);

                if (existingUser == null)
                {
                    Users.Add(user);
                }
                else
                {
                    existingUser.Tasks.Clear();
                    foreach (var task in user.Tasks)
                        existingUser.Tasks.Add(task);
                }
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
                FillUsers(randonUserResponse);
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
            Users = new ObservableCollection<User>();
            await _navigationService.NavigationTO(nameof(CreateUserPage), resetStack: true);

            await Application.Current.MainPage.DisplayAlert(
                "Sucesso",
                "Log Out realizado com sucesso.",
                "OK");
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
        [RelayCommand]
        private async Task ShowNotification(User user)
        {
            if (user == null) return;

            user.Notification = false;

            var message = user.MenssageNotification;
            user.MenssageNotification = string.Empty;


            var index = Users.IndexOf(user);
            if (index >= 0)
                Users[index] = user;
        }

    }
}