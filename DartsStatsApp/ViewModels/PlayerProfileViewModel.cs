using CommunityToolkit.Mvvm.ComponentModel;
using DartsStatsApp.Models;
using DartsStatsApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsStatsApp.ViewModels
{
    [QueryProperty(nameof(Id),"Id")]
    public class PlayerProfileViewModel : ObservableObject
    {
        #region fields
        private DbService _dbService;
        private int _id;
        private PlayerEntity? _player;
        private PlayerDataSummaryEntity _dataSummary;
        
        #endregion

        #region properties
        public int Id
        {
            get => _id;
            set
            {
                if (SetProperty(ref _id, value))
                {
                    _ = GetPlayerData();
                }
            }
        }

        public PlayerEntity? Player
        {
            get => _player;
            set
            {
                SetProperty(ref _player, value);
            }
        }
        public PlayerDataSummaryEntity DataSummary
        {
            get => _dataSummary;
            set
            {
                SetProperty(ref _dataSummary, value);
            }
        }
        #endregion

        public PlayerProfileViewModel(DbService dbService)
        {
            _dbService = dbService;
        }
        private async Task GetPlayerData()
        {
            var players = await _dbService.GetData<PlayerEntity>();
            Player = players.FirstOrDefault(p => p.Id == Id);

            var summary = await _dbService.GetData<PlayerDataSummaryEntity>();
            DataSummary = summary.FirstOrDefault(s => s.PlayerId == Id);
        }
    }
}
