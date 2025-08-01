using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Pages;

public partial class AddRandomUsersPage : ContentPage
{
	public AddRandomUsersPage(AddRandomUsersViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}