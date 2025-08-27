using SQLite;

namespace DartsStatsApp.Models
{
    [Table("Players")]
    class PlayerEntity
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
        public string BirthDate { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; }
        [Column("TotalEarnings")]
        public int TotalEarnings { get; set; }
    }
}