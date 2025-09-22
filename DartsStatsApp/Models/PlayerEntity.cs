using SQLite;

namespace DartsStatsApp.Models
{
    [Table("Players")]
    public class PlayerEntity
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Total9Darters { get; set; }
        public decimal TotalEarnings { get; set; }

        public int? OOMPlacement { get; set; }
        public decimal? OOMEarnings { get; set; }
    }
}