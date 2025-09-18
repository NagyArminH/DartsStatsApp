using SQLite;

namespace DartsStatsApp.Models
{
    [Table("Tournaments")]
    public class TournamentEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string TournamentName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}