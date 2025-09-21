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
            Task.Run(async () => await initializeDatabase()).Wait();
            MainPage = new AppShell();
           //_ = _dbService.InitializeDb();
        }
        private async Task initializeDatabase()
        {
            await _dbService.InitializeDb();
            await _dbService.InsertDataIntoDb();
        }
    }
}
