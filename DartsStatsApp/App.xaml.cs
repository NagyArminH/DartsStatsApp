using DartsStatsApp.Services;

namespace DartsStatsApp
{
    public partial class App : Application
    {
        private DbService _dbService;
        public App(DbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async () => await InitializeDatabase()).Wait();
            MainPage = new AppShell();
        }
        private async Task InitializeDatabase()
        {
            await _dbService.InitializeDb();
            await _dbService.InsertDataIntoDb();
        }
    }
}
