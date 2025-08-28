using SQLite;

namespace DartsStatsApp.Models
{
    [Table("Tournaments")]
    public class TournamentEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }
        [Column("TournamentName")]
        public string TournamentName { get; set; }
        [Column("Location")]
        public string Location { get; set; }
        [Column("StartDate")]
        public DateTime StartDate { get; set; }
        [Column("EndDate")]
        public DateTime EndDate { get; set; }
    }
}