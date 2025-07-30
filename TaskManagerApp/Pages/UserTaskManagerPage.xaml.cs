using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Pages;

public partial class UserTaskManagerPage : ContentPage
{
	public UserTaskManagerPage(UserTaskManagerViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}