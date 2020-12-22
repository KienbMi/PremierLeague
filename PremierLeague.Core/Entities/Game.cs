using System.ComponentModel.DataAnnotations;

namespace PremierLeague.Core.Entities
{
    public class Game : EntityObject
    {
        [Required]
        public int Round { get; set; }

        public Team HomeTeam { get; set; }

        public Team GuestTeam { get; set; }

        public int HomeGoals { get; set; }
        public int GuestGoals { get; set; }
    }
}

