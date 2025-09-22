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
        public ObservableCollection<TournamentEntity> Tournaments { get; } = new ObservableCollection<TournamentEntity>();

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
            Tournaments.Clear();
            foreach (var t in tournamentList)
            {
                Tournaments.Add(t);
            }
        }
    }
}
