using System.Collections.ObjectModel;
using System.Windows.Input;
using MyGamifiedTodoList.Models;
using MyGamifiedTodoList.Views;

namespace MyGamifiedTodoList.ViewModels;

public class NewTaskViewModel : BaseViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }

    public ObservableCollection<string> Difficulties { get; } = new() { "Easy", "Normal", "Hard" };
    public ObservableCollection<string> Priorities { get; } = new() { "Urgent", "Normal Speed", "Casual" };

    private string selectedDifficulty;
    public string SelectedDifficulty
    {
        get => selectedDifficulty;
        set
        {
            if (SetProperty(ref selectedDifficulty, value))
                UpdateExperiencePoints();
        }
    }

    private string selectedPriority;
    public string SelectedPriority
    {
        get => selectedPriority;
        set
        {
            if (SetProperty(ref selectedPriority, value))
                UpdateExperiencePoints();
        }
    }

    private int experiencePoints;
    public int ExperiencePoints
    {
        get => experiencePoints;
        set => SetProperty(ref experiencePoints, value);
    }

    public ICommand AddTaskCommand { get; }
    public ICommand CancelCommand { get; }

    public NewTaskViewModel()
    {
        AddTaskCommand = new Command(OnAddTask);
        CancelCommand = new Command(OnCancel);
    }

    private void UpdateExperiencePoints()
    {
        int difficultyXP = SelectedDifficulty switch
        {
            "Easy" => 10,
            "Normal" => 20,
            "Hard" => 30,
            _ => 0
        };

        int priorityXP = SelectedPriority switch
        {
            "Casual" => 5,
            "Normal Speed" => 10,
            "Urgent" => 15,
            _ => 0
        };

        ExperiencePoints = difficultyXP + priorityXP;
    }

    private async void OnAddTask()
    {
        var newTask = new TaskModel
        {
            Title = Title,
            Description = Description,
            Difficulty = SelectedDifficulty,
            Priority = SelectedPriority,
            ExperiencePoints = ExperiencePoints
        };

        MessagingCenter.Send(this, "AddTask", newTask); // Push task to TodoListViewModel
        await Shell.Current.GoToAsync(".."); // Go back
    }

    private async void OnCancel()
    {
        await Shell.Current.GoToAsync(".."); // Go back without adding
    }
}
