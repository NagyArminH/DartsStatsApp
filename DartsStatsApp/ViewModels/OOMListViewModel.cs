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
    public class OOMListViewModel : ObservableObject
    {
        private DbService _dbService;
        public ObservableCollection<PlayerEntity> playerList { get; } = new ObservableCollection<PlayerEntity>();
        public OOMListViewModel(DbService dbService)
        {
            _dbService = dbService;
            playerList = new ObservableCollection<PlayerEntity>();
            getAllOOMPlayers();
        }

        private async void getAllOOMPlayers()
        {
            var playerList = await _dbService.GetData<PlayerEntity>();

            var orderedList = from p in playerList
                              where p.OOMPlacement > 0
                              orderby p.OOMPlacement
                              select p;
            playerList.Clear();
            foreach (PlayerEntity player in orderedList)
            {
                playerList.Add(player);
            }
        }
    }
}
