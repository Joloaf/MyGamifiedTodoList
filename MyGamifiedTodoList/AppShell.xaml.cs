using MyGamifiedTodoList.Views;

namespace MyGamifiedTodoList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(NewTaskPage), typeof(NewTaskPage));
            Routing.RegisterRoute(nameof(TaskDetailsPage), typeof(TaskDetailsPage));
            Routing.RegisterRoute(nameof(ArchivePage), typeof(ArchivePage));
        }
    }
}