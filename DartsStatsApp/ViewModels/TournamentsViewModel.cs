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
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

namespace DartsStatsApp.ViewModels
{
    public class TournamentsViewModel
    {
        private DbService _dbService;
        public ObservableCollection<TournamentsPerMonth> GroupedTournaments { get; } = new ObservableCollection<TournamentsPerMonth>();
        public IAsyncRelayCommand NavigateToTournamentDetailsView { get; set; }

        public TournamentsViewModel(DbService dbService)
        {
            _dbService = dbService;
            NavigateToTournamentDetailsView = new AsyncRelayCommand<TournamentEntity>(navigateToTournamentDetalisView);
            getTournaments();
        }

        private async void getTournaments()
        {
            var tourneys = await _dbService.GetData<TournamentEntity>();

            var tournamentList = (from p in tourneys
                                  orderby p.StartDate ascending
                                 select p).ToList();
            
            var groupedTourneys = from t in tournamentList
                                  let helper = new DateTime(t.StartDate.Year, t.StartDate.Month,1)
                                  group t by helper into g
                                  orderby g.Key.Year, g.Key.Month
                                  select new TournamentsPerMonth
                                  { 
                                      Month = g.Key,
                                      Tournaments = new ObservableCollection<TournamentEntity>(g)
                                  };

            GroupedTournaments.Clear();

            foreach (var group in groupedTourneys)
                GroupedTournaments.Add(group);
        }

        private async Task navigateToTournamentDetalisView(TournamentEntity tournament)
        {
            if (tournament == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(TournamentDetailsView)}?TournamentId={tournament.Id}");
        }
    }

    public class TournamentsPerMonth
    {
        public DateTime Month { get; set; }
        public string MonthName => Month.ToString("yyyy. MMMM", new CultureInfo("en-US"));
        public ObservableCollection<TournamentEntity> Tournaments { get; set; }
    }
}