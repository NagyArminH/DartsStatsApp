using DartsStatsApp.Models;

namespace DartsStatsApp.Services
{
    public class StatSummaryService
    {
        private DbService _dbService;

        public StatSummaryService(DbService dbService)
        {
            _dbService = dbService;
        }

        public async Task PlayerStatSummary()
        {
            await _dbService.StatSummaryCreator();

            var stats = (await _dbService.GetData<MatchStatEntity>()).ToList();

            var statsByMatch = ( from s in stats
                               group s by s.MatchId into g
                               select new
                               {
                                   Key = g.Key,
                                   List = g.ToList()
                               }).ToDictionary(x=> x.Key, x=> x.List);
           
            var extendedDataList = new List<PlayerDataSummaryEntity>();

            foreach (var matchEntry in statsByMatch.Values)
            {
                foreach (var row in matchEntry)
                {
                    int legsLost = matchEntry.Where(r => r.PlayerId != row.PlayerId).Sum(r=> r.LegsWon); // megkeressük az ellenfelet és annak a játékosnak számoljuk meg a nyert legjét, ez lesz a jelenlegi iterált játékos vesztes legje
                    int setsLost = matchEntry.Where(r => r.PlayerId != row.PlayerId).Sum(r=> r.SetsWon ?? 0); // ugyanaz csak szettben

                    extendedDataList.Add(new PlayerDataSummaryEntity
                    {
                        PlayerId = row.PlayerId,
                        TotalMatches = 0,
                        LegsWon = row.LegsWon,
                        LegsLost = legsLost,
                        SetsWon = row.SetsWon ?? 0,
                        SetsLost = setsLost,
                        Average = row.Average,
                        CheckoutPercentage = row.CheckoutPercentage,
                        Total180s = row.Total180s,
                        Total140s = row.Total140s,
                        Total100Checkouts = (row.HighestCheckout >= 100) ? 1 : 0
                    });
                }
            }

            var sumPlayersStats = (from e in extendedDataList
                                  group e by e.PlayerId into g
                                  select new PlayerDataSummaryEntity
                                  {
                                      PlayerId = g.Key,
                                      TotalMatches = g.Count(),
                                      LegsWon = g.Sum(x => x.LegsWon),
                                      LegsLost = g.Sum(x => x.LegsLost),
                                      SetsWon = g.Sum(x=> x.SetsWon),
                                      SetsLost = g.Sum(x=> x.SetsLost),
                                      Average = Math.Round(g.Average(x => x.Average),2),
                                      CheckoutPercentage = Math.Round(g.Average(x => x.CheckoutPercentage), 2),
                                      Total180s = g.Sum(x=> x.Total180s),
                                      Total140s = g.Sum(x=> x.Total140s),
                                      Total100Checkouts = g.Sum(x=> x.Total100Checkouts),
                                  }).ToList();

            await _dbService.InsertPlayerDataSummary(sumPlayersStats);
        }
    }
}