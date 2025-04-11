using MyGamifiedTodoList.Models; // Add this namespace
using MyGamifiedTodoList.Views;
using MyGamifiedTodoList.ViewModels;

namespace MyGamifiedTodoList.Views
{
    [QueryProperty(nameof(Task), "Task")]
    public partial class TaskDetailsPage : ContentPage
    {
        private TaskModel _task;
        public TaskModel Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged();
            }
        }

        public TaskDetailsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
