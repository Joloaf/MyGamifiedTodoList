using System.Collections.ObjectModel;
using MyGamifiedTodoList.Models;
using MyGamifiedTodoList.ViewModels;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MyGamifiedTodoList.ViewModels
{
    public class TodoListViewModel : BaseViewModel
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }

        public TodoListViewModel()
        {
            Tasks = new ObservableCollection<TaskModel>();

            // Listen for new tasks from the NewTaskViewModel
            MessagingCenter.Subscribe<NewTaskViewModel, TaskModel>(this, "AddTask", (sender, task) =>
            {
                Tasks.Add(task);
            });
        }

        // Call this when a task is marked complete (e.g., swiped right)
        public void CompleteTask(TaskModel task)
        {
            if (Tasks.Contains(task))
            {
                // Send it to the Archive page
                MessagingCenter.Send(this, "TaskCompleted", task);

                // Remove it from the active list
                Tasks.Remove(task);
            }
        }

        // Placeholder if you want to implement a Remove/Delete action later (e.g., swiped left)
        public void RemoveTask(TaskModel task)
        {
            if (Tasks.Contains(task))
            {
                Tasks.Remove(task);
                // You could trigger a "TaskDeleted" message here if needed
            }
        }
    }
}
