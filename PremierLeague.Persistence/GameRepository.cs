using PremierLeague.Core.Contracts;
using PremierLeague.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PremierLeague.Persistence
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GameRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Game game)
        {
            _dbContext.Games.Add(game);
        }

        public async Task AddRangeAsync(IEnumerable<Game> games)
        => await _dbContext.Games.AddRangeAsync(games);
    }
}