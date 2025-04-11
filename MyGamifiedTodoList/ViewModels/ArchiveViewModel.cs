using System.Collections.ObjectModel;
using MyGamifiedTodoList.Models;

namespace MyGamifiedTodoList.ViewModels
{
    public class ArchiveViewModel : BaseViewModel
    {
        // Use a static collection to persist tasks across instance recreations
        private static ObservableCollection<TaskModel> _archivedTasks;

        public ObservableCollection<TaskModel> ArchivedTasks
        {
            get => _archivedTasks;
            private set => SetProperty(ref _archivedTasks, value);
        }

        public ArchiveViewModel()
        {
            // Initialize the static collection if it's null
            if (_archivedTasks == null)
            {
                _archivedTasks = new ObservableCollection<TaskModel>();
            }

            // Subscribe to completed tasks sent from TodoListViewModel
            MessagingCenter.Subscribe<TodoListViewModel, TaskModel>(this, "TaskCompleted", (sender, task) =>
            {
                // Add completed task to archived tasks if it's not already there
                if (!_archivedTasks.Contains(task))
                {
                    _archivedTasks.Add(task);
                    OnPropertyChanged(nameof(ArchivedTasks));
                }
            });
        }
    }
}
