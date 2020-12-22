using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeague.Core.Entities;

namespace PremierLeague.Core.Contracts
{
    public interface IGameRepository
    {
        Task AddRangeAsync(IEnumerable<Game> games);
        void Add(Game game);
    }
}