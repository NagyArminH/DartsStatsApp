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

            var allPlayers = await _dbService.GetData<PlayerEntity>();
            var playerMap = allPlayers.ToDictionary(p => p.Id, p => p.Name);

            var allMatches = (await _dbService.GetData<MatchEntity>()).Where(m => m.TournamentId == tournamentId).ToList();

            foreach (var match in allMatches)
            {
                match.Player1Name = playerMap.GetValueOrDefault(match.Player1Id, string.Empty);
                match.Player2Name = playerMap.GetValueOrDefault(match.Player2Id, string.Empty);
            }

            var grouped = from g in allMatches
                          group g by g.RoundOrder into g
                          orderby g.Key descending
                          select new MatchesPerRound
                          {
                              Round = g.First().RoundName,
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
