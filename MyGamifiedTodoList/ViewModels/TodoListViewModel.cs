using System.Collections.ObjectModel;
using MyGamifiedTodoList.Models;
using MyGamifiedTodoList.ViewModels;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MyGamifiedTodoList.Views;

namespace MyGamifiedTodoList.ViewModels
{
    public class TodoListViewModel : BaseViewModel
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }

        public ICommand ViewTaskCommand { get; }
        public ICommand CompleteTaskCommand { get; }
        public ICommand RemoveTaskCommand { get; }

        public TodoListViewModel()
        {
            Tasks = new ObservableCollection<TaskModel>();
            ViewTaskCommand = new Command<TaskModel>(OnViewTask);
            CompleteTaskCommand = new Command<TaskModel>(CompleteTask);
            RemoveTaskCommand = new Command<TaskModel>(RemoveTask);

            // Subscribe to the AddTask message
            MessagingCenter.Subscribe<NewTaskViewModel, TaskModel>(this, "AddTask", (sender, task) =>
            {
                Tasks.Add(task);
            });
        }

        // Inside the OnViewTask method, no changes are needed as the issue is with the missing reference.
        private async void OnViewTask(TaskModel task)
        {
            if (task == null)
                return;

            // Navigate to TaskDetailsPage and pass the selected task
            await Shell.Current.GoToAsync(nameof(TaskDetailsPage), true, new Dictionary<string, object>
            {
                { "Task", task }
            });
        }


        // Call this when a task is marked complete (e.g., swiped right)
        public void CompleteTask(TaskModel task)
        {
            if (Tasks.Contains(task))
            {
                // Mark as completed
                task.IsCompleted = true;

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
