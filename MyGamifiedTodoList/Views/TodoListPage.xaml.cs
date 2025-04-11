namespace MyGamifiedTodoList.Views;

public partial class TodoListPage : ContentPage
{
    public TodoListPage()
    {
        InitializeComponent();
    }

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewTaskPage());
    }
}
