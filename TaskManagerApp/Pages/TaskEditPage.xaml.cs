using TaskManagerApp.Models;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Pages;

[QueryProperty(nameof(CurrentUserIdStr), "CurrentUserId")]
[QueryProperty(nameof(TaskIdStr), "TaskId")]
public partial class TaskEditPage : ContentPage
{
    public string CurrentUserIdStr { get; set; }
    public string TaskIdStr { get; set; }

    private readonly TaskEditViewModel _vm;
    public TaskEditPage()
	{
		InitializeComponent();
	}
    public TaskEditPage(TaskEditViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var taskId = string.IsNullOrEmpty(TaskIdStr) ? Guid.Empty : Guid.Parse(TaskIdStr);
        _vm.SetContext(Guid.Parse(CurrentUserIdStr), taskId);
    }
}