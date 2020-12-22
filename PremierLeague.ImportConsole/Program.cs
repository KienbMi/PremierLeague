using PremierLeague.Core.Contracts;
using PremierLeague.Core.Entities;
using PremierLeague.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeague.ImportConsole
{
  class Program
  {
    static async Task Main()
    {
      Console.WriteLine("Import der Spiele und Teams in die Datenbank");
      using IUnitOfWork unitOfWork = new UnitOfWork();
      Console.WriteLine("Datenbank löschen");
      await unitOfWork.DeleteDatabaseAsync();
      Console.WriteLine("Datenbank migrieren");
      await unitOfWork.MigrateDatabaseAsync();
      Console.WriteLine("Spiele werden von premierleague.csv eingelesen");
      var games = (await ImportController.ReadFromCsvAsync()).ToArray();
      if (games.Length == 0)
      {
        Console.WriteLine("!!! Es wurden keine Spiele eingelesen");
        return;
      }
      Console.WriteLine($"  Es wurden {games.Count()} Spiele eingelesen!");
      var teams = games.SelectMany(g => new List<Team> { g.HomeTeam, g.GuestTeam }).Distinct().ToList();
      Console.WriteLine($"  Es wurden {teams.Count()} Teams eingelesen!");
      Console.WriteLine("Daten werden in Datenbank gespeichert (in Context übertragen)");
      Console.WriteLine("Zuerst die Teams, damit die maximale Rundenzahl bei der Validierung stimmt");
      await unitOfWork.Teams.AddRangeAsync(teams);
      await unitOfWork.SaveChangesAsync();
      Console.WriteLine("Dann die Games mit den TeamGoals und deren TeamId");
      await unitOfWork.Games.AddRangeAsync(games);
      await unitOfWork.SaveChangesAsync();
      Console.WriteLine();
      Console.WriteLine("Daten wurden in DB gespeichert!");
      Console.Write("Beenden mit Eingabetaste ...");
      Console.ReadLine();
    }
  }
}
