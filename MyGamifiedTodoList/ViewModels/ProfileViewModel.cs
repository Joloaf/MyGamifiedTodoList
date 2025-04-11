using MyGamifiedTodoList.Models;
using System.Collections.ObjectModel;
using MyGamifiedTodoList.Services;

namespace MyGamifiedTodoList.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly MongoDBService _mongoService;
    private int _totalExperiencePoints;
    private int _completedTasksCount;

    public int TotalExperiencePoints
    {
        get => _totalExperiencePoints;
        set
        {
            if (_totalExperiencePoints != value)
            {
                _totalExperiencePoints = value;
                OnPropertyChanged();
            }
        }
    }

    public int CompletedTasksCount
    {
        get => _completedTasksCount;
        set
        {
            if (_completedTasksCount != value)
            {
                _completedTasksCount = value;
                OnPropertyChanged();
            }
        }
    }

    public ProfileViewModel()
    {
        _mongoService = Microsoft.Maui.Controls.DependencyService.Get<MongoDBService>();

        // Subscribe to task updates
        MessagingCenter.Subscribe<TodoListViewModel, TaskModel>(this, "TaskCompleted", (sender, task) =>
        {
            UpdateProfileData();
        });

        MessagingCenter.Subscribe<NewTaskViewModel, TaskModel>(this, "AddTask", (sender, task) =>
        {
            UpdateProfileData();
        });

        // Load initial data
        UpdateProfileData();
    }

    private async void UpdateProfileData()
    {
        try
        {
            // Fetch completed tasks
            var completedTasks = await _mongoService.GetCompletedTasksAsync();
            CompletedTasksCount = completedTasks.Count;

            // Calculate total Experience Points
            TotalExperiencePoints = completedTasks.Sum(task => task.ExperiencePoints);
        }
        catch (Exception ex)
        {
            // Handle errors (log or display message)
            Console.WriteLine($"Error updating profile data: {ex.Message}");
        }
    }
}
