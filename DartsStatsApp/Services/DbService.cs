using DartsStatsApp.Models;
using SQLite;
using System.Diagnostics;
using System.Globalization;

namespace DartsStatsApp.Services
{
    public class DbService
    {
        private const string DB_NAME = "darts_stats_app_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public DbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME)); // adatbázis csatlakozás
        }

        public async Task InitializeDb()
        {
            await _connection.ExecuteAsync("PRAGMA foreign_keys = ON;"); // idegen kulcsok engedélyezése
            await _connection.CreateTablesAsync<PlayerEntity, TournamentEntity, MatchEntity, MatchStatEntity>(); // táblák létrehozása
            await ClearAllTables();
        }
        private async Task ClearAllTables()
        {
            await _connection.DeleteAllAsync<MatchStatEntity>();
            await _connection.DeleteAllAsync<MatchEntity>();

            await _connection.DeleteAllAsync<PlayerEntity>();
            await _connection.DeleteAllAsync<TournamentEntity>();
        }
        public async Task InsertDataIntoDb()
        {
            await GetDataFromPlayers();
            await GetDataFromTournaments();
            await GetDataFromMatches();
        }

        public async Task GetDataFromPlayers()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("PlayersData.csv");
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string row = await reader.ReadLineAsync();
                string[] helper = row.Split(',');

                PlayerEntity player = new PlayerEntity {
                    Id = int.Parse(helper[0]),
                    Name = helper[1],
                    Country = helper[2],
                    BirthDate = DateTime.TryParseExact(helper[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var birth) ? birth : (DateTime?)null,
                    Total9Darters = int.Parse(helper[4]),
                    TotalEarnings = decimal.Parse(helper[5]),
                    OOMPlacement = int.TryParse(helper[6], out int oomp) ? (int?)oomp : null,
                    OOMEarnings = decimal.TryParse(helper[7], out decimal oome) ? (decimal?)oome : null
                };
                await Create(player);
            }
        }

        public async Task GetDataFromTournaments()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("TournamentData.csv");
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string row = await reader.ReadLineAsync();
                string[] helper = row.Split(',');

                TournamentEntity tournaments = new TournamentEntity
                {
                    Id = int.Parse(helper[0]),
                    TournamentName = helper[1],
                    Location = helper[2],
                    StartDate = DateTime.ParseExact(helper[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact(helper[4], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };
                await Create(tournaments);
            }
        }

        public async Task GetDataFromMatches()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("MatchData.csv");
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string row = await reader.ReadLineAsync();
                string[] helper = row.Split(',');
                
                    MatchEntity matches = new MatchEntity
                    {
                        Id = int.Parse(helper[0]),
                        TournamentId = int.Parse(helper[1]),
                        Player1Id = int.Parse(helper[2]),
                        Player2Id = int.Parse(helper[3]),
                        WinnerId = int.Parse(helper[4]),
                        Date = DateTime.ParseExact(helper[5], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        RoundName = helper[6],
                        RoundOrder = int.Parse(helper[7]),
                        MatchScore = helper[8],
                        MatchFormat = helper[9]
                    };
                    await Create(matches);

            }
        }

        public async Task GetDataFromMatchStats()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("MatchStatData.csv");
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string row = await reader.ReadLineAsync();
                string[] helper = row.Split(',');

                MatchStatEntity matches = new MatchStatEntity
                {
                    Id = int.Parse(helper[0]),
                    MatchId = int.Parse(helper[1]),
                    PlayerId = int.Parse(helper[2]),
                    Average = double.Parse(helper[3]),
                    CheckoutPercentage = double.Parse(helper[4]),
                    Total180s = int.Parse(helper[5]),
                    Total140s = int.Parse(helper[6]),
                    HighestCheckout = int.Parse(helper[7]),
                    LegsWon = int.Parse(helper[8]),
                    SetsWon = int.TryParse(helper[9], out int stsw) ? (int?)stsw : null,
                };
                await Create(matches);

            }
        }

        public async Task Create<T> (T entity) where T : new()
        {
            await _connection.InsertAsync(entity);
        }

        public async Task<List<T>> GetData<T>() where T : new()
        {
            return await _connection.Table<T>().ToListAsync();
        }
    }
}
