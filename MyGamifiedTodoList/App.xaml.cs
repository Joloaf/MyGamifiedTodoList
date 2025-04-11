// MyGamifiedTodoList/App.xaml.cs
using Microsoft.Extensions.Logging;
using MyGamifiedTodoList.Services;

namespace MyGamifiedTodoList
{
    public partial class App : Application
    {
        // Your MongoDB Atlas connection string (keep it secret in production!)
        private readonly string connectionString = "mongodb://joachimcarlsson:VDFquapMtZ7pSS2Y@ac-2w8fskj-shard-00-00.m7dlefm.mongodb.net:27017,ac-2w8fskj-shard-00-01.m7dlefm.mongodb.net:27017,ac-2w8fskj-shard-00-02.m7dlefm.mongodb.net:27017/?replicaSet=atlas-35boj8-shard-0&ssl=true&authSource=admin&retryWrites=true&w=majority&appName=TodoList";


        public App()
        {
            InitializeComponent();

            // Register services
            Microsoft.Maui.Controls.DependencyService.Register<MongoDBService>();

            // Create a singleton instance of the MongoDB service
            var mongoService = new MongoDBService(connectionString);
            Microsoft.Maui.Controls.DependencyService.RegisterSingleton<MongoDBService>(mongoService);

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}
