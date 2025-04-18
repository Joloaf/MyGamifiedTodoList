﻿using MyGamifiedTodoList.Models;
using MyGamifiedTodoList.Services;
using MyGamifiedTodoList.DataManager;

namespace MyGamifiedTodoList.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly MongoDBService _mongoService;
    private int _totalExperiencePoints;
    private int _completedTasksCount;
    private string _randomQuote;

    public string RandomQuote
    {
        get => _randomQuote;
        set => SetProperty(ref _randomQuote, value);
    }

    public async Task LoadRandomQuoteAsync()
    {
        var quotes = await MotivationalQuoteManager.GetQuote("");
        if (quotes != null && quotes.Count > 0)
        {
            var quote = quotes[0];
            RandomQuote = $"\"{quote.quote}\" - {quote.author}";
        }
    }

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

        MessagingCenter.Subscribe<TodoListViewModel, TaskModel>(this, "TaskCompleted", (sender, task) =>
        {
            UpdateProfileData();
        });

        MessagingCenter.Subscribe<NewTaskViewModel, TaskModel>(this, "AddTask", (sender, task) =>
        {
            UpdateProfileData();
        });

        UpdateProfileData();
    }

    private async void UpdateProfileData()
    {
        try
        {
            var completedTasks = await _mongoService.GetCompletedTasksAsync();
            CompletedTasksCount = completedTasks.Count;

            TotalExperiencePoints = completedTasks.Sum(task => task.ExperiencePoints);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating profile data: {ex.Message}");
        }
    }
}