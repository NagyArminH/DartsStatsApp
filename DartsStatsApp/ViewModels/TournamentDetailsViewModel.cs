using DartsStatsApp.Models;
using DartsStatsApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace DartsStatsApp.ViewModels
{
    [QueryProperty(nameof(TournamentId), "TournamentId")]
    public class TournamentDetailsViewModel : ObservableObject
    {
        #region fields
        private int _tournamentId;
        private DbService _dbService;
        private TournamentEntity _tournament;
        #endregion

        #region properties
        public ObservableCollection<MatchesPerRound> GroupedMatches { get; } = new ObservableCollection<MatchesPerRound>();

        
        public int TournamentId
        {
            get => _tournamentId;
            set
            {
                if (SetProperty(ref _tournamentId, value))
                {
                    _ = LoadTournamentDetails(value);
                }
            }
        }

        public TournamentEntity Tournament
        {
            get => _tournament;
            set
            {
                SetProperty(ref _tournament, value);
            }
        }
        #endregion
        
        public TournamentDetailsViewModel(DbService dbService)
        {
            _dbService = dbService;
        }

        public async Task LoadTournamentDetails(int tournamentId)
        {
            Tournament = (await _dbService.GetData<TournamentEntity>()).FirstOrDefault(t => t.Id == tournamentId);

            var allMatches = (await _dbService.GetData<MatchEntity>()).Where(m => m.TournamentId == tournamentId).ToList();

            var grouped = from g in allMatches
                          group g by g.RoundName into g
                          orderby g.Key descending
                          select new MatchesPerRound
                          {
                              Round = g.Key,
                              Matches = new ObservableCollection<MatchEntity>(g)
                          };

            GroupedMatches.Clear();

            foreach (var match in grouped)
                GroupedMatches.Add(match);
        }
    }

    public class MatchesPerRound
    {
        public string Round { get; set; }
        public ObservableCollection<MatchEntity> Matches { get; set; }
    }
}
