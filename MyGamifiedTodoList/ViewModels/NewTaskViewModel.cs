using System.Collections.ObjectModel;
using System.Windows.Input;
using MyGamifiedTodoList.Models;
using MyGamifiedTodoList.Views;
using MyGamifiedTodoList.Services;

namespace MyGamifiedTodoList.ViewModels
{
    public class NewTaskViewModel : BaseViewModel
    {
        private readonly MongoDBService _mongoService;

        private string _selectedDifficulty;
        private string _selectedPriority;
        private int _experiencePoints;

        public string Title { get; set; }
        public string Description { get; set; }

        public string SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (_selectedDifficulty != value)
                {
                    _selectedDifficulty = value;
                    OnPropertyChanged();
                    CalculateExperiencePoints();
                }
            }
        }

        public string SelectedPriority
        {
            get => _selectedPriority;
            set
            {
                if (_selectedPriority != value)
                {
                    _selectedPriority = value;
                    OnPropertyChanged();
                    CalculateExperiencePoints();
                }
            }
        }

        public int ExperiencePoints
        {
            get => _experiencePoints;
            set
            {
                if (_experiencePoints != value)
                {
                    _experiencePoints = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Difficulties { get; }
        public ObservableCollection<string> Priorities { get; }

        public ICommand AddTaskCommand { get; }
        public ICommand CancelCommand { get; }

        public NewTaskViewModel()
        {
            _mongoService = Microsoft.Maui.Controls.DependencyService.Get<MongoDBService>();

            AddTaskCommand = new Command(OnAddTask);
            CancelCommand = new Command(OnCancel);

            Difficulties = new ObservableCollection<string> { "Easy", "Normal", "Hard" };
            Priorities = new ObservableCollection<string> { "Low", "Normal", "High" };

            SelectedDifficulty = "Normal";
            SelectedPriority = "Normal";
        }

        private void CalculateExperiencePoints()
        {
            int difficultyPoints = SelectedDifficulty switch
            {
                "Easy" => 10,
                "Normal" => 20,
                "Hard" => 30,
                _ => 0
            };

            int priorityPoints = SelectedPriority switch
            {
                "Low" => 5,
                "Normal" => 10,
                "High" => 15,
                _ => 0
            };

            ExperiencePoints = difficultyPoints + priorityPoints;
        }

        private async void OnAddTask()
        {
            var newTask = new TaskModel
            {
                Title = Title,
                Description = Description,
                Difficulty = SelectedDifficulty,
                Priority = SelectedPriority,
                ExperiencePoints = ExperiencePoints,
                IsCompleted = false
            };

            MessagingCenter.Send(this, "AddTask", newTask);

            await Shell.Current.GoToAsync("..");
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
