// MyGamifiedTodoList/Models/TaskModel.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyGamifiedTodoList.Models
{
    public class TaskModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public string Priority { get; set; }
        public int ExperiencePoints { get; set; }
        public bool IsCompleted { get; set; }

        // For newly created tasks where we don't have an ID yet
        public TaskModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        // Override Equals to prevent duplicate tasks
        public override bool Equals(object obj)
        {
            if (obj is not TaskModel other) return false;

            return Id == other.Id;
        }

        // Always override GetHashCode when overriding Equals
        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 0;
        }
    }
}
