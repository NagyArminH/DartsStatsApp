using DartsStatsApp.Models;
using DartsStatsApp.Services;
using DartsStatsApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartsStatsApp.ViewModels
{
    public class TournamentsViewModel
    {
        private DbService _dbService;
        public ObservableCollection<TournamentsPerMonth> GroupedTournaments { get; } = new ObservableCollection<TournamentsPerMonth>();
        public ICommand NavigateToTournamentDetailsView { get; set; }

        public TournamentsViewModel(DbService dbService)
        {
            _dbService = dbService;
            NavigateToTournamentDetailsView = new Command<TournamentEntity>(navigateToTournamentDetalisView);
            getTournaments();
        }

        private async void getTournaments()
        {
            var tourneys = await _dbService.GetData<TournamentEntity>();

            var tournamentList = (from p in tourneys
                                  orderby p.StartDate ascending
                                 select p).ToList();
            
            var groupedTourneys = from t in tournamentList
                                  group t by new { t.StartDate.Year, t.StartDate.Month } into g
                                  orderby g.Key.Year, g.Key.Month
                                  select new TournamentsPerMonth
                                  { 
                                      Month = new DateTime(g.Key.Year, g.Key.Month,1).ToString("yyyy. MMMM"),
                                      Tournaments = new ObservableCollection<TournamentEntity>(g)
                                  };

            GroupedTournaments.Clear();

            foreach (var group in groupedTourneys)
                GroupedTournaments.Add(group);
        }

        private async void navigateToTournamentDetalisView(TournamentEntity tournament)
        {
            if (tournament == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(TournamentDetailsView)}?TournamentId={tournament.Id}");
        }
    }

    public class TournamentsPerMonth
    {
        public string Month { get; set; }
        public ObservableCollection<TournamentEntity> Tournaments { get; set; }
    }
}