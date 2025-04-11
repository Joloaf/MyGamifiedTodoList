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

            MessagingCenter.Subscribe<NewTaskViewModel, TaskModel>(this, "AddTask", async (sender, task) =>
            {
                Tasks.Add(task);
                await _mongoService.CreateTaskAsync(task);
            });

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

            await Shell.Current.GoToAsync(nameof(TaskDetailsPage), true, new Dictionary<string, object>
            {
                { "Task", task }
            });
        }

        public async void CompleteTask(TaskModel task)
        {
            if (Tasks.Contains(task))
            {
                task.IsCompleted = true;

                await _mongoService.CompleteTaskAsync(task.Id);

                MessagingCenter.Send(this, "TaskCompleted", task);

                Tasks.Remove(task);
            }
        }

        public async void RemoveTask(TaskModel task)
        {
            if (Tasks.Contains(task))
            {
                await _mongoService.DeleteTaskAsync(task.Id);

                Tasks.Remove(task);
            }
        }
    }
}