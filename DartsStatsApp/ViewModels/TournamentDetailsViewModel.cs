using DartsStatsApp.Models;
using DartsStatsApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace DartsStatsApp.ViewModels
{
    [QueryProperty(nameof(TournamentId), "TournamentId")]
    public class TournamentDetailsViewModel : ObservableObject
    {
        #region fields
        private int _tournamentId;
        private DbService _dbService;
        private TournamentEntity _tournament;

        private bool _matchOpen;
        private MatchEntity? _selectedMatch;
        private MatchStatEntity? _playerAStats;
        private MatchStatEntity? _playerBStats;
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

        public bool MatchOpen
        {
            get => _matchOpen;
            set
            {
                SetProperty(ref _matchOpen, value);
            }
        }

        public MatchEntity? SelectedMatch
        {
            get => _selectedMatch;
            set
            {
                SetProperty(ref _selectedMatch, value);
            }
        }

        public MatchStatEntity? PlayerAStats
        {
            get => _playerAStats;
            set
            {
                SetProperty(ref _playerAStats, value);
            }
        }
        public MatchStatEntity? PlayerBStats
        {
            get => _playerBStats;
            set
            {
                SetProperty(ref _playerBStats, value);
            }
        }
        #endregion
        public IAsyncRelayCommand<int> OpenMatchupCommand { get; }
        public IRelayCommand CloseMatchupCommand { get; }
        public TournamentDetailsViewModel(DbService dbService)
        {
            _dbService = dbService;
            OpenMatchupCommand = new AsyncRelayCommand<int>(OpenSelectedMatch);
            CloseMatchupCommand = new RelayCommand(CloseMatchup);
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

        public async Task OpenSelectedMatch(int matchId)
        {
            var match = (from gr in GroupedMatches
                        from m in gr.Matches
                        where m.Id == matchId
                        select m).FirstOrDefault();
            if (match == null)
                return;

            
            SelectedMatch = match;
            var getStats = await _dbService.GetData<MatchStatEntity>();
            PlayerAStats = getStats.FirstOrDefault(s => s.MatchId == SelectedMatch.Id && s.PlayerId == SelectedMatch.Player1Id);
            PlayerBStats = getStats.FirstOrDefault(s => s.MatchId == SelectedMatch.Id && s.PlayerId == SelectedMatch.Player2Id);
            
            MatchOpen = true;
        }
        private void CloseMatchup()
        {
            MatchOpen = false;
        }
    }

    public class MatchesPerRound
    {
        public string Round { get; set; }
        public ObservableCollection<MatchEntity> Matches { get; set; }
    }
}
