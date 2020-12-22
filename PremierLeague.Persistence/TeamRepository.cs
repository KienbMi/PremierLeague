using Microsoft.EntityFrameworkCore;
using PremierLeague.Core.Contracts;
using PremierLeague.Core.DataTransferObjects;
using PremierLeague.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeague.Persistence
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TeamRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<Team> teams)
            => await _dbContext.Teams.AddRangeAsync(teams);

        public async Task<IEnumerable<Team>> GetAllAsync()
            => await _dbContext.Teams.OrderBy(_ => _.Name).ToArrayAsync();

        public async Task<Team> GetById(int id)
            => await _dbContext.Teams.FirstOrDefaultAsync(_ => _.Id == id);
        

        public async Task<IEnumerable<TeamTableRowDto>> GetTeamTableAsync()
        {
            var teams = (await _dbContext.Teams.Select(_ => new TeamTableRowDto
            {
                Rank = 0,
                Team = _,
                TeamName = _.Name,
                Matches = _.HomeGames.Count() + _.AwayGames.Count(),
                Won = _.HomeGames.Where(g => g.HomeGoals > g.GuestGoals).Count() + _.AwayGames.Where(g => g.HomeGoals < g.GuestGoals).Count(),
                Drawn = _.HomeGames.Where(g => g.HomeGoals == g.GuestGoals).Count() + _.AwayGames.Where(g => g.HomeGoals == g.GuestGoals).Count(),
                Lost = _.HomeGames.Where(g => g.HomeGoals < g.GuestGoals).Count() + _.AwayGames.Where(g => g.HomeGoals > g.GuestGoals).Count(),
                GoalsPlus = _.HomeGames.Sum(g => g.HomeGoals) + _.AwayGames.Sum(g => g.GuestGoals),
                GoalsMinus = _.HomeGames.Sum(g => g.GuestGoals) + _.AwayGames.Sum(g => g.HomeGoals)
            }).ToListAsync())
            .OrderByDescending(_ => _.Points)
            .ThenByDescending(_ => _.GoalsDiff)
            .ToList();

            int i = 1;
            teams.ForEach(t => t.Rank = i++);

            return teams;
        }
    }
}