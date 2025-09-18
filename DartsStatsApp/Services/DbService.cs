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
            InitializeDb().GetAwaiter().GetResult(); // DbService nem megy tovább amíg az InitializeDb-ben minden le nem futott
        }

        public async Task InitializeDb()
        {
            await _connection.ExecuteAsync("PRAGMA foreign_keys = ON;"); // idegen kulcsok engedélyezése
            await _connection.CreateTablesAsync<PlayerEntity, TournamentEntity, MatchEntity, MatchStatEntity>(); // táblák létrehozása
        }
    }
}
