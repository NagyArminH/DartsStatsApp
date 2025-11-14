using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsStatsApp.Models
{
    public class PlayerDataSummaryEntity
    {
        public int PlayerId { get; set; }
        public int TotalMatches { get; set; }
        public int MatchesWon { get; set; }
        public double WinPercentage { get; set; }
        public int LegsWon { get; set; }
        public int LegsLost { get; set; }
        public int SetsWon { get; set; }
        public int SetsLost { get; set; }

        public double Average { get; set; }
        public double CheckoutPercentage { get; set; }
        public int Total180s { get; set; }
        public int Total140s { get; set; }
        public int Total100Checkouts { get; set; }
    }
}
