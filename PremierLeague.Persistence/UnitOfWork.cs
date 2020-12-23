using Microsoft.EntityFrameworkCore;
using PremierLeague.Core.Contracts;
using PremierLeague.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeague.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// ConnectionString kommt aus den appsettings.json
        /// </summary>
        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();
            Teams = new TeamRepository(_dbContext);
            Games = new GameRepository(_dbContext);
        }

        public ITeamRepository Teams { get; }
        public IGameRepository Games { get; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task MigrateDatabaseAsync() => await _dbContext.Database.MigrateAsync();
        public async Task DeleteDatabaseAsync() => await _dbContext.Database.EnsureDeletedAsync();

        public async Task SaveChangesAsync()
        {
            var entities = _dbContext.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity);
            foreach (var entity in entities)
            {
                await ValidateEntityAsync(entity);
            }
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Hat ein Team in dieser Runde bereits gespielt?
        /// Liegt die Rundenzahl, abhängig von der Teamanzahl im gültigen Bereich?
        /// </summary>
        /// <param name="entity"></param>
        private async Task ValidateEntityAsync(object entity)
        {
            if (entity is Game game)
            {
                List<string> viewObjects = new List<string>();  
                if (await _dbContext.Games.AnyAsync(_ => _.Id != game.Id && _.Round == game.Round && (_.HomeTeam.Name == game.HomeTeam.Name || _.GuestTeam.Name == game.HomeTeam.Name)))
                {
                    viewObjects.Add("SelectedHomeTeam");
                }
                if (await _dbContext.Games.AnyAsync(_ => _.Id != game.Id &&_.Round == game.Round && (_.HomeTeam.Name == game.GuestTeam.Name || _.GuestTeam.Name == game.GuestTeam.Name)))
                {
                    viewObjects.Add("SelectedGuestTeam");
                }
                if (viewObjects.Count > 0)
                {
                    throw new ValidationException("Team hat in Runde bereits gespielt!", null, viewObjects);
                }
            }
            if (entity is Team team)
            {
                //throw new NotImplementedException("DB-Validierungen für Team implementieren!");
            }
        }
    }
}
