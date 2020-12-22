using System;
using System.Threading.Tasks;

namespace PremierLeague.Core.Contracts
{
  public interface IUnitOfWork : IDisposable
  {
    IGameRepository Games { get; }
    ITeamRepository Teams { get; }

    Task SaveChangesAsync();

    Task DeleteDatabaseAsync();
    Task MigrateDatabaseAsync();
  }
}
