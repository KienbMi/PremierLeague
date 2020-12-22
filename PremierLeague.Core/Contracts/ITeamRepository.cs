using PremierLeague.Core.DataTransferObjects;
using PremierLeague.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PremierLeague.Core.Contracts
{
  public interface ITeamRepository
  {
    Task AddRangeAsync(IEnumerable<Team> teams);
        Task<IEnumerable<TeamTableRowDto>> GetTeamTableAsync();
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team> GetById(int id);
    }
}