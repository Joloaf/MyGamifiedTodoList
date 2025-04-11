namespace MyGamifiedTodoList.Models
{
    public class TaskModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public string Priority { get; set; }
        public int ExperiencePoints { get; set; }
        public bool IsCompleted { get; set; }

        // Override Equals to prevent duplicate tasks
        public override bool Equals(object obj)
        {
            if (obj is not TaskModel other) return false;

            return Title == other.Title &&
                   Description == other.Description &&
                   Difficulty == other.Difficulty &&
                   Priority == other.Priority &&
                   ExperiencePoints == other.ExperiencePoints;
        }

        // Always override GetHashCode when overriding Equals
        public override int GetHashCode()
        {
            return (Title, Description, Difficulty, Priority, ExperiencePoints).GetHashCode();
        }
    }
}
