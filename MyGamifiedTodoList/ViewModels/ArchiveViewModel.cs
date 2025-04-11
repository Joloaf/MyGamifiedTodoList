// MyGamifiedTodoList/ViewModels/ArchiveViewModel.cs
using System.Collections.ObjectModel;
using MyGamifiedTodoList.Models;
using MyGamifiedTodoList.Services;

namespace MyGamifiedTodoList.ViewModels
{
    public class ArchiveViewModel : BaseViewModel
    {
        private readonly MongoDBService _mongoService;
        public ObservableCollection<TaskModel> ArchivedTasks { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ArchiveViewModel()
        {
            _mongoService = Microsoft.Maui.Controls.DependencyService.Get<MongoDBService>();

            ArchivedTasks = new ObservableCollection<TaskModel>();

            // Subscribe to completed tasks sent from TodoListViewModel
            MessagingCenter.Subscribe<TodoListViewModel, TaskModel>(this, "TaskCompleted", (sender, task) =>
            {
                // Add completed task to archived tasks if it's not already there
                if (!ArchivedTasks.Contains(task))
                {
                    ArchivedTasks.Add(task);
                }
            });

            // Load archived tasks when the ViewModel is created
            LoadArchivedTasksAsync();
        }

        private async void LoadArchivedTasksAsync()
        {
            try
            {
                IsBusy = true;
                var tasks = await _mongoService.GetCompletedTasksAsync();

                ArchivedTasks.Clear();
                foreach (var task in tasks)
                {
                    ArchivedTasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                // Handle errors (log or display message)
                Console.WriteLine($"Error loading archived tasks: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
