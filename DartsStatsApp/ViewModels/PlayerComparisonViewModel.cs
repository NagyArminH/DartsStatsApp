using CommunityToolkit.Mvvm.ComponentModel;
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
    public class PlayerComparisonViewModel : ObservableObject
    {
        #region fields
        private DbService _dbService;
        private ObservableCollection<PlayerOption> _playerOptionA = new ObservableCollection<PlayerOption>();
        private ObservableCollection<PlayerOption> _playerOptionB = new ObservableCollection<PlayerOption>();
        private List<PlayerDataSummaryEntity> _summary = new List<PlayerDataSummaryEntity>();
        private PlayerOption? _selectedPlayerA;
        private PlayerOption? _selectedPlayerB;
        private PlayerStatSummary _playerFormA;
        private PlayerStatSummary _playerFormB;
        private bool _isReady;
        #endregion

        #region properties
        public ObservableCollection<PlayerOption> PlayerOptionsA
        {
            get => _playerOptionA;
            set
            {
                SetProperty(ref _playerOptionA, value);
            }
        }

        public ObservableCollection<PlayerOption> PlayerOptionsB
        {
            get => _playerOptionB;
            set
            {
                SetProperty(ref _playerOptionB, value);
            }
        }

        public bool IsReady
        {
            get => _isReady;
            set
            {
                SetProperty(ref _isReady, value);
            }
        }

        public PlayerOption? SelectedPlayerA
        {
            get => _selectedPlayerA;
            set
            {
                if(SetProperty(ref _selectedPlayerA, value))
                    UpdateUIWhenPlayersSelected();
            }
        }

        public PlayerOption? SelectedPlayerB
        {
            get => _selectedPlayerB;
            set
            {
                if (SetProperty(ref _selectedPlayerB, value))
                    UpdateUIWhenPlayersSelected();
            }
        }

        public PlayerStatSummary PlayerFormA
        {
            get => _playerFormA;
            set
            {
                SetProperty(ref _playerFormA, value);
            }
        }
        public PlayerStatSummary PlayerFormB
        {
            get => _playerFormB;
            set
            {
                SetProperty(ref _playerFormB, value);
            }
        }
        #endregion
        public PlayerComparisonViewModel(DbService dbService)
        {
            _dbService = dbService;
            _ = LoadPlayers();
        }
        private async Task LoadPlayers()
        {
            _summary = await _dbService.GetData<PlayerDataSummaryEntity>();

            var players = await _dbService.GetData<PlayerEntity>();
            var rankedPlayers = from p in players
                           where p.OOMPlacement > 0
                           select p;

            var filteredPlayers = (from f in rankedPlayers
                                   join s in _summary on f.Id equals s.PlayerId
                                   orderby f.OOMPlacement
                                   select new PlayerOption
                                   {
                                       PlayerId = f.Id,
                                       DisplayName = f.Name,
                                   }).ToList();

            PlayerOptionsA.Clear();
            PlayerOptionsB.Clear();

            foreach (var player in filteredPlayers)
            {
                PlayerOptionsA.Add(player);
                PlayerOptionsB.Add(player);
            }

            UpdateUIWhenPlayersSelected();
        }

        private PlayerStatSummary BuildPlayerData(int playerId)
        {
            var sum = _summary.FirstOrDefault(x=>x.PlayerId == playerId);
            if(sum == null) return new PlayerStatSummary();

            return new PlayerStatSummary
            {
                Matches = sum.TotalMatches,
                Average = sum.Average,
                WinPercentage = sum.WinPercentage,
                CheckoutPercentage = sum.CheckoutPercentage,
                Total180s = sum.Total180s,
                Total140s = sum.Total140s,
                Total100PlusCheckouts = sum.Total100Checkouts
            };
        }
        private void UpdateUIWhenPlayersSelected()
        {
            int? playerAId = _selectedPlayerA?.PlayerId;
            int? playerBId = _selectedPlayerB?.PlayerId;

            bool bothHasValue = playerAId.HasValue && playerBId.HasValue;
            bool differentPlayers = playerAId != playerBId;
            IsReady = bothHasValue && differentPlayers;

            if(IsReady)
            {
                PlayerFormA = BuildPlayerData(playerAId.Value);
                PlayerFormB = BuildPlayerData(playerBId.Value);
            }
            else
            {
                PlayerFormA = new PlayerStatSummary();
                PlayerFormB = new PlayerStatSummary();
            }
        }
    }
    
    public class PlayerOption
    {
       public int PlayerId { get; set; }
       public string DisplayName { get; set; }
    }

    public class PlayerStatSummary
    {
        public int Matches { get; set; }
        public double  Average { get; set; }
        public double WinPercentage { get; set; }
        public double CheckoutPercentage { get; set; }
        public int Total180s { get; set; }
        public int Total140s { get; set; }
        public int Total100PlusCheckouts { get; set; }
    }
}