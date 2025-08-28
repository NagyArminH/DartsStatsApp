using SQLite;

namespace DartsStatsApp.Models
{
    [Table("MatchStats")]
    public class MatchStatEntity
    {
        // Egy játékosnak egy meccsen játszott statisztikái

        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        // Idegen kulcsok
        public int MatchId { get; set; } // MathEntity.Id
        public int PlayerId { get; set; } //PlayerEntity.Id

        public double Average { get; set; }
        public double CheckoutPercentage { get; set; }
        public int Total180s { get; set; }
        public int Total140s { get; set; }
        public int HighestCheckout { get; set; }
        public int LegsWon { get; set; }
        public int SetsWon { get; set; }
    }
}