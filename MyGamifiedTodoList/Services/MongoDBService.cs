using MongoDB.Driver;
using MyGamifiedTodoList.Models;
using System.Collections.ObjectModel;

namespace MyGamifiedTodoList.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<TaskModel> _tasksCollection;

        public MongoDBService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            }

            try
            {
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);

                // Use a longer timeout for initial connection
                settings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);

                var client = new MongoClient(settings);
                var database = client.GetDatabase("gamifiedtodolist");
                _tasksCollection = database.GetCollection<TaskModel>("tasks");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MongoDB connection error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<TaskModel>> GetAllTasksAsync()
        {
            return await _tasksCollection.Find(task => true).ToListAsync();
        }

        public async Task<List<TaskModel>> GetActiveTasksAsync()
        {
            return await _tasksCollection.Find(task => !task.IsCompleted).ToListAsync();
        }

        public async Task<List<TaskModel>> GetCompletedTasksAsync()
        {
            return await _tasksCollection.Find(task => task.IsCompleted).ToListAsync();
        }

        public async Task CreateTaskAsync(TaskModel task)
        {
            await _tasksCollection.InsertOneAsync(task);
        }

        public async Task UpdateTaskAsync(TaskModel task)
        {
            await _tasksCollection.ReplaceOneAsync(t => t.Id == task.Id, task);
        }

        public async Task CompleteTaskAsync(string taskId)
        {
            var filter = Builders<TaskModel>.Filter.Eq(t => t.Id, taskId);
            var update = Builders<TaskModel>.Update.Set(t => t.IsCompleted, true);
            await _tasksCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteTaskAsync(string taskId)
        {
            await _tasksCollection.DeleteOneAsync(t => t.Id == taskId);
        }
    }
}
