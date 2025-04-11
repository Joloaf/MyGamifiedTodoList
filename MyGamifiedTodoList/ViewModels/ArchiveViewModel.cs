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

            MessagingCenter.Subscribe<TodoListViewModel, TaskModel>(this, "TaskCompleted", (sender, task) =>
            {
                if (!ArchivedTasks.Contains(task))
                {
                    ArchivedTasks.Add(task);
                }
            });

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
                Console.WriteLine($"Error loading archived tasks: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
