using MyGamifiedTodoList.Views;

namespace MyGamifiedTodoList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes
            Routing.RegisterRoute(nameof(NewTaskPage), typeof(NewTaskPage));
            Routing.RegisterRoute(nameof(TaskDetailsPage), typeof(TaskDetailsPage));
        }
    }
}
