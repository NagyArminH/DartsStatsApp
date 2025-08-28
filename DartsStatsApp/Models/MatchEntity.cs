using SQLite;

namespace DartsStatsApp.Models
{
    [Table("Matches")]
    public class MatchEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        //Idegen kulcsok
        public int TournamentId { get; set; } // TournamentEntity.Id
        public int Player1Id { get; set; } // PlayerEntity.Id
        public int Player2Id { get; set; } // PlayerEntity.Id
        public int WinnerId { get; set; } // PlayerEntity.Id

        public DateTime Date { get; set; }
        public string RoundName { get; set; }
        public string MatchScore { get; set; }
        public string MatchFormat { get; set; }

    }
}