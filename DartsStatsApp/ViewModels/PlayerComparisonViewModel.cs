using DartsStatsApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsStatsApp.ViewModels
{
    public class PlayerComparisonViewModel
    {
        private DbService _dbService;

        public PlayerComparisonViewModel(DbService dbService)
        {
            _dbService = dbService;
        }


    }
}
