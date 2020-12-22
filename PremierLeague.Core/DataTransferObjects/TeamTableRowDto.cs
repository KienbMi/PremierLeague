using PremierLeague.Core.Entities;

namespace PremierLeague.Core.DataTransferObjects
{
    public class TeamTableRowDto
    {
        public int Rank { get; set; }
        public Team Team { get; set; }
        public string TeamName { get; set; }
        public int Matches { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsPlus { get; set; }
        public int GoalsMinus { get; set; }
        public int GoalsDiff
            => GoalsPlus - GoalsMinus;
        public int Points
            => Won * 3 + Drawn;
    }
}
