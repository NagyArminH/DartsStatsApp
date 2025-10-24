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
        // ugyanazok a játékosok kiszűrése
        // ha az egyik játékos van csak kiválasztva és a másik üres nincs a kiválasztottban adat fix: isReady miatt nem látszódik a grid
        // UI update: középre a nevek, bal jobb oldalt pedig a számok
        #region fields
        private DbService _dbService;
        private ObservableCollection<PlayerOption> _playerOptionA = new ObservableCollection<PlayerOption>();
        private ObservableCollection<PlayerOption> _playerOptionB = new ObservableCollection<PlayerOption>();
        private List<PlayerDataSummaryEntity> _summary = new List<PlayerDataSummaryEntity>();
        private List<MatchStatEntity> _stats = new List<MatchStatEntity>();
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

        #region commands

        #endregion
        public PlayerComparisonViewModel(DbService dbService)
        {
            _dbService = dbService;
            _ = LoadPlayers();
            _ = LoadPrecomputedData();
        }

        private async Task LoadPlayers()
        {
            var players = await _dbService.GetData<PlayerEntity>();
            var orderedPlayers = (from p in players
                                  orderby p.Name
                                  select new PlayerOption
                                  {
                                      PlayerId = p.Id,
                                      DisplayName = p.Name,
                                  }).ToList();

            
            PlayerOptionsA.Clear();
            PlayerOptionsB.Clear();

            foreach (var player in orderedPlayers)
            {
                PlayerOptionsA.Add(player);
                PlayerOptionsB.Add(player);
            }
        }

        private async Task LoadPrecomputedData()
        {
            _summary = await _dbService.GetData<PlayerDataSummaryEntity>();
            _stats = await _dbService.GetData<MatchStatEntity>();
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
        public double CheckoutPercentage { get; set; }
        public int Total180s { get; set; }
        public int Total140s { get; set; }
        public int Total100PlusCheckouts { get; set; }
    }
}