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
    [QueryProperty(nameof(Id),"Id")]
    public class PlayerProfileViewModel : ObservableObject
    {
        #region fields
        private DbService _dbService;
        private int _id;
        private PlayerEntity? _player;
        private PlayerDataSummaryEntity _dataSummary;
        private ObservableCollection<MonthlyAvg> _monthlyAverages = new ObservableCollection<MonthlyAvg>();
        
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
        public ObservableCollection<MonthlyAvg> MonthlyAverages
        {
            get => _monthlyAverages;
            set
            {
                SetProperty(ref _monthlyAverages, value);
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

            await GetMonthlyAverages();
        }

        private async Task GetMonthlyAverages()
        {
            var allStats = await _dbService.GetData<MatchStatEntity>();
            var allMatches = await _dbService.GetData<MatchEntity>();

            var playerStatWithDate = from p in allStats
                                      where p.PlayerId == Id
                                      join m in allMatches on p.MatchId equals m.Id
                                      select new
                                      {
                                          m.Date,
                                          p.Average
                                      };
            var groupByMonths = from s in playerStatWithDate
                                let helper = new DateTime(s.Date.Year, s.Date.Month, 1)
                                group s by helper into g
                                orderby g.Key
                                select new MonthlyAvg
                                {
                                    DateName = $"{g.Key:yyyy.MM}",
                                    Average = Math.Round(g.Average(v => v.Average))
                                };
            MonthlyAverages.Clear();

            foreach (var item in groupByMonths)
            {
                MonthlyAverages.Add(item);
            }
        }
    }

    public class MonthlyAvg
    {
        public string DateName { get; set; }
        public double Average { get; set; }
    }
}