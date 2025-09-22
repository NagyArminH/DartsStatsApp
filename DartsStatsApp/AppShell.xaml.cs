using DartsStatsApp.Views;

namespace DartsStatsApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            registerRoutes();
        }
        private void registerRoutes()
        {
            Routing.RegisterRoute(nameof(OOMListView), typeof(OOMListView));
        }
    }
}