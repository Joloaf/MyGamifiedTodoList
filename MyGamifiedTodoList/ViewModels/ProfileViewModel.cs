using MyGamifiedTodoList.Models;
using System.Collections.ObjectModel;

namespace MyGamifiedTodoList.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private int _totalXP;
    private int _tasksCompleted;

    public int TotalXP
    {
        get => _totalXP;
        set => SetProperty(ref _totalXP, value);
    }

    public int TasksCompleted
    {
        get => _tasksCompleted;
        set => SetProperty(ref _tasksCompleted, value);
    }

    public ProfileViewModel()
    {
        // Subscribe to completed task messages
        MessagingCenter.Subscribe<TodoListViewModel, TaskModel>(this, "TaskCompleted", (sender, task) =>
        {
            TotalXP += task.ExperiencePoints; // Add XP from the task
            TasksCompleted++; // Increment completed tasks
        });
    }
}
