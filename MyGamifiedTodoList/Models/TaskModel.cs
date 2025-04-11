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

        public TaskModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}