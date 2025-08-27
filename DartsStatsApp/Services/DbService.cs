using DartsStatsApp.Models;
using SQLite;

namespace DartsStatsApp.Services
{
    public class DbService
    {
        private const string DB_NAME = "darts_stats_app_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public SQLiteAsyncConnection Connection => _connection;

        public DbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME)); // adatbázis csatlakozás
        }

        public async Task InitializeTables()
        {
            await _connection.CreateTableAsync<PlayerEntity>();
        }
    }
}
