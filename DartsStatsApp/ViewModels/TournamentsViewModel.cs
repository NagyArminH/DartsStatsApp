using DartsStatsApp.Models;
using DartsStatsApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsStatsApp.ViewModels
{
    public class TournamentsViewModel
    {
        private DbService _dbService;
        public ObservableCollection<TournamentsPerMonth> GroupedTournaments { get; } = new ObservableCollection<TournamentsPerMonth>();

        public TournamentsViewModel(DbService dbService)
        {
            _dbService = dbService;
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
    }

    public class TournamentsPerMonth
    {
        public string Month { get; set; }
        public ObservableCollection<TournamentEntity> Tournaments { get; set; }
    }
}
