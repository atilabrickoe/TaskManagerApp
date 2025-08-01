using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManagerApp.Models;
using TaskManagerApp.Pages;
using TaskManagerApp.Services;
using TaskManagerApp.Services.Responses;
using EnumTask = TaskManagerApp.Models.Enums;

namespace TaskManagerApp.ViewModels
{
    public partial class TaskEditViewModel : ObservableObject
    {
        private readonly ITaskItemService _taskItemService;
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;


        [ObservableProperty]
        private TaskItem taskItem = new TaskItem();

        public List<EnumTask.TaskStatus> TaskStatusValues { get; } =
        Enum.GetValues(typeof(EnumTask.TaskStatus)).Cast<EnumTask.TaskStatus>().ToList();

        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        private User selectedUser;

        [ObservableProperty]
        private bool isUserPickerEnabled = true;

        public bool IsEditMode => TaskItem != null && TaskItem.Id != Guid.Empty;

        private User _currentUser;

        [ObservableProperty]
        private string pageTitle = "Tarefa";

        [ObservableProperty]
        private string saveButtonText = "Salvar";

        public TaskEditViewModel(
            ITaskItemService taskItemService,
            IUserService userService,
            INavigationService navigationService)
        {
            _taskItemService = taskItemService;
            _userService = userService;
            _navigationService = navigationService;
        }

        public async void Initialize(Guid currentUserId, Guid taskId)
        {
            var result = await _userService.GetAllUsers(false);
            if (result.Success && result.Data != null)
            {
                Users = new ObservableCollection<User>(result.Data);
            }

            _currentUser = Users.FirstOrDefault(u => u.Id == currentUserId);

            if (taskId != Guid.Empty)
            {
                var taskResponse = await _taskItemService.GetTaskByIdAsync(taskId, true);
                if (!taskResponse.Success)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", taskResponse.Message, "OK");
                    return;
                }
                TaskItem = taskResponse.Data;

                _currentUser = Users.FirstOrDefault(u => u.Id == TaskItem.IdUser);

                SelectedUser = _currentUser;
                IsUserPickerEnabled = true;
                PageTitle = "Editar Tarefa";
                SaveButtonText = "Atualizar";
            }
            else
            {
                TaskItem = new TaskItem
                {
                    DueDate = DateTime.Today,
                    IdUser = _currentUser.Id,
                    UserName = _currentUser.UserName
                };

                SelectedUser = _currentUser;
                IsUserPickerEnabled = false;
                PageTitle = "Nova Tarefa";
                SaveButtonText = "Adicionar";
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.NavigationTO(nameof(UserTaskManagerPage));
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (SelectedUser != null)
                TaskItem.IdUser = SelectedUser.Id;

            Response<TaskItem> response;
            if (IsEditMode)
                response = await _taskItemService.UpdateTaskAsync(TaskItem);
            else
                response = await _taskItemService.CreateTaskAsync(TaskItem);

            if (response.Success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Tarefa salva!", "OK");
                await _navigationService.NavigationTO(nameof(UserTaskManagerPage));
            }
            else
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "OK");
            return;
        }

        public void SetContext(Guid currentUserId, Guid taskId)
        {
            Initialize(currentUserId, taskId);
        }
    }
}