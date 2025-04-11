using System.Collections.ObjectModel;
using MyGamifiedTodoList.Models;

namespace MyGamifiedTodoList.ViewModels
{
    public class ArchiveViewModel : BaseViewModel
    {
        public ObservableCollection<TaskModel> ArchivedTasks { get; set; }

        public ArchiveViewModel()
        {
            ArchivedTasks = new ObservableCollection<TaskModel>();

            // Subscribe to completed tasks sent from TodoListViewModel
            MessagingCenter.Subscribe<TodoListViewModel, TaskModel>(this, "TaskCompleted", (sender, task) =>
            {
                ArchivedTasks.Add(task);
            });
        }
    }
}
