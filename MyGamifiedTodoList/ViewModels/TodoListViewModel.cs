// MyGamifiedTodoList/ViewModels/TodoListViewModel.cs
using System.Collections.ObjectModel;
using MyGamifiedTodoList.Models;
using MyGamifiedTodoList.Services;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MyGamifiedTodoList.Views;

namespace MyGamifiedTodoList.ViewModels
{
    public class TodoListViewModel : BaseViewModel
    {
        private readonly MongoDBService _mongoService;
        // Add this property to the TodoListViewModel class
        private bool _isBusy;
        public ObservableCollection<TaskModel> Tasks { get; set; }

        public ICommand ViewTaskCommand { get; }
        public ICommand CompleteTaskCommand { get; }
        public ICommand RemoveTaskCommand { get; }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public TodoListViewModel()
        {
            _mongoService = Microsoft.Maui.Controls.DependencyService.Get<MongoDBService>();

            Tasks = new ObservableCollection<TaskModel>();
            ViewTaskCommand = new Command<TaskModel>(OnViewTask);
            CompleteTaskCommand = new Command<TaskModel>(CompleteTask);
            RemoveTaskCommand = new Command<TaskModel>(RemoveTask);

            // Subscribe to the AddTask message
            MessagingCenter.Subscribe<NewTaskViewModel, TaskModel>(this, "AddTask", async (sender, task) =>
            {
                Tasks.Add(task);
                await _mongoService.CreateTaskAsync(task);
            });

            // Load tasks when the ViewModel is created
            LoadTasksAsync();
        }

        private async void LoadTasksAsync()
        {
            try
            {
                IsBusy = true;
                var tasks = await _mongoService.GetActiveTasksAsync();

                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                // Handle errors (log or display message)
                Console.WriteLine($"Error loading tasks: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

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

        public async void CompleteTask(TaskModel task)
        {
            if (Tasks.Contains(task))
            {
                // Mark as completed
                task.IsCompleted = true;

                // Update in MongoDB
                await _mongoService.CompleteTaskAsync(task.Id);

                // Send it to the Archive page
                MessagingCenter.Send(this, "TaskCompleted", task);

                // Remove it from the active list
                Tasks.Remove(task);
            }
        }

        public async void RemoveTask(TaskModel task)
        {
            if (Tasks.Contains(task))
            {
                // Delete from MongoDB
                await _mongoService.DeleteTaskAsync(task.Id);

                // Remove from UI
                Tasks.Remove(task);
            }
        }
    }
}
