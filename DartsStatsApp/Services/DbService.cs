using DartsStatsApp.Models;
using SQLite;

namespace DartsStatsApp.Services
{
    public class DbService
    {
        private const string DB_NAME = "darts_stats_app_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public DbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME)); // adatbázis csatlakozás
            InitializeDb().GetAwaiter().GetResult(); // DbService konstruktora nem megy tovább amíg az InitializeDb-ben minden le nem futott
        }

        public async Task InitializeDb()
        {
            await _connection.ExecuteAsync("PRAGMA foreign_keys = ON;"); // idegen kulcsok engedélyezése
            await _connection.CreateTablesAsync<PlayerEntity, TournamentEntity, MatchEntity, MatchStatEntity>(); // táblák létrehozása
            
            await InsertDataIntoDb();
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
                    BirthDate = DateTime.Parse(helper[3]),
                    Total9Darters = int.Parse(helper[4]),
                    TotalEarnings = decimal.Parse(helper[5]),
                    OOMPlacement = int.Parse(helper[6]),
                    OOMEarnings = decimal.Parse(helper[7])
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
                    StartDate = DateTime.Parse(helper[3]),
                    EndDate = DateTime.Parse(helper[4])
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
                    Date = DateTime.Parse(helper[5]),
                    RoundName = helper[6],
                    MatchScore = helper[7],
                    MatchFormat = helper[8]
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
