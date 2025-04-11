namespace MyGamifiedTodoList.Views;
using MyGamifiedTodoList.ViewModels;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = new ProfileViewModel();
    }
}
