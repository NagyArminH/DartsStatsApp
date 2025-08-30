using SQLite;

namespace DartsStatsApp.Models
{
    [Table("Players")]
    public class PlayerEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Country")]
        public string Country { get; set; }
        [Column("BirthDate")]
        public DateTime? BirthDate { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; }
        [Column("TotalEarnings")]
        public int Total9Darters { get; set; }
        public decimal TotalEarnings { get; set; }

        public int? OOMPlacement { get; set; }
        public decimal? OOMEarnings { get; set; }
    }
}