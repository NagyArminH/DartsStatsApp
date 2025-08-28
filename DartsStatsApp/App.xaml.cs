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
            //dbService.InitializeTables();

            MainPage = new AppShell();
        }
    }
}
