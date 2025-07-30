using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Pages;

public partial class CreateUserPage : ContentPage
{
    public CreateUserPage(CreateUserViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}